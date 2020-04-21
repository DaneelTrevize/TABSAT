using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class ModifyManagerControls : UserControl
    {
        private readonly ModifyManager modifyManager;
        private readonly StatusWriterDelegate statusWriter;
        private readonly ModifySaveControls modifySaveControls;

        public ModifyManagerControls( ModifyManager m, StatusWriterDelegate sW, string savesDirectory )
        {
            InitializeComponent();

            modifyManager = m;
            statusWriter = sW;
            modifySaveControls = new ModifySaveControls( statusWriter );
            modifySaveControls.Location = new System.Drawing.Point( 3, 0 );
            modifySaveControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            modifySaveControls.Name = "modifySaveControls";
            optionsSplitContainer.Panel1.Controls.Add( modifySaveControls );

            saveOpenFileDialog.Filter = "TAB Save Files|" + TAB.SAVES_FILTER;// + "|Data files|*.dat";
            saveOpenFileDialog.InitialDirectory = savesDirectory;
        }

        internal void reflectorOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            if( reflectorTextBox.InvokeRequired )
            {
                reflectorTextBox.BeginInvoke( new DataReceivedEventHandler( reflectorOutputHandler ), new[] { sendingProcess, outLine } );
            }
            else
            {
                if( !String.IsNullOrEmpty( outLine.Data ) )
                {
                    reflectorTextBox.AppendText( Environment.NewLine + outLine.Data );
                }
            }
        }

        internal void refreshSaveFileChoice()
        {
            if( saveFileGroupBox.Enabled )  // Don't permit tabbing out and back during modification operations to risk changing the save file
            {
                string saveFile = TAB.GetMostRecentSave( saveOpenFileDialog.InitialDirectory );
                if( saveFile != null )
                {
                    saveOpenFileDialog.FileName = saveFile; // No need to Path.GetFileName( saveFile ), doesn't help FileDialog only displaying the last ~8.3 chars
                    setSaveFile( saveFile );
                }
            }
        }

        private void resetButton_Click( object sender, EventArgs e )
        {
            modifySaveControls.resetChoices();
        }

        private void saveFileChooseButton_Click( object sender, EventArgs e )
        {
            if( saveOpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                string file = saveOpenFileDialog.FileName;
                if( TAB.IsFileWithinDirectory( file, BackupsManager.DEFAULT_BACKUP_DIRECTORY ) )    // Doesn't use a dynamic value for the current backups directory, from the other tab's BackupManager...
                {
                    // Editing a backup will not trigger a checksum update once the modified file is repacked, confuses AutoBackup UI
                    statusWriter( "Please do not modify files within the backups directory: " + file );
                    return;
                }

                setSaveFile( file );
            }
        }

        private void setSaveFile( string saveFile )
        {
            if( modifyManager.getState() == ModifyManager.SaveState.EXTRACTED )
            {
                // We're skipping repacking
                extractRepackSaveButton.Text = "Manual Modify";

                if( reflectorStopRepackCheckBox.Checked )
                {
                    modifyManager.stopReflector();
                }

                enableChoices();
            }

            saveFileTextBox.Text = saveFile;

            modifyManager.setSaveFile( saveFile );

            reassessExtractionOption();
        }

        private void reassessExtractionOption()     // Refactor around state & event?
        {
            quickModifySaveButton.Enabled = modifyManager.hasSaveFile();
            extractRepackSaveButton.Enabled = modifyManager.hasSaveFile();
        }

        private void disableChoices()
        {
            // Don't let different options be chosen during file operations
            resetButton.Enabled = false;
            modifySaveControls.Enabled = false;
            saveFileGroupBox.Enabled = false;
            quickModifySaveButton.Enabled = false;
            extractRepackSaveButton.Enabled = false;
            extractLeaveCheckBox.Enabled = false;
            reflectorStopRepackCheckBox.Enabled = false;
            reflectorStopExtractCheckBox.Enabled = false;
        }

        private void enableChoices()
        {
            resetButton.Enabled = true;
            modifySaveControls.Enabled = true;
            saveFileGroupBox.Enabled = true;
            extractLeaveCheckBox.Enabled = true;
            reflectorStopRepackCheckBox.Enabled = true;
            reflectorStopExtractCheckBox.Enabled = reflectorStopRepackCheckBox.Checked;
        }

        private void modifySaveButton_Click( object sender, EventArgs e )
        {
            disableChoices();

            reflectorStopButton.Enabled = false;    // Don't allow stopping attempts during operation

            modifySaveBackgroundWorker = new BackgroundWorker();
            modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( modifySave_DoWork );
            modifySaveBackgroundWorker.RunWorkerAsync();
        }

        private void modifySave_DoWork( object sender, DoWorkEventArgs e )
        {
            // Firstly extract the save
            if( !extractSave() )
            {
                if( reflectorStopRepackCheckBox.Checked )    // Refactor how this _Click handles potentially failing in 2+ places and trying to stop the Reflector in 3...
                {
                    modifyManager.stopReflector();
                }

                resetSaveFileChoice();
                return;
            }

            // Make modifications
            if( !modifyExtractedSave() )
            {
                if( reflectorStopRepackCheckBox.Checked )
                {
                    modifyManager.stopReflector();
                }

                resetSaveFileChoice();
                return;
            }

            // Backup & Repack the save
            backupAndRepackSave();

            if( reflectorStopRepackCheckBox.Checked )
            {
                modifyManager.stopReflector();
            }
            else
            {   // reflectorExitRadioButton.Checked == true
                reflectorStopButton.Enabled = true;
            }

            if( !extractLeaveCheckBox.Checked )
            {
                // Remove the extracted files
                modifyManager.removeDecryptedDir();
                statusWriter( "Removed extracted files." );
            }

            resetSaveFileChoice();
        }

        private void extractRepackSaveButton_Click( object sender, EventArgs e )
        {
            disableChoices();

            reflectorStopButton.Enabled = false;    // Don't allow stopping attempts during operation

            switch( modifyManager.getState() )
            {
                case ModifyManager.SaveState.SET:
                    //Extracting

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( extractSave_DoWork );
                    modifySaveBackgroundWorker.RunWorkerAsync();

                    break;
                case ModifyManager.SaveState.EXTRACTED:
                    // Repacking

                    backupAndRepackSave();

                    if( reflectorStopRepackCheckBox.Checked )
                    {
                        modifyManager.stopReflector();
                    }
                    else
                    {
                        reflectorStopButton.Enabled = true; // If the reflector isn't being automatically stopped, let the user manually do so
                    }

                    extractRepackSaveButton.Text = "Manual Modify";

                    resetSaveFileChoice();

                    break;
                default:
                    // Error?
                    return;
            }
        }

        private void extractSave_DoWork( object sender, DoWorkEventArgs e )
        {
            if( !extractSave() )
            {
                resetSaveFileChoice();
            }
            else
            {
                modifyExtractedSave();  // Unchecked

                extractRepackSaveButton.Text = "Repack the Save File";
                extractRepackSaveButton.Enabled = true;     // For repacking
                saveFileGroupBox.Enabled = true;            // For skipping
            }

            if( reflectorStopExtractCheckBox.Checked )
            {
                modifyManager.stopReflector();
            }
            else
            {
                reflectorStopButton.Enabled = true; // If the reflector isn't being automatically stopped, let the user manually do so
            }
        }

        private void reflectorStopRepackCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            reflectorStopExtractCheckBox.Enabled = reflectorStopRepackCheckBox.Checked;
            reflectorStopExtractCheckBox.Checked = false;   // Never leave it checked when disabled. Elsewhere can't check enabled status before using checked status as it's disabled during operations.
        }

        private void reflectorStopButton_Click( object sender, EventArgs e )
        {
            reflectorStopButton.Enabled = false;
            modifyManager.stopReflector();
        }

        private bool extractSave()
        {
            extractRepackSaveButton.Enabled = false;  // Should live in extractSaveButton_Click(), thus not be triggered by modifySaveButton_Click()?

            string saveFile = modifyManager.extractSave( !extractLeaveCheckBox.Checked );
            if( saveFile == null )
            {
                statusWriter( "Unable to extract save file." );
                return false;
            }
            else
            {
                statusWriter( "Extracted save file:\t\t" + saveFile );
                return true;
            }
        }

        private bool modifyExtractedSave()
        {
            if( !modifySaveControls.anyModificationChosen() )
            {
                statusWriter( "No modifications chosen." );
                return true;
            }

            SaveEditor dataEditor = modifyManager.getSaveEditor();

            return modifySaveControls.modifySave( dataEditor );
        }

            private void backupAndRepackSave()
        {
            if( backupCheckBox.Checked )
            {
                string backupFile = modifyManager.backupSave();
                if( backupFile == null )
                {
                    statusWriter( "Unable to backup save file." );
                    return;
                }
                else
                {
                    statusWriter( "Save File backed up to:\t" + backupFile );
                }
            }

            string newSaveFile = modifyManager.repackDirAsSave();
            statusWriter( "New Save File created:\t" + newSaveFile );
        }

        private void resetSaveFileChoice()
        {
            if( saveFileTextBox.InvokeRequired )
            {
                saveFileTextBox.BeginInvoke( new Action( () => resetSaveFileChoice() ) );
            }
            else
            {
                saveFileTextBox.Text = "";
                modifyManager.setSaveFile( null );

                reassessExtractionOption();

                enableChoices();
            }
        }

        internal void removeReflector()
        {
            // Need to synchronise/mutex access to tabSAT?
            modifyManager.stopReflector();
            modifyManager.removeReflector();
        }
    }
}
