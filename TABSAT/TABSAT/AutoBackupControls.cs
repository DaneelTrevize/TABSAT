using System;
using System.ComponentModel;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class AutoBackupControls : UserControl
    {
        /*
         *  Upon first switching to tab page containing this control:
         *  If the backups directory doesn't exist, create it
         *  Process as though choosing a backups directory
         *  Start the saves directory watcher.
         *  
         *  Upon user choosing different backups directory:
         *  checksum all backups
         *  recalculate which active saves have backups
         *  
         *  Upon saves watcher notification:
         *  We need to determine if _Backup generation by the game is via Rename("move") or Created + Deleted...
         *  
         *  For deletions, remove item from active saves list (& unpair from backups treeview?)
         *  
         *  For renames, just update name in active saves list?
         *  
         *  For creations, busy wait for the check file to also exist, before assuming the save file writing is done?
         *  Or filter for check files too, track internal state of expected check creation/change?
         *  Then add new active save to list, checksum, check for backup?
         *  
         *  For changes, re-checksum active save & check for backup
         *  
         *  
         *  Need to avoid calculating both sets of checksums at the same time? Or at least don't both try use the single progressBar?
         */

        private string savesDirectory;
        private StatusWriterDelegate statusWriter;
        private BackupsManager backupsManager;

        public AutoBackupControls( string savesDir, StatusWriterDelegate sW )
        {
            InitializeComponent();

            savesCheckedListBox.ItemCheck += savesCheckedListBox_ItemCheck;

            savesDirectory = savesDir;
            statusWriter = sW;
            backupsManager = null;
        }

        private void backupFolderChooseButton_Click( object sender, EventArgs e )
        {
            if( backupsFolderBrowserDialog.ShowDialog() == DialogResult.OK )
            {
                disableControls();

                changeBackupsDirectory( backupsFolderBrowserDialog.SelectedPath );
            }
        }

        private void savesCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            // We need to ignore user-generated check change clicks, but allow check changes when the control is user-disabled
            if( ( (Control) sender ).Enabled )
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void savesCheckedListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            int i = savesCheckedListBox.SelectedIndex;
            backupButton.Enabled = false;
            if( i != -1 )
            {
                if( savesCheckedListBox.GetItemCheckState( i ) == CheckState.Unchecked )
                {
                    backupButton.Enabled = true;
                }
                backupsManager.tryDisplayBackupNode( savesCheckedListBox.SelectedItem.ToString() );
            }
        }

        private void backupButton_Click( object sender, EventArgs e )
        {
            disableControls();

            int i = savesCheckedListBox.SelectedIndex;
            if( i == -1 )
            {
                return;
            }
            //statusWriter( "SelectedIndex: " + i );
            CheckState c = backupsManager.backupActiveSave( savesCheckedListBox.SelectedItem.ToString() );

            refreshBackupsView();

            // No need to refreshActiveSaves() when we can update the item directly
            savesCheckedListBox.Enabled = false;    // Temporarily enable ItemCheck events to take effect
            savesCheckedListBox.SetItemCheckState( i, c );
            savesCheckedListBox.Enabled = true;

            savesCheckedListBox.SelectedIndex = -1; // Deselect the item

            enableControls();
        }

        private void backupsTreeView_AfterSelect( object sender, TreeViewEventArgs e )
        {
            TreeNode backupNode = backupsTreeView.SelectedNode;
            restoreButton.Enabled = backupNode != null && (
                                    ( backupNode.Level == 2 && backupNode.GetNodeCount( true ) == 1 )
                                    || ( backupNode.Level == 3 && backupNode.GetNodeCount( true ) == 0 ) );
            
            // We can try ScrollIntoView a paired ActiveSave item in the ListBox
            string baseName;
            switch( backupNode.Level )
            {
                case 1:
                    baseName = backupNode.Text;
                    break;
                case 2:
                    baseName = backupNode.Parent.Text;
                    break;
                case 3:
                    baseName = backupNode.Parent.Parent.Text;
                    break;
                default:
                    baseName = null;
                    break;
            }
            if( baseName != null )
            {
                for( int i = 0; i < savesCheckedListBox.Items.Count; i++ )
                {
                    var item = savesCheckedListBox.Items[i];
                    if( item.ToString() == baseName )
                    {
                        savesCheckedListBox.TopIndex = i;
                        break;
                    }
                }
            }
        }

        private void restoreButton_Click( object sender, EventArgs e )
        {
            disableControls();

            if( backupsTreeView.SelectedNode == null )
            {
                Console.Error.WriteLine( "Restore button should not have been enabled." );
                return;
            }

            TreeNode backupNode = backupsTreeView.SelectedNode;
            if( backupNode.Level == 2 && backupNode.GetNodeCount( true ) == 1 )
            {
                backupNode = backupNode.FirstNode;
            }

            if( backupsManager.restoreBackup( backupNode.FullPath ) )
            {
                statusWriter( "Successfully restored: " + backupNode.Text );
            }
            else
            {
                statusWriter( "Failed to restore: " + backupNode.Text );
            }

            backupsTreeView.SelectedNode = backupNode.Parent.Parent;    // The top level save name set folder, rather than the specific timestamped entry we were trying to restore
            // Don't set SelectedNode = null else the _AfterSelect event won't occur and the restore button won't indirectly disable

            refreshActiveSaves();

            enableControls();
        }

        private void AutoBackupControls_Load( object sender, EventArgs e )
        {
            backupsManager = new BackupsManager( savesDirectory, statusWriter );
            
            backupsManager.reloadActiveSaves();
            refreshActiveSaves();
            
            changeBackupsDirectory( BackupsManager.getDefaultBackupDirectory() );
        }

        private void refreshActiveSaves()
        {
            savesCheckedListBox.Enabled = false;    // Temporarily allow ItemCheck events to take effect
            backupsManager.displayActiveSaves( savesCheckedListBox );
            savesGroupBox.Text = "Active Save Files: " + savesCheckedListBox.Items.Count;
            savesCheckedListBox.Enabled = true;
        }

        private void refreshBackupsView()
        {
            backupsManager.displayBackups( backupsTreeView );
            backupsGroupBox.Text = "Backups: " + backupsManager.getBackupsCount();//backupsTreeView.GetNodeCount( true );
        }

        private void changeBackupsDirectory( string backupsDir )
        {
            disableControls();

            progressBar.Value = 0;

            autoBackupsBackgroundWorker = new BackgroundWorker();
            autoBackupsBackgroundWorker.WorkerReportsProgress = true;

            autoBackupsBackgroundWorker.DoWork += new DoWorkEventHandler( CalculateBackupsChecksums_DoWork );
            autoBackupsBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler( CalculateChecksums_ProgressChanged );
            autoBackupsBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( CalculateBackupsChecksums_RunWorkerCompleted );
            autoBackupsBackgroundWorker.RunWorkerAsync( backupsDir );
        }

        private void disableControls()
        {
            backupFolderGroupBox.Enabled = false;
            copySaveGroupBox.Enabled = false;
        }

        private void enableControls()
        {
            backupFolderGroupBox.Enabled = true;
            copySaveGroupBox.Enabled = true;
        }

        private void CalculateBackupsChecksums_DoWork( object sender, DoWorkEventArgs e )
        {
            e.Result = backupsManager.setBackupsDirectory( sender as BackgroundWorker, e );
        }

        private void CalculateChecksums_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
            //statusWriter( "  " + e.ProgressPercentage + "% " );
            progressBar.Value = e.ProgressPercentage;
        }

        private void CalculateBackupsChecksums_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if( e.Error != null )
            {
                statusWriter( " Error:" + Environment.NewLine + e.Error.Message );
            }
            else
            {
                statusWriter( "Finished recalculating Backups checksums." );
            }
            progressBar.Value = 0;

            backupsFolderBrowserDialog.SelectedPath = backupsManager.getBackupsDirectory();
            backupsDirectoryTextBox.Text = backupsFolderBrowserDialog.SelectedPath;
            MainWindow.shiftTextViewRight( backupsDirectoryTextBox );

            refreshBackupsView();

            autoBackupsBackgroundWorker = new BackgroundWorker();
            autoBackupsBackgroundWorker.WorkerReportsProgress = true;

            autoBackupsBackgroundWorker.DoWork += new DoWorkEventHandler( CalculateActiveChecksums_DoWork );
            autoBackupsBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler( CalculateChecksums_ProgressChanged );
            autoBackupsBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( CalculateActiveChecksums_RunWorkerCompleted );
            autoBackupsBackgroundWorker.RunWorkerAsync();
        }

        private void CalculateActiveChecksums_DoWork( object sender, DoWorkEventArgs e )
        {
            e.Result = backupsManager.calculateActiveChecksums( sender as BackgroundWorker );
        }

        private void CalculateActiveChecksums_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if( e.Error != null )
            {
                statusWriter( " Error:" + Environment.NewLine + e.Error.Message );
            }
            else
            {
                statusWriter( "Finished recalculating Active Save File checksums." );
            }
            progressBar.Value = 0;

            refreshActiveSaves();

            backupFolderChooseButton.Enabled = true;

            enableControls();

            //backupsManager.startWatcher();
        }
    }
}
