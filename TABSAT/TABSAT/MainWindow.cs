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
        }

        private TABSAT getTABSAT()
        {
            if( tabSAT == null )
            {
                tabSAT = new TABSAT( reflectorDir, tabDir, openSaveFileDialog.InitialDirectory, new DataReceivedEventHandler(reflectorOutputHandler) );
            }
            return tabSAT;
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
                saveFileTextBox.Text = openSaveFileDialog.FileName;
                shiftTextViewRight( saveFileTextBox );

                // Refactor this path formation into a TABSAT static method?
                decryptDir = Path.ChangeExtension( openSaveFileDialog.FileName, null ) + "_decrypted";
                dataFile = Path.Combine( decryptDir, "Data" );

                reassessExtractionOption();
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
                //reflectorExtractRadioButton.Enabled = true;
                extractSaveButton.Enabled = decryptDir != null;
            }
            else
            {
                modifyGroupBox.Enabled = true;
                manualGroupBox.Enabled = false;
                //reflectorExtractRadioButton.Enabled = false;
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

            // Assume this is single use only for now
            tabSAT.tidyUp();
            tabSAT = null;

            if( extractTidyRadioButton.Checked )
            {
                // Remove the extracted files...
                statusTextBox.AppendText( "Should now remove the extracted files..." + Environment.NewLine );
            }
        }

        private void disableChoices()
        {
            // Don't let different options be chosen during file operations
            mutantGroupBox.Enabled = false;
            terrainGroupBox.Enabled = false;
            saveFileGroupBox.Enabled = false;
            extractGroupBox.Enabled = false;
            modifyGroupBox.Enabled = false;
        }

        private void enableChoices()
        {
            mutantGroupBox.Enabled = true;
            terrainGroupBox.Enabled = true;
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
                return;
            }
            else
            {
                modifyExtractedSave();

                repackSaveButton.Enabled = true;
                skipRepackButton.Enabled = true;
            }
        }

        private void repackSaveButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;
            skipRepackButton.Enabled = false;

            backupAndRepackSave();

            // Assume this is single use only for now
            tabSAT.tidyUp();
            tabSAT = null;
        }

        private void skipRepackButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;
            skipRepackButton.Enabled = false;

            resetSaveFileChoice();

            // Assume this is single use only for now
            tabSAT.tidyUp();
            tabSAT = null;
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

                getTABSAT().decryptSaveToDir( saveFile, decryptDir );
                return true;
            }
        }

        private void modifyExtractedSave()
        {
            SaveEditor dataEditor = new SaveEditor( dataFile );
            if( mutantsMoveRadio.Checked )
            {
                statusTextBox.AppendText( "Relocating Mutants." + Environment.NewLine );
                dataEditor.relocateMutants();
            }
            if( themeCheckBox.Checked )
            {
                KeyValuePair<SaveEditor.ThemeType, string> kv = (KeyValuePair<SaveEditor.ThemeType, string>) themeComboBox.SelectedItem;
                statusTextBox.AppendText( "Changing Theme to " + kv.Value + '.' + Environment.NewLine );
                dataEditor.changeTheme( kv.Key );
            }
            dataEditor.save( dataFile );
        }

        private void backupAndRepackSave()
        {
            string saveFile = openSaveFileDialog.FileName;
            string backupFile = TABSAT.backupSave( saveFile );
            if( backupFile == null )
            {
                statusTextBox.AppendText( "Unable to backup save file." + Environment.NewLine );
            }
            else
            {
                statusTextBox.AppendText( "Save File backed up to:\t" + backupFile + Environment.NewLine );

                string newSaveFile = getTABSAT().repackDirAsSave( decryptDir, saveFile );
                statusTextBox.AppendText( "New Save File created:\t" + newSaveFile + Environment.NewLine );

                // Purge the temporary decrypted versions of this new save file
                string decryptedSave = decryptDir + TABReflector.TABReflector.saveExtension;
                string decryptedCheck = decryptDir + TABReflector.TABReflector.checkExtension;
                File.Delete( decryptedSave );
                File.Delete( decryptedCheck );
            }

            resetSaveFileChoice();
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
            if( tabSAT != null )
            {
                tabSAT.tidyUp();
                tabSAT = null;
            }
        }
    }
}
