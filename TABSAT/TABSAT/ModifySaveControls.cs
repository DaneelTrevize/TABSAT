using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class ModifySaveControls : UserControl
    {
        private readonly ModifyManager modifyManager;
        private readonly StatusWriterDelegate statusWriter;

        public ModifySaveControls( ModifyManager m, StatusWriterDelegate sW, string savesDirectory )
        {
            InitializeComponent();

            modifyManager = m;
            statusWriter = sW;

            saveOpenFileDialog.Filter = "TAB Save Files|*" + TAB.SAVE_EXTENSION;// + "|Data files|*.dat";
            saveOpenFileDialog.InitialDirectory = savesDirectory;

            giftComboBox.DataSource = new BindingSource( SaveEditor.giftableTypeNames, null );
            giftComboBox.DisplayMember = "Value";
            giftComboBox.ValueMember = "Key";

            themeComboBox.DataSource = new BindingSource( SaveEditor.themeTypeNames, null );
            themeComboBox.DisplayMember = "Value";
            themeComboBox.ValueMember = "Key";

            mutantReplaceAllComboBox.SelectedIndex = 0;
            mutantMoveWhatComboBox.SelectedIndex = 0;
            mutantMoveGlobalComboBox.SelectedIndex = 0;
            vodReplaceComboBox.SelectedIndex = 0;
            giftComboBox.SelectedIndex = 3;
            themeComboBox.SelectedIndex = 3;
            // Register these following event handlers after the above 'default' index choices, to avoid handling non-user events.

            mutantReplaceAllComboBox.SelectedIndexChanged += new EventHandler( mutantReplaceAllComboBox_SelectedIndexChanged );
            var mutantChoicesHandler = new EventHandler( mutantMoveEitherComboBox_SelectedIndexChanged );
            mutantMoveGlobalComboBox.SelectedIndexChanged += mutantChoicesHandler;
            mutantMoveWhatComboBox.SelectedIndexChanged += mutantChoicesHandler;

            fogClearRadioButton.CheckedChanged += new EventHandler( fogReduceRadio_CheckedChanged );
            var fogNotReduceHandler = new EventHandler( fogNotReduceRadio_CheckedChanged );
            fogLeaveRadioButton.CheckedChanged += fogNotReduceHandler;
            fogRemoveRadioButton.CheckedChanged += fogNotReduceHandler;
            fogShowFullRadioButton.CheckedChanged += fogNotReduceHandler;

            var vodNotPanelHandler = new EventHandler( vodNotPanelRadioButton_CheckedChanged );
            vodLeaveRadioButton.CheckedChanged += vodNotPanelHandler;
            vodRemoveRadioButton.CheckedChanged += vodNotPanelHandler;
            vodReplaceRadioButton.CheckedChanged += new EventHandler( vodReplaceRadio_CheckedChanged );
            vodReplaceComboBox.SelectedIndexChanged += new EventHandler( vodReplaceComboBox_SelectedIndexChanged );

            var mayorsNotGiftChoicesHandler = new EventHandler( mayorsNotGiftRadioButton_CheckedChanged );
            mayorsLeaveRadioButton.CheckedChanged += mayorsNotGiftChoicesHandler;
            mayorsDisableRadioButton.CheckedChanged += mayorsNotGiftChoicesHandler;
            giftComboBox.SelectedIndexChanged += new EventHandler( giftComboBox_SelectedIndexChanged );

            themeComboBox.SelectedIndexChanged += new EventHandler( themeComboBox_SelectedIndexChanged );
        }

        internal void reflectorOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            /*if( reflectorTextBox.InvokeRequired )     // Waiting for the right thread stalls the realtime displaying of Reflector process output
            {
                reflectorTextBox.BeginInvoke( new DataReceivedEventHandler( reflectorOutputHandler ), new[] { sendingProcess, outLine } );
            }
            else
            {*/
            if( !String.IsNullOrEmpty( outLine.Data ) )
            {
                reflectorTextBox.AppendText( outLine.Data + Environment.NewLine );
            }
            //}
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

        private void saveFileChooseButton_Click( object sender, EventArgs e )
        {
            if( saveOpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                string file = saveOpenFileDialog.FileName;
                if( TAB.IsFileWithinDirectory( file, BackupsManager.DEFAULT_BACKUP_DIRECTORY ) )    // Doesn't use a dynamic value for the current backups directory, from the other tab's BackupManager...
                {
                    // Editing a backup will not trigger a checksum update once the modified file is repacked, confuses AutoBackup UI
                    statusWriter( "Please do not modify files within the backups directory: " + file );
                }
                else
                {
                    setSaveFile( file );
                }
            }
        }

        private void setSaveFile( string saveFile )
        {
            saveFileTextBox.Text = saveFile;
            MainWindow.shiftTextViewRight( saveFileTextBox );

            modifyManager.setSaveFile( saveFile );

            reassessExtractionOption();
        }

        private void mutantsSimpleRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mutantReplaceAllRadio.Checked = false;
                mutantsMoveRadio.Checked = false;
            }
        }

        private void mutantReplaceAllComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            mutantReplaceAllRadio.Checked = true;
        }

        private void mutantReplaceAllRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mutantsNothingRadio.Checked = false;
                mutantsRemoveRadio.Checked = false;
                mutantsMoveRadio.Checked = false;
            }
        }

        private void mutantsMoveRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mutantReplaceAllRadio.Checked = false;
                mutantsNothingRadio.Checked = false;
                mutantsRemoveRadio.Checked = false;
            }
        }

        private void mutantMoveEitherComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            mutantsMoveRadio.Checked = true;
        }

        private void fogNumericUpDown_ValueChanged( object sender, EventArgs e )
        {
            fogClearRadioButton.Checked = true;
        }

        private void fogNotReduceRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                fogClearRadioButton.Checked = false;
            }
        }

        private void fogReduceRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                fogLeaveRadioButton.Checked = false;
                fogRemoveRadioButton.Checked = false;
                fogShowFullRadioButton.Checked = false;
            }
        }

        private void vodNotPanelRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                vodReplaceRadioButton.Checked = false;
                vodStackRadioButton.Checked = false;
            }
        }

        private void vodReplaceRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                vodLeaveRadioButton.Checked = false;
                vodRemoveRadioButton.Checked = false;
                vodStackRadioButton.Checked = false;
            }
        }

        private void vodReplaceComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            vodReplaceRadioButton.Checked = true;
        }

        private void vodStackNumericUpDown_ValueChanged( object sender, EventArgs e )
        {
            vodStackRadioButton.Checked = true;
        }

        private void vodStackRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                vodLeaveRadioButton.Checked = false;
                vodRemoveRadioButton.Checked = false;
                vodReplaceRadioButton.Checked = false;
            }
        }

        private void mayorsGiftRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mayorsLeaveRadioButton.Checked = false;
                mayorsDisableRadioButton.Checked = false;
            }
        }

        private void mayorsNotGiftRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mayorsGiftRadioButton.Checked = false;
            }
        }

        private void giftComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            mayorsGiftRadioButton.Checked = true;
        }

        private void giftNumericUpDown_ValueChanged( object sender, EventArgs e )
        {
            mayorsGiftRadioButton.Checked = true;
        }

        private void themeComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            themeCheckBox.Checked = true;
        }

        private void extractRadioButtons_CheckedChanged( object sender, EventArgs e )
        {
            // Do stuff only if the radio button is checked (or the action will run twice).
            if( ( (RadioButton) sender ).Checked )
            {
                reassessExtractionOption();
            }
        }

        private void reassessExtractionOption()
        {
            if( extractManualRadioButton.Checked )
            {
                modifyGroupBox.Enabled = false;
                manualGroupBox.Enabled = true;
                extractSaveButton.Enabled = modifyManager.hasSaveFile();
                reflectorExtractRadioButton.Enabled = true;
            }
            else
            {
                modifyGroupBox.Enabled = true;
                modifySaveButton.Enabled = modifyManager.hasSaveFile();
                manualGroupBox.Enabled = false;
                reflectorExtractRadioButton.Enabled = false;
                if( reflectorExtractRadioButton.Checked )
                {
                    reflectorRepackRadioButton.Checked = true;
                }
            }
        }

        private void modifySaveButton_Click( object sender, EventArgs e )
        {
            disableChoices();

            if( !extractManualRadioButton.Checked )
            {
                //tabSAT.hasSaveFile(); // When could this _Click be called if we didn't already have a saveFile set?
                //tabSAT.setSaveFile( openSaveFileDialog.FileName );
                // Firstly extract the save
                if( !extractSave() )
                {
                    if( reflectorRepackRadioButton.Checked )    // Refactor how this _Click handles potentially failing in 2+ places and trying to stop the Reflector in 3...
                    {
                        modifyManager.stopReflector();
                    }

                    resetSaveFileChoice();
                    return;
                }
            }

            // Make modifications
            if( !modifyExtractedSave() )
            {
                if( reflectorRepackRadioButton.Checked )
                {
                    modifyManager.stopReflector();
                }

                resetSaveFileChoice();
                return;
            }

            if( !extractManualRadioButton.Checked )
            {
                // Backup & Repack the save
                backupAndRepackSave();
            }

            if( reflectorRepackRadioButton.Checked )
            {
                modifyManager.stopReflector();
            }

            if( extractTidyRadioButton.Checked )
            {
                // Remove the extracted files
                modifyManager.removeDecryptedDir();
                statusWriter( "Removed extracted files." );
            }

            resetSaveFileChoice();
        }

        private void disableChoices()
        {
            // Don't let different options be chosen during file operations
            mutantGroupBox.Enabled = false;
            fogGroupBox.Enabled = false;
            vodGroupBox.Enabled = false;
            mayorsBonusesGroupBox.Enabled = false;
            themeGroupBox.Enabled = false;
            saveFileGroupBox.Enabled = false;
            modifyGroupBox.Enabled = false;
            //manualGroupBox.Enabled = false;    // Don't disable this for when we're doing a multi-button-click-required manual cycle
            extractGroupBox.Enabled = false;
            reflectorGroupBox.Enabled = false;
        }

        private void enableChoices()
        {
            mutantGroupBox.Enabled = true;
            fogGroupBox.Enabled = true;
            vodGroupBox.Enabled = true;
            mayorsBonusesGroupBox.Enabled = true;
            themeGroupBox.Enabled = true;
            saveFileGroupBox.Enabled = true;
            modifyGroupBox.Enabled = true;
            //manualGroupBox.Enabled = true;
            extractGroupBox.Enabled = true;
            reflectorGroupBox.Enabled = true;
        }

        private void extractSaveButton_Click( object sender, EventArgs e )
        {
            if( !modifyManager.hasSaveFile() )
            {
                // Error?
                return;
            }

            disableChoices();

            if( !extractSave() )
            {
                resetSaveFileChoice();

                if( reflectorExtractRadioButton.Checked )
                {
                    modifyManager.stopReflector();
                }
                return;
            }
            else
            {
                modifyExtractedSave();  // Unchecked

                repackSaveButton.Enabled = true;
                skipRepackButton.Enabled = true;
            }

            if( reflectorExtractRadioButton.Checked )
            {
                modifyManager.stopReflector();
            }
        }

        private void repackSaveButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;
            skipRepackButton.Enabled = false;

            backupAndRepackSave();

            if( reflectorRepackRadioButton.Checked )
            {
                modifyManager.stopReflector();
            }

            resetSaveFileChoice();
        }

        private void skipRepackButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;
            skipRepackButton.Enabled = false;

            if( reflectorRepackRadioButton.Checked )
            {
                modifyManager.stopReflector();
            }

            resetSaveFileChoice();
        }

        private bool extractSave()
        {
            extractSaveButton.Enabled = false;  // Should live in extractSaveButton_Click(), thus not be triggered by modifySaveButton_Click()?

            string saveFile = modifyManager.extractSave( extractTidyRadioButton.Checked );
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
            if( mutantsNothingRadio.Checked && fogLeaveRadioButton.Checked && vodLeaveRadioButton.Checked && mayorsLeaveRadioButton.Checked && !themeCheckBox.Checked )
            {
                statusWriter( "No modifications chosen." );
                return true;
            }

            SaveEditor dataEditor = modifyManager.getSaveEditor();

            try
            {
                // Mutants
                if( mutantsRemoveRadio.Checked )
                {
                    statusWriter( "Removing Mutants." );
                    dataEditor.removeMutants();
                }
                else if( mutantReplaceAllRadio.Checked )
                {
                    bool toGiantNotMutant = mutantReplaceAllComboBox.SelectedIndex == 0;
                    statusWriter( "Replacing all " + ( toGiantNotMutant ? "Mutants with Giants." : "Giants with Mutants." ) );
                    dataEditor.replaceHugeZombies( toGiantNotMutant );
                }
                else if( mutantsMoveRadio.Checked )
                {
                    bool toGiantNotMutant = mutantMoveWhatComboBox.SelectedIndex == 0;
                    bool perDirection = mutantMoveGlobalComboBox.SelectedIndex == 1;
                    statusWriter( "Relocating Mutants to farthest " + ( toGiantNotMutant ? "Giant" : "Mutant" ) + ( perDirection ? " per Compass quadrant if possible." : " on the map." ) );
                    dataEditor.relocateMutants( toGiantNotMutant, perDirection );
                }

                // Fog
                if( fogRemoveRadioButton.Checked )
                {
                    statusWriter( "Removing all the fog." );
                    dataEditor.removeFog();
                }
                else if( fogClearRadioButton.Checked )
                {
                    int radius = Convert.ToInt32( fogNumericUpDown.Value );
                    statusWriter( "Removing the fog with cell range: " + radius );
                    dataEditor.removeFog( radius );
                }
                else if( fogShowFullRadioButton.Checked )
                {
                    statusWriter( "Revealing the map." );
                    dataEditor.showFullMap();
                }

                // VODs
                if( vodRemoveRadioButton.Checked )
                {
                    statusWriter( "Removing VODs." );
                    dataEditor.removeVODs();
                }
                else if( vodReplaceRadioButton.Checked )
                {
                    statusWriter( "Replacing VOD buildings." );
                    dataEditor.resizeVODs( vodReplaceComboBox.SelectedIndex == 1 );
                }
                else if( vodStackRadioButton.Checked )
                {
                    statusWriter( "Would stack VODs." );
                }

                // Mayors
                if( mayorsDisableRadioButton.Checked )
                {
                    statusWriter( "Disabling Mayors." );
                    dataEditor.disableMayors();
                }
                else if( mayorsGiftRadioButton.Checked )
                {
                    KeyValuePair<SaveEditor.GiftableTypes, string> kv = (KeyValuePair<SaveEditor.GiftableTypes, string>) giftComboBox.SelectedItem;
                    int typeID = Convert.ToInt32( giftNumericUpDown.Value );
                    statusWriter( "Gifting " + typeID + "x " + kv.Value + "." );
                    dataEditor.giftEntities( kv.Key, typeID );
                }

                // Theme
                if( themeCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.ThemeType, string> kv = (KeyValuePair<SaveEditor.ThemeType, string>) themeComboBox.SelectedItem;
                    statusWriter( "Changing Theme to " + kv.Value + '.' );
                    dataEditor.changeTheme( kv.Key );
                }
                dataEditor.save();
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( "Problem modifying save file: " + e.Message + Environment.NewLine + e.StackTrace );
                statusWriter( "Problem modifying save file: " + e.Message );
                return false;
            }
            return true;
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
            saveFileTextBox.Text = "";
            modifyManager.setSaveFile( null );

            reassessExtractionOption();

            enableChoices();
        }

        internal void removeReflector()
        {
            // Need to synchronise/mutex access to tabSAT?
            modifyManager.stopReflector();
            modifyManager.removeReflector();
        }
    }
}
