using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using static TABSAT.MainWindow;

namespace TABSAT
{
    internal partial class ModifySaveControls : UserControl
    {
        private TABSAT tabSAT;
        private StatusWriterDelegate statusWriter;

        public ModifySaveControls( TABSAT t, StatusWriterDelegate sW, string savesDirectory )
        {
            InitializeComponent();

            tabSAT = t;
            statusWriter = sW;

            saveOpenFileDialog.Filter = "TAB Save Files|*" + TABReflector.TABReflector.saveExtension;// + "|Data files|*.dat";
            saveOpenFileDialog.InitialDirectory = savesDirectory;

            themeComboBox.DataSource = new BindingSource( SaveEditor.themeTypeNames, null );
            themeComboBox.DisplayMember = "Value";
            themeComboBox.ValueMember = "Key";

            mutantMoveWhatComboBox.SelectedIndex = 0;
            mutantMoveGlobalComboBox.SelectedIndex = 0;
            themeComboBox.SelectedIndex = 3;
            // Register these event handlers after the above 'default' choices, to avoid handling non-user events.
            var mutantChoicesHandler = new System.EventHandler( mutantMoveEitherComboBox_SelectedIndexChanged );
            mutantMoveGlobalComboBox.SelectedIndexChanged += mutantChoicesHandler;
            mutantMoveWhatComboBox.SelectedIndexChanged += mutantChoicesHandler;
            themeComboBox.SelectedIndexChanged += new System.EventHandler( themeComboBox_SelectedIndexChanged );

            reduceFogRadioButton.CheckedChanged += new System.EventHandler( fogReduceRadio_CheckedChanged );
            var fogNotReduceHandler = new System.EventHandler( fogNotReduceRadio_CheckedChanged );
            leaveFogRadioButton.CheckedChanged += fogNotReduceHandler;
            removeFogRadioButton.CheckedChanged += fogNotReduceHandler;
            showFullRadioButton.CheckedChanged += fogNotReduceHandler;

            string saveFile = TABSAT.findMostRecentSave( savesDirectory );
            if( saveFile != null )
            {
                saveOpenFileDialog.FileName = saveFile; // No need to Path.GetFileName( saveFile ), doesn't help FileDialog only displaying the last ~8.3 chars
                setSaveFile( saveFile );
            }
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


        private void saveFileChooseButton_Click( object sender, EventArgs e )
        {
            if( saveOpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                setSaveFile( saveOpenFileDialog.FileName );
            }
        }

        private void setSaveFile( string saveFile )
        {
            saveFileTextBox.Text = saveFile;
            MainWindow.shiftTextViewRight( saveFileTextBox );

            tabSAT.setSaveFile( saveFile );

            reassessExtractionOption();
        }

        private void themeComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            themeCheckBox.Checked = true;
        }

        private void mutantsNotMoveRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                mutantsMoveRadio.Checked = false;
            }
        }

        private void mutantsMoveRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
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
            reduceFogRadioButton.Checked = true;
        }

        private void fogNotReduceRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                reduceFogRadioButton.Checked = false;
            }
        }

        private void fogReduceRadio_CheckedChanged( object sender, EventArgs e )
        {
            if( ( (RadioButton) sender ).Checked )
            {
                leaveFogRadioButton.Checked = false;
                removeFogRadioButton.Checked = false;
                showFullRadioButton.Checked = false;
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
                reflectorExtractRadioButton.Enabled = true;
                extractSaveButton.Enabled = tabSAT.hasSaveFile();
            }
            else
            {
                modifyGroupBox.Enabled = true;
                manualGroupBox.Enabled = false;
                reflectorExtractRadioButton.Enabled = false;
                if( reflectorExtractRadioButton.Checked )
                {
                    reflectorRepackRadioButton.Checked = true;
                }
                modifySaveButton.Enabled = tabSAT.hasSaveFile();
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
                        tabSAT.stopReflector();
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
                    tabSAT.stopReflector();
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
                tabSAT.stopReflector();
            }

            if( extractTidyRadioButton.Checked )
            {
                // Remove the extracted files
                tabSAT.removeDecryptedDir();
                statusWriter( "Removed extracted files." );
            }

            resetSaveFileChoice();
        }

        private void disableChoices()
        {
            // Don't let different options be chosen during file operations
            mutantGroupBox.Enabled = false;
            fogGroupBox.Enabled = false;
            themeGroupBox.Enabled = false;
            saveFileGroupBox.Enabled = false;
            extractGroupBox.Enabled = false;
            modifyGroupBox.Enabled = false;
        }

        private void enableChoices()
        {
            mutantGroupBox.Enabled = true;
            fogGroupBox.Enabled = true;
            themeGroupBox.Enabled = true;
            saveFileGroupBox.Enabled = true;
            extractGroupBox.Enabled = true;
            modifyGroupBox.Enabled = true;
        }

        private void extractSaveButton_Click( object sender, EventArgs e )
        {
            if( !tabSAT.hasSaveFile() )
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
                    tabSAT.stopReflector();
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
                tabSAT.stopReflector();
            }
        }

        private void repackSaveButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;
            skipRepackButton.Enabled = false;

            backupAndRepackSave();

            if( reflectorRepackRadioButton.Checked )
            {
                tabSAT.stopReflector();
            }

            resetSaveFileChoice();
        }

        private void skipRepackButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;
            skipRepackButton.Enabled = false;

            if( reflectorRepackRadioButton.Checked )
            {
                tabSAT.stopReflector();
            }

            resetSaveFileChoice();
        }

        private bool extractSave()
        {
            extractSaveButton.Enabled = false;  // Should live in extractSaveButton_Click(), thus not be triggered by modifySaveButton_Click()?

            string saveFile = tabSAT.extractSave();
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
            if( !mutantsRemoveRadio.Checked && !mutantsMoveRadio.Checked && !showFullRadioButton.Checked && !removeFogRadioButton.Checked && !reduceFogRadioButton.Checked && !themeCheckBox.Checked )
            {
                // Nothing to do
                return true;
            }

            SaveEditor dataEditor = tabSAT.getSaveEditor();

            try
            {
                if( mutantsRemoveRadio.Checked )
                {
                    statusWriter( "Removing Mutants." );
                    dataEditor.removeMutants();
                }
                else if( mutantsMoveRadio.Checked )
                {
                    bool toGiantNotMutant = mutantMoveWhatComboBox.SelectedIndex == 0;
                    bool perDirection = mutantMoveGlobalComboBox.SelectedIndex == 1;
                    statusWriter( "Relocating Mutants to farthest " + ( toGiantNotMutant ? "Giant" : "Mutant" ) + ( perDirection ? " per Compass quadrant if possible." : " on the map." ) );
                    dataEditor.relocateMutants( toGiantNotMutant, perDirection );
                }

                if( removeFogRadioButton.Checked )
                {
                    statusWriter( "Removing all the fog." );
                    dataEditor.removeFog();
                }
                else if( reduceFogRadioButton.Checked )
                {
                    int radius = Convert.ToInt32( fogNumericUpDown.Value );
                    statusWriter( "Removing the fog with cell range: " + radius );
                    dataEditor.removeFog( radius );
                }
                else if( showFullRadioButton.Checked )
                {
                    statusWriter( "Revealing the map." );
                    dataEditor.showFullMap();
                }

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
                string backupFile = tabSAT.backupSave();
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

            string newSaveFile = tabSAT.repackDirAsSave();
            statusWriter( "New Save File created:\t" + newSaveFile );
        }

        private void resetSaveFileChoice()
        {
            saveFileTextBox.Text = "";
            tabSAT.setSaveFile( null );

            enableChoices();

            reassessExtractionOption();
        }

        internal void removeReflector()
        {
            // Need to synchronise/mutex access to tabSAT?
            tabSAT.stopReflector();
            tabSAT.removeReflector();
        }
    }
}
