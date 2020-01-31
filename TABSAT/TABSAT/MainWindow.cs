using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class MainWindow : Form
    {
        private TABSAT tabSAT;

        private static void shiftTextViewRight( TextBox textBox )
        {
            // Set the cursor to the end of the file path, to better see the file name
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        public MainWindow( string savesDirectory )
        {
            InitializeComponent();

            string reflectorDir = TABSAT.getReflectorDirectory();

            string tabDir = TABSAT.findTABdirectory();
            /*
            while( tabDir == null )
            {
                // open dialog to choose directory, test for TAB via TABSAT?
            }
            */
            statusTextBox.AppendText( "Reflector directory:\t\t" + reflectorDir + Environment.NewLine );
            statusTextBox.AppendText( "They Are Billions directory:\t" + tabDir + Environment.NewLine );

            openSaveFileDialog.InitialDirectory = savesDirectory;

            tabSAT = new TABSAT( reflectorDir, tabDir, openSaveFileDialog.InitialDirectory, new DataReceivedEventHandler( reflectorOutputHandler ) );

            string saveFile = TABSAT.findMostRecentSave( savesDirectory );
            if( saveFile != null )
            {
                openSaveFileDialog.FileName = saveFile; // No need to Path.GetFileName( saveFile ), doesn't help FileDialog only displaying the last ~8.3 chars
                setSaveFile( saveFile );
            }
        }

        private void reflectorOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
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
            if( openSaveFileDialog.ShowDialog() == DialogResult.OK )
            {
                setSaveFile( openSaveFileDialog.FileName  );
            }
        }

        private void setSaveFile( string saveFile )
        {
            saveFileTextBox.Text = saveFile;
            shiftTextViewRight( saveFileTextBox );

            tabSAT.setSaveFile( saveFile );

            reassessExtractionOption();
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
                statusTextBox.AppendText( "Removed extracted files." + Environment.NewLine );
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
                statusTextBox.AppendText( "Unable to extract save file." + Environment.NewLine );
                return false;
            }
            else
            {
                statusTextBox.AppendText( "Extracted save file:\t\t" + saveFile + Environment.NewLine );
                return true;
            }
        }

        private bool modifyExtractedSave()
        {
            SaveEditor dataEditor = tabSAT.getSaveEditor();

            try
            {
                if( mutantsRemoveRadio.Checked )
                {
                    statusTextBox.AppendText( "Removing Mutants." + Environment.NewLine );
                    dataEditor.removeMutants();
                }
                else if( mutantsMoveRadio.Checked )
                {
                    bool toGiantNotMutant = mutantMoveWhatComboBox.SelectedIndex == 0;
                    bool perDirection = mutantMoveGlobalComboBox.SelectedIndex == 1;
                    statusTextBox.AppendText( "Relocating Mutants to farthest " + (toGiantNotMutant ? "Giant":"Mutant") + (perDirection ? " per Compass quadrant if possible." : " on the map.") + Environment.NewLine );
                    dataEditor.relocateMutants( toGiantNotMutant, perDirection );
                }

                if( removeFogRadioButton.Checked )
                {
                    statusTextBox.AppendText( "Removing all the fog." + Environment.NewLine );
                    dataEditor.removeFog();
                }
                else if( reduceFogRadioButton.Checked )
                {
                    int radius = Convert.ToInt32( fogNumericUpDown.Value );
                    statusTextBox.AppendText( "Removing the fog with cell range: " + radius + Environment.NewLine );
                    dataEditor.removeFog( radius );
                }
                else if( showFullRadioButton.Checked )
                {
                    statusTextBox.AppendText( "Revealing the map." + Environment.NewLine );
                    dataEditor.showFullMap();
                }

                if( themeCheckBox.Checked )
                {
                    KeyValuePair<SaveEditor.ThemeType, string> kv = (KeyValuePair<SaveEditor.ThemeType, string>) themeComboBox.SelectedItem;
                    statusTextBox.AppendText( "Changing Theme to " + kv.Value + '.' + Environment.NewLine );
                    dataEditor.changeTheme( kv.Key );
                }
                dataEditor.save();
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( "Problem modifying save file: " + e.Message + Environment.NewLine + e.StackTrace );
                statusTextBox.AppendText( "Problem modifying save file: " + e.Message + Environment.NewLine );
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
                    statusTextBox.AppendText( "Unable to backup save file." + Environment.NewLine );
                    return;
                }
                else
                {
                    statusTextBox.AppendText( "Save File backed up to:\t" + backupFile + Environment.NewLine );
                }
            }

            string newSaveFile = tabSAT.repackDirAsSave();
            statusTextBox.AppendText( "New Save File created:\t" + newSaveFile + Environment.NewLine );
        }

        private void resetSaveFileChoice()
        {
            saveFileTextBox.Text = "";
            tabSAT.setSaveFile( null );

            enableChoices();

            reassessExtractionOption();
        }

        private void MainWindow_Load( object sender, EventArgs e )
        {
            themeComboBox.DataSource = new BindingSource( SaveEditor.themeTypeNames, null );
            themeComboBox.DisplayMember = "Value";
            themeComboBox.ValueMember = "Key";

            mutantMoveWhatComboBox.SelectedIndex = 0;
            mutantMoveGlobalComboBox.SelectedIndex = 0;
            themeComboBox.SelectedIndex = 3;
        }

        private void MainWindow_FormClosing( object sender, FormClosingEventArgs e )
        {
            // Need to synchronise/mutex access to tabSAT?
            tabSAT.stopReflector();
            tabSAT.removeReflector();
        }
    }
}
