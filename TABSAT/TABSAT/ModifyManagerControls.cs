using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class ModifyManagerControls : UserControl
    {
        private enum EditingState : byte
        {
            CHOOSING_SAVE,
            SAVE_CHOSEN,
            QUICK_EXTRACTING,
            PROBLEM_ENCOUNTED,
            QUICK_EXTRACTED,
            QUICK_REPACKING,
            SAVE_REPACKED,
            MANUAL_EXTRACTING,
            MANUAL_EXTRACTED,
            MANUAL_REPACKING
        };

        private readonly ModifyManager modifyManager;
        private readonly StatusWriterDelegate statusWriter;
        private readonly ModifyChoicesControls modifyChoicesControls;
        private EditingState editingState;

        public ModifyManagerControls( ModifyManager m, StatusWriterDelegate sW, string savesDirectory )
        {
            InitializeComponent();

            modifyManager = m;
            statusWriter = sW;

            modifyChoicesControls = new ModifyChoicesControls
            {
                Location = new System.Drawing.Point( 0, 0 ),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Name = "modifyChoicesControls"
            };
            optionsSplitContainer.Panel1.Controls.Add( modifyChoicesControls );

            editingState = EditingState.CHOOSING_SAVE;

            saveOpenFileDialog.Filter = "TAB Save Files|" + TAB.SAVES_FILTER;// + "|Data files|*.dat";
            saveOpenFileDialog.InitialDirectory = savesDirectory;
        }

        internal void reflectorOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            if( !String.IsNullOrEmpty( outLine.Data ) )
            {
                if( reflectorTextBox.InvokeRequired )
                {
                    reflectorTextBox.BeginInvoke( new DataReceivedEventHandler( reflectorOutputHandler ), new[] { sendingProcess, outLine } );
                }
                else
                {
                    reflectorTextBox.AppendText( Environment.NewLine + outLine.Data );
                }
            }
        }

        private void setEditingState( EditingState newState )
        {
            if( this.InvokeRequired )
            {
                this.Invoke( new Action( () => setEditingState( newState ) ) );
            }
            else
            {
                switch( newState )
                {
                    // Use a mapping of valid state transitions..?

                    case EditingState.CHOOSING_SAVE:
                        if( editingState != EditingState.SAVE_REPACKED &&
                            editingState != EditingState.MANUAL_EXTRACTED &&
                            editingState != EditingState.PROBLEM_ENCOUNTED )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        if( editingState == EditingState.MANUAL_EXTRACTED )
                        {
                            // Skipping repacking
                            updateButtonsAfterManualRepacking();
                            considerStoppingReflector( reflectorStopRepackCheckBox.Checked );
                        }
                        editingState = newState;

                        saveFileTextBox.Text = "";
                        modifyManager.setSaveFile( null );

                        enableControls();
                        break;
                    case EditingState.SAVE_CHOSEN:
                        if( editingState != EditingState.CHOOSING_SAVE &&
                            editingState != EditingState.SAVE_CHOSEN )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        editingState = newState;

                        enableControls();
                        break;
                    case EditingState.QUICK_EXTRACTING:
                    case EditingState.MANUAL_EXTRACTING:
                    case EditingState.MANUAL_REPACKING:
                        editingState = newState;

                        disableControls();
                        break;
                    case EditingState.PROBLEM_ENCOUNTED:
                        if( editingState != EditingState.QUICK_EXTRACTING &&
                            editingState != EditingState.QUICK_EXTRACTED &&
                            editingState != EditingState.QUICK_REPACKING &&
                            editingState != EditingState.MANUAL_EXTRACTING &&
                            editingState != EditingState.MANUAL_EXTRACTED &&
                            editingState != EditingState.MANUAL_REPACKING )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        // Did we fail during a work sequence that would end after extraction or after repacking?
                        bool wasJustExtracting = editingState == EditingState.MANUAL_EXTRACTING || editingState == EditingState.MANUAL_EXTRACTED;
                        considerStoppingReflector( wasJustExtracting ? reflectorStopExtractCheckBox.Checked : reflectorStopRepackCheckBox.Checked );
                        editingState = newState;

                        break;
                    case EditingState.QUICK_EXTRACTED:
                    // We don't actually consider stopping and restarting the reflector during quick modifications.
                    case EditingState.QUICK_REPACKING:
                        editingState = newState;

                        break;
                    case EditingState.SAVE_REPACKED:
                        if( editingState != EditingState.QUICK_REPACKING &&
                            editingState != EditingState.MANUAL_REPACKING )
                        {
                            throw new InvalidOperationException( "Invalid new state: " + newState + " for current state: " + editingState );
                        }
                        if( editingState == EditingState.MANUAL_REPACKING )
                        {
                            updateButtonsAfterManualRepacking();
                        }
                        editingState = newState;

                        considerStoppingReflector( reflectorStopRepackCheckBox.Checked );
                        break;
                    case EditingState.MANUAL_EXTRACTED:
                        editingState = newState;

                        updateButtonsBeforeManualRepacking();

                        considerStoppingReflector( reflectorStopExtractCheckBox.Checked );
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented EditingState: " + newState );
                }
            }
        }

        private void disableControls()
        {
            // Don't let different options be chosen during file operations
            resetButton.Enabled = false;
            modifyChoicesControls.Enabled = false;
            saveFileGroupBox.Enabled = false;
            extractLeaveCheckBox.Enabled = false;
            reflectorStopRepackCheckBox.Enabled = false;
            reflectorStopExtractCheckBox.Enabled = false;

            extractRepackSaveButton.Enabled = false;
            quickSkipSaveButton.Enabled = false;

            reflectorStopButton.Enabled = false;        // Don't allow stopping attempts during operation
        }

        private void enableControls()
        {
            if( editingState == EditingState.CHOOSING_SAVE )
            {
                resetButton.Enabled = true;
                modifyChoicesControls.Enabled = true;
                saveFileGroupBox.Enabled = true;
                extractLeaveCheckBox.Enabled = true;
                reflectorStopRepackCheckBox.Enabled = true;
                reflectorStopExtractCheckBox.Enabled = reflectorStopRepackCheckBox.Checked;
            }
            extractRepackSaveButton.Enabled = editingState == EditingState.SAVE_CHOSEN;
            quickSkipSaveButton.Enabled = editingState == EditingState.SAVE_CHOSEN;
        }

        private void considerStoppingReflector( bool autoStop )
        {
            if( autoStop )
            {
                stopReflector();
            }
            else
            {
                reflectorStopButton.Enabled = modifyManager.reflectorReadyToStop();
            }
        }

        private void updateButtonsAfterManualRepacking()
        {
            extractRepackSaveButton.Text = "Manual Modify";
            quickSkipSaveButton.Text = "Quick Modify";
        }

        private void updateButtonsBeforeManualRepacking()
        {
            extractRepackSaveButton.Text = "Repack the Save File";
            quickSkipSaveButton.Text = "Skip repacking";
            extractRepackSaveButton.Enabled = true;
            quickSkipSaveButton.Enabled = true;

            extractLeaveCheckBox.Enabled = true;
            reflectorStopRepackCheckBox.Enabled = true;
        }

        internal void refreshSaveFileChoice()
        {
            if( editingState == EditingState.CHOOSING_SAVE || editingState == EditingState.SAVE_CHOSEN )
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
            modifyChoicesControls.resetChoices();
            refreshSaveFileChoice();
        }

        private void saveFileChooseButton_Click( object sender, EventArgs e )
        {
            if( saveOpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                string saveFile = saveOpenFileDialog.FileName;
                if( TAB.IsFileWithinDirectory( saveFile, BackupsManager.DEFAULT_BACKUP_DIRECTORY ) )    // Maybe could use a dynamic value for the current backups directory, from the other tab's BackupManager...
                {
                    // Editing a backup will not trigger a checksum update once the modified file is repacked, confuses AutoBackup UI
                    statusWriter( "Please do not modify files within the backups directory: " + saveFile );
                    return;
                }

                setSaveFile( saveFile );
            }
        }

        private void setSaveFile( string saveFile )
        {
            saveFileTextBox.Text = saveFile;
            setEditingState( EditingState.SAVE_CHOSEN );
            modifyManager.setSaveFile( saveFile );
        }

        private void quickSkipSaveButton_Click( object sender, EventArgs e )
        {
            switch( editingState )
            {
                case EditingState.SAVE_CHOSEN:
                    // Quick modifying

                    setEditingState( EditingState.QUICK_EXTRACTING );

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( quickModify_DoWork );
                    modifySaveBackgroundWorker.RunWorkerCompleted += repackingWork_RunWorkerCompleted;
                    bool anyMods = modifyChoicesControls.anyModificationChosen();
                    modifySaveBackgroundWorker.RunWorkerAsync( ( !extractLeaveCheckBox.Checked, backupCheckBox.Checked, anyMods, anyMods ? modifyChoicesControls.getChoices() : null ) );

                    break;
                case EditingState.MANUAL_EXTRACTED:
                    // Skipping repacking

                    setEditingState( EditingState.CHOOSING_SAVE );

                    break;
                default:
                    Console.Error.WriteLine( "quickSkipSaveButton_Click() should not have been invoked, invalid editing state." );
                    return;
            }
        }

        private void quickModify_DoWork( object sender, DoWorkEventArgs e )
        {
            (bool useTempDir, bool backupSave, bool anyMods, ModifyChoices choices) = ((bool, bool, bool, ModifyChoices)) e.Argument;

            // Firstly extract the save
            bool result = extractSave( useTempDir );
            if( result )
            {
                setEditingState( EditingState.QUICK_EXTRACTED );

                // Make modifications
                result = modifyExtractedSave( anyMods, choices );
                if( result )
                {
                    // No modified state to set, moving on...

                    // Backup & Repack the save
                    setEditingState( EditingState.QUICK_REPACKING );
                    result = backupAndRepackSave( backupSave );
                }

                if( useTempDir )
                {
                    // Remove the extracted files
                    modifyManager.removeDecryptedDir();
                    statusWriter( "Removed extracted files." );
                }
            }

            e.Result = result;
        }

        private void repackingWork_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if( e.Error != null )
            {
                statusWriter( "Error while operating on the save: " + e.Error.ToString() );
            }
            // We don't care to use the worker reference to report progress or make the task cancellable.
            else
            {
                if( (bool) e.Result )
                {
                    setEditingState( EditingState.SAVE_REPACKED );
                }
                else
                {
                    setEditingState( EditingState.PROBLEM_ENCOUNTED );
                }
            }

            setEditingState( EditingState.CHOOSING_SAVE );
        }

        private void extractRepackSaveButton_Click( object sender, EventArgs e )
        {
            switch( editingState )
            {
                case EditingState.SAVE_CHOSEN:
                    // Extracting

                    setEditingState( EditingState.MANUAL_EXTRACTING );

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( manualExtractSave_DoWork );
                    modifySaveBackgroundWorker.RunWorkerCompleted += manualExtractSave_RunWorkerCompleted;
                    bool anyMods = modifyChoicesControls.anyModificationChosen();
                    modifySaveBackgroundWorker.RunWorkerAsync( ( !extractLeaveCheckBox.Checked, anyMods, anyMods ? modifyChoicesControls.getChoices() : null ) );

                    break;
                case EditingState.MANUAL_EXTRACTED:
                    // Repacking

                    setEditingState( EditingState.MANUAL_REPACKING );

                    modifySaveBackgroundWorker = new BackgroundWorker();
                    modifySaveBackgroundWorker.DoWork += (s,a) => a.Result = backupAndRepackSave( backupCheckBox.Checked );
                    modifySaveBackgroundWorker.RunWorkerCompleted += repackingWork_RunWorkerCompleted;
                    modifySaveBackgroundWorker.RunWorkerAsync();

                    break;
                default:
                    Console.Error.WriteLine( "extractRepackSaveButton_Click() should not have been invoked, invalid editing state." );
                    return;
            }
        }

        private void manualExtractSave_DoWork( object sender, DoWorkEventArgs e )
        {
            (bool useTempDir, bool anyMods, ModifyChoices choices) = ((bool, bool, ModifyChoices)) e.Argument;
            bool result = extractSave( useTempDir );
            if( result )
            {
                setEditingState( EditingState.MANUAL_EXTRACTED );
                result = modifyExtractedSave( anyMods, choices );
                // No modified state to set, moving on...
            }

            e.Result = result;
        }

        private void manualExtractSave_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            if( e.Error != null )
            {
                statusWriter( "Error while extracting or modifying the save: " + e.Error.ToString() );
            }
            // We don't care to use the worker reference to report progress or make the task cancellable.
            else
            {
                // State is already MANUAL_EXTRACTED if successful extraction or modification.
                if( !(bool) e.Result )
                {
                    setEditingState( EditingState.PROBLEM_ENCOUNTED );

                    setEditingState( EditingState.CHOOSING_SAVE );
                }
            }
        }

        private bool extractSave( bool useTempDir )
        {
            string saveFile = modifyManager.extractSave( useTempDir );
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

        private bool modifyExtractedSave( in bool anyMods, in ModifyChoices choices )
        {
            if( !anyMods )
            {
                statusWriter( "No modifications chosen." );
                return true;
            }

            SaveEditor dataEditor = modifyManager.getSaveEditor();
            if( dataEditor == null )
            {
                statusWriter( "Unable to read extracted save file." );
                return false;
            }
            if( choices != null && !ModifyChoices.modifySave( choices, dataEditor, statusWriter ) )
            {
                statusWriter( "Unable to modify extracted save file." );
                return false;
            }
            return true;
        }

        private bool backupAndRepackSave( bool backupSave )
        {
            if( backupSave )
            {
                string backupFile = modifyManager.backupSave();
                if( backupFile == null )
                {
                    statusWriter( "Unable to backup save file." );
                    return false; // To not overwrite an existing copy of the save with the repacked files
                }
                else
                {
                    statusWriter( "Save File backed up to:\t" + backupFile );
                }
            }

            string newSaveFile = modifyManager.repackDirAsSave();   // Assumed to always work...
            statusWriter( "New Save File created:\t" + newSaveFile );
            return newSaveFile != null;
        }

        private void reflectorStopRepackCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            reflectorStopExtractCheckBox.Enabled = reflectorStopRepackCheckBox.Checked;
            reflectorStopExtractCheckBox.Checked = false;   // Never leave it checked when disabled. Elsewhere can't check enabled status before using checked status as it's disabled during operations.
        }

        private void reflectorStopButton_Click( object sender, EventArgs e )
        {
            stopReflector();
        }

        private void stopReflector()
        {
            reflectorStopButton.Enabled = false;
            modifyManager.stopReflector();
        }

        internal void removeReflector()
        {
            // We don't check we're in a state to do any of this, or if it succeeds...
            stopReflector();
            modifyManager.removeReflector();
        }
    }
}
