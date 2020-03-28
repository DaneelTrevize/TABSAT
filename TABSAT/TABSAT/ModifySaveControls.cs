using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class ModifySaveControls : UserControl
    {
        private readonly ModifyManager modifyManager;
        private readonly StatusWriterDelegate statusWriter;
        private readonly List<CheckBox> ccExtrasCheckBoxes;
        private readonly List<CheckBox> warehousesFillCheckBoxes;
        private readonly List<CheckBox> generalCheckBoxes;

        private static bool anyChecked( IList<CheckBox> boxes )
        {
            foreach( var c in boxes )
            {
                if( c.Checked )
                {
                    return true;
                }
            }
            return false;
        }

        public ModifySaveControls( ModifyManager m, StatusWriterDelegate sW, string savesDirectory )
        {
            InitializeComponent();

            modifyManager = m;
            statusWriter = sW;
            ccExtrasCheckBoxes = new List<CheckBox>( 3 );
            ccExtrasCheckBoxes.Add( ccExtraFoodCheckBox );
            ccExtrasCheckBoxes.Add( ccExtraEnergyCheckBox );
            ccExtrasCheckBoxes.Add( ccExtraWorkersCheckBox );
            warehousesFillCheckBoxes = new List<CheckBox>( 5 );
            warehousesFillCheckBoxes.Add( warehousesFillWoodCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillStoneCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillIronCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillOilCheckBox );
            warehousesFillCheckBoxes.Add( warehousesFillGoldCheckBox );
            generalCheckBoxes = new List<CheckBox>( 3 );
            generalCheckBoxes.Add( themeCheckBox );
            generalCheckBoxes.Add( swarmsCheckBox );
            generalCheckBoxes.Add( disableMayorsCheckBox );

            saveOpenFileDialog.Filter = "TAB Save Files|" + TAB.SAVES_FILTER;// + "|Data files|*.dat";
            saveOpenFileDialog.InitialDirectory = savesDirectory;

            vodReplaceComboBox.DataSource = new BindingSource( SaveEditor.vodSizesNames, null );
            vodReplaceComboBox.DisplayMember = "Value";
            vodReplaceComboBox.ValueMember = "Key";

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
            giftComboBox.SelectedIndex = 1;
            themeComboBox.SelectedIndex = 3;

            // Register these following event handlers after the above 'default' index choices, to avoid handling non-user events.

            var comboHandler = new EventHandler( comboBox_SelectedIndexChanged );
            var numericHandler = new EventHandler( numericUpDown_ValueChanged );

            mutantReplaceAllComboBox.SelectedIndexChanged += comboHandler;
            mutantMoveGlobalComboBox.SelectedIndexChanged += comboHandler;
            mutantMoveWhatComboBox.SelectedIndexChanged += comboHandler;

            fogNumericUpDown.ValueChanged += numericHandler;

            vodReplaceComboBox.SelectedIndexChanged += comboHandler;
            vodStackNumericUpDown.ValueChanged += numericHandler;

            ccFoodNumericUpDown.ValueChanged += numericHandler;
            ccEnergyNumericUpDown.ValueChanged += numericHandler;
            ccWorkersNumericUpDown.ValueChanged += numericHandler;

            giftComboBox.SelectedIndexChanged += comboHandler;
            giftNumericUpDown.ValueChanged += numericHandler;

            themeComboBox.SelectedIndexChanged += comboHandler;
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
                    reflectorTextBox.AppendText( outLine.Data + Environment.NewLine );
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

            modifyManager.setSaveFile( saveFile );

            reassessExtractionOption();
        }

        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            var box = (ComboBox) sender;

            if( box == mutantReplaceAllComboBox )
            {
                mutantReplaceAllRadio.Checked = true;
            }
            else if( box == mutantMoveWhatComboBox || box == mutantMoveGlobalComboBox )
            {
                mutantsMoveRadio.Checked = true;
            }
            else if( box == vodReplaceComboBox )
            {
                vodReplaceRadioButton.Checked = true;
            }
            else if( box == giftComboBox )
            {
                ccGiftCheckBox.Checked = true;
            }
            else if( box == themeComboBox )
            {
                themeCheckBox.Checked = true;
            }
        }

        private void numericUpDown_ValueChanged( object sender, EventArgs e )
        {
            var num = (NumericUpDown) sender;

            if( num == fogNumericUpDown )
            {
                fogClearRadioButton.Checked = true;
            }
            else if( num == vodStackNumericUpDown )
            {
                vodStackRadioButton.Checked = true;
            }
            else if( num == ccFoodNumericUpDown )
            {
                ccExtraFoodCheckBox.Checked = true;
            }
            else if( num == ccEnergyNumericUpDown )
            {
                ccExtraEnergyCheckBox.Checked = true;
            }
            else if( num == ccWorkersNumericUpDown )
            {
                ccExtraWorkersCheckBox.Checked = true;
            }
            else if( num == giftNumericUpDown )
            {
                ccGiftCheckBox.Checked = true;
            }
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

        private void disableChoices()
        {
            // Don't let different options be chosen during file operations
            mutantGroupBox.Enabled = false;
            vodGroupBox.Enabled = false;
            fogGroupBox.Enabled = false;
            ccExtraGroupBox.Enabled = false;
            warehousesGroupBox.Enabled = false;
            generalGroupBox.Enabled = false;
            saveFileGroupBox.Enabled = false;
            modifyGroupBox.Enabled = false;
            //manualGroupBox.Enabled = false;    // Don't disable this for when we're doing a multi-button-click-required manual cycle
            extractGroupBox.Enabled = false;
            reflectorGroupBox.Enabled = false;
        }

        private void enableChoices()
        {
            mutantGroupBox.Enabled = true;
            vodGroupBox.Enabled = true;
            fogGroupBox.Enabled = true;
            ccExtraGroupBox.Enabled = true;
            warehousesGroupBox.Enabled = true;
            generalGroupBox.Enabled = true;
            saveFileGroupBox.Enabled = true;
            modifyGroupBox.Enabled = true;
            //manualGroupBox.Enabled = true;
            extractGroupBox.Enabled = true;
            reflectorGroupBox.Enabled = true;
        }

        private void modifySaveButton_Click( object sender, EventArgs e )
        {
            disableChoices();

            modifySaveBackgroundWorker = new BackgroundWorker();
            modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( modifySave_DoWork );
            modifySaveBackgroundWorker.RunWorkerAsync();
        }

        private void modifySave_DoWork( object sender, DoWorkEventArgs e )
        {
            if( !extractManualRadioButton.Checked )
            {
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

        private void extractSaveButton_Click( object sender, EventArgs e )
        {
            if( !modifyManager.hasSaveFile() )
            {
                // Error?
                return;
            }

            disableChoices();

            modifySaveBackgroundWorker = new BackgroundWorker();
            modifySaveBackgroundWorker.DoWork += new DoWorkEventHandler( extractSave_DoWork );
            modifySaveBackgroundWorker.RunWorkerAsync();
        }

        private void extractSave_DoWork( object sender, DoWorkEventArgs e )
        {
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
            if( mutantsNothingRadio.Checked && vodLeaveRadioButton.Checked && fogLeaveRadioButton.Checked && !anyChecked( ccExtrasCheckBoxes ) && !ccGiftCheckBox.Checked && !anyChecked( warehousesFillCheckBoxes ) && !anyChecked( generalCheckBoxes ) )
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

                // VODs
                if( vodRemoveRadioButton.Checked )
                {
                    statusWriter( "Removing VODs." );
                    dataEditor.removeVODs();
                }
                else if( vodReplaceRadioButton.Checked )
                {
                    KeyValuePair<SaveEditor.VodSizes, string> kv = (KeyValuePair<SaveEditor.VodSizes, string>) vodReplaceComboBox.SelectedItem;
                    statusWriter( "Replacing all VOD buildings with " + kv.Value + '.' );
                    dataEditor.resizeVODs( kv.Key );
                }
                else if( vodStackRadioButton.Checked )
                {
                    statusWriter( "Would stack VODs." );
                }

                // Fog of War
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

                // Command Center Extras
                if( anyChecked( ccExtrasCheckBoxes ) )
                {
                    statusWriter( "Altering Command Center extra supplies." );
                    dataEditor.setExtraSupply( ccExtraFoodCheckBox.Checked ? (int) ccFoodNumericUpDown.Value : 0, ccExtraEnergyCheckBox.Checked ? (int) ccEnergyNumericUpDown.Value : 0, ccExtraWorkersCheckBox.Checked ? (int) ccWorkersNumericUpDown.Value : 0 );
                }

                if( ccGiftCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.GiftableTypes, string> kv = (KeyValuePair<SaveEditor.GiftableTypes, string>) giftComboBox.SelectedItem;
                    int typeID = Convert.ToInt32( giftNumericUpDown.Value );
                    statusWriter( "Gifting " + typeID + "x " + kv.Value + "." );
                    dataEditor.giftEntities( kv.Key, typeID );
                }

                // Fill Resource Storage
                if( anyChecked( warehousesFillCheckBoxes ) )
                {
                    statusWriter( "Filling storage for specified resources." );
                    dataEditor.fillStorage( warehousesFillGoldCheckBox.Checked, warehousesFillWoodCheckBox.Checked, warehousesFillStoneCheckBox.Checked, warehousesFillIronCheckBox.Checked, warehousesFillOilCheckBox.Checked );
                }

                // General Rules
                if( themeCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.ThemeType, string> kv = (KeyValuePair<SaveEditor.ThemeType, string>) themeComboBox.SelectedItem;
                    statusWriter( "Changing Theme to " + kv.Value + '.' );
                    dataEditor.changeTheme( kv.Key );
                }
                if( swarmsCheckBox.Checked )
                {
                    statusWriter( "Enabling earlier waves to come from 2 directions, and later waves to come from 3." );
                    dataEditor.splitSwarms();
                }
                if( disableMayorsCheckBox.Checked )
                {
                    statusWriter( "Disabling Mayors." );
                    dataEditor.disableMayors();
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
