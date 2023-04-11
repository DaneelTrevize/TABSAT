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
        float CCX();
        float CCY();
        int CellsCount();
        SaveReader.ThemeType Theme();
        SaveReader.LayerData getLayerData( SaveReader.MapLayers layer );
        int getDistance( MapNavigation.Position position );
        MapNavigation.Direction? getDirection( MapNavigation.Position position );
        int getNavigableCount( int x, int y, int res );
        int getZombieCount( int x, int y, int res, SortedSet<SaveReader.ScalableZombieGroups> groups );
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

        internal enum VodSizes
        {
            SMALL,
            MEDIUM,
            LARGE
        }
        internal static readonly Dictionary<VodSizes, string> vodSizesNames;

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

        internal enum GiftableTypes : UInt64
        {
            Ranger = 11462509610414451330,
            SoldierRegular = 8122295062332983407,
            Sniper = 6536008488763521408,
            Lucifer = 16241120227094315491,
            Thanatos = 13687916016325214957,
            Titan = 15625692077980454078,
            Mutant = 4795230226196477375,
            EnergyWoodTower = 3581872206503330117,      // Tesla tower
            MillWood = 869623577388046954,
            MillIron = 12238914991741132226,
            PowerPlant = 12703689153551509267,
            Sawmill = 6484699889268923215,
            Quarry = 4012164333689948063,
            AdvancedQuarry = 6574833960938744452,
            OilPlatform = 15110117066074335339,
            HunterCottage = 706050193872584208,
            FishermanCottage = 13910727858942983852,
            Farm = 7709119203238641805,
            AdvancedFarm = 877281890077159856,
            WareHouse = 13640414733981798546,
            Market = 5507471650351043258,
            //WinterMarket = 5749655342014653624,
            Bank = 5036892806562984913,
            TentHouse = 17301104073651661026,
            CottageHouse = 1886362466923065378,
            StoneHouse = 17389931916361639317,
            WallWood = 16980392503923994773,
            WallStone = 7684920400170855714,
            GateWood = 8865737575894196495,
            GateStone = 18390252716895796075,
            WatchTowerWood = 11206202837167900273,
            WatchTowerStone = 16597317129181541225,
            TrapStakes = 14605210100319949981,          // Stakes Traps
            TrapBlades = 2562764233779101744,           // Wire Fence Traps
            TrapMine = 3791255408779778776,             // Land Mine
            WoodWorkshop = 2943963846200136989,
            StoneWorkshop = 11153810025740407576,
            Foundry = 14944401376001533849,
            SoldiersCenter = 17945382406851792953,
            AdvancedUnitCenter = 8857617519118038933,
            LookoutTower = 9352245195514814739,
            RadarTower = 10083572309367106690,
            Ballista = 1621013738552581284,
            MachineGun = 1918604527945708480,           // Wasp
            ShockingTower = 7671446590444700196,
            Executor = 782017986530656774,
            TheInn = 5797915750707445077,
            TheCrystalPalace = 7936948209186953569,
            // TheGreatTelescope = ,                    // The Silent Beholder
            TheSpire = 6908380734610266301,
            TheTransmutator = 5872990212787919747,
            TheAcademy = 8274629648718325688,
            TheVictorious = 6953739609864588774
        }
        internal static readonly Dictionary<GiftableTypes, string> giftableTypeNames;

        protected enum ScalableZombieTypes : UInt64
        {
            ZombieWeakA = 13102967879573781082,
            ZombieWeakB = 11373321006229815036,
            ZombieWeakC = 4497312170973781002,
            ZombieWorkerA = 17464596434855839240,
            ZombieWorkerB = 10676594063526581,
            ZombieMediumA = 3569719832138441992,
            ZombieMediumB = 12882220683103625178,
            ZombieDressedA = 8945324363763426993,
            ZombieStrongA = 6498716987293858679,
            ZombieHarpy = 1214272082232025268,
            ZombieVenom = 12658363830661735733
        }
        internal enum ScalableZombieGroups
        {
            WEAK,
            MEDIUM,
            DRESSED,
            STRONG,
            VENOM,
            HARPY
        }
        protected static readonly Dictionary<ScalableZombieGroups, SortedSet<ScalableZombieTypes>> scalableZombieTypeGroups;

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

        static SaveReader()
        {
            vodSizesNames = new Dictionary<VodSizes, string> {
                { VodSizes.SMALL, "Dwellings" },
                { VodSizes.MEDIUM, "Taverns" },
                { VodSizes.LARGE, "City Halls" }
            };

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

            giftableTypeNames = new Dictionary<GiftableTypes, string> {
                { GiftableTypes.Ranger, "Ranger" },
                { GiftableTypes.SoldierRegular, "Soldier" },
                { GiftableTypes.Sniper, "Sniper" },
                { GiftableTypes.Lucifer, "Lucifer" },
                { GiftableTypes.Thanatos, "Thanatos" },
                { GiftableTypes.Titan, "Titan" },
                { GiftableTypes.Mutant, "Mutant" },
                { GiftableTypes.EnergyWoodTower, "Tesla Tower" },
                { GiftableTypes.MillWood, "Mill" },
                { GiftableTypes.MillIron, "Advanced Mill" },
                { GiftableTypes.PowerPlant, "PowerPlant" },
                { GiftableTypes.Sawmill, "Sawmill" },
                { GiftableTypes.Quarry, "Quarry" },
                { GiftableTypes.AdvancedQuarry, "Advanced Quarry" },
                { GiftableTypes.OilPlatform, "Oil Platform" },
                { GiftableTypes.HunterCottage, "Hunter Cottage" },
                { GiftableTypes.FishermanCottage, "Fisherman Cottage" },
                { GiftableTypes.Farm, "Farm" },
                { GiftableTypes.AdvancedFarm, "Advanced Farm" },
                { GiftableTypes.WareHouse, "Warehouse" },
                { GiftableTypes.Market, "Market" },
                { GiftableTypes.Bank, "Bank" },
                { GiftableTypes.TentHouse, "Tent" },
                { GiftableTypes.CottageHouse, "Cottage" },
                { GiftableTypes.StoneHouse, "Stone House" },
                { GiftableTypes.WallWood, "Wood Wall" },
                { GiftableTypes.WallStone, "Stone Wall" },
                { GiftableTypes.GateWood, "Wood Gate" },
                { GiftableTypes.GateStone, "Stone Gate" },
                { GiftableTypes.WatchTowerWood, "Wood Tower" },
                { GiftableTypes.WatchTowerStone, "Stone Tower" },
                { GiftableTypes.TrapStakes, "Stakes Trap" },
                { GiftableTypes.TrapBlades, "Wire Fence Trap" },
                { GiftableTypes.TrapMine, "Land Mine" },
                { GiftableTypes.WoodWorkshop, "Wood Workshop" },
                { GiftableTypes.StoneWorkshop, "Stone Workshop" },
                { GiftableTypes.Foundry, "Foundry" },
                { GiftableTypes.SoldiersCenter, "Soldiers' Center" },
                { GiftableTypes.AdvancedUnitCenter, "Engineering Center" },
                { GiftableTypes.LookoutTower, "Lookout Tower" },
                { GiftableTypes.RadarTower, "Radar Tower" },
                { GiftableTypes.Ballista, "Ballista" },
                { GiftableTypes.MachineGun, "Wasp" },
                { GiftableTypes.ShockingTower, "Shocking Tower" },
                { GiftableTypes.Executor, "Executor" },
                { GiftableTypes.TheInn, "The Inn" },
                { GiftableTypes.TheCrystalPalace, "The Crystal Palace" },
                { GiftableTypes.TheSpire, "The Lightning Spire" },
                { GiftableTypes.TheAcademy, "The Academy of Immortals" },
                { GiftableTypes.TheVictorious, "The Victorious" },
                { GiftableTypes.TheTransmutator, "The Atlas Transmutator" }
            };

            scalableZombieTypeGroups = new Dictionary<ScalableZombieGroups, SortedSet<ScalableZombieTypes>> {
                { ScalableZombieGroups.WEAK, new SortedSet<ScalableZombieTypes> { ScalableZombieTypes.ZombieWeakA, ScalableZombieTypes.ZombieWeakB, ScalableZombieTypes.ZombieWeakC } },
                { ScalableZombieGroups.MEDIUM, new SortedSet<ScalableZombieTypes> { ScalableZombieTypes.ZombieWorkerA, ScalableZombieTypes.ZombieWorkerB, ScalableZombieTypes.ZombieMediumA, ScalableZombieTypes.ZombieMediumB } },
                { ScalableZombieGroups.DRESSED, new SortedSet<ScalableZombieTypes> { ScalableZombieTypes.ZombieDressedA } },
                { ScalableZombieGroups.STRONG, new SortedSet<ScalableZombieTypes> { ScalableZombieTypes.ZombieStrongA } },
                { ScalableZombieGroups.VENOM, new SortedSet<ScalableZombieTypes> { ScalableZombieTypes.ZombieVenom } },
                { ScalableZombieGroups.HARPY, new SortedSet<ScalableZombieTypes> { ScalableZombieTypes.ZombieHarpy } }
            };

            swarmTimings = new SortedDictionary<GameFinish, SwarmTimingSet> {
                { GameFinish.Day100, new SwarmTimingSet(    // Taken from a <Simple name="ChallengeType" value="CommunityChallenge" /> map
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
                { GameFinish.Day50, new SwarmTimingSet(
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
        private readonly MapNavigation.FlowGraph flowGraph;
        private readonly MapNavigation.IntQuadTree navQuadTree;
        private readonly SortedDictionary<ScalableZombieTypes, MapNavigation.IntQuadTree> popQuadTrees;

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

        internal static XElement getFirstComplexPropertyNamed( XElement c, string name )
        {
            return getFirstPropertyOfTypeNamed( c, "Complex", name );
        }

        protected static void extractCoordinates( XElement property, out int x, out int y )
        {
            string xy = (string) property.Attribute( "value" );
            string[] xySplit = xy.Split( ';' );
            x = int.Parse( xySplit[0] );
            y = int.Parse( xySplit[1] );
        }

        protected static void extractCoordinates( XElement property, out float x, out float y )
        {
            string xy = (string) property.Attribute( "value" );
            string[] xySplit = xy.Split( ';' );
            x = float.Parse( xySplit[0] );
            y = float.Parse( xySplit[1] );
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
            XAttribute nCells = getFirstSimplePropertyNamed( getGeneratedLevel(), "NCells" ).Attribute( "value" );
            if( !Int32.TryParse( nCells.Value, out cellsCount ) )
            {
                Console.Error.WriteLine( "Unable to find the number of cells in the map." );
                cellsCount = 256;
            }

            //      <Properties>
            //        <Simple name="CurrentCommandCenterCell"
            XElement currentCommandCenterCell = getFirstSimplePropertyNamed( levelComplex, "CurrentCommandCenterCell" );
            extractCoordinates( currentCommandCenterCell, out commandCenterX, out commandCenterY );
            //Console.WriteLine( "CurrentCommandCenterCell: " + commandCenterX + ", " + commandCenterY );

            entities = new LevelEntities( levelComplex );

            layerDataRegex = new Regex( @"(?:\d+\|){2}(?<data>.+)", RegexOptions.Compiled );    //value="256|256|AAAA..."
            layerDataCache = new SortedDictionary<MapLayers, LayerData>();

            flowGraph = new MapNavigation.FlowGraph( cellsCount, getLayerData( MapLayers.Navigable ).values );
            flowGraph.floodFromCC( commandCenterX, commandCenterY );    // Should be constructor?

            navQuadTree = new MapNavigation.IntQuadTree( 0, 0, cellsCount );
            populateNavQuadTree();

            popQuadTrees = new SortedDictionary<ScalableZombieTypes, MapNavigation.IntQuadTree>();
            populatePopQuadTree();
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

                    if( getDistance( new MapNavigation.Position( x, y ) ) != MapNavigation.UNNAVIGABLE )
                    {
                        navQuadTree.Add( x, y );
                    }
                }
            }
        }

        private void populatePopQuadTree()
        {
            foreach( ScalableZombieTypes zombieType in Enum.GetValues( typeof( ScalableZombieTypes ) ) )
            {
                var popQuadTree = new MapNavigation.IntQuadTree( 0, 0, cellsCount );
                popQuadTrees.Add( zombieType, popQuadTree );
            }

            foreach( var t in getLevelZombieTypesItems() )
            {
                UInt64 zombieTypeInt = Convert.ToUInt64( t.Element( "Simple" ).Attribute( "value" ).Value );
                //Console.WriteLine( "zombieTypeInt: " + zombieTypeInt );
                if( !Enum.IsDefined( typeof( ScalableZombieTypes ), zombieTypeInt ) )
                {
                    continue;   // Can't scale this type
                }
                ScalableZombieTypes zombieType = (ScalableZombieTypes) zombieTypeInt;

                var col = t.Element( "Collection" );
                foreach( var com in col.Element( "Items" ).Elements( "Complex" ) )
                {
                    // Get zombie coordinates, convert to ints/position, add to quadtree...
                    extractCoordinates( getFirstSimplePropertyNamed( com, "B" ), out float p_x, out float p_y );

                    popQuadTrees[zombieType].Add( (int) p_x, (int) p_y );
                }
            }
        }

        public string Name()
        {
            return Directory.GetParent( dataFile ).Name;
        }

        public float CCX()
        {
            return commandCenterX;
        }

        public float CCY()
        {
            return commandCenterY;
        }

        public int CellsCount()
        {
            return cellsCount;
        }

        public ThemeType Theme()
        {
            XAttribute levelThemeType = getFirstSimplePropertyNamed( getMapDrawer(), "ThemeType" ).Attribute( "value" );
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

        protected IEnumerable<XElement> getLevelZombieTypesItems()
        {
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
                        XElement layerFogSimple = getFirstSimplePropertyNamed( levelComplex, "LayerFog" );
                        values = trimData( Convert.FromBase64String( (string) layerFogSimple.Attribute( "value" ) ), layer );
                        break;
                    case MapLayers.Activity:
                        res = 64;
                        XElement layerActivitySimple = getFirstSimplePropertyNamed( levelComplex, "LayerActivity" );
                        Match match = layerDataRegex.Match( (string) layerActivitySimple.Attribute( "value" ) );
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

            XElement layerSimple = getFirstSimplePropertyNamed( getFirstComplexPropertyNamed( getMapDrawer(), layerName ), "Cells" );
            string encoded_value = (string) layerSimple.Attribute( "value" );
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

            var impassibleTemplates = new[] { LevelEntities.OILPOOL_ID_TEMPLATE, LevelEntities.FortressBarLeft_ID_TEMPLATE, LevelEntities.FortressBarRight_ID_TEMPLATE, LevelEntities.TruckA_ID_TEMPLATE };
            var impassibles = from c in getExtraEntities().Elements( "Complex" )
                              where impassibleTemplates.Contains( (UInt64) getFirstSimplePropertyNamed( c, "IDTemplate" ).Attribute( "value" ) )
                              select c;
            foreach( var c in impassibles )
            {
                extractCoordinates( getFirstSimplePropertyNamed( c, "Position" ), out float p_x, out float p_y );
                extractCoordinates( getFirstSimplePropertyNamed( c, "Size" ), out int s_x, out int s_y );
                int corner_x = (int) p_x - ( s_x / 2 );
                int corner_y = (int) p_y - ( s_y / 2 );
                for( int x = 0; x < s_x; x++ )
                {
                    for( int y = 0; y < s_y; y++ )
                    {
                        int i = MapNavigation.axesToIndex( res, corner_x + x, corner_y + y );
                        values[i] = NAVIGABLE_BLOCKED;
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

        public int getDistance( MapNavigation.Position position )
        {
            return flowGraph.getDistance( position );
        }

        public MapNavigation.Direction? getDirection( MapNavigation.Position position )
        {
            return flowGraph.getDirection( position );
        }

        public int getNavigableCount( int x, int y, int res )
        {
            return navQuadTree.getCount( x, y, res );
        }

        public int getZombieCount( int x, int y, int res, SortedSet<ScalableZombieGroups> groups )
        {
            int count = 0;
            foreach( var g in groups )
            {
                foreach( ScalableZombieTypes t in scalableZombieTypeGroups[g] )
                {
                    var tree = popQuadTrees[t];
                    count += tree.getCount( x, y, res );
                }
            }
            return count;
        }
    }
}
