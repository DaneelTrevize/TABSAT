using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public MapSelectorControl( string savesDirectory, StatusWriterDelegate sW )
        {
            InitializeComponent();

            statusWriter = sW;

            mapOpenFileDialog.Filter = "TAB Save Files|" + TAB.SAVES_FILTER;// + "|Data files|*.dat";
            mapOpenFileDialog.InitialDirectory = savesDirectory;
        }


        private void mapFileChooseButton_Click( object sender, EventArgs e )
        {
            if( mapOpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                viewMap( mapOpenFileDialog.FileName );
            }
        }

        private void viewMap( string saveFile )
        {
            var extractedSave = System.IO.Path.Combine( ModifyManager.DEFAULT_EDITS_DIRECTORY, System.IO.Path.GetFileNameWithoutExtension( saveFile ) );
            if( System.IO.Directory.Exists( extractedSave ) )
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
}
