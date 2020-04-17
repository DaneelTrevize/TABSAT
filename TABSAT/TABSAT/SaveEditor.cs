using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TABSAT
{
    internal class SaveEditor
    {
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

        internal enum GiftableTypes : UInt64
        {
            Ranger = 11462509610414451330,
            SoldierRegular = 8122295062332983407,
            Sniper = 6536008488763521408,
            Lucifer = 16241120227094315491,
            Thanatos = 13687916016325214957,
            Titan = 15625692077980454078,
            // Mutant
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

        internal enum ScalableZombieTypes : UInt64
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
        internal static readonly Dictionary<ScalableZombieGroups,SortedSet<ScalableZombieTypes>> scalableZombieTypeGroups;

        static SaveEditor()
        {
            Dictionary<VodSizes, string> vsn = new Dictionary<VodSizes, string>();
            vsn.Add( VodSizes.SMALL, "Dwellings" );
            vsn.Add( VodSizes.MEDIUM, "Taverns" );
            vsn.Add( VodSizes.LARGE, "City Halls" );
            vodSizesNames = new Dictionary<VodSizes, string>( vsn );

            Dictionary<ThemeType, string> ttn = new Dictionary<ThemeType, string>();
            ttn.Add( ThemeType.FA, "Deep Forest" );
            ttn.Add( ThemeType.BR, "Dark Moorland" );
            ttn.Add( ThemeType.TM, "Peaceful Lowlands" );
            ttn.Add( ThemeType.AL, "Frozen Highlands" );
            ttn.Add( ThemeType.DS, "Desert Wasteland" );
            ttn.Add( ThemeType.VO, "Caustic Lands" );
            themeTypeNames = new Dictionary<ThemeType, string>( ttn );

            Dictionary<GiftableTypes, string> gtn = new Dictionary<GiftableTypes, string>();
            gtn.Add( GiftableTypes.Ranger, "Ranger" );
            gtn.Add( GiftableTypes.SoldierRegular, "Soldier" );
            gtn.Add( GiftableTypes.Sniper, "Sniper" );
            gtn.Add( GiftableTypes.Lucifer, "Lucifer" );
            gtn.Add( GiftableTypes.Thanatos, "Thanatos" );
            gtn.Add( GiftableTypes.Titan, "Titan" );
            gtn.Add( GiftableTypes.EnergyWoodTower, "Tesla Tower" );
            gtn.Add( GiftableTypes.MillWood, "Mill" );
            gtn.Add( GiftableTypes.MillIron, "Advanced Mill" );
            gtn.Add( GiftableTypes.PowerPlant, "PowerPlant" );
            gtn.Add( GiftableTypes.Sawmill, "Sawmill" );
            gtn.Add( GiftableTypes.Quarry, "Quarry" );
            gtn.Add( GiftableTypes.AdvancedQuarry, "Advanced Quarry" );
            gtn.Add( GiftableTypes.OilPlatform, "Oil Platform" );
            gtn.Add( GiftableTypes.HunterCottage, "Hunter Cottage" );
            gtn.Add( GiftableTypes.FishermanCottage, "Fisherman Cottage" );
            gtn.Add( GiftableTypes.Farm, "Farm" );
            gtn.Add( GiftableTypes.AdvancedFarm, "Advanced Farm" );
            gtn.Add( GiftableTypes.WareHouse, "Warehouse" );
            gtn.Add( GiftableTypes.Market, "Market" );
            gtn.Add( GiftableTypes.Bank, "Bank" );
            gtn.Add( GiftableTypes.TentHouse, "Tent" );
            gtn.Add( GiftableTypes.CottageHouse, "Cottage" );
            gtn.Add( GiftableTypes.StoneHouse, "Stone House" );
            gtn.Add( GiftableTypes.WallWood, "Wood Wall" );
            gtn.Add( GiftableTypes.WallStone, "Stone Wall" );
            gtn.Add( GiftableTypes.GateWood, "Wood Gate" );
            gtn.Add( GiftableTypes.GateStone, "Stone Gate" );
            gtn.Add( GiftableTypes.WatchTowerWood, "Wood Tower" );
            gtn.Add( GiftableTypes.WatchTowerStone, "Stone Tower" );
            gtn.Add( GiftableTypes.TrapStakes, "Stakes Trap" );
            gtn.Add( GiftableTypes.TrapBlades, "Wire Fence Trap" );
            gtn.Add( GiftableTypes.TrapMine, "Land Mine" );
            gtn.Add( GiftableTypes.WoodWorkshop, "Wood Workshop" );
            gtn.Add( GiftableTypes.StoneWorkshop, "Stone Workshop" );
            gtn.Add( GiftableTypes.Foundry, "Foundry" );
            gtn.Add( GiftableTypes.SoldiersCenter, "Soldiers' Center" );
            gtn.Add( GiftableTypes.AdvancedUnitCenter, "Engineering Center" );
            gtn.Add( GiftableTypes.LookoutTower, "Lookout Tower" );
            gtn.Add( GiftableTypes.RadarTower, "Radar Tower" );
            gtn.Add( GiftableTypes.Ballista, "Ballista" );
            gtn.Add( GiftableTypes.MachineGun, "Wasp" );
            gtn.Add( GiftableTypes.ShockingTower, "Shocking Tower" );
            gtn.Add( GiftableTypes.Executor, "Executor" );
            gtn.Add( GiftableTypes.TheInn, "The Inn" );
            gtn.Add( GiftableTypes.TheCrystalPalace, "The Crystal Palace" );
            gtn.Add( GiftableTypes.TheSpire, "The Lightning Spire" );
            gtn.Add( GiftableTypes.TheAcademy, "The Academy of Immortals" );
            gtn.Add( GiftableTypes.TheVictorious, "The Victorious" );
            gtn.Add( GiftableTypes.TheTransmutator, "The Atlas Transmutator" );
            giftableTypeNames = new Dictionary<GiftableTypes, string>( gtn );

            scalableZombieTypeGroups = new Dictionary<ScalableZombieGroups, SortedSet<ScalableZombieTypes>>();
            scalableZombieTypeGroups.Add( ScalableZombieGroups.WEAK, new SortedSet<ScalableZombieTypes>() { ScalableZombieTypes.ZombieWeakA, ScalableZombieTypes.ZombieWeakB, ScalableZombieTypes.ZombieWeakC } );
            scalableZombieTypeGroups.Add( ScalableZombieGroups.MEDIUM, new SortedSet<ScalableZombieTypes>() { ScalableZombieTypes.ZombieWorkerA, ScalableZombieTypes.ZombieWorkerB, ScalableZombieTypes.ZombieMediumA, ScalableZombieTypes.ZombieMediumB } );
            scalableZombieTypeGroups.Add( ScalableZombieGroups.DRESSED, new SortedSet<ScalableZombieTypes>() { ScalableZombieTypes.ZombieDressedA } );
            scalableZombieTypeGroups.Add( ScalableZombieGroups.STRONG, new SortedSet<ScalableZombieTypes>() { ScalableZombieTypes.ZombieStrongA } );
            scalableZombieTypeGroups.Add( ScalableZombieGroups.VENOM, new SortedSet<ScalableZombieTypes>() { ScalableZombieTypes.ZombieVenom } );
            scalableZombieTypeGroups.Add( ScalableZombieGroups.HARPY, new SortedSet<ScalableZombieTypes>() { ScalableZombieTypes.ZombieHarpy } );
        }

        // TAB cell coordinates are origin top left, before 45 degree rotation clockwise. Positive x is due SE, positive y is due SW.
        enum CompassDirection
        {
            North,
            East,
            South,
            West
        }

        private const string CLIFE_TYPE = @"ZX.Components.CLife, TheyAreBillions";
        private const string CINFLAMABLE = @"ZX.Components.CInflamable, TheyAreBillions";
        private const string CBEHAVIOUR_TYPE = @"ZX.Components.CBehaviour, TheyAreBillions";
        private const string CMOVABLE_TYPE = @"ZX.Components.CMovable, TheyAreBillions";

        private const string GIANT_TYPE = @"ZX.Entities.ZombieGiant, TheyAreBillions";
        private const string MUTANT_TYPE = @"ZX.Entities.ZombieMutant, TheyAreBillions";
        private const string VOD_SMALL_TYPE = @"ZX.Entities.DoomBuildingSmall, TheyAreBillions";
        private const string VOD_MEDIUM_TYPE = @"ZX.Entities.DoomBuildingMedium, TheyAreBillions";
        private const string VOD_LARGE_TYPE = @"ZX.Entities.DoomBuildingLarge, TheyAreBillions";
        private const string WAREHOUSE_TYPE = @"ZX.Entities.WareHouse, TheyAreBillions";
        private const string OILPOOL_TYPE = @"ZX.Entities.OilSource, TheyAreBillions";

        private const string GIANT_BEHAVIOUR_TYPE = @"ZX.Behaviours.BHZombieGiant, TheyAreBillions";
        private const string MUTANT_BEHAVIOUR_TYPE = @"ZX.Behaviours.BHZombie, TheyAreBillions";

        private const string GIANT_ID_TEMPLATE = @"6179780658058987152";
        private const string MUTANT_ID_TEMPLATE = @"4885015758634569309";
        private const string VOD_LARGE_ID_TEMPLATE = @"3441286325348372349";
        private const string VOD_MEDIUM_ID_TEMPLATE = @"293812117068830615";
        private const string VOD_SMALL_ID_TEMPLATE = @"8702552346733362645";
        private const string OILPOOL_ID_TEMPLATE = @"14597207313853823957";
        //private const string foodTruck_ID_TEMPLATE = @"x";
        //private const string turretDefence_ID_TEMPLATE = @"x";    // Multiple types, per compass direction?
        //private const string pickableRubble_ID_TEMPLATE = @"x";
        //private const string energyMast_ID_TEMPLATE = @"x";
        //private const string bigForestMast_ID_TEMPLATE = @"x";
        //private const string volcano_ID_TEMPLATE = @"x";          // Multiple sizes?

        private const string GIANT_LIFE = @"10000";
        private const string MUTANT_LIFE = @"4000";
        private const string VOD_LARGE_LIFE = @"4000";
        private const string VOD_MEDIUM_LIFE = @"1500";
        private const string VOD_SMALL_LIFE = @"400";

        private const string GIANT_SIZE = @"1.6;1.6";
        private const string MUTANT_SIZE = @"0.8;0.8";
        private const string VOD_LARGE_SIZE = @"4;4";
        private const string VOD_MEDIUM_SIZE = @"3;3";
        private const string VOD_SMALL_SIZE = @"2;2";

        private const string GIANT_PROJECT_IMAGE = @"5072922660204167778";
        private const string MUTANT_PROJECT_IMAGE = @"3097669356589096184";

        // "&" will be encoded to "&amp;" when setting XAttribute value
        private const string GENERATORS_2_DIRECTIONS = @"N & E | E & S | S & W | W & N | N & S | E & W";
        private const string GENERATORS_3_DIRECTIONS = @"E & S & W | S & W & N | W & N & E | N & E & S";

        private const int RESOURCE_STORE_CAPACITY = 50;
        private const int GOLD_STORAGE_FACTOR = 40;

        private const UInt64 FIRST_NEW_ID = 0x8000000000000000;

        private class HugeZombieIconComparer : IComparer<HugeZombieIcon>
        {
            public int Compare( HugeZombieIcon a, HugeZombieIcon b )
            {
                return b.getDistanceSquared().CompareTo( a.getDistanceSquared() );  // b.CompareTo( a ) for reversed order
            }
        }
        private static readonly IComparer<HugeZombieIcon> iconDistanceComparer = new HugeZombieIconComparer();

        private class HugeZombie
        {
            readonly XElement entityItem;
            public readonly ulong id;
            private readonly XElement complex;
            private bool isMutantNotGiant;
            private XElement components;
            private XElement behaviour;
            private XElement cMovable;
            private XAttribute entityPositionValue;

            public HugeZombie( XElement i )
            {
                entityItem = i;
                id = (ulong) entityItem.Element( "Simple" ).Attribute( "value" );

                complex = entityItem.Element( "Complex" );
                isMutantNotGiant = (string) complex.Attribute( "type" ) == MUTANT_TYPE;

                //Console.WriteLine( ( isMutantNotGiant ? "Mut" : "Gi" ) + "ant:\t\t" + id );
            }

            internal bool IsMutantNotGiant()
            {
                return isMutantNotGiant;
            }

            private XElement getComponents()
            {
                if( components == null )
                {
                    components = SaveEditor.getComponents( complex );
                }
                return components;
            }

            private XElement getBehaviour()
            {
                if( behaviour == null )
                {
                    XElement currentCBehaviour = getComplexItemOfType( getComponents(), CBEHAVIOUR_TYPE );
                    behaviour = getFirstComplexPropertyNamed( currentCBehaviour, "Behaviour" );
                }
                return behaviour;
            }

            private XElement getMovable()
            {
                if( cMovable == null )
                {
                    cMovable = getComplexItemOfType( getComponents(), CMOVABLE_TYPE );
                }
                return cMovable;
            }

            private XAttribute getPositionValue()
            {
                if( entityPositionValue == null )
                {
                    entityPositionValue = getFirstSimplePropertyNamed( complex, "Position" ).Attribute( "value" );
                }
                return entityPositionValue;
            }

            internal void relocate( HugeZombie other )
            {
                if( this.id == other.id )
                {
                    return;
                }

                // We're after 3 different values in 2 different <Complex under Components
                // 1
                XElement currentBehaviourTargetPosition = getFirstSimplePropertyNamed( getFirstComplexPropertyNamed( getBehaviour(), "Data" ), "TargetPosition" );
                
                // 2
                XElement currentMovableTargetPosition = getFirstSimplePropertyNamed( getMovable(), "TargetPosition" );
                // 3
                XElement currentLastDestinyProcessed = getFirstSimplePropertyNamed( getMovable(), "LastDestinyProcessed" );
                
                string farthestPositionString = (string) other.getPositionValue();
                
                getPositionValue().SetValue( farthestPositionString );
                getFirstSimplePropertyNamed( complex, "LastPosition" ).SetAttributeValue( "value", farthestPositionString );
                if( currentBehaviourTargetPosition != null )
                {
                    currentBehaviourTargetPosition.SetAttributeValue( "value", farthestPositionString );
                }
                if( currentMovableTargetPosition != null )
                {
                    currentMovableTargetPosition.SetAttributeValue( "value", farthestPositionString );
                }
                if( currentLastDestinyProcessed != null )
                {
                    currentLastDestinyProcessed.SetAttributeValue( "value", farthestPositionString );
                }
            }

            internal void delete()
            {
                entityItem.Remove();
            }

            internal void swapZombieType()
            {
                /*
                 * change the <Item><Complex type=> from ZX.Entities.ZombieMutant to ZombieGiant
                 * change the <Item><Complex><Properties><Simple name= value=> IDTemplate, Flags, Size
                 * change the <Item><Complex><Properties><Collection name="Components"><Items><Complex type="ZX.Components.CLife..."><Simple name="Life" value="4000" /> life value
                 * 
                 * mutants have <Item><Complex><Properties><Collection name="Components"><Items><Complex type="ZX.Components.CInflamable..."> but giants don't
                 * 
                 * replace all <Item><Complex><Properties><Collection name="Components"><Items><Complex type="ZX.Components.CBehaviour...>? But keep all
                 * ...<Properties><Complex name="Behaviour"><Properties><Complex name="Data"><Properties><Complex name="InternalData"><Properties><Collection name="OpenNodes"><Items><Simple value=...> values?
                 * A lot of ID numbers are paired with state numbers under ..<Complex name="InternalData"><Properties><Dictionary name="MemCompositorCurrentChild"><Items><Item><Simple value=>
                 * 
                 * The NodeStates Dictionary has Items that aren't unique to each zombie entity, or uncommon between save files...
                 * 
                 * <Complex type="ZX.Components.CMovable...><Properties><Collection name="Path"><Properties><Simple name="Capacity" value="0" /> is value 4 for Giants
                 * 
                 * (It seems <Simple name="BehaviourModelCheckSum" value=> is always "2014060645", for Mutants or Giants, or Rangers, Ravens, etc)
                 * 
                 * and reset <Item><Complex><Properties><Collection name="Components"><Items><Complex type="ZX.Components.CMovable..."><Properties><Null name="TargetPosition" /> ?
                 * 
                 */

                XElement cLife = getComplexItemOfType( getComponents(), CLIFE_TYPE );

                XElement path = ( from c in getMovable().Element( "Properties" ).Elements( "Collection" )
                                  where (string) c.Attribute( "name" ) == "Path"
                                  select c ).SingleOrDefault();

                string type;
                string flags;
                string template;
                string life;
                string behaviour;
                string pathCapacity;
                string size;
                if( isMutantNotGiant )
                {
                    type = GIANT_TYPE;
                    flags = "None";
                    template = GIANT_ID_TEMPLATE;
                    life = GIANT_LIFE;
                    behaviour = GIANT_BEHAVIOUR_TYPE;
                    pathCapacity = "4";
                    size = GIANT_SIZE;

                    XElement cInflamable = getComplexItemOfType( getComponents(), CINFLAMABLE, false );
                    if( cInflamable != null )
                    {
                        cInflamable.Remove();
                    }
                }
                else
                {
                    type = MUTANT_TYPE;
                    flags = "IsOneCellSize";
                    template = MUTANT_ID_TEMPLATE;
                    life = MUTANT_LIFE;
                    behaviour = MUTANT_BEHAVIOUR_TYPE;
                    pathCapacity = "0";
                    size = MUTANT_SIZE;

                    // Doesn't yet add this element for Giants that lack it...
                    string cInflamable = @"<Complex type=""ZX.Components.CInflamable, TheyAreBillions"">
                        <Properties>
                          <Complex name=""EntityRef"">
                            <Properties>
                              <Simple name=""IDEntity"" value=""0"" />
                            </Properties>
                          </Complex>
                          <Null name=""Fire"" />
                          <Simple name=""TimeUnderFire"" value=""0"" />
                        </Properties>
                      </Complex>";
                }

                complex.Attribute( "type" ).SetValue( type );
                getFirstSimplePropertyNamed( complex, "Flags" ).Attribute( "value" ).SetValue( flags );
                getFirstSimplePropertyNamed( complex, "IDTemplate" ).Attribute( "value" ).SetValue( template );
                getFirstSimplePropertyNamed( cLife, "Life" ).Attribute( "value" ).SetValue( life );
                getBehaviour().Attribute( "type" ).SetValue( behaviour );
                getFirstSimplePropertyNamed( path, "Capacity" ).Attribute( "value" ).SetValue( pathCapacity );
                getFirstSimplePropertyNamed( complex, "Size" ).Attribute( "value" ).SetValue( size );

                isMutantNotGiant = !isMutantNotGiant;
            }
        }

        private class HugeZombieIcon
        {
            private readonly SaveEditor editor;
            private readonly XElement icon; //<Complex type="ZX.ZXMiniMapIndicatorInfo...">
            public readonly ulong id;
            private readonly XElement image;    //<Simple name="IDProjectImage">
            private bool isMutantNotGiant;
            private XAttribute iconCellValue;
            private CompassDirection? dir;  // Nullability tracks lazy calculating of this and distance
            private int distanceSquared;

            public HugeZombieIcon( SaveEditor e, XElement i )
            {
                editor = e;
                icon = i;

                //                 <Complex name="EntityRef">
                //                   <Properties >
                //                     <Simple name = "IDEntity"
                id = (ulong) getFirstComplexPropertyNamed( icon, "EntityRef" ).Element( "Properties" ).Element( "Simple" ).Attribute( "value" );

                image = getFirstSimplePropertyNamed( icon, "IDProjectImage" );
                isMutantNotGiant = (string) image.Attribute( "value" ) == MUTANT_PROJECT_IMAGE;

                dir = null;

                //Console.WriteLine( ( isMutantNotGiant ? "Mut" : "Gi" ) + "ant Icon:\t" + id );
            }

            private XAttribute getCellValue()
            {
                if( iconCellValue == null )
                {
                    XElement cellSimple = getFirstSimplePropertyNamed( icon, "Cell" );
                    iconCellValue = cellSimple.Attribute( "value" );
                }
                return iconCellValue;
            }

            internal bool IsMutantNotGiant()
            {
                return isMutantNotGiant;
            }

            internal CompassDirection getDirection()
            {
                if( dir == null )
                {
                    assessPosition();
                }
                return (CompassDirection) dir;
            }

            internal int getDistanceSquared()
            {
                if( dir == null )
                {
                    assessPosition();
                }
                return distanceSquared;
            }

            private void assessPosition()
            {
                string xy = (string) getCellValue();
                //Console.WriteLine( "Cell:\t\t" + xy );
                string[] xySplit = xy.Split( ';' );
                int x = int.Parse( xySplit[0] );
                int y = int.Parse( xySplit[1] );

                if( x <= editor.commandCenterX )
                {
                    // North or West
                    dir = y <= editor.commandCenterY ? CompassDirection.North : CompassDirection.West;
                }
                else
                {
                    // East or South
                    dir = y <= editor.commandCenterY ? CompassDirection.East : CompassDirection.South;
                }

                distanceSquared = ( x - editor.commandCenterX ) * ( x - editor.commandCenterX ) + ( y - editor.commandCenterY ) * ( y - editor.commandCenterY );

                //Console.WriteLine( ( isMutantNotGiant ? "Mut" : "Gi" ) + "ant:\t\t" + id + ",\tPosition: " + x + ", " + y + "\tis: " + dir + ",\tdistanceSquared: " + distanceSquared );
            }

            internal void relocate( HugeZombieIcon otherIcon )
            {
                if( this.id == otherIcon.id )
                {
                    return;
                }

                getCellValue().SetValue( otherIcon.getCellValue().Value );
                dir = null;     // no need to trigger assessPosition() yet

                //Console.WriteLine( "Mutant icon: " + id + " relocated to: " + getCellValue().Value );
            }

            internal void delete()
            {
                icon.Remove();
            }

            internal void swapZombieType()
            {
                // replace the <Complex type="ZX.ZXMiniMapIndicatorInfo...><Properties> minimap icon's <Simple name="IDProjectImage" value=> and <Simple name="Title" value=> & "Text"

                string project_image;
                string text;
                string title;
                if( isMutantNotGiant )
                {
                    project_image = GIANT_PROJECT_IMAGE;
                    text = @"It was previously detected as a Mutant.";
                    title = @"Infected Giant";
                }
                else
                {
                    project_image = MUTANT_PROJECT_IMAGE;
                    text = @"It was previously detected as a Giant.";
                    title = @"Infected Mutant";
                }

                image.Attribute( "value" ).SetValue( project_image );
                getFirstSimplePropertyNamed( icon, "Text" ).Attribute( "value" ).SetValue( text );
                getFirstSimplePropertyNamed( icon, "Title" ).Attribute( "value" ).SetValue( title );

                isMutantNotGiant = !isMutantNotGiant;
            }
        }

        private readonly string dataFile;
        private readonly XElement data;
        private readonly XElement levelComplex;
        private XElement levelEntitiesItems;
        private XElement generatedLevel;
        private XElement extension;
        private XElement mapDrawer;
        private readonly int commandCenterX;
        private readonly int commandCenterY;
        private readonly SortedDictionary<ulong, HugeZombie> mutants;
        private readonly SortedDictionary<ulong, HugeZombie> giants;
        private readonly LinkedList<HugeZombieIcon> mutantIcons;
        private readonly LinkedList<HugeZombieIcon> giantIcons;
        private UInt64 nextNewID;

        private static XElement getFirstPropertyOfTypeNamed( XElement c, string type, string name )
        {
            return ( from s in c.Element( "Properties" ).Elements( type )
                     where (string) s.Attribute( "name" ) == name
                     select s ).FirstOrDefault();   // Avoid exception risk of First()?
        }
        
        private static XElement getFirstSimplePropertyNamed( XElement c, string name )
        {
            return getFirstPropertyOfTypeNamed( c, "Simple", name );
        }

        private static XElement getFirstComplexPropertyNamed( XElement c, string name )
        {
            return getFirstPropertyOfTypeNamed( c, "Complex", name );
        }

        private static XElement getComponents( XElement complex )
        {
            return ( from c in complex.Element( "Properties" ).Elements( "Collection" )
                     where (string) c.Attribute( "name" ) == "Components"
                     select c ).Single();
        }

        private static XElement getComplexItemOfType( XElement components, string type, bool assumeExists = true )
        {
            var i = ( from c in components.Element( "Items" ).Elements( "Complex" )
                     where (string) c.Attribute( "type" ) == type
                     select c );
            return assumeExists ? i.Single() : i.SingleOrDefault();
        }


        public SaveEditor( string filesPath )
        {
            dataFile = Path.Combine( filesPath, "Data" );
            if( !File.Exists( dataFile ) )
            {
                throw new ArgumentException( "Data file does not exist: " + dataFile );
            }

            data = XElement.Load( dataFile );

            mutants = new SortedDictionary<ulong, HugeZombie>();
            giants = new SortedDictionary<ulong, HugeZombie>();

            //<Complex name="Root" type="ZX.ZXGameState, TheyAreBillions">
            //  < Properties >
            //    <Complex name="LevelState">
            levelComplex = getFirstComplexPropertyNamed( data, "LevelState" );


            //      <Properties>
            //        <Simple name="CurrentCommandCenterCell"
            XElement currentCommandCenterCell = getFirstSimplePropertyNamed( levelComplex, "CurrentCommandCenterCell" );
            string xy = (string) currentCommandCenterCell.Attribute( "value" );
            string[] xySplit = xy.Split( ';' );
            commandCenterX = int.Parse( xySplit[0] );
            commandCenterY = int.Parse( xySplit[1] );

            //Console.WriteLine( "CurrentCommandCenterCell: " + commandCenterX + ", " + commandCenterY );

            mutantIcons = new LinkedList<HugeZombieIcon>();
            giantIcons = new LinkedList<HugeZombieIcon>();

            nextNewID = 0;
        }

        internal void save()    // Doesn't take parameter to save modified file to a different location?
        {
            data.Save( dataFile );
        }

        private IEnumerable<XElement> getLevelZombieTypesItems()
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

        private UInt64 getHighestID()
        {
            // Get all the UInt64 entity IDs, from LevelEntities, LevelFastSerializedEntities, ExtraEntities

            SortedSet<UInt64> uniqueIDs = new SortedSet<UInt64>();
            void addIDs( IEnumerable<string> ids )
            {
                foreach( string i in ids )
                {
                    UInt64 id = Convert.ToUInt64( i );
                    if( !uniqueIDs.Add( id ) )
                    {
                        Console.Error.WriteLine( "Duplicate ID found: " + i );
                    }
                }
            }

            IEnumerable<string> entityIDs =
                from i in getLevelEntitiesItems().Elements( "Item" )
                select i.Element( "Simple" ).Attribute( "value" ).Value;
            //Console.WriteLine( "entityIDs: " + entityIDs.Count() );
            addIDs( entityIDs );
            //Console.WriteLine( "Unique IDs: " + uniqueIDs.Count );

            /*
             *        <Dictionary name="LevelFastSerializedEntities" >
             *          <Items>
             *            <Item>
             *              <Simple />
             *              <Collection >
             *                <Items>
             *                  <Complex>
             *                    <Properties>
             *                      <Simple name="A" value=
             */
            var fastItems = from zombieType in getLevelZombieTypesItems()
                            select zombieType.Element( "Collection" ).Element( "Items" );
            //Console.WriteLine( "zombieTypes: " + fastItems.Count() );
            var fastIDs = from c in fastItems.Elements( "Complex" )
                          let s = c.Element( "Properties" ).Elements( "Simple" )
                          from a in s
                          where (string) a.Attribute( "name" ) == "A"
                          select (string) a.Attribute( "value" );
            //Console.WriteLine( "fastIDs: " + fastIDs.Count() );
            addIDs( fastIDs );
            //Console.WriteLine( "Unique IDs: " + uniqueIDs.Count );

            /*
             *          <Collection name="ExtraEntities" >
                          <Items>
                            <Complex>
                              <Properties>
                                <Simple name="ID" value="
             */
            XElement extrasItems = getFirstPropertyOfTypeNamed( getMapDrawer(), "Collection", "ExtraEntities" ).Element( "Items" );
            var extrasIDs = from c in extrasItems.Elements( "Complex" )
                            select getFirstSimplePropertyNamed( c, "ID" ).Attribute( "value" ).Value;
            //Console.WriteLine( "extrasIDs: " + extrasIDs.Count() );
            addIDs( extrasIDs );
            //Console.WriteLine( "Unique IDs: " + uniqueIDs.Count );
            /*
            void dumpIDs()
            {
                StringBuilder ids = new StringBuilder( uniqueIDs.Count * 16 );  // 16 hex chars per 64 bit uint64
                foreach( UInt64 id in uniqueIDs )
                {
                    ids.AppendFormat( "{0:X16}\n", id );
                }
                DirectoryInfo saveDir = Directory.GetParent( dataFile );
                string idFile = Path.Combine( saveDir.Parent.FullName, saveDir.Name + "_IDs.txt" );     // In the edits directory, file named after the save
                Console.WriteLine( "Dumping IDs to " + idFile );
                File.WriteAllText( idFile, ids.ToString() );
            }
            //dumpIDs();
            */
            return uniqueIDs.Last();
        }

        private UInt64 getNewID()
        {
            if( nextNewID == 0 )    // Need to actually assess whether this file has already had greater-than-FIRST_NEW_ID values used.
            {
                UInt64 currentHighestID = getHighestID();
                nextNewID = FIRST_NEW_ID > currentHighestID ? FIRST_NEW_ID : currentHighestID + 1;
                //Console.WriteLine( "nextNewID: " + nextNewID );
            }

            return nextNewID++;
            /*
             * IDs seem to be (needlessly?) random, not just unique.
             * But not beyond max positive signed 64bit int (0x7FFF...), yet using an unsigned 64bit int. Room for 0x8000...+
             * The naming of <Complex name="Random"><Properties> <Simple name="Mt" ... indicates Mersenne Twister PRNG.
             * 
             * The value of this Random changes upon starting to build a new Tent:
             *         <Complex name="CurrentGeneratedLevel">
             *           <Properties>
             *             <Complex name="Random">
             *               <Properties>
             *                 <Simple name="Mt" value=
             * Also found at:
             *             <Complex name="Data">
             *               <Properties>
             *                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
             *                   <Properties>
             *                     <Complex name="MapDrawer">
             *                       <Properties>
             *                         <Collection name="ExtraEntities"
             *                         </Collection>
             *                         <Complex name="Random">
             *                           <Properties>
             *                             <Simple name="Mt" value=
             * 
             * The other Random values, for the level as a whole, and the swarm generators (which are stored in duplicate), don't change after the single Tent building action.
             * Resetting the save and repeating the action does not result in the same ID being generated, or same Random value being saved.
             */
             /*
              * An alternative solution would be to assume some zombies/ExtraEntities still remain, and remove 1 & all references to it (attack target, etc), to reuse their ID.
              * This only really works for relatively few, higher impact use-cases of 'new' IDs.
              */
        }

        private XElement getLevelEntitiesItems()
        {
            if( levelEntitiesItems == null )
            {
                levelEntitiesItems = getFirstPropertyOfTypeNamed( levelComplex, "Dictionary", "LevelEntities" ).Element( "Items" );
            }
            return levelEntitiesItems;
        }

        private XElement getGeneratedLevel()
        {
            if( generatedLevel == null )
            {
                generatedLevel = getFirstComplexPropertyNamed( levelComplex, "CurrentGeneratedLevel" );
            }
            return generatedLevel;
        }

        private XElement getDataExtension()
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

        private IEnumerable<XElement> getLevelEntitiesOfTypes( params string[] types )
        {
            return
                from i in getLevelEntitiesItems().Elements( "Item" )
                where
                    ( from c in i.Elements( "Complex" )
                      let type = (string) c.Attribute( "type" )
                      where types.Contains( type )
                      select c ).Any()
                select i;
        }

        internal void scalePopulation( decimal scale )
        {
            if( scale < 0.0M )
            {
                throw new ArgumentOutOfRangeException( "Scale must not be negative." );
            }

            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            SortedDictionary<ScalableZombieTypes, decimal> scalableZombieTypeFactors = new SortedDictionary<ScalableZombieTypes, decimal>();
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWeakA, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWeakB, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWeakC, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWorkerA, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWorkerB, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieMediumA, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieMediumB, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieDressedA, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieStrongA, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieVenom, scale );
            scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieHarpy, scale );
            scalePopulation( scalableZombieTypeFactors );
        }

        internal void scalePopulation( SortedDictionary<ScalableZombieGroups, decimal> scalableZombieGroupFactors )
        {
            SortedDictionary<ScalableZombieTypes, decimal> scalableZombieTypeFactors = new SortedDictionary<ScalableZombieTypes, decimal>();

            foreach( var k_v in scalableZombieGroupFactors )
            {
                decimal scale = k_v.Value;

                if( scale < 0.0M )
                {
                    throw new ArgumentOutOfRangeException( "Scale must not be negative." );
                }
                if( scale == 1.0M )
                {
                    continue;     // Nothing needs be done
                }

                switch( k_v.Key )
                {
                    case ScalableZombieGroups.WEAK:
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWeakA, scale );
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWeakB, scale );
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWeakC, scale );
                        break;
                    case ScalableZombieGroups.MEDIUM:
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWorkerA, scale );
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieWorkerB, scale );
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieMediumA, scale );
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieMediumB, scale );
                        break;
                    case ScalableZombieGroups.DRESSED:
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieDressedA, scale );
                        break;
                    case ScalableZombieGroups.STRONG:
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieStrongA, scale );
                        break;
                    case ScalableZombieGroups.VENOM:
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieVenom, scale );
                        break;
                    case ScalableZombieGroups.HARPY:
                        scalableZombieTypeFactors.Add( ScalableZombieTypes.ZombieHarpy, scale );
                        break;
                    default:
                        break;
                }
            }

            scalePopulation( scalableZombieTypeFactors );
        }

        private void scalePopulation( SortedDictionary<ScalableZombieTypes, decimal> scalableZombieTypeFactors )
        {
            /*
             *        <Dictionary name="LevelFastSerializedEntities" >
             *          <Items>
             *            <Item>
             *              <Simple />
             *              <Collection >
             *                <Items>
             *                  <Complex>
             *                    <Properties>
             *                      <Simple name="A" value=
             */

            void duplicateZombie( XElement com, LinkedList<XElement> copiedZombies )
            {
                XElement zCopy = new XElement( com );       // Duplicate at the same position
                ( from s in zCopy.Element( "Properties" ).Elements( "Simple" )
                  where (string) s.Attribute( "name" ) == "A"
                  select s ).First().SetAttributeValue( "value", getNewID() );
                copiedZombies.AddLast( zCopy );
            }

            XAttribute getCapacity( XElement col )
            {
                // Get the capacity for each ZombieType
                /*
                 *             <Item>
                 *               <Simple />
                 *               <Collection >
                 *                 <Properties>
                 *                   <Simple name="Capacity" value=
                 * 
                 */
                return ( from s in col.Element( "Properties" ).Elements( "Simple" )
                         where (string) s.Attribute( "name" ) == "Capacity"
                         select s ).First().Attribute( "value" );
            }

            Random rand = new Random();
            var selectedZombies = new LinkedList<XElement>();

            var zombieTypes = getLevelZombieTypesItems();

            foreach( var t in zombieTypes )
            {
                UInt64 zombieTypeInt = Convert.ToUInt64( t.Element( "Simple" ).Attribute( "value" ).Value );
                //Console.WriteLine( "zombieTypeInt: " + zombieTypeInt );
                if( !Enum.IsDefined( typeof( ScalableZombieTypes ), zombieTypeInt ) )
                {
                    continue;   // Can't scale this type
                }

                ScalableZombieTypes zombieType = (ScalableZombieTypes) zombieTypeInt;
                if( !scalableZombieTypeFactors.ContainsKey( zombieType ) )
                {
                    continue;   // Won't be scaling this type
                }

                decimal scale = scalableZombieTypeFactors[zombieType];
                int multiples = (int) scale;                // How many duplicates to certainly make of each zombie
                double chance = (double) ( scale % 1 );     // The chance of making 1 more duplicate per zombie
                //Console.WriteLine( "multiples: " + multiples + ", chance: " + chance );

                var col = t.Element( "Collection" );

                if( scale < 1.0M )
                {
                    // chance is now chance to not remove existing zombies
                    // selectedZombies will be those removed

                    if( scale == 0.0M )
                    {
                        // No need to iterate and count
                        col.Element( "Items" ).RemoveNodes();

                        getCapacity( col ).SetValue( 0 );
                    }
                    else
                    {
                        // 0 > scale < 1
                        foreach( var com in col.Element( "Items" ).Elements( "Complex" ) )
                        {
                            if( chance < rand.NextDouble() )
                            {
                                // Removing within foreach doen't work, without .ToList(). Might as well collect candidates so we also have an O(1) count for later too
                                selectedZombies.AddLast( com );
                            }
                        }
                        //Console.WriteLine( "selectedZombies: " + selectedZombies.Count );

                        foreach( var i in selectedZombies )
                        {
                            i.Remove();
                        }

                        // Update capacity count
                        var cap = getCapacity( col );
                        UInt64 newCap = (UInt64) ( Convert.ToInt32( cap.Value ) - selectedZombies.Count );  // Assume actual UInt64 Capacity value is positive Int32 size and no less than Count removed
                        cap.SetValue( newCap );
                        //Console.WriteLine( "newCap: " + newCap );

                        selectedZombies.Clear();
                    }
                }
                else
                {

                    foreach( var com in col.Element( "Items" ).Elements( "Complex" ) )
                    {
                        // First the certain duplications
                        for( int m = multiples; m <= multiples; m++ )
                        {
                            duplicateZombie( com, selectedZombies );
                        }
                        // And now the chance-based duplication
                        if( chance >= rand.NextDouble() )   // If the chance is not less than the roll
                        {
                            duplicateZombie( com, selectedZombies );
                        }
                    }
                    //Console.WriteLine( "selectedZombies: " + selectedZombies.Count );

                    foreach( var i in selectedZombies )
                    {
                        col.Element( "Items" ).Add( i );
                    }

                    // Update capacity count
                    var cap = getCapacity( col );
                    UInt64 newCap = Convert.ToUInt64( cap.Value ) + (UInt64) selectedZombies.Count;
                    cap.SetValue( newCap );
                    //Console.WriteLine( "newCap: " + newCap );

                    selectedZombies.Clear();
                }
            }
        }

        private void findMutantsAndGiants()
        {
            IEnumerable<XElement> bigBoys = getLevelEntitiesOfTypes( MUTANT_TYPE, GIANT_TYPE );

            //Console.WriteLine( "bigBoys count: " + bigBoys.Count() );
            foreach( XElement big in bigBoys )
            {
                HugeZombie z = new HugeZombie( big );
                ( z.IsMutantNotGiant() ? mutants : giants ).Add( z.id, z );
            }

            XElement iconsColl = getFirstPropertyOfTypeNamed( levelComplex, "Collection", "MiniMapIndicators" );

            // Find all Mutant minimap icons
            foreach( XElement c in iconsColl.Element( "Items" ).Elements( "Complex" ) )
            {
                HugeZombieIcon z = new HugeZombieIcon( this, c );
                if( z.IsMutantNotGiant() )
                {
                    mutantIcons.AddLast( z );
                }
                else
                {
                    giantIcons.AddLast( z );
                }
            }

            //Console.WriteLine( "Mutants count: " + mutants.Count() + ", icons count: " + mutantIcons.Count() );
            //Console.WriteLine( "Giants count: " + giants.Count() + ", icons count: " + giantIcons.Count() );
        }

        internal void removeMutants()
        {
            findMutantsAndGiants();

            // Remove mutants and their icons
            foreach( HugeZombie z in mutants.Values )
            {
                z.delete();
            }
            foreach( HugeZombieIcon i in mutantIcons )
            {
                i.delete();
            }

        }

        internal void replaceHugeZombies( bool toGiantNotMutant )
        {
            findMutantsAndGiants();

            if( toGiantNotMutant && !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to replace." );
                return;
            }
            if( !toGiantNotMutant && !giants.Any() )
            {
                //Console.WriteLine( "No Giants to replace." );
                return;
            }

            /*
             * for each HugeZombie:
             * 
             * Change their Type, Life, etc.
             * Reset their behaviour, and target positions, but keep OpenNodes IDs?
             * 
             */
            var fromDictionary = toGiantNotMutant ? mutants : giants;
            var toDictionary = toGiantNotMutant ? giants : mutants;
            foreach( var z in fromDictionary.Values )
            {
                z.swapZombieType();
                toDictionary.Add( z.id, z );
            }
            fromDictionary.Clear();

            // Also swap the Icons types for associated EntityRef IDEntity values
            LinkedList<HugeZombieIcon> fromList = toGiantNotMutant ? mutantIcons : giantIcons;
            LinkedList<HugeZombieIcon> toList = toGiantNotMutant ? giantIcons : mutantIcons;
            foreach( var icon in fromList )
            {
                icon.swapZombieType();
                toList.AddLast( icon );
            }
            fromList.Clear();
        }

        internal void relocateMutants( bool toGiantNotMutant, bool perDirection )
        {
            findMutantsAndGiants();

            if( !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to relocate." );
                return;
            }

            // Globally, or per direction, we'll have a list of huge zombie icons to find the farthest of, and a list of mutants to relocate, as well as their corresponding icons?

            SortedDictionary<CompassDirection,List<HugeZombieIcon>> mutantsPerDirection = new SortedDictionary<CompassDirection,List<HugeZombieIcon>>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                mutantsPerDirection.Add( d, new List<HugeZombieIcon>( mutants.Count ) );
            }
            SortedDictionary<CompassDirection,List<HugeZombieIcon>> giantsPerDirection = new SortedDictionary<CompassDirection,List<HugeZombieIcon>>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                giantsPerDirection.Add( d, new List<HugeZombieIcon>( giants.Count ) );
            }
            List<HugeZombieIcon> farthestMutantShortlist = new List<HugeZombieIcon>();
            List<HugeZombieIcon> farthestGiantShortlist = new List<HugeZombieIcon>();


            // Split icons by direction
            foreach( HugeZombieIcon mutant in mutantIcons )
            {
                mutantsPerDirection[mutant.getDirection()].Add( mutant );
            }
            foreach( HugeZombieIcon giant in giantIcons )
            {
                giantsPerDirection[giant.getDirection()].Add( giant );
            }

            // Sort each direction, take the farthest, form a new shortlist and sort that for the overall farthest
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                List<HugeZombieIcon> mutantsInDirection = mutantsPerDirection[d];
                if( mutantsInDirection.Count > 0 )
                {
                    mutantsInDirection.Sort( iconDistanceComparer );
                    farthestMutantShortlist.Add( mutantsInDirection.First() );
                }
            }
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                List<HugeZombieIcon> giantsInDirection = giantsPerDirection[d];
                if( giantsInDirection.Count > 0 )
                {
                    giantsInDirection.Sort( iconDistanceComparer );
                    farthestGiantShortlist.Add( giantsInDirection.First() );
                }
            }

            farthestMutantShortlist.Sort( iconDistanceComparer );
            farthestGiantShortlist.Sort( iconDistanceComparer );
            HugeZombieIcon globalFarthestMutant = farthestMutantShortlist.FirstOrDefault();
            HugeZombieIcon globalFarthestGiant = farthestGiantShortlist.FirstOrDefault();
            

            if( perDirection )
            {
                // Get sorted mutant group per direction, also giants if toGiantNotMutant
                // Get farthest group mutant, or giant if toGiantNotMutant, or global farthest (could be either) if no local giant
                // relocate all group mutants to chosen farthest

                foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
                {
                    List<HugeZombieIcon> mutantsInDirection = mutantsPerDirection[d];
                    if( mutantsInDirection.Count > 0 )
                    {
                        // There are mutants in this direction to relocation

                        HugeZombieIcon relocateTo = null;
                        // Should and could we use a Giant?
                        if( toGiantNotMutant )
                        {
                            List<HugeZombieIcon> giantsInDirection = giantsPerDirection[d];
                            if( giantsInDirection.Count > 0 )
                            {
                                relocateTo = giantsInDirection.First();
                            }
                            else
                            {
                                //Console.WriteLine( "No Giants to relocate Mutants onto in direction: " + d + ", using global farthest Giant." );
                                relocateTo = globalFarthestGiant;
                                if( relocateTo == null )
                                {
                                    //Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
                                    relocateTo = globalFarthestMutant;
                                }
                            }
                        }
                        else
                        {
                            relocateTo = mutantsInDirection.First();
                        }

                        relocateMutants( mutantsInDirection, relocateTo );
                    }
                }
            }
            else
            {
                // Should and could we use a Giant?
                HugeZombieIcon farthestIcon = toGiantNotMutant ? globalFarthestGiant : globalFarthestMutant;
                if( farthestIcon == null )
                {
                    //Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
                    farthestIcon = globalFarthestMutant;
                }

                relocateMutants( mutantIcons, farthestIcon );
            }

        }

        private void relocateMutants( ICollection<HugeZombieIcon> mutantIcons, HugeZombieIcon farthestIcon )
        {
            foreach( HugeZombieIcon icon in mutantIcons )
            {
                HugeZombie mutant = mutants[icon.id];
                icon.relocate( farthestIcon );
                HugeZombie farthest = ( farthestIcon.IsMutantNotGiant() ? mutants : giants )[farthestIcon.id];
                mutant.relocate( farthest );
            }
        }

        private IEnumerable<XElement> getVODs()
        {
            return getLevelEntitiesOfTypes( VOD_SMALL_TYPE, VOD_MEDIUM_TYPE, VOD_LARGE_TYPE );
            //Console.WriteLine( "vodItems: " + vodItems.Count() );
        }

        internal void resizeVODs( VodSizes vodSize )
        {
            /*
             * To change VOD building sizes:
             * Change the <Complex type=
             * Change the   <Properties>
                              <Simple name="IDTemplate" value=
             * Change the     <Collection name="Components" elementType="DXVision.DXComponent, DXVision">
                                <Items>
                                  <Complex type="ZX.Components.CLife, TheyAreBillions">
                                    <Properties>
                                      <Simple name="Life" value=
             * Change the
                  <Simple name="Size" value=
             *
             * Ignore?        <Complex type="ZX.Components.CTransparentIfNearUnits, TheyAreBillions">
                                <Properties>
                                  <Simple name="HiddingUnits" value="False" />
                                  <Simple name="CheckHiddingUnits" value="False" />
             */

            IEnumerable<XElement> vodItems = getVODs();

            string newType;
            string newIDTemplate;
            string newLife;
            string newSize;
            switch( vodSize )
            {
                default:
                case VodSizes.SMALL:
                    newType = VOD_SMALL_TYPE;
                    newIDTemplate = VOD_SMALL_ID_TEMPLATE;
                    newLife = VOD_SMALL_LIFE;
                    newSize = VOD_SMALL_SIZE;
                    break;
                case VodSizes.MEDIUM:
                    newType = VOD_MEDIUM_TYPE;
                    newIDTemplate = VOD_MEDIUM_ID_TEMPLATE;
                    newLife = VOD_MEDIUM_LIFE;
                    newSize = VOD_MEDIUM_SIZE;
                    break;
                case VodSizes.LARGE:
                    newType = VOD_LARGE_TYPE;
                    newIDTemplate = VOD_LARGE_ID_TEMPLATE;
                    newLife = VOD_LARGE_LIFE;
                    newSize = VOD_LARGE_SIZE;
                    break;
            }

            foreach( XElement v in vodItems.ToList() )  // no ToList() leads to only removing 1 <item> per save modify cycle?!
            {
                XElement complex = v.Element( "Complex" );
                complex.SetAttributeValue( "type", newType );
                getFirstSimplePropertyNamed( complex, "IDTemplate" ).SetAttributeValue( "value", newIDTemplate );
                getFirstSimplePropertyNamed( getComplexItemOfType( getComponents( complex ), CLIFE_TYPE ), "Life" ).SetAttributeValue( "value", newLife );
                getFirstSimplePropertyNamed( complex, "Size" ).SetAttributeValue( "value", newSize );
            }
        }

        private void duplicateLevelEntities( IEnumerable<XElement> items, uint multiplier )
        {
            // Assume a collection of LevelEntity Items. For each, generate multiplier-many duplicates in the same position, with new unique IDs. Add them to LevelEntities once all prepared.
            LinkedList<XElement> copiedItems = new LinkedList<XElement>();

            foreach( XElement i in items )
            {
                for( uint m = 1; m < multiplier; m++ )   // Start at 1 because we already have the first copy of each VOD in the map
                {
                    XElement iCopy = new XElement( i );

                    var newID = getNewID();
                    XElement simple = iCopy.Element( "Simple" );
                    simple.SetAttributeValue( "value", newID );
                    XElement complex = iCopy.Element( "Complex" );
                    getFirstSimplePropertyNamed( complex, "ID" ).SetAttributeValue( "value", newID );

                    copiedItems.AddLast( iCopy );
                }
            }
            //Console.WriteLine( "copiedItems: " + copiedItems.Count );

            var e = getLevelEntitiesItems();
            foreach( var i in copiedItems )
            {
                e.Add( i );
            }
        }

        internal void stackVODbuildings( VodSizes size, decimal scale )
        {
            // Could use some add/remove entity delegates, to refactor this and zombie type scaling? Except zombie type collections can be RemoveNodes()'d per type, while level entities are all mixed in 1 collection.
            
            if( scale < 0.0M )
            {
                throw new ArgumentOutOfRangeException( "Scale must not be negative." );
            }

            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            void duplicateVOD( XElement i, LinkedList<XElement> copiedVODs )
            {
                XElement vCopy = new XElement( i );       // Duplicate at the same position
                var newID = getNewID();
                vCopy.Element( "Simple" ).SetAttributeValue( "value", newID );
                getFirstSimplePropertyNamed( vCopy.Element( "Complex" ), "ID" ).SetAttributeValue( "value", newID );

                copiedVODs.AddLast( vCopy );
            }

            Random rand = new Random();
            var selectedVODs = new LinkedList<XElement>();

            string vodType;
            switch( size )
            {
                default:
                case VodSizes.SMALL:
                    vodType = VOD_SMALL_TYPE;
                    break;
                case VodSizes.MEDIUM:
                    vodType = VOD_MEDIUM_TYPE;
                    break;
                case VodSizes.LARGE:
                    vodType = VOD_LARGE_TYPE;
                    break;
            }

            uint multiples = (uint) scale;                // How many duplicates to certainly make of each entity
            double chance = (double) ( scale % 1 );     // The chance of making 1 more duplicate per entity

            if( scale < 1.0M )
            {
                // chance is now chance to not remove existing VODs
                // selectedVODs will be those removed

                // 0 >= scale < 1
                foreach( var i in getLevelEntitiesOfTypes( vodType ) )
                {
                    if( scale == 0.0M || chance < rand.NextDouble() )
                    {
                        selectedVODs.AddLast( i );
                    }
                }
                //Console.WriteLine( "selectedVODs: " + selectedVODs.Count );

                foreach( var i in selectedVODs )
                {
                    i.Remove();
                }
            }
            else
            {

                foreach( var i in getLevelEntitiesOfTypes( vodType ) )
                {
                    // First the certain duplications
                    for( uint m = multiples; m <= multiples; m++ )
                    {
                        duplicateVOD( i, selectedVODs );
                    }
                    // And now the chance-based duplication
                    if( chance >= rand.NextDouble() )   // If the chance is not less than the roll
                    {
                        duplicateVOD( i, selectedVODs );
                    }
                }
                //Console.WriteLine( "selectedVODs: " + selectedVODs.Count );

                var e = getLevelEntitiesItems();
                foreach( var i in selectedVODs )
                {
                    e.Add( i );
                }
            }
        }

        internal void removeFog( uint radius = 0 )
        {
            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            //          <Properties>
            //            <Simple name="NCells" value="256" />
            XElement ncells = getFirstSimplePropertyNamed( getGeneratedLevel(), "NCells" );

            string cells = (string) ncells.Attribute( "value" );
            int size;
            if( !Int32.TryParse( cells, out size ) )
            {
                Console.Error.WriteLine( "Unable to find the number of cells in the map." );
                return;
            }
            int rawLength = 4 * size * size;        // 4 bytes just to store 00 00 00 FF or 00 00 00 00, yuck

            // Sadly we can't use String.Create<TState>(Int32, TState, SpanAction<Char,TState>) to avoid duplicate allocation prior to creating the final string

            byte[] clearFog = new byte[rawLength];  // Defaulting to 00 00 00 00, good if we want less than 50% fog...

            if( radius > 0 )
            {
                circularFog( size, clearFog, radius );
            }

            string layerFog = Convert.ToBase64String( clearFog );
            //Console.WriteLine( layerFog );

            XElement layerFogSimple = getFirstSimplePropertyNamed( levelComplex, "LayerFog" );
            layerFogSimple.SetAttributeValue( "value", layerFog );
        }

        private void circularFog( int size, byte[] clearFog, uint r )
        {
            int radius = (int) r;   // Just assume it'll fit

            void setFog( int s, byte[] f, int x, int y )
            {
                // Assuming linear assignment, rather than anything fancy like a Hilbert curve...
                int wordIndex = ( ( y * s ) + x ) * 4;
                f[wordIndex + 3] = 0xFF;
            }

            void setFogLine( int s, byte[] f, int xStart, int y, int xEnd )
            {
                for( int x = xStart; x < xEnd; x++ )
                {
                    setFog( s, f, x, y );
                }
            }

            // Fill outside the circle with 00 00 00 FF, aka step 4 bytes at a time. Centered on CC
            int beforeCCx = commandCenterX - radius;
            int beforeCCy = commandCenterY - radius;
            int afterCCx = commandCenterX + radius;
            int afterCCy = commandCenterY + radius;

            // Quick fill in "thirds" of before, adjacent, after (CC +- radius)
            for( int x = 0; x < size; x++ )
            {
                // 1/3 before
                for( int y = 0; y < beforeCCy; y++ )
                {
                    setFog( size, clearFog, x, y );
                }
                // 1/3 after
                for( int y = afterCCy; y < size; y++ )
                {
                    setFog( size, clearFog, x, y );
                }
            }

            for( int y = beforeCCy; y < afterCCy; y++ )
            {
                // 1/9 adjacent before
                for( int x = 0; x < beforeCCx; x++ )
                {
                    setFog( size, clearFog, x, y );
                }
                // 1/9 adjacent after
                for( int x = afterCCx; x < size; x++ )
                {
                    setFog( size, clearFog, x, y );
                }
            }
            // Central 1/9 left to do

            // In calculating the xFromCC to the circle edge in a single anticlockwise 45degree octant between y=0 and y=x, we would have all the values to duplicate the octant & its mirror in all 4 corners.

            // We'll just skip messing with Bresenham and use square roots, for a 90degree arc quadrant, and mirror 4 times.
            int radiusSquared = radius * radius;
            int radiusBeforeCommandCenterX = commandCenterX - radius;
            int radiusAfterCommandCenterX = commandCenterX + radius;

            int xFromCC;// = radius;
            int yFromCC = 0;
            while( yFromCC <= radius )
            {
                xFromCC = Convert.ToInt32( Math.Sqrt( radiusSquared - (yFromCC * yFromCC) ) );

                // E to SE to S
                setFogLine( size, clearFog, commandCenterX + xFromCC, commandCenterY + yFromCC, radiusAfterCommandCenterX );

                // W to SW to S
                setFogLine( size, clearFog, radiusBeforeCommandCenterX, commandCenterY + yFromCC, commandCenterX - xFromCC );

                // W to NW to N
                setFogLine( size, clearFog, radiusBeforeCommandCenterX, commandCenterY - yFromCC, commandCenterX - xFromCC );

                // E to NE to N
                setFogLine( size, clearFog, commandCenterX + xFromCC, commandCenterY - yFromCC, radiusAfterCommandCenterX );

                yFromCC += 1;
            }
        }

        internal void showFullMap()
        {
            //        <Simple name="ShowFullMap" value="False" />
            XElement ShowFullMap = getFirstSimplePropertyNamed( levelComplex, "ShowFullMap" );
            ShowFullMap.SetAttributeValue( "value", "True" );
        }

        private XElement getMapDrawer()
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

        internal void addExtraSupplies( uint food, uint energy, uint workers )
        {
            bool addValue( XElement extra, uint add )
            {
                int value = 0;
                if( !Int32.TryParse( extra.Attribute( "value" ).Value, out value ) )
                {
                    return false;
                }

                value += (int) add;
                extra.SetAttributeValue( "value", value );
                return true;
            }

            if( food > 0 )
            {
                if( !addValue( getFirstSimplePropertyNamed( levelComplex, "CommandCenterExtraFood" ), food ) )
                {
                    Console.Error.WriteLine( "Unable to get the current CC Food supply value." );
                }
            }
            if( energy > 0 )
            {
                if( !addValue( getFirstSimplePropertyNamed( levelComplex, "CommandCenterExtraEnergy" ), energy ) )
                {
                    Console.Error.WriteLine( "Unable to get the current CC Energy supply value." );
                }
            }
            if( workers > 0 )
            {
                if( !addValue( getFirstSimplePropertyNamed( levelComplex, "CommandCenterExtraWorkers" ), workers ) )
                {
                    Console.Error.WriteLine( "Unable to get the current CC Workers supply value." );
                }
            }
        }

        internal void giftEntities( GiftableTypes giftable, uint count )
        {
            string giftableID = giftable.ToString( "D" );
            XElement templates = getFirstPropertyOfTypeNamed( levelComplex, "Dictionary", "BonusEntityTemplates" );
            /*if( templates == null )
            {
                Console.Error.WriteLine( "Unable to find BonusEntityTemplates." );
                return;
            }*/
            XElement items = templates.Element( "Items" );
            if( items.HasElements )
            {
                // Add to the existing gifted entities if this type already has some
                foreach( XElement i in items.Elements( "Item" ) )
                {
                    var simples = ( from s in i.Elements( "Simple" ) select s );
                    if( simples.First().Attribute( "value" ).Value == giftableID )    // Template/TypeID matches
                    {
                        XAttribute secondSimpleValue = simples.Skip( 1 ).First().Attribute( "value" );
                        int existing;
                        if( !Int32.TryParse( secondSimpleValue.Value, out existing ) )
                        {
                            Console.Error.WriteLine( "Unable to find the number of gifted entities of type: " + giftable );
                        }
                        else
                        {
                            secondSimpleValue.SetValue( existing + count );
                        }

                        return;
                    }
                }
            }
            // Else add a new gifted entity pairing to this dictionary
            XElement giftItem = new XElement( "Item" );

            XElement simple = new XElement( "Simple" );
            simple.SetAttributeValue( "value", giftableID );
            giftItem.Add( simple );

            simple = new XElement( "Simple" );
            simple.SetAttributeValue( "value", count );
            giftItem.Add( simple );

            items.Add( giftItem );
        }

        internal void fillStorage( bool gold, bool wood, bool stone, bool iron, bool oil )
        {
            /*
             * Storage. Assume from ZXRules tables:
             * Entities: WareHouse	ResourcesStorage	50
             * Global: GoldStorageFactor	int	40				Max gold stored = GoldStorageFactor * (Sum(Maxtorage per Entity))
             * (1+ WareHouse count) * 50 for non-gold resources doesn't account for mayors that increase CC storage, but should only be an underestimate and safe to set to at least this much.
             */

            // Get warehouses count
            int warehousesCount = getLevelEntitiesOfTypes( WAREHOUSE_TYPE ).Count();

            int storesCapacity = (1 + warehousesCount) * RESOURCE_STORE_CAPACITY;   // "1+" assumes base CC storage, no mayor +25/+50 upgrades

            // Update CC stored values, per resource
            if( gold )
            {
                getFirstSimplePropertyNamed( levelComplex, "Gold" ).SetAttributeValue( "value", storesCapacity * GOLD_STORAGE_FACTOR );
            }
            if( wood )
            {
                getFirstSimplePropertyNamed( levelComplex, "Wood" ).SetAttributeValue( "value", storesCapacity );
            }
            if( stone )
            {
                getFirstSimplePropertyNamed( levelComplex, "Stone" ).SetAttributeValue( "value", storesCapacity );
            }
            if( iron )
            {
                getFirstSimplePropertyNamed( levelComplex, "Iron" ).SetAttributeValue( "value", storesCapacity );
            }
            if( oil )
            {
                getFirstSimplePropertyNamed( levelComplex, "Oil" ).SetAttributeValue( "value", storesCapacity );
            }
        }

        internal void changeTheme( ThemeType theme )
        {
            string themeValue = theme.ToString();
            //Console.WriteLine( "Changing ThemeType to:" + themeValue );

            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            //                  <Properties>
            //                    <Complex name="MapDrawer">
            //                      <Properties>
            //                        <Simple name="ThemeType" value=
            XElement levelThemeType = getFirstSimplePropertyNamed( getMapDrawer(), "ThemeType" );

            //<Complex name="Root" type="ZX.ZXGameState, TheyAreBillions">
            //  < Properties >
            //    <Complex name="SurvivalModeParams">
            XElement paramsComplex = getFirstComplexPropertyNamed( data, "SurvivalModeParams" );
            //      <Properties>
            //        <Simple name="ThemeType"
            XElement paramsThemeType = getFirstSimplePropertyNamed( paramsComplex, "ThemeType" );

            levelThemeType.SetAttributeValue( "value", themeValue );
            paramsThemeType.SetAttributeValue( "value", themeValue );
        }

        internal void splitSwarms()
        {
            /*
             * The LevelEvents Collection appears to be duplicated (w.r.t. Generators) at different depths in a single save file,
             * a single method works from both given the correct starting element.
             */
            void splitEasyAndHardSwarms( XElement collectionContainer )
            {
                XElement eventsItems = getFirstPropertyOfTypeNamed( collectionContainer, "Collection", "LevelEvents" ).Element( "Items" );
                var easyComplex = ( from c in eventsItems.Elements( "Complex" )
                                    where (string) getFirstSimplePropertyNamed( c, "Name" ).Attribute( "value" ) == "Swarm Easy"
                                    select c ).FirstOrDefault();
                if( easyComplex != null )
                {
                    getFirstSimplePropertyNamed( easyComplex, "Generators" ).SetAttributeValue( "value", GENERATORS_2_DIRECTIONS );
                }

                var hardComplex = ( from c in eventsItems.Elements( "Complex" )
                                    where (string) getFirstSimplePropertyNamed( c, "Name" ).Attribute( "value" ) == "Swarm Hard"
                                    select c ).FirstOrDefault();
                if( hardComplex != null )
                {
                    getFirstSimplePropertyNamed( hardComplex, "Generators" ).SetAttributeValue( "value", GENERATORS_3_DIRECTIONS );
                }
            }

            /*
             *    <Complex name="LevelState">
             *      <Properties>
             *        <Collection name="LevelEvents" elementType="ZX.GameSystems.ZXLevelEvent, TheyAreBillions">
             *		    <Items>
             *            <Complex>
             *              <Properties>
             *                <Simple name="Name" value=
             *                <Simple name="Generators" value=
             */
            // getFirstPropertyOfTypeNamed( levelComplex, "Collection", "LevelEvents" ).Element( "Items" );
            splitEasyAndHardSwarms( levelComplex );

            /*
             *         <Complex name="CurrentGeneratedLevel">
             *          <Properties>
             *            <Complex name="Data">
             *              <Properties>
             *                <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
             *                  <Properties>
             *                    <Collection name="LevelEvents" elementType="ZX.GameSystems.ZXLevelEvent, TheyAreBillions">
             *					  <Items>
             *                        <Complex>
             *                          <Properties>
             *                            <Simple name="Name" value="Swarm Easy" />
             *                            <Simple name="Generators" value=
             */
            // getFirstPropertyOfTypeNamed( getDataExtension(), "Collection", "LevelEvents" ).Element( "Items" );
            splitEasyAndHardSwarms( getDataExtension() );

        }

        internal void disableMayors()
        {
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            //                  <Properties>
            //                    <Simple name="AllowMayors" value="True" />
            XElement allowMayors = getFirstSimplePropertyNamed( getDataExtension(), "AllowMayors" );
            allowMayors.SetAttributeValue( "value", "False" );
        }
    }
}
