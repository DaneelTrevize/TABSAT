using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TABSAT.MainWindow;

namespace TABSAT
{
    public partial class MapSelectorControl : UserControl
    {

        private readonly StatusWriterDelegate statusWriter;

        public MapSelectorControl( string editsDirectory, StatusWriterDelegate sW )
        {
            InitializeComponent();

            statusWriter = sW;

            mapFolderBrowserDialog.SelectedPath = editsDirectory;

            // Try to find the most recently extracted save's edit directory.
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
                }
            }

            extractedSaveTextBox.Text = mapFolderBrowserDialog.SelectedPath;
        }


        private void extractedSaveChooseButton_Click( object sender, EventArgs e )
        {
            if( mapFolderBrowserDialog.ShowDialog() == DialogResult.OK )
            {
                extractedSaveTextBox.Text = mapFolderBrowserDialog.SelectedPath;
                viewMap( mapFolderBrowserDialog.SelectedPath );
            }
            else
            {
                extractedSaveTextBox.Text = "";
            }
        }

        private void viewMap( string extractedSave )
        {
            /*
            BeginInvoke(    // Assume all calls to this containing method are from off-UI-thread workers?
                new Action( () => {

                } )
            );*/
            var mapData = new SaveReader( extractedSave );
            var mapViewer = new MapViewerControl( mapData );
            Form f = new Form();
            f.Text = mapData.Name() + " - MapViewer";
            f.Width = mapViewer.Width + 20;
            f.Height = mapViewer.Height + 40;
            mapViewer.Dock = DockStyle.Fill;
            f.Controls.Add( mapViewer );
            f.FormClosing += ( object sender, FormClosingEventArgs e ) => { mapViewer.clearCache(); };
            f.Show();
        }
    }
}
