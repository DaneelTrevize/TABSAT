using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class MainWindow : Form
    {
        private string reflectorDir;
        private string tabDir;
        private TABSAT tabSAT;
        private string decryptDir;
        private string dataFile;

        private static void shiftTextViewRight( TextBox textBox )
        {
            // Set the cursor to the end of the file path, to better see the file name
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        public MainWindow( string savesDirectory )
        {
            InitializeComponent();

            openSaveFileDialog.InitialDirectory = savesDirectory;

            string saveFile = TABSAT.findMostRecentSave( savesDirectory );
            if( saveFile != null )
            {
                openSaveFileDialog.FileName = saveFile; // No need to Path.GetFileName( saveFile ), doesn't help FileDialog only displaying the last ~8.3 chars
                setSaveFile( saveFile );
            }

            reflectorDir = Directory.GetCurrentDirectory();

            tabDir = TABSAT.findTABdirectory();
            /*
            while( tabDir == null )
            {
                // open dialog to choose directory, test for TAB via TABSAT?
            }
            */
            statusTextBox.AppendText( "Reflector directory:\t\t" + reflectorDir + Environment.NewLine );
            statusTextBox.AppendText( "They Are Billions directory:\t" + tabDir + Environment.NewLine );

            tabSAT = new TABSAT( reflectorDir, tabDir, openSaveFileDialog.InitialDirectory, new DataReceivedEventHandler( reflectorOutputHandler ) );
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
                setSaveFile( openSaveFileDialog.FileName );
            }
        }

        private void setSaveFile( string saveFile )
        {
            saveFileTextBox.Text = openSaveFileDialog.FileName;
            shiftTextViewRight( saveFileTextBox );

            // Refactor this path formation into a TABSAT static method?
            decryptDir = Path.ChangeExtension( saveFile, null ) + "_decrypted";
            dataFile = Path.Combine( decryptDir, "Data" );

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
                extractSaveButton.Enabled = decryptDir != null;
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
                modifySaveButton.Enabled = decryptDir != null;
            }
        }

        private void modifySaveButton_Click( object sender, EventArgs e )
        {
            disableChoices();

            if( !extractManualRadioButton.Checked )
            {
                // Firstly extract the save
                if( !extractSave( openSaveFileDialog.FileName ) )
                {
                    resetSaveFileChoice();
                    return;
                }
            }

            // Make modifications
            modifyExtractedSave();

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
                Directory.Delete( decryptDir, true );
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
            string saveFile = openSaveFileDialog.FileName;
            if( saveFile == "" )
            {
                // Error?
                return;
            }

            disableChoices();

            if( !extractSave( saveFile ) )
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
                modifyExtractedSave();

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

        private bool extractSave( string saveFile )
        {
            if( Directory.Exists( decryptDir ) )
            {
                statusTextBox.AppendText( "Unable to extract, directory already exists: " + decryptDir + Environment.NewLine );
                return false;
            }
            else
            {
                extractSaveButton.Enabled = false;
                statusTextBox.AppendText( "Extracted files to:\t\t" + decryptDir + Environment.NewLine );

                tabSAT.decryptSaveToDir( saveFile, decryptDir );
                return true;
            }
        }

        private void modifyExtractedSave()
        {
            SaveEditor dataEditor = new SaveEditor( dataFile );

            try
            {
                if( mutantsRemoveRadio.Checked )
                {
                    statusTextBox.AppendText( "Removing Mutants." + Environment.NewLine );
                    dataEditor.removeMutants();
                }
                if( mutantsMoveRadio.Checked )
                {
                    bool toGiantNotMutant = mutantMoveWhatComboBox.SelectedIndex == 0;
                    statusTextBox.AppendText( "Relocating Mutants to farthest " + (toGiantNotMutant ? "Giant":"Mutant") + '.' + Environment.NewLine );
                    dataEditor.relocateMutants( toGiantNotMutant );
                }
                if( removeFogRadioButton.Checked )
                {
                    statusTextBox.AppendText( "Removing the fog." + Environment.NewLine );
                    dataEditor.removeAllFog();
                }
                if( showFullRadioButton.Checked )
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
                dataEditor.save( dataFile );
            }
            catch (Exception e )
            {
                Console.Error.WriteLine( "Problem modifying save file: " + e.Message + Environment.NewLine + e.StackTrace );
                statusTextBox.AppendText( "Problem modifying save file: " + e.Message + Environment.NewLine );
            }
        }

        private void backupAndRepackSave()
        {
            string saveFile = openSaveFileDialog.FileName;
            if( backupCheckBox.Checked )
            {
                string backupFile = TABSAT.backupSave( saveFile );
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

            string newSaveFile = tabSAT.repackDirAsSave( decryptDir, saveFile );
            statusTextBox.AppendText( "New Save File created:\t" + newSaveFile + Environment.NewLine );

            // Purge the temporary decrypted versions of this new save file
            string decryptedSave = decryptDir + TABReflector.TABReflector.saveExtension;
            string decryptedCheck = decryptDir + TABReflector.TABReflector.checkExtension;
            File.Delete( decryptedSave );
            File.Delete( decryptedCheck );
        }

        private void resetSaveFileChoice()
        {
            saveFileTextBox.Text = "";
            decryptDir = null;
            dataFile = null;

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
