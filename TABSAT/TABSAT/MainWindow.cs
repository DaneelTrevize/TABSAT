using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class MainWindow : Form
    {

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

            reflectorDirTextBox.Text = Directory.GetCurrentDirectory();

            tabDirTextBox.Text = TABSAT.findTABdirectory();
        }

        private TABSAT getTABSAT()
        {
            if( tabSAT == null )
            {
                tabSAT = new TABSAT( reflectorDirTextBox.Text, tabDirTextBox.Text, openSaveFileDialog.InitialDirectory, new DataReceivedEventHandler(reflectorOutputHandler) );
            }
            return tabSAT;
        }

        private void reflectorOutputHandler( object sendingProcess, DataReceivedEventArgs outLine )
        {
            /*if( reflectorTextBox.InvokeRequired )
            {
                reflectorTextBox.BeginInvoke( new DataReceivedEventHandler( reflectorOutputHandler ), new[] { sendingProcess, outLine } );
            }
            else
            {*/
                if( !String.IsNullOrEmpty( outLine.Data ) )
                {
                    reflectorTextBox.AppendText( Environment.NewLine + outLine.Data );
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

                extractSaveButton.Enabled = true;
            }
        }

        private void extractSaveButton_Click( object sender, EventArgs e )
        {
            string saveFile = openSaveFileDialog.FileName;
            if( saveFile != "" )
            {
                if( Directory.Exists( decryptDir ) )
                {
                    extractedDirTextBox.Text = "Extraction directory already exists: " + decryptDir;
                }
                else
                {
                    saveFileChooseButton.Enabled = false;
                    extractSaveButton.Enabled = false;
                    extractedDirTextBox.Text = decryptDir;
                    shiftTextViewRight( extractedDirTextBox );

                    getTABSAT().decryptSaveToDir( saveFile, decryptDir );

                    repackSaveButton.Enabled = true;
                }

                mutantModifyButton.Enabled = File.Exists( dataFile );
            }
        }

        private void mutantModifyButton_Click( object sender, EventArgs e )
        {
            mutantModifyButton.Enabled = false;
            SaveEditor dataEditor = new SaveEditor( dataFile );
            dataEditor.relocateMutants();
            dataEditor.save( dataFile );
        }

        private void repackSaveButton_Click( object sender, EventArgs e )
        {
            repackSaveButton.Enabled = false;

            string saveFile = openSaveFileDialog.FileName;
            string backupFile = TABSAT.backupSave( saveFile );
            if( backupFile != null )
            {
                backupSaveFileTextBox.Text = backupFile;
                shiftTextViewRight( backupSaveFileTextBox );

                repackedSaveFileTextBox.Text = getTABSAT().repackDirAsSave( decryptDir, saveFile );
                shiftTextViewRight( repackedSaveFileTextBox );

                // Purge the temporary decrypted versions of this new save file
                string decryptedSave = decryptDir + TABReflector.TABReflector.saveExtension;
                string decryptedCheck = decryptDir + TABReflector.TABReflector.checkExtension;
                File.Delete( decryptedSave );
                File.Delete( decryptedCheck );
                // null out decryptDir and dataFile?
            }
            else
            {
                backupSaveFileTextBox.Text = "Unable to backup save file.";
            }

            // Assume this is single use only for now
            tabSAT.tidyUp();
            tabSAT = null;
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
