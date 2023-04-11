using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    public partial class SaveSelectorControl : UserControl
    {

        private readonly StatusWriterDelegate statusWriter;

        public SaveSelectorControl( string editsDirectory, StatusWriterDelegate sW )
        {
            InitializeComponent();

            statusWriter = sW;

            mapFolderBrowserDialog.SelectedPath = editsDirectory;
        }

        internal void refreshSaveFileChoice()
        {
            // Try to find the most recently extracted save's edit directory.
            var editsDirectory = mapFolderBrowserDialog.SelectedPath;
            if( !Directory.Exists( editsDirectory ) )
            {
                statusWriter( "The provided edits directory does not exist." );
            }
            else
            {
                DirectoryInfo editsDirInfo = new DirectoryInfo( editsDirectory );
                DirectoryInfo[] editsInfo = editsDirInfo.GetDirectories();
                if( editsInfo.Length > 0 )
                {
                    IOrderedEnumerable<DirectoryInfo> sortedEditsInfo = editsInfo.OrderByDescending( s => s.LastWriteTimeUtc );
                    DirectoryInfo newestEdit = sortedEditsInfo.First();
                    mapFolderBrowserDialog.SelectedPath = newestEdit.FullName;
                    viewMapButton.Enabled = true;
                }
            }

            extractedSaveTextBox.Text = mapFolderBrowserDialog.SelectedPath;
        }

        private void extractedSaveChooseButton_Click( object sender, EventArgs e )
        {
            if( mapFolderBrowserDialog.ShowDialog() == DialogResult.OK )
            {
                extractedSaveTextBox.Text = mapFolderBrowserDialog.SelectedPath;
                viewMapButton.Enabled = true;
            }
            else
            {
                viewMapButton.Enabled = false;
                extractedSaveTextBox.Text = "";
            }
        }

        private void viewMapButton_Click( object sender, EventArgs e )
        {
            viewMapButton.Enabled = false;
            viewMap( mapFolderBrowserDialog.SelectedPath );
        }

        private void viewMap( string extractedSave )
        {
            /*
            BeginInvoke(    // Assume all calls to this containing method are from off-UI-thread workers?
                new Action( () => {

                } )
            );*/
            SaveReader mapData;
            try
            {
                mapData = new SaveReader( extractedSave );
            }
            catch( Exception e )
            {
                Console.Error.WriteLine( "Problem opening extracted save file: " + e.Message + Environment.NewLine + e.StackTrace );
                statusWriter( "Unable to read extracted save file." );
                return;
            }
            var mapViewer = new MapViewerControl( mapData );
            Form f = new Form
            {
                Text = mapData.Name() + " - MapViewer",
                Width = mapViewer.Width + 20,
                Height = mapViewer.Height + 40
            };
            mapViewer.Dock = DockStyle.Fill;
            f.Controls.Add( mapViewer );
            f.FormClosing += ( object sender, FormClosingEventArgs e ) => { mapViewer.ClearCache(); };
            f.Show();
        }
    }
}
