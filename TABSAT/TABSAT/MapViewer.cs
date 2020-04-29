using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static TABSAT.MainWindow;
using static TABSAT.SaveReader;

namespace TABSAT
{
    public partial class MapViewer : Form
    {
        private const int BASE_PIXELS_PER_CELL = 2;

        private readonly StatusWriterDelegate statusWriter;
        private MapData mapData;    // To fit diagonally, the image side would need to be Math.Sqrt( cells * cells / 2 ) * 2 ~= 362
        private static readonly SortedDictionary<MapLayers, SortedDictionary<byte, Brush>> layersBrushes;
        private static readonly Brush unknown = new SolidBrush( Color.HotPink );
        private static readonly Brush arrowBrush = new SolidBrush( Color.White );
        private static readonly Pen redPen = new Pen( Color.FromArgb( 0x7F, 0xFF, 0x00, 0x00 ) );
        private readonly SortedDictionary<int, Image> terrainCache;
        private readonly SortedDictionary<int, Image> fogCache;
        private readonly SortedDictionary<int, Image> activityCache;
        private readonly SortedDictionary<MapNavigation.Direction, Image> arrows;
        static MapViewer()
        {
            layersBrushes = new SortedDictionary<MapLayers, SortedDictionary<byte, Brush>>();
            Brush grass = new SolidBrush( Color.FromArgb( 0x7F, 0xAD, 0xFF, 0x2F ) );
            Brush water = new SolidBrush( Color.DeepSkyBlue );
            Brush rock = new SolidBrush( Color.FromArgb( 0xFF, 0x80, 0x40, 0x00 ) );
            Brush trees = new SolidBrush( Color.DarkGreen );
            Brush stone = new SolidBrush( Color.DarkGray );
            Brush iron = new SolidBrush( Color.SteelBlue );
            Brush gold = new SolidBrush( Color.Yellow );
            //Brush fortress = new SolidBrush( Color.Black );
            Brush fog = new SolidBrush( Color.FromArgb( 0x7F, 0x00, 0x00, 0x00 ) );

            layersBrushes.Add( MapLayers.Terrain, new SortedDictionary<byte, Brush>
            {
                { TERRAIN_WATER, water },
                { TERRAIN_GRASS, grass }
            } );

            layersBrushes.Add( MapLayers.Objects, new SortedDictionary<byte, Brush>
            {
                { OBJECTS_ROCKS, rock },
                { OBJECTS_TREES, trees },
                { OBJECTS_GOLD, gold },
                { OBJECTS_STONE, stone },
                { OBJECTS_IRON, iron }
            } );
            /*
            layersBrushes.Add( MapLayers.Fortress, new SortedDictionary<byte, Brush>
            {
                { 0x01, fortress }
            } );
            */
            layersBrushes.Add( MapLayers.Fog, new SortedDictionary<byte, Brush>
            {
                { 0xFF, fog }
            } );
            layersBrushes.Add( MapLayers.Navigable, new SortedDictionary<byte, Brush>
            {
                { 0x00, new SolidBrush( Color.FromArgb( 0xBF, 0xFF, 0xFF, 0xFF ) ) },
                { NAVIGABLE_BLOCKED, new SolidBrush( Color.FromArgb( 0xBF, 0x00, 0x00, 0x00 ) ) }
            } );
        }

        internal MapViewer( StatusWriterDelegate sW, MapData m )
        {
            InitializeComponent();

            statusWriter = sW;
            mapData = m;
            terrainCache = new SortedDictionary<int, Image>();
            fogCache = new SortedDictionary<int, Image>();
            activityCache = new SortedDictionary<int, Image>();
            arrows = new SortedDictionary<MapNavigation.Direction, Image>();

            Text = "MapViewer - " + mapData.Name();

            FormClosing += new FormClosingEventHandler( MapViewer_FormClosing );
            mapTrackBar.ValueChanged += new EventHandler( mapTrackBar_ValueChanged );
            terrainCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            fogCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            activityCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            navigableCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            distanceCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            directionCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );

            updateMapImage( mapTrackBar.Value );
        }

        private void mapTrackBar_ValueChanged( object sender, EventArgs e )
        {
            //statusWriter( "MapViewer zoom now: " + mapTrackBar.Value );
            updateMapImage( mapTrackBar.Value );
        }
        
        private void layersCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            updateMapImage( mapTrackBar.Value );  // Regenerate for current image settings
        }

        private void updateMapImage( int zoom )
        {
            arrows.Clear();

            int cellSize = BASE_PIXELS_PER_CELL * zoom;
            int cells = mapData.CellsCount();
            int mapSize = cells * cellSize;

            // Now generate the combined image
            Image map = new Bitmap( mapSize, mapSize );     // +1 for nicer grid, even though first pixels are cells covered in grid and last pixels are outside of cells but with grid
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                // Background, should use MapTheme for earth colour?
                mapGraphics.Clear( Color.LightGray );

                if( terrainCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( terrainCache, cellSize, mapSize, MapLayers.Terrain, MapLayers.Objects/*, MapLayers.Fortress*/ ), 0, 0, mapSize, mapSize );
                    // MapLayers.Roads, MapLayers.Pipes, MapLayers.Belts
                }

                // MapLayers.Zombie
                
                if( fogCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( fogCache, cellSize, mapSize, MapLayers.Fog ), 0, 0, mapSize, mapSize );
                }
                
                if( activityCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( activityCache, cellSize, mapSize, MapLayers.Activity ), 0, 0, mapSize, mapSize );
                }

                if( navigableCheckBox.Checked )
                {
                    mapGraphics.DrawImage( generateMapImage( mapSize, MapLayers.Navigable ), 0, 0, mapSize, mapSize );
                }

                if( distanceCheckBox.Checked )
                {
                    drawDistance( cells, mapSize, mapGraphics );
                }

                if( directionCheckBox.Checked )
                {
                    drawDirection( cells, mapSize, mapGraphics );
                }

                // Grid
                //drawGrid( cellSize, cells, mapGraphics );

                // Transform to gaming view
                //rotateImage( mapSize, mapGraphics, map );
            }

            mapPictureBox.Image = map;
        }

        private Image getCachedImage( SortedDictionary<int, Image> cache, int cellSize, int mapSize, params MapLayers[] layers )
        {
            Image map;
            if( !cache.TryGetValue( cellSize, out map ) )
            {
                map = generateMapImage( mapSize, layers );
                cache[cellSize] = map;
            }
            return map;
        }

        private Image generateMapImage( int mapSize, params MapLayers[] layers )
        {
            Image map = new Bitmap( mapSize, mapSize );

            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                mapGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Diagonal diamond
                /*Brush brush = new SolidBrush( Color.DarkGray );
                int halfSide = mapSize / 2;
                Point[] points = new Point[] { new Point( 0, halfSide ), new Point( halfSide, 0 ), new Point( mapSize, halfSide ), new Point( halfSide, mapSize ) };
                mapGraphics.FillPolygon( brush, points );*/

                foreach( var layer in layers )
                {
                    drawLayer( layer, mapSize, mapGraphics );
                }
            }

            return map;
        }

        private void drawLayer( MapLayers layer, int mapSize, Graphics mapGraphics )
        {
            LayerData layerData = mapData.getLayerData( layer );
            int cellSize = mapSize / layerData.res;

            SortedDictionary<byte, Brush> layerBrushes;
            if( !layersBrushes.TryGetValue( layer, out layerBrushes ) )
            {
                layerBrushes = null;
            }

            for( int x = 0; x < layerData.res; x++ )
            {
                for( int y = 0; y < layerData.res; y++ )
                {
                    int index = ( x * layerData.res ) + y;
                    if( layer == MapLayers.Fog )
                    {
                        index = ( y * layerData.res ) + x;
                    }
                    var cell = layerData.values[index];

                    if( cell == 0x00 && layer != MapLayers.Navigable )
                    {
                        continue;   // Skip drawing fully transparent cells in this layer
                    }

                    if( layer == MapLayers.Activity )
                    {
                        Brush partialRed = new SolidBrush( Color.FromArgb( Math.Min( cell + 64, 255 ), 0xFF, 0x00, 0x00 ) );
                        mapGraphics.FillRectangle( partialRed, x * cellSize, y * cellSize, cellSize, cellSize );
                        mapGraphics.DrawRectangle( redPen, x * cellSize, y * cellSize, cellSize - 1, cellSize - 1 );
                    }
                    else
                    {
                        Brush brush;
                        if( layerBrushes == null || !layerBrushes.TryGetValue( cell, out brush ) )
                        {
                            brush = unknown;
                        }
                        mapGraphics.FillRectangle( brush, x * cellSize, y * cellSize, cellSize, cellSize );
                    }
                }
            }
        }

        private void drawDistance( int cells, int mapSize, Graphics mapGraphics )
        {
            int cellSize = mapSize / cells;
            for( int x = 0; x < cells; x++ )
            {
                for( int y = 0; y < cells; y++ )
                {
                    var distance = mapData.getDistance( new MapNavigation.Position( x, y ) );
                    var cell = distance == int.MaxValue ? 0 : Math.Max( 255 - ( (int) (distance * 1.5) ), 4 );
                    Brush pathing = new SolidBrush( Color.FromArgb( 0x7F, cell, cell, cell ) );
                    mapGraphics.FillRectangle( pathing, x * cellSize, y * cellSize, cellSize, cellSize );
                }
            }
        }

        private void drawDirection( int cells, int mapSize, Graphics mapGraphics )
        {
            int cellSize = mapSize / cells;
            for( int x = 0; x < cells; x++ )
            {
                for( int y = 0; y < cells; y++ )
                {
                    var direction = mapData.getDirection( new MapNavigation.Position( x, y ) );
                    if( direction != null )
                    {
                        mapGraphics.DrawImage( getArrow( (MapNavigation.Direction) direction, cellSize ), x * cellSize, y * cellSize, cellSize, cellSize );
                    }
                }
            }
        }

        private Image getArrow( MapNavigation.Direction direction, int cellSize )
        {
            Image arrow;
            if( !arrows.TryGetValue( direction, out arrow ) )
            {
                arrow = new Bitmap( cellSize, cellSize );
                using( Graphics arrowGraphics = Graphics.FromImage( arrow ) )
                {
                    Point[] points;
                    var quarter = cellSize / 4;
                    var half = cellSize / 2;
                    var threeQuarters = cellSize - quarter;
                    switch( direction )
                    {
                        case MapNavigation.Direction.NORTHEAST:
                            points = new Point[] { new Point( quarter, threeQuarters ), new Point( half, 0 ), new Point( threeQuarters, threeQuarters ) };
                            break;
                        case MapNavigation.Direction.EAST:
                            points = new Point[] { new Point( quarter, quarter ), new Point( cellSize, 0 ), new Point( threeQuarters, threeQuarters ) };
                            break;
                        case MapNavigation.Direction.SOUTHEAST:
                            points = new Point[] { new Point( quarter, quarter ), new Point( cellSize, half ), new Point( quarter, threeQuarters ) };
                            break;
                        case MapNavigation.Direction.SOUTH:
                            points = new Point[] { new Point( threeQuarters, quarter ), new Point( cellSize, cellSize ), new Point( quarter, threeQuarters ) };
                            break;
                        case MapNavigation.Direction.SOUTHWEST:
                            points = new Point[] { new Point( quarter, quarter ), new Point( half, cellSize ), new Point( threeQuarters, quarter ) };
                            break;
                        case MapNavigation.Direction.WEST:
                            points = new Point[] { new Point( quarter, quarter ), new Point( threeQuarters, threeQuarters ), new Point( 0, cellSize ) };
                            break;
                        case MapNavigation.Direction.NORTHWEST:
                            points = new Point[] { new Point( 0, half ), new Point( threeQuarters, quarter ), new Point( threeQuarters, threeQuarters ) };
                            break;
                        case MapNavigation.Direction.NORTH:
                            points = new Point[] { new Point( 0, 0 ), new Point( quarter, threeQuarters ), new Point( threeQuarters, quarter ) };
                            break;
                        default:
                            points = new Point[] { new Point( half, 0 ), new Point( cellSize, half ), new Point( half, cellSize ), new Point( 0, half ) };
                            break;
                    }

                    arrowGraphics.FillPolygon( arrowBrush, points );
                }

                arrows.Add( direction, arrow );
            }

            return arrow;
        }

        private void drawGrid( int cellSize, int cells, Graphics mapGraphics )
        {
            //int mapSize = cells * cellSize;
            Pen pen = new Pen( Brushes.White );
            for( int x = 0; x < cells; x++ )
            {
                mapGraphics.DrawLine( pen, x * cellSize, 0, x * cellSize, cells * cellSize );
            }
            for( int y = 0; y < cells; y++ )
            {
                mapGraphics.DrawLine( pen, 0, y * cellSize, cells * cellSize, y * cellSize );
            }
        }

        private void rotateImage( int mapSize, Graphics mapGraphics, Image map )
        {
            // Rotate the image about the center
            int center = mapSize / 2;
            mapGraphics.TranslateTransform( center, center );
            mapGraphics.RotateTransform( 45 );
            mapGraphics.TranslateTransform( -center, -center );
            mapGraphics.DrawImage( map, 0, 0 );
            mapGraphics.ResetTransform();
        }

        private void MapViewer_FormClosing( object sender, FormClosingEventArgs e )
        {
            foreach( var map in terrainCache.Values )
            {
                map.Dispose();
            }
            terrainCache.Clear();
            foreach( var map in fogCache.Values )
            {
                map.Dispose();
            }
            fogCache.Clear();
            foreach( var map in activityCache.Values )
            {
                map.Dispose();
            }
            activityCache.Clear();
        }
    }
}
