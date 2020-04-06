using System;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class MainWindow : Form
    {
        private const string NOW_UTC_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";

        private ModifySaveControls modifySaveC;
        private AutoBackupControls autoBackupC;
        private UpdatesManager updatesM;

        public delegate void StatusWriterDelegate( string status );

        internal static void shiftTextViewRight( TextBox textBox )
        {
            // Set the cursor to the end of the file path, to better see the file name
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args )
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainWindow( TAB.DEFAULT_SAVES_DIRECTORY ) );

        }

        public MainWindow( string savesDirectory )
        {
            InitializeComponent();
            //Width = 950;
            //Height = 900;
            //MinimumSize = new System.Drawing.Size( 800, 825 );

            string TABdirectory = TAB.GetExeDirectory();
            /*
            while( TABdirectory == null )
            {
                // open dialog to choose directory, test for TAB via TABSAT?
            }
            */
            statusWriter( "They Are Billions directory:\t" + TABdirectory );

            initModifySaveControl( TABdirectory, savesDirectory );

            initAutoBackupControl( savesDirectory );

            tabControl1.SelectedIndexChanged += tabControl_SelectedIndexChanged;

            updatesM = new UpdatesManager( statusWriter );
        }

        // We don't care for using the Opacity or TransparencyKey properties, so we can reduce flicker from drawing many controls
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void statusWriter( string status )
        {
            if( statusTextBox.InvokeRequired )
            {
                statusTextBox.BeginInvoke( new Action( () => statusWriter( status ) ) );
            }
            else
            {
                statusTextBox.AppendText( DateTime.UtcNow.ToString( NOW_UTC_FORMAT ) + " - " + status + Environment.NewLine );
            }
        }

        private void initModifySaveControl( string TABdirectory, string savesDirectory )
        {
            ReflectorManager reflectorManager = new ReflectorManager( TABdirectory );
            ModifyManager modifyManager = new ModifyManager( TABdirectory, reflectorManager, ModifyManager.DEFAULT_EDITS_DIRECTORY );

            modifySaveC = new ModifySaveControls( modifyManager, statusWriter, savesDirectory );
            modifySaveC.Dock = DockStyle.Fill;
            modifySaveC.Name = "modifySaveC";
            saveEditorTabPage.Controls.Add( modifySaveC );

            reflectorManager.setOutputHandler( modifySaveC.reflectorOutputHandler );
        }

        private void initAutoBackupControl( string savesDirectory )
        {
            autoBackupC = new AutoBackupControls( savesDirectory, statusWriter );
            autoBackupC.Dock = DockStyle.Fill;
            autoBackupC.Name = "autoBackupC";
            autoBackupTabPage.Controls.Add( autoBackupC );
        }

        private void tabControl_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( tabControl1.SelectedIndex == 1 )    // Assumes Modify tab page is 2nd
            {
                modifySaveC.refreshSaveFileChoice();
            }
        }

        private void MainWindow_FormClosing( object sender, FormClosingEventArgs e )
        {
            modifySaveC.removeReflector();
            autoBackupC.stopWatcher();
        }
    }
}
