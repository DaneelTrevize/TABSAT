using System;
using System.Drawing;
using System.Windows.Forms;

namespace TABSAT
{
    public partial class AreaSelectorControl : UserControl
    {
        // We could reduce the height by making "Modify everywhere/within sections" use 1 RadioButton and a ComboBox..?
        // In theory we could also add "Do not modify" to that ComboBox, but it doesn't seem like good UX.

        private static readonly SolidBrush backgroundBrush = new SolidBrush( Color.DarkGray );
        private static readonly Pen gridPen = new Pen( Color.Black );
        private static readonly SolidBrush highlightBrush = new SolidBrush( Color.Yellow );

        private ModifyChoices.AreaChoices areaChoice;
        private byte directions;    // If we assume 1 section must always be selected, and fix that as the middle one, we only need a byte for the other directions

        // Assuming size doesn't change
        private readonly int thirdWidth;
        private readonly int thirdHeight;
        private readonly int halfWidth;
        private readonly int halfHeight;

        public AreaSelectorControl()
        {
            InitializeComponent();

            areaChoice = ModifyChoices.AreaChoices.None;
            directions = 0;

            thirdWidth = mapPictureBox.Width / 3;
            thirdHeight = mapPictureBox.Height / 3;
            halfWidth = mapPictureBox.Width / 2;
            halfHeight = mapPictureBox.Height / 2;

            boundaryComboBox.SelectedIndex = 0;

            nothingRadioButton.CheckedChanged += radioButton_CheckedChanged;
            everywhereRadioButton.CheckedChanged += radioButton_CheckedChanged;
            sectionsRadioButton.CheckedChanged += radioButton_CheckedChanged;
            radiusRadioButton.CheckedChanged += radioButton_CheckedChanged;

            boundaryComboBox.SelectedIndexChanged += radiusControl_Changed;
            radiusLabel.Click += radiusControl_Changed;
            radiusNumericUpDown.ValueChanged += radiusControl_Changed;

            updateMapImage();
        }

        private void updateMapImage()
        {
            Image map = new Bitmap( mapPictureBox.Width, mapPictureBox.Height );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                var radius_percent = radiusNumericUpDown.Value / 100;
                var width_percent = (int) ( mapPictureBox.Width * radius_percent );
                //var height_percent = (int) ( mapPictureBox.Height * radius_percent );     // We assume this is the larger dimension, and that we want a circle not an ellipse.
                var half_width_percent = width_percent / 2;

                switch( areaChoice )
                {
                    default:
                    case ModifyChoices.AreaChoices.None:
                        mapGraphics.FillRectangle( backgroundBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );

                        mapGraphics.DrawLine( gridPen, thirdWidth, 0, thirdWidth, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, mapPictureBox.Width * 2 / 3, 0, mapPictureBox.Width * 2 / 3, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, 0, thirdHeight, mapPictureBox.Width - 1, thirdHeight );
                        mapGraphics.DrawLine( gridPen, 0, mapPictureBox.Height * 2 / 3, mapPictureBox.Width - 1, mapPictureBox.Height * 2 / 3 );

                        mapGraphics.DrawEllipse( gridPen, halfWidth - half_width_percent, halfHeight - half_width_percent, width_percent, width_percent );
                        break;
                    case ModifyChoices.AreaChoices.Everywhere:
                        mapGraphics.FillRectangle( highlightBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );
                        break;
                    case ModifyChoices.AreaChoices.Sections:
                        mapGraphics.FillRectangle( backgroundBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );

                        mapGraphics.FillRectangle( highlightBrush, thirdWidth, thirdHeight, thirdWidth, thirdHeight );

                        mapGraphics.DrawLine( gridPen, thirdWidth, 0, thirdWidth, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, mapPictureBox.Width * 2 / 3, 0, mapPictureBox.Width * 2 / 3, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, 0, thirdHeight, mapPictureBox.Width - 1, thirdHeight );
                        mapGraphics.DrawLine( gridPen, 0, mapPictureBox.Height * 2 / 3, mapPictureBox.Width - 1, mapPictureBox.Height * 2 / 3 );
                        break;
                    case ModifyChoices.AreaChoices.WithinRadius:
                        mapGraphics.FillRectangle( backgroundBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );
                        mapGraphics.FillEllipse( highlightBrush, halfWidth - half_width_percent, halfHeight - half_width_percent, width_percent, width_percent );
                        break;
                    case ModifyChoices.AreaChoices.BeyondRadius:
                        //System.Drawing.Drawing2D.CombineMode.Exclude;
                        mapGraphics.FillRectangle( highlightBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );
                        mapGraphics.FillEllipse( backgroundBrush, halfWidth - half_width_percent, halfHeight - half_width_percent, width_percent, width_percent );
                        break;
                }

                mapGraphics.DrawRectangle( gridPen, 0, 0, mapPictureBox.Width - 1, mapPictureBox.Height - 1 );
            }
            mapPictureBox.Image = map;
        }

        private void radioButton_CheckedChanged( object sender, EventArgs e )
        {
            if( nothingRadioButton.Checked )
            {
                areaChoice = ModifyChoices.AreaChoices.None;
            }
            else if( everywhereRadioButton.Checked )
            {
                areaChoice = ModifyChoices.AreaChoices.Everywhere;
            }
            else if( sectionsRadioButton.Checked )
            {
                areaChoice = ModifyChoices.AreaChoices.Sections;
            }
            else if( radiusRadioButton.Checked )
            {
                radiusChoiceChanged();
            }
            updateMapImage();
        }

        private void radiusChoiceChanged()
        {
            if( boundaryComboBox.SelectedIndex == 0 )
            {
                areaChoice = ModifyChoices.AreaChoices.WithinRadius;
            }
            else if( boundaryComboBox.SelectedIndex == 1 )
            {
                areaChoice = ModifyChoices.AreaChoices.BeyondRadius;
            }
        }

        private void radiusControl_Changed( object sender, EventArgs e )
        {
            radiusRadioButton.Checked = true;
            radiusChoiceChanged();
            updateMapImage();
        }

        internal ModifyChoices.AreaChoices AreaChoice()
        {
            return areaChoice;
        }

        internal byte Radius()
        {
            return Convert.ToByte( radiusNumericUpDown.Value );
        }

        internal byte Sections()
        {
            return directions;
        }

        internal void reset()
        {
            nothingRadioButton.Checked = true;
        }

        internal void disableRadiusChoice()
        {
            radiusRadioButton.Enabled = false;
            boundaryComboBox.Enabled = false;
            radiusLabel.Enabled = false;
            radiusNumericUpDown.Enabled = false;
        }

        internal void enableRadiusChoice()
        {
            radiusRadioButton.Enabled = true;
            boundaryComboBox.Enabled = true;
            radiusLabel.Enabled = true;
            radiusNumericUpDown.Enabled = true;
        }

        internal void ensureSomeArea()
        {
            if( areaChoice == ModifyChoices.AreaChoices.None )
            {
                SetAreaEverywhere();
            }
        }

        internal void SetAreaEverywhere()
        {
            everywhereRadioButton.Checked = true;
        }
    }
}
