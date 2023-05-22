using System;
using System.Drawing;
using System.Text;
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
        private readonly int twoThirdWidth;
        private readonly int thirdHeight;
        private readonly int twoThirdHeight;
        private readonly int halfWidth;
        private readonly int halfHeight;

        public AreaSelectorControl()
        {
            InitializeComponent();

            areaChoice = ModifyChoices.AreaChoices.None;
            directions = 0;

            thirdWidth = mapPictureBox.Width / 3;
            twoThirdWidth = mapPictureBox.Width * 2 / 3;
            thirdHeight = mapPictureBox.Height / 3;
            twoThirdHeight = mapPictureBox.Height * 2 / 3;
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

            mapPictureBox.MouseClick += mapPictureBox_MouseClick;

            updateMapImage();
        }

        private void updateMapImage()
        {
            Image map = new Bitmap( mapPictureBox.Width, mapPictureBox.Height );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                var width_fraction = (int) ( mapPictureBox.Width * radiusNumericUpDown.Value / 65 );
                var height_fraction = (int) ( mapPictureBox.Height * radiusNumericUpDown.Value / 90 );
                var half_ellipse_width = width_fraction / 2;
                var half_ellipse_height = height_fraction / 2;

                switch( areaChoice )
                {
                    default:
                    case ModifyChoices.AreaChoices.None:
                        mapGraphics.FillRectangle( backgroundBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );

                        mapGraphics.DrawLine( gridPen, thirdWidth, 0, thirdWidth, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, twoThirdWidth, 0, twoThirdWidth, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, 0, thirdHeight, mapPictureBox.Width - 1, thirdHeight );
                        mapGraphics.DrawLine( gridPen, 0, twoThirdHeight, mapPictureBox.Width - 1, twoThirdHeight );

                        mapGraphics.DrawEllipse( gridPen, halfWidth - half_ellipse_width, halfHeight - half_ellipse_height, width_fraction, height_fraction );
                        break;
                    case ModifyChoices.AreaChoices.Everywhere:
                        mapGraphics.FillRectangle( highlightBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );
                        break;
                    case ModifyChoices.AreaChoices.Sections:
                        mapGraphics.FillRectangle( backgroundBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );

                        if( containsDirection( directions, MapNavigation.Direction.NORTHWEST) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, 0, 0, thirdWidth, thirdHeight );
                        }
                        if( containsDirection( directions, MapNavigation.Direction.NORTH ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, thirdWidth, 0, thirdWidth, thirdHeight );
                        }
                        if( containsDirection( directions, MapNavigation.Direction.NORTHEAST ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, twoThirdWidth, 0, thirdWidth, thirdHeight );
                        }
                        if( containsDirection( directions, MapNavigation.Direction.WEST ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, 0, thirdHeight, thirdWidth, thirdHeight );
                        }

                        mapGraphics.FillRectangle( highlightBrush, thirdWidth, thirdHeight, thirdWidth, thirdHeight );

                        if( containsDirection( directions, MapNavigation.Direction.EAST ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, twoThirdWidth, thirdHeight, thirdWidth, thirdHeight );
                        }
                        if( containsDirection( directions, MapNavigation.Direction.SOUTHWEST ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, 0, twoThirdHeight, thirdWidth, thirdHeight );
                        }
                        if( containsDirection( directions, MapNavigation.Direction.SOUTH ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, thirdWidth, twoThirdHeight, thirdWidth, thirdHeight );
                        }
                        if( containsDirection( directions, MapNavigation.Direction.SOUTHEAST ) )
                        {
                            mapGraphics.FillRectangle( highlightBrush, twoThirdWidth, twoThirdHeight, thirdWidth, thirdHeight );
                        }


                        mapGraphics.DrawLine( gridPen, thirdWidth, 0, thirdWidth, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, twoThirdWidth, 0, twoThirdWidth, mapPictureBox.Height - 1 );
                        mapGraphics.DrawLine( gridPen, 0, thirdHeight, mapPictureBox.Width - 1, thirdHeight );
                        mapGraphics.DrawLine( gridPen, 0, twoThirdHeight, mapPictureBox.Width - 1, twoThirdHeight );
                        break;
                    case ModifyChoices.AreaChoices.WithinRadius:
                        mapGraphics.FillRectangle( backgroundBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );
                        mapGraphics.FillEllipse( highlightBrush, halfWidth - half_ellipse_width, halfHeight - half_ellipse_height, width_fraction, height_fraction );
                        break;
                    case ModifyChoices.AreaChoices.BeyondRadius:
                        //System.Drawing.Drawing2D.CombineMode.Exclude;
                        mapGraphics.FillRectangle( highlightBrush, 0, 0, mapPictureBox.Width, mapPictureBox.Height );
                        mapGraphics.FillEllipse( backgroundBrush, halfWidth - half_ellipse_width, halfHeight - half_ellipse_height, width_fraction, height_fraction );
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
                directions = 0;
            }
            else if( everywhereRadioButton.Checked )
            {
                areaChoice = ModifyChoices.AreaChoices.Everywhere;
                directions = 0xFF;
            }
            else if( sectionsRadioButton.Checked )
            {
                areaChoice = ModifyChoices.AreaChoices.Sections;
            }
            else if( radiusRadioButton.Checked )
            {
                radiusChoiceChanged();
                if( areaChoice == ModifyChoices.AreaChoices.WithinRadius )
                {
                    directions = (byte) (MapNavigation.Direction.NORTH | MapNavigation.Direction.WEST | MapNavigation.Direction.EAST | MapNavigation.Direction.SOUTH);
                }
                else //if( areaChoice == ModifyChoices.AreaChoices.BeyondRadius )
                {
                    directions = (byte) (MapNavigation.Direction.NORTHWEST | MapNavigation.Direction.NORTHEAST | MapNavigation.Direction.SOUTHWEST | MapNavigation.Direction.SOUTHEAST);
                }
            }
            updateMapImage();
        }

        private void radiusChoiceChanged()
        {
            if( boundaryComboBox.SelectedIndex == 0 )
            {
                areaChoice = ModifyChoices.AreaChoices.WithinRadius;
            }
            else //if( boundaryComboBox.SelectedIndex == 1 )
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

        private void mapPictureBox_MouseClick( object sender, MouseEventArgs e )
        {
            if( sectionsRadioButton.Enabled )
            {
                sectionsRadioButton.Checked = true;
                if( e.X < thirdWidth )
                {
                    if( e.Y < thirdHeight )
                    {
                        toggleDirection( MapNavigation.Direction.NORTHWEST );
                    }
                    else if( e.Y > twoThirdHeight )
                    {
                        toggleDirection( MapNavigation.Direction.SOUTHWEST );
                    }
                    else
                    {
                        toggleDirection( MapNavigation.Direction.WEST );
                    }
                }
                else if( e.X > twoThirdWidth )
                {
                    if( e.Y < thirdHeight )
                    {
                        toggleDirection( MapNavigation.Direction.NORTHEAST );
                    }
                    else if( e.Y > twoThirdWidth )
                    {
                        toggleDirection( MapNavigation.Direction.SOUTHEAST );
                    }
                    else
                    {
                        toggleDirection( MapNavigation.Direction.EAST );
                    }
                }
                else
                {
                    if( e.Y < thirdHeight )
                    {
                        toggleDirection( MapNavigation.Direction.NORTH );
                    }
                    else if( e.Y > twoThirdWidth )
                    {
                        toggleDirection( MapNavigation.Direction.SOUTH );
                    }
                    // else middle
                }
                updateMapImage();
            }
        }

        private void toggleDirection( in MapNavigation.Direction direction )
        {
            directions ^= (byte) direction;
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

        internal static bool containsDirection( in byte sections, in MapNavigation.Direction direction )
        {
            return (sections & (byte) direction) != 0;
        }

        internal void reset()
        {
            radiusNumericUpDown.Value = 45;
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

        internal static String formatSections( byte sections )
        {
            StringBuilder sb = new StringBuilder();

            if( containsDirection( sections, MapNavigation.Direction.NORTHWEST ) )
            {
                sb.Append( " NW" );
            }
            if( containsDirection( sections, MapNavigation.Direction.NORTH ) )
            {
                sb.Append( " N" );
            }
            if( containsDirection( sections, MapNavigation.Direction.NORTHEAST ) )
            {
                sb.Append( " NE" );
            }

            if( containsDirection( sections, MapNavigation.Direction.WEST ) )
            {
                sb.Append( " W" );
            }

            sb.Append( " M" );

            if( containsDirection( sections, MapNavigation.Direction.EAST ) )
            {
                sb.Append( " E" );
            }

            if( containsDirection( sections, MapNavigation.Direction.SOUTHWEST ) )
            {
                sb.Append( " SW" );
            }
            if( containsDirection( sections, MapNavigation.Direction.SOUTH ) )
            {
                sb.Append( " S" );
            }
            if( containsDirection( sections, MapNavigation.Direction.SOUTHEAST ) )
            {
                sb.Append( " SE" );
            }

            sb.Append( "." );

            return sb.ToString();
        }
    }
}
