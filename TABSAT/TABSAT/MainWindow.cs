using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class MainWindow : Form
    {
        private const string NOW_UTC_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";

        private ModifyManagerControls modifySaveC;
        private AutoBackupControls autoBackupC;
        private SaveSelectorControl saveSelectorC;
        private readonly UpdatesManager updatesM;

        public delegate void StatusWriterDelegate( string status );


        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport( "kernel32", SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern bool AttachConsole( int dwProcessId );


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args )
        {
            string TABdirectory = args.Length >= 1 ? args[0] : TAB.GetExeDirectory();
            string savesDirectory = args.Length >= 2 ? args[1] : TAB.DEFAULT_SAVES_DIRECTORY;

            if( AttachConsole( ATTACH_PARENT_PROCESS ) )
            {
                Console.WriteLine( "They Are Billions directory:\t" + TABdirectory );
                Console.WriteLine( "Saves directory:\t\t" + savesDirectory );
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainWindow( TABdirectory, savesDirectory ) );
        }

        public MainWindow( string TABdirectory, string savesDirectory )
        {
            InitializeComponent();

            initModifySaveControl( TABdirectory, savesDirectory );

            initAutoBackupControl( savesDirectory );

            initSaveSelectorControl();

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
                statusTextBox.AppendText( Environment.NewLine + DateTime.UtcNow.ToString( NOW_UTC_FORMAT ) + " - " + status );
            }
        }

        private void initModifySaveControl( string TABdirectory, string savesDirectory )
        {
            //statusWriter( "They Are Billions directory:\t" + TABdirectory );

            ReflectorManager reflectorManager = null;
            ModifyManager modifyManager = null;
            try
            {
                reflectorManager = new ReflectorManager( TABdirectory );
                modifyManager = new ModifyManager( TABdirectory, reflectorManager, ModifyManager.DEFAULT_EDITS_DIRECTORY );
                modifySaveC = new ModifyManagerControls( modifyManager, statusWriter, savesDirectory );
            }
            catch( Exception e )
            {
                statusWriter( "Unable to initialise Save Modification control." );
                statusWriter( e.Message );

                modifySaveC = null;
                return;
            }

            modifySaveC.Dock = DockStyle.Fill;
            modifySaveC.Name = "modifySaveC";
            saveEditorTabPage.Controls.Add( modifySaveC );

            reflectorManager.setOutputHandler( modifySaveC.reflectorOutputHandler );
        }

        private void initAutoBackupControl( string savesDirectory )
        {
            try
            {
                autoBackupC = new AutoBackupControls( savesDirectory, statusWriter );
            }
            catch( Exception e )
            {
                statusWriter( "Unable to initialise Backups control." );
                statusWriter( e.Message );

                autoBackupC = null;
                return;
            }
            autoBackupC.Dock = DockStyle.Fill;
            autoBackupC.Name = "autoBackupC";
            autoBackupTabPage.Controls.Add( autoBackupC );
        }

        private void initSaveSelectorControl()
        {
            try
            {
                saveSelectorC = new SaveSelectorControl( ModifyManager.DEFAULT_EDITS_DIRECTORY, statusWriter );
            }
            catch( Exception e )
            {
                statusWriter( "Unable to initialise Save Selector control." );
                statusWriter( e.Message );

                saveSelectorC = null;
                return;
            }
            saveSelectorC.Dock = DockStyle.Fill;
            saveSelectorC.Name = "saveSelectorC";
            saveSelectorTabPage.Controls.Add( saveSelectorC );
        }

        private void tabControl_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( tabControl1.SelectedIndex == 1 )    // Assumes Modify tab page is 2nd
            {
                modifySaveC?.refreshSaveFileChoice();
            }
            if( tabControl1.SelectedIndex == 3 )    // Assumes Viewer tab page is 4th
            {
                saveSelectorC?.refreshSaveFileChoice();
            }
        }

        private void MainWindow_FormClosing( object sender, FormClosingEventArgs e )
        {
            modifySaveC?.removeReflector();
            autoBackupC?.stopWatcher();
        }
    }
}
