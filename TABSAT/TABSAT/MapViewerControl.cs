using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static TABSAT.SaveReader;

namespace TABSAT
{
    public partial class MapViewerControl : UserControl
    {
        private const int BASE_PIXELS_PER_CELL = 2;
        //private const float X_TRANSFORM = 3 / 4;
        //private const float Y_TRANSFORM = 15 / 16;

        private readonly MapData mapData;    // To fit diagonally, the image side would need to be Math.Sqrt( cells * cells / 2 ) * 2 ~= 362
        private static readonly SortedDictionary<ThemeType, Color> backgroundMap;
        private static readonly SortedDictionary<MapLayers, SortedDictionary<byte, Brush>> layersBrushes;
        private static readonly Brush unknown = new SolidBrush( Color.HotPink );
        private static readonly Brush arrowBrush = new SolidBrush( Color.White );
        private static readonly Pen redPen = new Pen( Color.FromArgb( 0x7F, 0xFF, 0x00, 0x00 ) );
        private readonly SortedDictionary<ViewLayer, SortedDictionary<int, Image>> layerCache;
        private readonly SortedDictionary<MapNavigation.Direction, Image> arrows;

        private enum ViewLayer
        {
            Terrain,
            Fog,
            Activity,
            Navigable,
            Distance,
            Direction,
            NavQuads,
            ZombieQuads
        }

        static MapViewerControl()
        {
            backgroundMap = new SortedDictionary<ThemeType, Color> {
                { ThemeType.FA, Color.FromArgb( 0xFF, 0x60, 0x60, 0x00 ) }, // 0x22, 0x6B, 0x22
                { ThemeType.BR, Color.FromArgb( 0xFF, 0x50, 0x40, 0x10 ) }, // 0x30, 0x18, 0x0B
                { ThemeType.TM, Color.FromArgb( 0xFF, 0x90, 0xB0, 0x90 ) },
                { ThemeType.AL, Color.FromArgb( 0xFF, 0xF5, 0xF5, 0xFF ) }, // 0xCE, 0xF2, 0xFF
                { ThemeType.DS, Color.FromArgb( 0xFF, 0xF0, 0xE5, 0xBC ) }, // 0xFF, 0xCB, 0x69
                { ThemeType.VO, Color.LightSlateGray }                      // 0x23, 0x63, 0x58
            };

            layersBrushes = new SortedDictionary<MapLayers, SortedDictionary<byte, Brush>>();
            Brush grass = new SolidBrush( Color.FromArgb( 0x7F, 0xAD, 0xFF, 0x2F ) );
            Brush water = new SolidBrush( Color.DeepSkyBlue );
            Brush rock = new SolidBrush( Color.FromArgb( 0xFF, 0x40, 0x20, 0x00 ) );
            Brush trees = new SolidBrush( Color.DarkGreen );
            Brush stone = new SolidBrush( Color.FromArgb( 0xA9, 0xA9, 0xB9, 0xA9 ) );
            Brush iron = new SolidBrush( Color.SteelBlue );
            Brush gold = new SolidBrush( Color.Yellow );
            //Brush fortress = new SolidBrush( Color.Black );
            Brush fog = new SolidBrush( Color.FromArgb( 0x3F, 0x00, 0x00, 0x00 ) );

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
                { 0x00, new SolidBrush( Color.FromArgb( 0x7F, 0xFF, 0xFF, 0xFF ) ) },
                { NAVIGABLE_BLOCKED, new SolidBrush( Color.FromArgb( 0xBF, 0x00, 0x00, 0x00 ) ) }
            } );
        }

        internal MapViewerControl( MapData m )
        {
            InitializeComponent();

            mapData = m;
            layerCache = new SortedDictionary<ViewLayer, SortedDictionary<int, Image>>();
            foreach( ViewLayer layer in Enum.GetValues( typeof( ViewLayer ) ) )
            {
                layerCache[layer] = new SortedDictionary<int, Image>();
            }
            arrows = new SortedDictionary<MapNavigation.Direction, Image>();

            zoomTrackBar.ValueChanged += new EventHandler( zoomTrackBar_ValueChanged );
            terrainCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            fogCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            activityCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            navigableCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            distanceCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            directionCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            navQuadsCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            navQuadsTrackBar.ValueChanged += new EventHandler( navQuadsTrackBar_ValueChanged );

            zombieComboBox.DataSource = Enum.GetValues( typeof( LevelEntities.ScalableZombieGroups ) );
            zombieComboBox.SelectedIndex = 0;
            zombieComboBox.SelectedIndexChanged += new EventHandler( zombieComboBox_SelectedIndexChanged );

            zombieCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            zombieTrackBar.ValueChanged += new EventHandler( zombieTrackBar_ValueChanged );

            vodsCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            hugeCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );

            gridCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );
            rotateCheckBox.CheckedChanged += new EventHandler( layersCheckBox_CheckedChanged );

            updateMapImage( zoomTrackBar.Value );
        }

        private void zoomTrackBar_ValueChanged( object sender, EventArgs e )
        {
            //statusWriter( "MapViewer zoom now: " + zoomTrackBar.Value );
            updateMapImage( zoomTrackBar.Value );
        }

        private void layersCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            updateMapImage( zoomTrackBar.Value );  // Regenerate for current image settings
        }

        private void navQuadsTrackBar_ValueChanged( object sender, EventArgs e )
        {
            clearCache( ViewLayer.NavQuads );

            if( navQuadsCheckBox.Checked )
            {
                updateMapImage( zoomTrackBar.Value );
            }
        }

        private void zombieComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            clearCache( ViewLayer.ZombieQuads );

            updateMapImage( zoomTrackBar.Value );
        }

        private void zombieTrackBar_ValueChanged( object sender, EventArgs e )
        {
            clearCache( ViewLayer.ZombieQuads );

            if( zombieCheckBox.Checked )
            {
                updateMapImage( zoomTrackBar.Value );
            }
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
                // Background
                if( !backgroundMap.TryGetValue( mapData.Theme(), out Color background ) )
                {
                    background = Color.LightGray;
                }
                mapGraphics.Clear( background );

                if( terrainCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.Terrain, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                // MapLayers.Zombie
                
                if( fogCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.Fog, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }
                
                if( activityCheckBox.Checked )
                {
                    // Should we cache this as it's a full map of data to parse, or just draw directly as it's often so few pixels non-transparent..?
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.Activity, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                if( navigableCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.Navigable, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                if( distanceCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.Distance, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                if( directionCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.Direction, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                if( navQuadsCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.NavQuads, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                if( zombieCheckBox.Checked )
                {
                    mapGraphics.DrawImage( getCachedImage( ViewLayer.ZombieQuads, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                if( vodsCheckBox.Checked )
                {
                    drawVODs( mapGraphics, mapSize );
                }

                if( hugeCheckBox.Checked )
                {
                    drawHuge( mapGraphics, mapSize );
                }

                if( gridCheckBox.Checked )
                {
                    // Grid
                    drawGrid( cellSize, cells, mapGraphics );
                }

                if( rotateCheckBox.Checked )
                {
                    // Transform to gaming view
                    rotateImage( mapSize, map, mapGraphics );
                }
            }

            mapPictureBox.Image = map;
            //mapPictureBox.Size = new Size( (int) ( mapSize * X_TRANSFORM ), (int) ( mapSize * Y_TRANSFORM ) );
        }

        private Image getCachedImage( ViewLayer layer, int cellSize, int mapSize )
        {
            // For layers that draw a flat shape per cell (all but Direction?), is it not better to cache a resolution-independant image, 1 pixel/color per cell..?
            // Directions could also be encoded as colors, then not just simply scaled to 1 cell square rendering. Or just draw the arrows, uncached full res image.

            SortedDictionary<int, Image> cache = layerCache[layer];
            if( !cache.TryGetValue( cellSize, out Image map ) )
            {
                switch( layer )
                {
                    case ViewLayer.Terrain:
                        map = generateMapImage( mapSize, MapLayers.Terrain, MapLayers.Objects/*, MapLayers.Fortress*/ );
                        // MapLayers.Roads, MapLayers.Pipes, MapLayers.Belts
                        break;
                    case ViewLayer.Fog:
                        map = generateMapImage( mapSize, MapLayers.Fog );
                        break;
                    case ViewLayer.Activity:
                        map = generateMapImage( mapSize, MapLayers.Activity );
                        break;
                    case ViewLayer.Navigable:
                        map = generateMapImage( mapSize, MapLayers.Navigable );
                        break;
                    case ViewLayer.Distance:
                        map = generateDistanceImage( mapSize );
                        break;
                    case ViewLayer.Direction:
                        map = generateDirectionImage( mapSize );
                        break;
                    case ViewLayer.NavQuads:
                        map = generateNavQuadsImage( mapSize );
                        break;
                    case ViewLayer.ZombieQuads:
                        map = generateZombieQuadsImage( mapSize );
                        break;
                    default:
                        map = new Bitmap( mapSize, mapSize );
                        break;
                }
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

                // Should there be a choice for the fog layer be used to mask player-invisible areas, rather than be transparent..?
            }

            return map;
        }

        private void drawLayer( MapLayers layer, int mapSize, Graphics mapGraphics )
        {
            LayerData layerData = mapData.getLayerData( layer );
            int cellSize = mapSize / layerData.res;

            if( !layersBrushes.TryGetValue( layer, out SortedDictionary<byte, Brush> layerBrushes ) )
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
                        if( layerBrushes == null || !layerBrushes.TryGetValue( cell, out Brush brush ) )
                        {
                            brush = unknown;
                        }
                        mapGraphics.FillRectangle( brush, x * cellSize, y * cellSize, cellSize, cellSize );
                    }
                }
            }
        }

        private Image generateDistanceImage( int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                int cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                for( int x = 0; x < cells; x++ )
                {
                    for( int y = 0; y < cells; y++ )
                    {
                        var distance = mapData.getDistance( new MapNavigation.Position( x, y ) );
                        var brightness = distance == MapNavigation.UNNAVIGABLE ? 0 : Math.Max( 255 - ( (int) ( distance * 1.75 ) ), 4 );
                        Brush pathing = new SolidBrush( Color.FromArgb( 0x9F, brightness, brightness, brightness ) );
                        mapGraphics.FillRectangle( pathing, x * cellSize, y * cellSize, cellSize, cellSize );
                    }
                }
            }
            return map;
        }

        private Image generateDirectionImage( int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                int cells = mapData.CellsCount();
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
            return map;
        }

        private Image generateNavQuadsImage( int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                int cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                int quadRes = (int) Math.Pow( 2, navQuadsTrackBar.Value );
                int maxPerQuad = quadRes * quadRes;
                int quadSize = quadRes * cellSize;
                for( int x = 0; x < cells; x += quadRes )
                {
                    for( int y = 0; y < cells; y += quadRes )
                    {
                        var navCount = mapData.getNavigableCount( x, y, quadRes );
                        var density = navCount * 255 / maxPerQuad;
                        Brush densityBrush = new SolidBrush( Color.FromArgb( 0x7F, density, density, density ) );
                        mapGraphics.FillRectangle( densityBrush, x * cellSize, y * cellSize, quadSize, quadSize );
                    }
                }
            }
            return map;
        }

        private Image generateZombieQuadsImage( int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                Enum.TryParse( zombieComboBox.SelectedValue.ToString(), out LevelEntities.ScalableZombieGroups group );
                var groups = new SortedSet<LevelEntities.ScalableZombieGroups>() { group };

                int cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                int quadRes = (int) Math.Pow( 2, zombieTrackBar.Value );
                int maxPerQuad = quadRes * quadRes * 2; // 2 zombies per cell?
                int quadSize = quadRes * cellSize;
                for( int x = 0; x < cells; x += quadRes )
                {
                    for( int y = 0; y < cells; y += quadRes )
                    {
                        var zCount = mapData.getZombieCount( x, y, quadRes, groups );
                        var density = Math.Min( zCount * 255 / maxPerQuad, 0xDF );  // 0xDF so always some transparency, mostly for distance to show through
                                                                                    // This calc is not working right, especially around zoom level 4, should also apply to NavQuads..!
                        if( density > 0 )
                        {
                            Brush densityBrush = new SolidBrush( Color.FromArgb( density, 0xFF, 0x00, 0x00 ) );
                            mapGraphics.FillRectangle( densityBrush, x * cellSize, y * cellSize, quadSize, quadSize );
                            mapGraphics.DrawRectangle( redPen, x * cellSize, y * cellSize, quadSize - 1, quadSize - 1 );
                        }
                    }
                }
            }
            return map;
        }

        private void drawVODs( Graphics mapGraphics, int mapSize )
        {
            int cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            foreach( LevelEntities.VODTypes vodType in Enum.GetValues( typeof( LevelEntities.VODTypes ) ) )
            {
                var positions = mapData.getVodPositions( vodType );
                foreach( var p in positions )
                {
                    Point[] corners;
                    switch( vodType )
                    {
                        default:
                        case LevelEntities.VODTypes.DoomBuildingSmall:
                            corners = new Point[]{
                                new Point( (p.x + 0) * cellSize, (p.y + 0) * cellSize ),
                                new Point( (p.x + 2) * cellSize, (p.y + 0) * cellSize ),
                                new Point( (p.x + 2) * cellSize, (p.y + 1) * cellSize ),
                                new Point( (p.x + 1) * cellSize, (p.y + 2) * cellSize ),
                                new Point( (p.x + 0) * cellSize, (p.y + 2) * cellSize )
                            };
                            break;
                        case LevelEntities.VODTypes.DoomBuildingMedium:
                            corners = new Point[]{
                                new Point( (p.x - 1) * cellSize, (p.y - 1) * cellSize ),
                                new Point( (p.x + 2) * cellSize, (p.y - 1) * cellSize ),
                                new Point( (p.x + 2) * cellSize, (p.y + 1) * cellSize ),
                                new Point( (p.x + 1) * cellSize, (p.y + 2) * cellSize ),
                                new Point( (p.x - 1) * cellSize, (p.y + 2) * cellSize )
                            };
                            break;
                        case LevelEntities.VODTypes.DoomBuildingLarge:
                            corners = new Point[]{
                                new Point( (p.x - 1) * cellSize, (p.y - 1) * cellSize ),
                                new Point( (p.x + 2) * cellSize, (p.y - 1) * cellSize ),
                                new Point( (p.x + 3) * cellSize, (p.y + 1) * cellSize ),
                                new Point( (p.x + 1) * cellSize, (p.y + 3) * cellSize ),
                                new Point( (p.x - 1) * cellSize, (p.y + 2) * cellSize )
                            };
                            break;
                    }

                    var vodColor = Color.FromArgb( 0xBF, 0xFF, 0x7F, 0x00 );
                    mapGraphics.FillPolygon( new SolidBrush( vodColor ), corners );
                }
            }
        }

        private void drawHuge( Graphics mapGraphics, int mapSize )
        {
            int cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            foreach( LevelEntities.HugeTypes hugeType in Enum.GetValues( typeof( LevelEntities.HugeTypes ) ) )
            {
                int vodSize = cellSize * 2;     // Use hugeType to determine size/icon...
                var positions = mapData.getHugePositions( hugeType );
                foreach( var p in positions )
                {
                    var hugeColor = hugeType == LevelEntities.HugeTypes.Mutant ? Color.FromArgb( 0xFF, 0xBF, 0x00, 0xFF ) : Color.FromArgb( 0xFF, 0x3F, 0xBF, 0x9F );
                    mapGraphics.FillRectangle( new SolidBrush( hugeColor ), p.x * cellSize, p.y * cellSize, vodSize, vodSize );
                }
            }
        }

        private Image getArrow( MapNavigation.Direction direction, int cellSize )
        {
            if( !arrows.TryGetValue( direction, out Image arrow ) )
            {
                arrow = new Bitmap( cellSize, cellSize );
                using( Graphics arrowGraphics = Graphics.FromImage( arrow ) )
                {
                    arrowGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

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
            Pen pen = new Pen( new SolidBrush( Color.FromArgb( 0x77, 0xFF, 0xFF, 0xFF ) ) );
            for( int x = 1; x < cells; x++ )
            {
                mapGraphics.DrawLine( pen, x * cellSize, 0, x * cellSize, cells * cellSize );
            }
            for( int y = 1; y < cells; y++ )
            {
                mapGraphics.DrawLine( pen, 0, y * cellSize, cells * cellSize, y * cellSize );
            }
        }

        private void rotateImage( int mapSize, Image map, Graphics outputGraphics )
        {
            Image rotatedMap = new Bitmap( mapSize, mapSize/*mapSize * 3 / 4, mapSize * 15 / 16*/ );
            using( Graphics newGraphics = Graphics.FromImage( rotatedMap ) )
            {
                newGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                newGraphics.Clear( Color.Black );
                // Rotate the image about the center
                int center = mapSize / 2;
                newGraphics.TranslateTransform( center, center );
                newGraphics.RotateTransform( 45 );
                newGraphics.TranslateTransform( -center, -center );
                newGraphics.DrawImage( map, 0, 0/*-mapSize / 24, mapSize / 16*/ );
                newGraphics.ResetTransform();

                //outputGraphics.Clear( Color.Black );
                outputGraphics.DrawImage( rotatedMap, 0, 0/*-mapSize / 8, -mapSize / 11*/ );
            }
        }

        internal void ClearCache()
        {
            foreach( ViewLayer layer in Enum.GetValues( typeof( ViewLayer ) ) )
            {
                clearCache( layer );
            }
        }

        private void clearCache( ViewLayer layer )
        {
            var cache = layerCache[layer];
            foreach( var map in cache.Values )
            {
                map.Dispose();
            }
            cache.Clear();
        }
    }
}
