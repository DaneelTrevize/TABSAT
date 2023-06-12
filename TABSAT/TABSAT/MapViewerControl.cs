using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static TABSAT.SaveReader;

namespace TABSAT
{
    public partial class MapViewerControl : UserControl
    {
        private const ushort BASE_PIXELS_PER_CELL = 2;
        //private const float X_TRANSFORM = 3 / 4;
        //private const float Y_TRANSFORM = 15 / 16;

        private readonly MapData mapData;    // To fit diagonally, the image side would need to be Math.Sqrt( cells * cells / 2 ) * 2 ~= 362
        private static readonly SortedDictionary<ThemeType, Color> backgroundMap;
        private static readonly SortedDictionary<MapLayers, SortedDictionary<byte, Brush>> layersBrushes;
        private static readonly Brush unknown = new SolidBrush( Color.HotPink );
        private static readonly Brush arrowBrush = new SolidBrush( Color.White );
        private static readonly Brush ccBrush = new SolidBrush( Color.FromArgb( 0xBF, 0x1F, 0x3F, 0xBF ) );
        private static readonly Brush weakBrush = new SolidBrush( Color.FromArgb( 0xAF, 0x3F, 0x7F, 0xFF ) );
        private static readonly Brush mediumBrush = new SolidBrush( Color.FromArgb( 0xAF, 0xCF, 0x6F, 0x00 ) );
        private static readonly Brush dressedBrush = new SolidBrush( Color.FromArgb( 0xCF, 0xBF, 0xBF, 0x3F ) );
        private static readonly Brush strongBrush = new SolidBrush( Color.FromArgb( 0xEF, 0x6F, 0x5F, 0xCF ) );
        private static readonly Brush harpyBrush = new SolidBrush( Color.FromArgb( 0xEF, 0xCF, 0x00, 0xFF ) );
        private static readonly Brush venomBrush = new SolidBrush( Color.FromArgb( 0xCF, 0x1F, 0xBF, 0x9F ) );
        private static readonly Brush swarmBrush = new SolidBrush( Color.FromArgb( 0x7F, 0xFF, 0xBF, 0x00 ) );
        private static readonly Brush vodBrush = new SolidBrush( Color.FromArgb( 0xBF, 0xFF, 0xCF, 0x0F ) );
        private static readonly Brush mutantBrush = new SolidBrush( Color.FromArgb( 0xEF, 0xFF, 0x3F, 0x3F ) );
        private static readonly Brush giantBrush = new SolidBrush( Color.FromArgb( 0xEF, 0x00, 0x3F, 0xFF ) );
        private static readonly Brush pickableBrush = new SolidBrush( Color.FromArgb( 0xDF, 0x00, 0xFF, 0x0F ) );
        private static readonly Brush joinableBrush = new SolidBrush( Color.FromArgb( 0xFF, 0xFF, 0xBF, 0x00 ) );
        private static readonly Pen redPen = new Pen( Color.FromArgb( 0x7F, 0xFF, 0x00, 0x00 ) );
        private readonly SortedDictionary<ViewLayer, SortedDictionary<ushort, Image>> layerCache;
        private readonly SortedDictionary<MapNavigation.Direction, Image> arrows;

        private enum ViewLayer : byte
        {
            Terrain,
            Fog,
            Activity,
            Navigable,
            Distance,
            Direction,
            NavQuads,
            WeakZombies,
            MediumZombies,
            DressedZombies,
            StrongZombies,
            HarpyZombies,
            VenomZombies
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
            Brush gold = new SolidBrush( Color.FromArgb( 0xBF, 0xFF, 0xFF, 0x00 ) );
            //Brush fortress = new SolidBrush( Color.Black );
            Brush fog = new SolidBrush( Color.FromArgb( 0xDF, 0x00, 0x00, 0x00 ) );

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
                { 0x00, new SolidBrush( Color.FromArgb( 0x9F, 0xFF, 0xFF, 0xFF ) ) },
                { NAVIGABLE_BLOCKED, new SolidBrush( Color.FromArgb( 0x9F, 0x00, 0x00, 0x00 ) ) }
            } );
        }

        internal MapViewerControl( MapData m, bool showSpoilers )
        {
            InitializeComponent();

            mapData = m;
            layerCache = new SortedDictionary<ViewLayer, SortedDictionary<ushort, Image>>();
            foreach( ViewLayer layer in Enum.GetValues( typeof( ViewLayer ) ) )
            {
                layerCache[layer] = new SortedDictionary<ushort, Image>();
            }
            arrows = new SortedDictionary<MapNavigation.Direction, Image>();

            if( showSpoilers )
            {
                fogCheckBox.Checked = false;
                navigableCheckBox.Checked = true;
                navQuadsCheckBox.Checked = false;
                mediumCheckBox.Checked = true;
                dressedCheckBox.Checked = true;
                harpyCheckBox.Checked = true;
                swarmZsCheckBox.Checked = true;
                vodsCheckBox.Checked = true;
            }

            zoomTrackBar.ValueChanged += zoomTrackBar_ValueChanged;
            terrainCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            fogCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            activityCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            navigableCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            distanceCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            directionCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            navQuadsCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            navQuadsTrackBar.ValueChanged += navQuadsTrackBar_ValueChanged;

            weakCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            mediumCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            dressedCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            strongCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            harpyCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            venomCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;

            swarmIsCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            swarmZsCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;

            vodsCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            hugeCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            pickablesCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            joinableCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;

            gridCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;
            rotateCheckBox.CheckedChanged += layersCheckBox_CheckedChanged;

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

        private void updateMapImage( in int zoom )
        {

            arrows.Clear();

            ushort cellSize = (ushort) (BASE_PIXELS_PER_CELL * zoom);
            ushort cells = mapData.CellsCount();
            int mapSize = cells * cellSize;

            // Now generate the combined image
            Image map = new Bitmap( mapSize, mapSize );     // +1 for nicer grid, even though first pixels are cells covered in grid and last pixels are outside of cells but with grid
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                void drawCachedImage( in ViewLayer layer )
                {
                    mapGraphics.DrawImage( getCachedImage( layer, cellSize, mapSize ), 0, 0, mapSize, mapSize );
                }

                // Background
                if( !backgroundMap.TryGetValue( mapData.Theme(), out Color background ) )
                {
                    background = Color.LightGray;
                }
                mapGraphics.Clear( background );

                if( terrainCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.Terrain );
                }

                // MapLayers.Zombie
                
                if( fogCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.Fog );
                }
                
                if( activityCheckBox.Checked )
                {
                    // Should we cache this as it's a full map of data to parse, or just draw directly as it's often so few pixels non-transparent..?
                    drawCachedImage( ViewLayer.Activity );
                }

                if( navigableCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.Navigable );
                }

                if( distanceCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.Distance );
                }

                if( directionCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.Direction );
                }

                if( navQuadsCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.NavQuads );
                }

                if( weakCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.WeakZombies );
                }
                if( mediumCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.MediumZombies );
                }
                if( dressedCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.DressedZombies );
                }
                if( strongCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.StrongZombies );
                }
                if( harpyCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.HarpyZombies );
                }
                if( venomCheckBox.Checked )
                {
                    drawCachedImage( ViewLayer.VenomZombies );
                }

                if( vodsCheckBox.Checked )
                {
                    drawVODs( mapGraphics, mapSize );
                }

                if( swarmIsCheckBox.Checked )
                {
                    drawSwarmIcons( mapGraphics, mapSize );
                }
                if( swarmZsCheckBox.Checked )
                {
                    drawSwarmZombies( mapGraphics, mapSize );
                }

                if( hugeCheckBox.Checked )
                {
                    drawHuge( mapGraphics, mapSize );
                }

                if( pickablesCheckBox.Checked )
                {
                    drawPickables( mapGraphics, mapSize );
                }

                if( joinableCheckBox.Checked )
                {
                    drawJoinables( mapGraphics, mapSize );
                }

                // Draw CommandCenter
                var cc = mapData.getCCPosition();
                mapGraphics.FillRectangle( ccBrush, ( cc.x - 2 ) * cellSize, ( cc.y - 2 ) * cellSize, cellSize * 5, cellSize * 5 );

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

        private Image getCachedImage( ViewLayer layer, in ushort cellSize, in int mapSize )
        {
            // For layers that draw a flat shape per cell (all but Direction?), is it not better to cache a resolution-independant image, 1 pixel/color per cell..?
            // Directions could also be encoded as colors, then not just simply scaled to 1 cell square rendering. Or just draw the arrows, uncached full res image.

            SortedDictionary<ushort, Image> cache = layerCache[layer];
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
                    case ViewLayer.WeakZombies:
                        map = generateZombieImage( mapSize, LevelEntities.ScalableZombieGroups.WEAK );
                        break;
                    case ViewLayer.MediumZombies:
                        map = generateZombieImage( mapSize, LevelEntities.ScalableZombieGroups.MEDIUM );
                        break;
                    case ViewLayer.DressedZombies:
                        map = generateZombieImage( mapSize, LevelEntities.ScalableZombieGroups.DRESSED );
                        break;
                    case ViewLayer.StrongZombies:
                        map = generateZombieImage( mapSize, LevelEntities.ScalableZombieGroups.STRONG );
                        break;
                    case ViewLayer.HarpyZombies:
                        map = generateZombieImage( mapSize, LevelEntities.ScalableZombieGroups.HARPY );
                        break;
                    case ViewLayer.VenomZombies:
                        map = generateZombieImage( mapSize, LevelEntities.ScalableZombieGroups.VENOM );
                        break;
                    default:
                        map = new Bitmap( mapSize, mapSize );
                        break;
                }
                cache[cellSize] = map;
            }
            return map;
        }

        private Image generateMapImage( in int mapSize, params MapLayers[] layers )
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

        private void drawLayer( MapLayers layer, in int mapSize, Graphics mapGraphics )
        {
            LayerData layerData = mapData.getLayerData( layer );
            int cellSize = mapSize / layerData.res;

            if( !layersBrushes.TryGetValue( layer, out SortedDictionary<byte, Brush> layerBrushes ) )
            {
                layerBrushes = null;
            }

            for( ushort x = 0; x < layerData.res; x++ )
            {
                for( ushort y = 0; y < layerData.res; y++ )
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

        private Image generateDistanceImage( in int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                ushort cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                for( ushort x = 0; x < cells; x++ )
                {
                    for( ushort y = 0; y < cells; y++ )
                    {
                        var distance = mapData.pathDistanceToCC( new MapNavigation.Position( x, y ) );
                        var darkness = distance == MapNavigation.UNNAVIGABLE ? 0x3F : Math.Min( (int) (distance * 2.25), 0xFF );
                        Brush pathing = new SolidBrush( Color.FromArgb( darkness, 0x00, 0x00, 0x00 ) );
                        mapGraphics.FillRectangle( pathing, x * cellSize, y * cellSize, cellSize, cellSize );
                    }
                }
            }
            return map;
        }

        private Image generateDirectionImage( in int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                ushort cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                for( ushort x = 0; x < cells; x++ )
                {
                    for( ushort y = 0; y < cells; y++ )
                    {
                        var direction = mapData.pathDirectionToCC( new MapNavigation.Position( x, y ) );
                        if( direction != null )
                        {
                            mapGraphics.DrawImage( getArrow( (MapNavigation.Direction) direction, cellSize ), x * cellSize, y * cellSize, cellSize, cellSize );
                        }
                    }
                }
            }
            return map;
        }

        private Image generateNavQuadsImage( in int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                ushort cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                ushort quadRes = (ushort) Math.Pow( 2, navQuadsTrackBar.Value );
                int maxPerQuad = quadRes * quadRes;
                int quadSize = quadRes * cellSize;
                for( ushort x = 0; x < cells; x += quadRes )
                {
                    for( ushort y = 0; y < cells; y += quadRes )
                    {
                        var navCount = mapData.getNavigableCount( x, y, quadRes );
                        int density = navCount * 255 / maxPerQuad;
                        Brush densityBrush = new SolidBrush( Color.FromArgb( 0x3F, density, density, density ) );
                        mapGraphics.FillRectangle( densityBrush, x * cellSize, y * cellSize, quadSize, quadSize );
                    }
                }
            }
            return map;
        }
        /*
        private Image generateZombieQuadsImage( in int mapSize )
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                Enum.TryParse( zombieComboBox.SelectedValue.ToString(), out LevelEntities.ScalableZombieGroups group );
                var groups = new SortedSet<LevelEntities.ScalableZombieGroups>() { group };

                ushort cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                ushort quadRes = (ushort) Math.Pow( 2, zombieTrackBar.Value );
                int quadSize = quadRes * cellSize;
                for( ushort x = 0; x < cells; x += quadRes )
                {
                    for( ushort y = 0; y < cells; y += quadRes )
                    {
                        var zCount = mapData.getZombieCount( x, y, quadRes, groups );
                        if( zCount > 0 )
                        {
                            int density = Math.Max( 0x1F, (int) Math.Log( zCount * 1024 / (quadRes * quadRes) ) * 28 );     // Linear density differences are not noticable enough.
                            density = Math.Min( density, 0xDF );                                                            // Cap it <= 0xFF. Use 0xDF so always some transparency.
                            Brush densityBrush = new SolidBrush( Color.FromArgb( density, 0xFF, 0x00, 0x00 ) );
                            mapGraphics.FillRectangle( densityBrush, x * cellSize, y * cellSize, quadSize, quadSize );
                            //mapGraphics.DrawRectangle( redPen, x * cellSize, y * cellSize, quadSize - 1, quadSize - 1 );
                        }
                    }
                }
            }
            return map;
        }
        */
        private Image generateZombieImage( in int mapSize, LevelEntities.ScalableZombieGroups group )   // Unify zombie group layers into 1 image..?
        {
            Image map = new Bitmap( mapSize, mapSize );
            using( Graphics mapGraphics = Graphics.FromImage( map ) )
            {
                ushort cells = mapData.CellsCount();
                int cellSize = mapSize / cells;
                Brush zombieBrush;
                switch( group )
                {
                    case LevelEntities.ScalableZombieGroups.WEAK:
                        zombieBrush = weakBrush;
                        break;
                    case LevelEntities.ScalableZombieGroups.MEDIUM:
                        zombieBrush = mediumBrush;
                        break;
                    case LevelEntities.ScalableZombieGroups.DRESSED:
                        zombieBrush = dressedBrush;
                        break;
                    case LevelEntities.ScalableZombieGroups.STRONG:
                        zombieBrush = strongBrush;
                        break;
                    case LevelEntities.ScalableZombieGroups.HARPY:
                        zombieBrush = harpyBrush;
                        break;
                    case LevelEntities.ScalableZombieGroups.VENOM:
                        zombieBrush = venomBrush;
                        break;
                    default:
                        zombieBrush = new SolidBrush( Color.FromArgb( 0xBF, 0xFF, 0x00, 0x00 ) );
                        break;
                }
                foreach( var pos in mapData.getZombiePositions( group ) )
                {
                    mapGraphics.FillRectangle( zombieBrush, pos.x * cellSize, pos.y * cellSize, cellSize, cellSize );
                }
            }
            return map;
        }

        private void drawSwarmIcons( Graphics mapGraphics, in int mapSize )
        {
            ushort cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            var positions = mapData.getSwarmIconPositions();
            foreach( var p in positions )
            {
                mapGraphics.FillEllipse( swarmBrush, ( p.x - 3 ) * cellSize, ( p.y - 3 ) * cellSize, cellSize * 7, cellSize * 7 );
            }
        }

        private void drawSwarmZombies( Graphics mapGraphics, in int mapSize )
        {
            ushort cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            var positions = mapData.getSwarmZombiePositions();
            foreach( var p in positions )
            {
                mapGraphics.FillRectangle( swarmBrush, p.x * cellSize, p.y * cellSize, cellSize, cellSize );
            }
        }

        private void drawVODs( Graphics mapGraphics, in int mapSize )
        {
            ushort cells = mapData.CellsCount();
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
                                new Point( (p.x + 2) * cellSize, (p.y - 1) * cellSize),
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

                    mapGraphics.FillPolygon( vodBrush, corners );
                }
            }
        }

        private void drawHuge( Graphics mapGraphics, in int mapSize )
        {
            ushort cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            foreach( var hugeType in new LevelEntities.HugeTypes[] { LevelEntities.HugeTypes.Giant, LevelEntities.HugeTypes.Mutant } )  // This order so Mutants are on top and not obscured by Giants
            {
                var positions = mapData.getHugePositions( hugeType );
                foreach( var p in positions )
                {
                    if( hugeType == LevelEntities.HugeTypes.Mutant )
                    {
                        mapGraphics.FillEllipse( mutantBrush, ( p.x - 1 ) * cellSize, ( p.y - 1 ) * cellSize, cellSize * 3, cellSize * 3 );
                    }
                    else
                    {
                        mapGraphics.FillEllipse( giantBrush, (p.x - 1) * cellSize, (p.y - 1) * cellSize, cellSize * 3, cellSize * 3 );
                    }
                }
            }
        }

        private void drawPickables( Graphics mapGraphics, in int mapSize )
        {
            ushort cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            foreach( LevelEntities.PickableTypes pickableType in Enum.GetValues( typeof( LevelEntities.PickableTypes ) ) )
            {
                var positions = mapData.getPickablePositions( pickableType );
                foreach( var p in positions )
                {
                    switch( pickableType )
                    {
                        default:
                            mapGraphics.FillRectangle( pickableBrush, p.x * cellSize, p.y * cellSize, cellSize, cellSize );
                            break;
                    }
                }
            }
        }

        private void drawJoinables( Graphics mapGraphics, in int mapSize )
        {
            ushort cells = mapData.CellsCount();
            int cellSize = mapSize / cells;
            foreach( var joinableType in LevelEntities.joinableTypes )
            {
                var positions = mapData.getJoinablePositions( (LevelEntities.GiftableTypes) joinableType );
                foreach( var p in positions )
                {
                    switch( joinableType )
                    {
                        case (UInt64) LevelEntities.GiftableTypes.RadarTower:
                            mapGraphics.FillRectangle( joinableBrush, p.x * cellSize, p.y * cellSize, cellSize, cellSize );
                            break;
                        default:
                            mapGraphics.FillRectangle( joinableBrush, (p.x - 1) * cellSize, (p.y - 1) * cellSize, cellSize * 2, cellSize * 2 );
                            break;
                    }
                }
            }
        }

        private Image getArrow( MapNavigation.Direction direction, in int cellSize )
        {
            if( !arrows.TryGetValue( direction, out Image arrow ) )
            {
                arrow = new Bitmap( cellSize, cellSize );
                using( Graphics arrowGraphics = Graphics.FromImage( arrow ) )
                {
                    arrowGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    Point[] points;
                    int quarter = cellSize / 4;
                    int half = cellSize / 2;
                    int threeQuarters = cellSize - quarter;
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

        private void drawGrid( in ushort cellSize, in ushort cells, Graphics mapGraphics )
        {
            Pen pen = new Pen( new SolidBrush( Color.FromArgb( 0x77, 0xFF, 0xFF, 0xFF ) ) );
            for( ushort x = 1; x < cells; x++ )
            {
                mapGraphics.DrawLine( pen, x * cellSize, 0, x * cellSize, cells * cellSize );
            }
            for( ushort y = 1; y < cells; y++ )
            {
                mapGraphics.DrawLine( pen, 0, y * cellSize, cells * cellSize, y * cellSize );
            }
        }

        private void rotateImage( in int mapSize, Image map, Graphics outputGraphics )
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
