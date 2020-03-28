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
         *  Review if _Backup generation by the game is via Rename("move") or Created + Deleted...
         *  
         *  For deletions, remove item from active saves list
         *  
         *  For renames, just update name in active saves list?
         *  
         *  For creations, once the check file exists, assume the save file writing is done?
         *  Then add new active save to list, checksum, check for backup?
         *  
         *  For changes, re-checksum active save & check for backup
         */

        private readonly string savesDirectory;
        private readonly StatusWriterDelegate statusWriter;
        private BackupsManager backupsManager;

        public AutoBackupControls( string savesDir, StatusWriterDelegate sW )
        {
            InitializeComponent();

            savesCheckedListBox.ItemCheck += savesCheckedListBox_ItemCheck;

            savesDirectory = savesDir;
            statusWriter = sW;
            backupsManager = null;
        }

        private void autoBackupCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            backupsManager.setAutoBackup( autoBackupCheckBox.Checked );
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
                else if( savesCheckedListBox.GetItemCheckState( i ) == CheckState.Checked )
                {
                    TreeNode[] backupNodes = backupsTreeView.Nodes.Find( savesCheckedListBox.SelectedItem.ToString(), false );
                    if( backupNodes.Length > 0 )
                    {
                        TreeNode backupNode = backupNodes[0];
                        backupNode.EnsureVisible();
                    }
                }
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
            string baseName = savesCheckedListBox.SelectedItem.ToString();
            CheckState c = backupsManager.backupActiveSave( baseName );

            //backupsManager.tryDisplayBackupNode( baseName );    // Seems to cause unpredictable TreeNode selection, which drives possible CheckedListBox paired selection/scrolling

            // tryDisplayBackupNode() doesn't fire ActiveSavesChanged event, but we can update the item directly
            savesCheckedListBox.Enabled = false;    // Temporarily enable ItemCheck events to take effect
            savesCheckedListBox.SetItemCheckState( i, c );
            savesCheckedListBox.Enabled = true;

            savesCheckedListBox.SelectedIndex = -1; // Deselect the item

            enableControls();
        }

        internal void stopWatcher()
        {
            if( backupsManager != null )
            {
                backupsManager.setAutoBackup( false );

                backupsManager.stopWatcher();
            }
        }

        private void backupsTreeView_AfterSelect( object sender, TreeViewEventArgs e )
        {
            TreeNode backupNode = backupsTreeView.SelectedNode;
            restoreButton.Enabled = backupNode != null && backupNode.Level == 1;
            
            // We can try make visible a paired ActiveSave item in the ListBox
            string baseName;
            switch( backupNode.Level )
            {
                case 0:
                    baseName = backupNode.Text;
                    break;
                case 1:
                    baseName = backupNode.Parent.Text;
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

            if( backupsManager.restoreBackup( backupNode.Parent.Text, backupNode.Text ) )
            {
                statusWriter( "Successfully restored from:\t\\" + backupNode.FullPath + "\\" );
            }
            else
            {
                statusWriter( "Failed to restore from:\t\t\\" + backupNode.FullPath + "\\" );
            }

            enableControls();
        }

        private void AutoBackupControls_Load( object sender, EventArgs e )
        {
            backupsManager = new BackupsManager( savesDirectory, statusWriter );

            backupsManager.ActiveSavesChanged += refreshActiveSaves;
            backupsManager.BackupSavesChanged += refreshBackupSaves;

            backupsManager.loadActiveSaves();
            
            changeBackupsDirectory( BackupsManager.DEFAULT_BACKUP_DIRECTORY );
        }

        public void refreshActiveSaves( object sender, BackupsManager.ActiveSavesChangedEventArgs e )
        {
            if( savesCheckedListBox.InvokeRequired )
            {
                savesCheckedListBox.BeginInvoke( new Action( () => refreshActiveSaves( sender, e ) ) );
            }
            else
            {
                savesCheckedListBox.Enabled = false;    // Temporarily allow ItemCheck events to take effect

                savesCheckedListBox.Items.Clear();
                savesCheckedListBox.SuspendLayout();
                foreach( var entry in e.ActiveSaves )
                {
                    savesCheckedListBox.Items.Add( entry.Key, entry.Value );
                }
                savesCheckedListBox.ResumeLayout();

                savesGroupBox.Text = "Active Save Files: " + savesCheckedListBox.Items.Count;
                savesCheckedListBox.Enabled = true;
            }
        }

        private void refreshBackupSaves( object sender, BackupsManager.BackupSavesChangedEventArgs e )
        {
            if( backupsTreeView.InvokeRequired )
            {
                backupsTreeView.BeginInvoke( new Action( () => refreshBackupSaves( sender, e ) ) );
            }
            else
            {
                backupsTreeView.Enabled = false;

                backupsTreeView.BeginUpdate();
                backupsTreeView.Nodes.Clear();
                backupsTreeView.Nodes.AddRange( e.Backups );
                backupsTreeView.EndUpdate();

                backupsGroupBox.Text = "Backups: " + e.Count;
                backupsTreeView.Enabled = true;
            }
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
                statusWriter( "Error calculating Backups checksums: " + Environment.NewLine + e.Error.Message );
            }
            progressBar.Value = 0;

            backupsFolderBrowserDialog.SelectedPath = backupsManager.getBackupsDirectory();
            backupsDirectoryTextBox.Text = backupsFolderBrowserDialog.SelectedPath;

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
                statusWriter( "Error calculating Active Save File checksums: " + Environment.NewLine + e.Error.Message );
            }
            progressBar.Value = 0;

            backupFolderChooseButton.Enabled = true;

            enableControls();

            backupsManager.startWatcher();
        }
    }
}
