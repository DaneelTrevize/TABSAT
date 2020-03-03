using System;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class MainWindow : Form
    {
        private const string nowUtcFormat = "yyyy/MM/dd HH:mm:ss.fff";

        private ModifySaveControls modifySaveC;
        private AutoBackupControls autoBackupC;

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
            Application.Run( new MainWindow( TABSAT.defaultSavesDirectory ) );

        }

        public MainWindow( string savesDirectory )
        {
            InitializeComponent();
            //Height = 900;

            string reflectorDir = TABSAT.getReflectorDirectory();

            string tabDir = TABSAT.findTABdirectory();
            /*
            while( tabDir == null )
            {
                // open dialog to choose directory, test for TAB via TABSAT?
            }
            */
            statusWriter( "Reflector directory:\t\t" + reflectorDir );
            statusWriter( "They Are Billions directory:\t" + tabDir );

            initModifySaveControl( reflectorDir, tabDir, savesDirectory );

            initAutoBackupControl( savesDirectory );
        }

        private void statusWriter( string status )
        {
            if( statusTextBox.InvokeRequired )
            {
                statusTextBox.BeginInvoke( new Action( () => statusWriter( status ) ) );
            }
            else
            {
                statusTextBox.AppendText( DateTime.UtcNow.ToString( nowUtcFormat ) + " - " + status + Environment.NewLine );
            }
        }

        private void initModifySaveControl( string reflectorDir, string tabDir, string savesDirectory )
        {
            ReflectorManager reflectorManager = new ReflectorManager( reflectorDir, tabDir );
            TABSAT tabSAT = new TABSAT( reflectorManager, TABSAT.getDefaultEditsDirectory() );

            modifySaveC = new ModifySaveControls( tabSAT, statusWriter, savesDirectory );
            //modifySaveC.Location = new System.Drawing.Point( 4, 4 );
            modifySaveC.Anchor = (AnchorStyles) ( ( ( ( AnchorStyles.Top | AnchorStyles.Bottom ) | AnchorStyles.Left ) | AnchorStyles.Right ) );
            modifySaveC.Name = "modifySaveC";
            saveEditorTabPage.Controls.Add( modifySaveC );

            reflectorManager.setOutputHandler( modifySaveC.reflectorOutputHandler );
        }

        private void initAutoBackupControl( string savesDirectory )
        {
            autoBackupC = new AutoBackupControls( savesDirectory, statusWriter );
            //autoBackupC.Location = new System.Drawing.Point( 4, 4 );
            autoBackupC.Anchor = (AnchorStyles) ( ( ( ( AnchorStyles.Top | AnchorStyles.Bottom ) | AnchorStyles.Left ) | AnchorStyles.Right ) );
            autoBackupC.Name = "autoBackupC";
            autoBackupTabPage.Controls.Add( autoBackupC );
        }

        private void MainWindow_FormClosing( object sender, FormClosingEventArgs e )
        {
            modifySaveC.removeReflector();
            autoBackupC.stopWatcher();
        }
    }
}
