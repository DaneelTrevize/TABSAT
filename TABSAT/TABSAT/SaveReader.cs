using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TABSAT
{
    interface MapData
    {
        string Name();
        int CellsCount();
        SaveReader.ThemeType Theme();
        SaveReader.LayerData getLayerData( SaveReader.MapLayers layer );
        void getCCPosition( out int X, out int Y );
        int getCCDistance( MapNavigation.Position position );
        MapNavigation.Direction? getCCDirection( MapNavigation.Position position );
        int getNavigableCount( in int x, in int y, in int res );
        int getZombieCount( in int x, in int y, in int res, SortedSet<LevelEntities.ScalableZombieGroups> groups );
        LinkedList<MapNavigation.Position> getVodPositions( in LevelEntities.VODTypes vodType );
        LinkedList<MapNavigation.Position> getHugePositions( in LevelEntities.HugeTypes vodType );
        LinkedList<MapNavigation.Position> getPickablePositions( in LevelEntities.PickableTypes pickableType );
        LinkedList<MapNavigation.Position> getJoinablePositions( in LevelEntities.GiftableTypes joinableType );
    }

    class SaveReader : MapData
    {
        internal const int TERRAIN_WATER = 0x01;
        internal const int TERRAIN_GRASS = 0x02;
        internal const int OBJECTS_ROCKS = 0x01;
        internal const int OBJECTS_TREES = 0x02;
        internal const int OBJECTS_GOLD = 0x03;
        internal const int OBJECTS_STONE = 0x04;
        internal const int OBJECTS_IRON = 0x05;
        internal const int NAVIGABLE_BLOCKED = 0x01;

        internal enum ThemeType
        {
            FA,
            BR,
            TM,
            AL,
            DS,
            VO
        }
        internal static readonly Dictionary<ThemeType, string> themeTypeNames;

        internal enum SwarmDirections
        {
            ONE,
            TWO,
            ALL_BUT_ONE,
            ALL
        }
        internal static readonly Dictionary<SwarmDirections, string> SwarmDirectionsNames;

        protected readonly struct SwarmTimings
        {
            internal SwarmTimings( string s, string g, string r, string n )
            {
                startTime = s;
                gameTime = g;
                repeatTime = r;
                notifyTime = n;
            }
            internal string startTime { get; }
            internal string gameTime { get; }
            internal string repeatTime { get; }
            internal string notifyTime { get; }
        }
        protected readonly struct SwarmTimingSet
        {
            internal SwarmTimingSet( SwarmTimings Won, SwarmTimings Final, SwarmTimings Easy, SwarmTimings Hard, SwarmTimings Weak, SwarmTimings Medium )
            {
                this.Won = Won;
                this.Final = Final;
                this.Easy = Easy;
                this.Hard = Hard;
                this.Weak = Weak;
                this.Medium = Medium;
            }
            internal SwarmTimings Won { get; }
            internal SwarmTimings Final { get; }
            internal SwarmTimings Easy { get; }
            internal SwarmTimings Hard { get; }
            internal SwarmTimings Weak { get; }
            internal SwarmTimings Medium { get; }
        }
        protected enum GameFinish
        {
            Day50,
            Day80,
            Day100,
            Day120,
            Day150
        }
        protected static readonly SortedDictionary<GameFinish, SwarmTimingSet> swarmTimings;

        internal enum MapLayers
        {
            Terrain,
            Objects,
            Roads,
            Zombies,
            Fortress,
            Pipes,
            Belts,
            Fog,
            Activity,
            Navigable
        }

        internal enum ImpassableTypes : UInt64
        {
            OilSource = 14597207313853823957,
            TruckA = 1130949242559706282,
            FortressBarLeft = 1858993070642015232,
            FortressBarRight = 5955209075099213047,
            TensionTowerMediumFlip = 2617794739528169237,
            TensionTowerMedium = 4533866769353242870,
            TensionTowerHighFlip = 2342596987766548617,
            TensionTowerHigh = 3359149191582161849
        }

        /*private const string RuinTreasureA_BR_Type = @"5985530356264170826";
        private const string RuinTreasureA_TM_Type = @"257584999789546783";
        private const string RuinTreasureA_AL_Type = @"8971922455791567927";
        private const string RuinTreasureA_DS_Type = @"3137634406804904509";
        private const string RuinTreasureA_VO_Type = @"807600697508101881";
        private const string VOLCANO_Type = @"5660774435759652919";      // Multiple sizes?*/
        //private const string _Type = @"";

        static SaveReader()
        {
            themeTypeNames = new Dictionary<ThemeType, string> {
                { ThemeType.FA, "Deep Forest" },
                { ThemeType.BR, "Dark Moorland" },
                { ThemeType.TM, "Peaceful Lowlands" },
                { ThemeType.AL, "Frozen Highlands" },
                { ThemeType.DS, "Desert Wasteland" },
                { ThemeType.VO, "Caustic Lands" },
            };

            SwarmDirectionsNames = new Dictionary<SwarmDirections, string> {
                { SwarmDirections.ONE, "Any 1" },
                { SwarmDirections.TWO, "Any 2" },
                { SwarmDirections.ALL_BUT_ONE, "All but 1" },
                { SwarmDirections.ALL, "All" }
            };

            swarmTimings = new SortedDictionary<GameFinish, SwarmTimingSet> {
                { GameFinish.Day100, new SwarmTimingSet(
                    Won: new SwarmTimings( "2400", "2400", "0", "0" ),
                    Final: new SwarmTimings( "2208", "2208", "0", "2184" ),
                    Easy: new SwarmTimings( "312", "312", "240", "304" ),
                    Hard: new SwarmTimings( "1210", "1200", "168", "1202" ),
                    Weak: new SwarmTimings( "48", "48", "48", "0" ),
                    Medium: new SwarmTimings( "616", "600", "144", "0" ) ) },
                { GameFinish.Day80, new SwarmTimingSet(
                    Won: new SwarmTimings( "1920", "1920", "0", "0" ),
                    Final: new SwarmTimings( "1767", "1767", "0", "1743" ),
                    Easy: new SwarmTimings( "252", "250", "192", "244" ),
                    Hard: new SwarmTimings( "969", "960", "135", "961" ),
                    Weak: new SwarmTimings( "39", "39", "39", "0" ),
                    Medium: new SwarmTimings( "482", "480", "116", "0" ) ) },
                { GameFinish.Day50, new SwarmTimingSet(     // Taken from a <Simple name="ChallengeType" value="CommunityChallenge" /> map
                    Won: new SwarmTimings( "1200", "1200", "0", "0" ),
                    Final: new SwarmTimings( "1104", "1104", "0", "1080" ),
                    Easy: new SwarmTimings( "174", "156", "120", "166" ),
                    Hard: new SwarmTimings( "610", "600", "84", "602" ),
                    Weak: new SwarmTimings( "120", "120", "60", "0" ),
                    Medium: new SwarmTimings( "482", "480", "120", "0" ) ) }
            };
        }

        // TAB cell coordinates are origin top left?, before 45 degree rotation clockwise. Positive x is due SE, positive y is due SW?
        internal enum CompassDirection
        {
            North,
            East,
            South,
            West
        }

        protected const string LEVEL_EVENT_GAME_WON_NAME = @"Game Won";
        protected const string SWARM_FINAL_NAME = @"Final Swarm";
        protected const string SWARM_EASY_NAME = @"Swarm Easy";
        protected const string SWARM_HARD_NAME = @"Swarm Hard";
        protected const string SWARM_ROAMING_WEAK_NAME = @"Roaming Infected Weak";
        protected const string SWARM_ROAMING_MEDIUM_NAME = @"Roaming Infected Medium";
        // "&" will be encoded to "&amp;" when setting XAttribute value
        protected const string GENERATORS_1_DIRECTION = @"N | E | S | W";
        protected const string GENERATORS_2_DIRECTIONS = @"N & E | E & S | S & W | W & N | N & S | E & W";
        protected const string GENERATORS_3_DIRECTIONS = @"E & S & W | S & W & N | W & N & E | N & E & S";
        protected const string GENERATORS_4_DIRECTIONS = @"N & E & S & W";

        protected const int RESOURCE_STORE_CAPACITY = 50;
        protected const int GOLD_STORAGE_FACTOR = 40;

        private const int BYTES_PER_WORD = 4;

        protected readonly string dataFile;
        protected readonly XElement data;
        protected readonly XElement levelComplex;
        protected readonly int cellsCount;
        protected readonly int commandCenterX;
        protected readonly int commandCenterY;
        protected readonly LevelEntities entities;
        private readonly Regex layerDataRegex;
        private XElement generatedLevel;
        private XElement extension;
        private XElement mapDrawer;
        private XElement extrasItems;
        private readonly SortedDictionary<MapLayers, LayerData> layerDataCache;
        private MapNavigation.FlowGraph flowGraph;
        private MapNavigation.IntQuadTree navQuadTree;
        private SortedDictionary<LevelEntities.ScalableZombieTypes, MapNavigation.IntQuadTree> popQuadTrees;
        private readonly SortedDictionary<UInt64, LinkedList<MapNavigation.Position>> entityTypeToPositions;
        private readonly SortedDictionary<LevelEntities.GiftableTypes, LinkedList<MapNavigation.Position>> joinablePositions;

        internal static XElement getFirstPropertyOfTypeNamed( XElement c, string type, string name )    // 5 Collections, 3 Dictionaries
        {
            return ( from s in c.Element( "Properties" ).Elements( type )
                     where (string) s.Attribute( "name" ) == name
                     select s ).FirstOrDefault();   // Avoid exception risk of First()?
        }

        internal static XElement getFirstSimplePropertyNamed( XElement c, string name )
        {
            return getFirstPropertyOfTypeNamed( c, "Simple", name );
        }

        internal static XAttribute getValueAttOfSimpleProp( XElement c, string name )
        {
            /*
            <ComplexElement>
              <Properties>
                <Simple name=name value="..." />
             */
            return getFirstPropertyOfTypeNamed( c, "Simple", name ).Attribute( "value" );
        }

        internal static XElement getFirstComplexPropertyNamed( XElement c, string name )
        {
            return getFirstPropertyOfTypeNamed( c, "Complex", name );
        }

        protected static void extractInts( XElement property, out int x, out int y )
        {
            string xy = (string) property.Attribute( "value" );
            string[] xySplit = xy.Split( ';' );
            x = int.Parse( xySplit[0] );
            y = int.Parse( xySplit[1] );
        }

        protected static void extractCoordinates( XElement item, out int x, out int y, in bool alreadyComplex = false, string positionPropertyName = "Position" )
        {
            var complex = item;
            if( !alreadyComplex )
            {
                // Assume we have been passed an element from an Items collection of Item elements (i.e. under LevelEntities) each with a Complex containing a "Position" Simple Property,
                // rather than an element from an Items collection of Complex elements each with a propertyName Simple Property.
                complex = item.Element( "Complex" );
            }
            string xy = (string) getValueAttOfSimpleProp( complex, positionPropertyName );
            string[] xySplit = xy.Split( ';' );
            var float_x = float.Parse( xySplit[0] );
            var float_y = float.Parse( xySplit[1] );
            x = (int) float_x;
            y = (int) float_y;
        }

        internal SaveReader( string filesPath )
        {
            dataFile = Path.Combine( filesPath, "Data" );
            if( !File.Exists( dataFile ) )
            {
                throw new ArgumentException( "Data file does not exist: " + dataFile );
            }

            data = XElement.Load( dataFile );

            //<Complex name="Root" type="ZX.ZXGameState, TheyAreBillions">
            //  < Properties >
            //    <Complex name="LevelState">
            levelComplex = getFirstComplexPropertyNamed( data, "LevelState" );

            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            //          <Properties>
            //            <Simple name="NCells" value="256" />
            XAttribute nCells = getValueAttOfSimpleProp( getGeneratedLevel(), "NCells" );
            if( !Int32.TryParse( nCells.Value, out cellsCount ) )
            {
                Console.Error.WriteLine( "Unable to find the number of cells in the map." );
                cellsCount = 256;
            }

            //      <Properties>
            //        <Simple name="CurrentCommandCenterCell"
            XElement currentCommandCenterCell = getFirstSimplePropertyNamed( levelComplex, "CurrentCommandCenterCell" );
            extractInts( currentCommandCenterCell, out commandCenterX, out commandCenterY );
            //Console.WriteLine( "CurrentCommandCenterCell: " + commandCenterX + ", " + commandCenterY );

            entities = new LevelEntities( levelComplex );

            layerDataRegex = new Regex( @"(?:\d+\|){2}(?<data>.+)", RegexOptions.Compiled );    //value="256|256|AAAA..."
            layerDataCache = new SortedDictionary<MapLayers, LayerData>();

            flowGraph = null;
            navQuadTree = null;
            popQuadTrees = null;
            entityTypeToPositions = new SortedDictionary<UInt64, LinkedList<MapNavigation.Position>>();
            joinablePositions = new SortedDictionary<LevelEntities.GiftableTypes, LinkedList<MapNavigation.Position>>();
        }

        public string Name()
        {
            return Directory.GetParent( dataFile ).Name;
        }

        public int CellsCount()
        {
            return cellsCount;
        }

        public ThemeType Theme()
        {
            XAttribute levelThemeType = getValueAttOfSimpleProp( getMapDrawer(), "ThemeType" );
            ThemeType theme = (ThemeType) Enum.Parse( typeof(ThemeType), levelThemeType.Value );
            return theme;
        }

        protected XElement getGeneratedLevel()
        {
            if( generatedLevel == null )
            {
                generatedLevel = getFirstComplexPropertyNamed( levelComplex, "CurrentGeneratedLevel" );
            }
            return generatedLevel;
        }

        protected XElement getDataExtension()
        {
            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            //          <Properties>
            //            <Complex name="Data">
            //              <Properties>
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            if( extension == null )
            {
                extension = getFirstComplexPropertyNamed( getGeneratedLevel(), "Data" ).Element( "Properties" ).Element( "Complex" );   // only one Complex, named Extension
            }
            return extension;
        }

        protected XElement getMapDrawer()
        {
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            //                  <Properties>
            //                    <Complex name="MapDrawer">
            if( mapDrawer == null )
            {
                mapDrawer = getFirstComplexPropertyNamed( getDataExtension(), "MapDrawer" );
            }
            return mapDrawer;
        }

        protected IEnumerable<XElement> getInactiveZombieItems()
        {
            // This seems to list only idle map zombie entities, not player-triggered or swarm generated entities.
            /*
             *        <Dictionary name="LevelFastSerializedEntities" >
             *          <Items>
             *            <Item>
             *              <Simple />
             *              <Collection >
             */
            return getFirstPropertyOfTypeNamed( levelComplex, "Dictionary", "LevelFastSerializedEntities" ).Element( "Items" ).Elements( "Item" );
        }

        protected XElement getExtraEntities()
        {
            //                    <Complex name="MapDrawer">
            //                      <Properties>
            //                        <Collection name="ExtraEntities" elementType="DXVision.DXEntity, DXVision">
            if( extrasItems == null )
            {
                extrasItems = getFirstPropertyOfTypeNamed( getMapDrawer(), "Collection", "ExtraEntities" ).Element( "Items" );
            }
            return extrasItems;
        }

        internal readonly struct LayerData
        {
            internal readonly int res;
            internal readonly byte[] values;    // Assuming linear assignment, rather than anything fancy like a Hilbert curve
            internal LayerData( int r, byte[] v )
            {
                res = r;
                values = v;
            }
        }

        public LayerData getLayerData( MapLayers layer )
        {
            if( !layerDataCache.TryGetValue( layer, out LayerData data ) )
            {
                int res = cellsCount;

                byte[] values;

                switch( layer )
                {
                    case MapLayers.Terrain:
                        values = trimData( getLayer( "LayerTerrain" ), layer );
                        break;
                    case MapLayers.Objects:
                        values = trimData( getLayer( "LayerObjects" ), layer );
                        break;
                    case MapLayers.Roads:
                        values = trimData( getLayer( "LayerRoads" ), layer );
                        break;
                    case MapLayers.Zombies:
                        values = trimData( getLayer( "LayerZombies" ), layer );
                        break;
                    case MapLayers.Fortress:
                        values = trimData( getLayer( "LayerFortress" ), layer );
                        break;
                    case MapLayers.Pipes:
                        values = trimData( getLayer( "LayerPipes" ), layer );
                        break;
                    case MapLayers.Belts:
                        values = trimData( getLayer( "LayerBelts" ), layer );
                        break;
                    case MapLayers.Fog:
                        XAttribute layerFogSimpleValue = getValueAttOfSimpleProp( levelComplex, "LayerFog" );
                        values = trimData( Convert.FromBase64String( (string) layerFogSimpleValue ), layer );
                        break;
                    case MapLayers.Activity:
                        res = 64;
                        XAttribute layerActivitySimpleValue = getValueAttOfSimpleProp( levelComplex, "LayerActivity" );
                        Match match = layerDataRegex.Match( (string) layerActivitySimpleValue );
                        if( match.Success )
                        {
                            values = trimData( Convert.FromBase64String( match.Groups["data"].Value ), layer );
                        }
                        else
                        {
                            Console.Error.WriteLine( "Problem parsing LayerActivity." );
                            values = new byte[res * res];
                        }
                        break;
                    case MapLayers.Navigable:
                        values = generateNavigableData( res );
                        break;
                    default:
                        values = new byte[res * res];    // Defaulting to 00
                        break;
                }

                data = new LayerData( res, values );

                layerDataCache.Add( layer, data );
            }

            return data;
        }

        private byte[] getLayer( string layerName )
        {
            byte[] fullData;

            string encoded_value = (string) getValueAttOfSimpleProp( getFirstComplexPropertyNamed( getMapDrawer(), layerName ), "Cells" );
            Match match = layerDataRegex.Match( encoded_value );
            if( match.Success )
            {
                encoded_value = match.Groups["data"].Value;
                fullData = Convert.FromBase64String( encoded_value );
            }
            else
            {
                fullData = new byte[cellsCount * cellsCount * BYTES_PER_WORD];  // Defaulting to 00 00 00 00
            }

            return fullData;
        }

        private byte[] trimData( byte[] fullData, MapLayers layer )
        {

            //SortedDictionary<byte, int> unknownBytesFirstIndex = new SortedDictionary<byte, int>();

            // Trim from mostly-unused words to single bytes
            int wordCount = fullData.Length / BYTES_PER_WORD;
            byte[] trimmedData = new byte[wordCount];
            for( int i = 0; i < wordCount; i++ )
            {
                int wordIndex = i * BYTES_PER_WORD;
                int byteOffset = layer == MapLayers.Fog ? 3 : 0;    // Fog is in the byte at the other end of the word
                /*for( int b = 0; b < BYTES_PER_WORD; b++ )
                {
                    byte value = fullData[wordIndex + b];
                    if( b == byteOffset )
                    {
                        trimmedData[i] = value;
                    }
                    else if( value != 0x00 && !unknownBytesFirstIndex.ContainsKey( value ) )
                    {
                        unknownBytesFirstIndex.Add( value, wordIndex + b );
                    }
                }*/
                trimmedData[i] = fullData[wordIndex + byteOffset];
            }
            /*
            foreach( var k_v in unknownBytesFirstIndex )
            {
                Console.Error.WriteLine( "layerData " + layer + ", index:" + k_v.Value + " unknown value: " + k_v.Key );
            }
            */
            return trimmedData;
        }

        private byte[] generateNavigableData( int res )
        {
            byte[] values = new byte[res * res];

            LayerData terrain = getLayerData( MapLayers.Terrain );
            for( int b = 0; b < terrain.values.Length; b++ )
            {
                switch( terrain.values[b] )
                {
                    case TERRAIN_WATER:
                        values[b] = NAVIGABLE_BLOCKED;
                        break;
                    default:
                        break;
                }
            }

            LayerData objects = getLayerData( MapLayers.Objects );
            for( int b = 0; b < objects.values.Length; b++ )
            {
                switch( objects.values[b] )
                {
                    case OBJECTS_ROCKS:
                    case OBJECTS_TREES:
                        values[b] = NAVIGABLE_BLOCKED;
                        break;
                    default:
                        break;
                }
            }

            // Use a caching dictionary of ImpassableTypes to class/struct/record of positions & size, because generateNavigableData() is called every res change...
            foreach( ImpassableTypes impassableType in Enum.GetValues( typeof( ImpassableTypes ) ) )
            {
                UInt64 impassibleTypeID = (UInt64) impassableType;
                var impassibles = from c in getExtraEntities().Elements( "Complex" )
                                  where (UInt64) getValueAttOfSimpleProp( c, "IDTemplate" ) == impassibleTypeID
                                  select c;
                foreach( var c in impassibles )
                {
                    extractCoordinates( c, out int p_x, out int p_y, true );
                    extractInts( getFirstSimplePropertyNamed( c, "Size" ), out int s_x, out int s_y );
                    int corner_x = p_x - ( s_x / 2 );
                    int corner_y = p_y - ( s_y / 2 );
                    for( int x = 0; x < s_x; x++ )
                    {
                        for( int y = 0; y < s_y; y++ )
                        {
                            int i = MapNavigation.axesToIndex( res, corner_x + x, corner_y + y );
                            values[i] = NAVIGABLE_BLOCKED;
                        }
                    }
                }
            }

            // Remove CC from navigable positions
            for( int x = -2; x <= 2; x++ )
            {
                for( int y = -2; y <= 2; y++ )
                {
                    int i = MapNavigation.axesToIndex( res, commandCenterX + x, commandCenterY + y );
                    values[i] = NAVIGABLE_BLOCKED;
                }
            }

            return values;
        }

        public void getCCPosition( out Int32 X, out Int32 Y )
        {
            X = commandCenterX;
            Y = commandCenterY;
        }

        public int getCCDistance( MapNavigation.Position position )
        {
            return getFlowGraph().getDistance( position );
        }

        public MapNavigation.Direction? getCCDirection( MapNavigation.Position position )
        {
            return getFlowGraph().getDirection( position );
        }

        public int getNavigableCount( in int x, in int y, in int res )
        {
            return getNavQuadTree().getCount( x, y, res );
        }

        public int getZombieCount( in int x, in int y, in int res, SortedSet<LevelEntities.ScalableZombieGroups> groups )
        {
            int count = 0;
            foreach( var g in groups )
            {
                foreach( LevelEntities.ScalableZombieTypes t in LevelEntities.scalableZombieTypeGroups[g] )
                {
                    var tree = getPopQuadTrees()[t];
                    count += tree.getCount( x, y, res );
                }
            }
            return count;
        }

        public LinkedList<MapNavigation.Position> getVodPositions( in LevelEntities.VODTypes vodType )
        {
            UInt64 entityType = (UInt64) vodType;
            return getPositions( entityType );
        }

        public LinkedList<MapNavigation.Position> getHugePositions( in LevelEntities.HugeTypes hugeType )
        {
            UInt64 entityType = (UInt64) hugeType;
            return getPositions( entityType );
        }

        public LinkedList<MapNavigation.Position> getPickablePositions( in LevelEntities.PickableTypes pickableType )
        {
            UInt64 entityType = (UInt64) pickableType;
            return getPositions( entityType );
        }

        private LinkedList<MapNavigation.Position> getPositions( in UInt64 entityType )
        {
            if( !entityTypeToPositions.ContainsKey( entityType ) )
            {
                populatePositions( entityType );
            }
            return entityTypeToPositions[entityType];
        }

        public LinkedList<MapNavigation.Position> getJoinablePositions( in LevelEntities.GiftableTypes joinableType )
        {
            if( !joinablePositions.ContainsKey( joinableType ) )
            {
                // Can't use a generic version for all entity item types because it needs to be filtered by Team value.
                var positions = new LinkedList<MapNavigation.Position>();
                IEnumerable<XElement> joinableItems = entities.getNeutralJoinablesOfType( (UInt64) joinableType );
                foreach( var joinable in joinableItems )
                {
                    extractCoordinates( joinable, out int x, out int y );
                    positions.AddLast( new MapNavigation.Position( x, y ) );
                }
                joinablePositions.Add( joinableType, positions );
            }
            return joinablePositions[joinableType];
        }

        private MapNavigation.FlowGraph getFlowGraph()
        {
            if( flowGraph == null )
            {
                flowGraph = new MapNavigation.FlowGraph( cellsCount, getLayerData( MapLayers.Navigable ).values );
                flowGraph.floodFromCC( commandCenterX, commandCenterY );    // Should be constructor?
            }
            return flowGraph;
        }

        private MapNavigation.IntQuadTree getNavQuadTree()
        {
            if( navQuadTree == null )
            {
                navQuadTree = new MapNavigation.IntQuadTree( 0, 0, cellsCount );
                populateNavQuadTree();
            }
            return navQuadTree;
        }

        private SortedDictionary<LevelEntities.ScalableZombieTypes, MapNavigation.IntQuadTree> getPopQuadTrees()
        {
            if( popQuadTrees == null )
            {
                popQuadTrees = new SortedDictionary<LevelEntities.ScalableZombieTypes, MapNavigation.IntQuadTree>();
                populatePopQuadTree();
            }
            return popQuadTrees;
        }

        private void populateNavQuadTree()
        {
            //            <Simple name="PlayableArea" value="54;54;148;148" />
            for( int x = 0; x < cellsCount; x++ )
            {
                for( int y = 0; y < cellsCount; y++ )
                {
                    // North (-West)
                    if( y + 148 < cellsCount - x )
                    {
                        continue;
                    }
                    // South (-East)
                    if( y - 148 > cellsCount - x )
                    {
                        continue;
                    }
                    //
                    if( x > y + 108 )
                    {
                        continue;
                    }
                    //
                    if( x <= y - 108 )
                    {
                        continue;
                    }

                    if( getCCDistance( new MapNavigation.Position( x, y ) ) != MapNavigation.UNNAVIGABLE )
                    {
                        navQuadTree.Add( x, y );
                    }
                }
            }
        }

        private void populatePopQuadTree()
        {
            foreach( LevelEntities.ScalableZombieTypes zombieType in Enum.GetValues( typeof( LevelEntities.ScalableZombieTypes ) ) )
            {
                var popQuadTree = new MapNavigation.IntQuadTree( 0, 0, cellsCount );
                popQuadTrees.Add( zombieType, popQuadTree );
            }

            foreach( var t in getInactiveZombieItems() )
            {
                UInt64 zombieTypeInt = Convert.ToUInt64( t.Element( "Simple" ).Attribute( "value" ).Value );
                //Console.WriteLine( "zombieTypeInt: " + zombieTypeInt );
                if( !Enum.IsDefined( typeof( LevelEntities.ScalableZombieTypes ), zombieTypeInt ) )
                {
                    continue;   // Can't scale this type
                }
                LevelEntities.ScalableZombieTypes zombieType = (LevelEntities.ScalableZombieTypes) zombieTypeInt;

                var col = t.Element( "Collection" );
                foreach( var com in col.Element( "Items" ).Elements( "Complex" ) )
                {
                    // Get zombie coordinates, convert to ints/position, add to quadtree
                    extractCoordinates( com, out int p_x, out int p_y, true, "B" );
                    popQuadTrees[zombieType].Add( p_x, p_y );
                }

                // Now also count the active & generated zombies of this type from LevelEntities
                IEnumerable<XElement> entityItems = entities.getEntitiesOfType( zombieTypeInt );
                foreach( var entity in entityItems )
                {
                    extractCoordinates( entity, out int x, out int y );
                    popQuadTrees[zombieType].Add( x, y );
                }
            }
        }

        private void populatePositions( in UInt64 entityType )
        {
            var positions = new LinkedList<MapNavigation.Position>();
            IEnumerable<XElement> items = entities.getEntitiesOfType( entityType );
            foreach( var item in items )
            {
                extractCoordinates( item, out int x, out int y );
                positions.AddLast( new MapNavigation.Position( x, y ) );
            }
            entityTypeToPositions.Add( entityType, positions );
        }
        
        private void populatePositions( in ImpassableTypes impassableType )
        {
            UInt64 impassibleTypeID = (UInt64) impassableType;
            var positions = new LinkedList<MapNavigation.Position>();

            var impassibles = from c in getExtraEntities().Elements( "Complex" )
                              where (UInt64) getValueAttOfSimpleProp( c, "IDTemplate" ) == impassibleTypeID
                              select c;
            foreach( var c in impassibles )
            {
                extractCoordinates( c, out int x, out int y, true );
                positions.AddLast( new MapNavigation.Position( x, y ) );
            }
            entityTypeToPositions.Add( impassibleTypeID, positions );
        }
    }
}
