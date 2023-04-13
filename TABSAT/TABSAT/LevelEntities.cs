using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TABSAT
{
    internal class LevelEntities
    {
        private const string GIANT_TYPE = @"ZX.Entities.ZombieGiant, TheyAreBillions";
        private const string MUTANT_TYPE = @"ZX.Entities.ZombieMutant, TheyAreBillions";
        private const string VOD_SMALL_TYPE = @"ZX.Entities.DoomBuildingSmall, TheyAreBillions";
        private const string VOD_MEDIUM_TYPE = @"ZX.Entities.DoomBuildingMedium, TheyAreBillions";
        private const string VOD_LARGE_TYPE = @"ZX.Entities.DoomBuildingLarge, TheyAreBillions";
        //private const string WAREHOUSE_TYPE = @"ZX.Entities.WareHouse, TheyAreBillions";
        //private const string OILPOOL_TYPE = @"ZX.Entities.OilSource, TheyAreBillions";
        //@"ZX.Entities.Raven, TheyAreBillions"  @"12735209386004068058"
        //@"ZX.Entities.PickableEnergy, TheyAreBillions"  @"5768965425817495539"
        //@"ZX.Entities.PickableFood, TheyAreBillions"  @"3195137037877540492"
        //@"ZX.Entities.CommandCenter, TheyAreBillions"  @"3153977018683405164"
        //@"ZX.Entities.ExplosiveBarrel, TheyAreBillions"  @"4963903858315893432"
        //@"ZX.Entities.PickableGold, TheyAreBillions"  @"18025200598184898750"
        //@"ZX.Entities.PickableWood, TheyAreBillions"  @"526554950743885365"
        //@"ZX.Entities.PickableStone, TheyAreBillions"  @"3280721770111095074"
        //@"ZX.Entities.PickableIron, TheyAreBillions"  @"13398426522155325346"
        //@"ZX.Entities.PickableOil, TheyAreBillions"  @"6915758403222462548"

        //@"ZX.Entities.MapSign, TheyAreBillions"  @"8008600744737996051"               SignWoodSmallA
        //@"ZX.Entities.GenericInteractive, TheyAreBillions"  @"3895270953278513917"    Console
        //@"ZX.Entities.GenericInteractive, TheyAreBillions"  @"4750080934714817242"    Communicator
        //@"ZX.Entities., TheyAreBillions"  @""

        //@"1040" @"ZX.Components.CUnitGenerator"

        private const string CLIFE_TYPE = @"ZX.Components.CLife, TheyAreBillions";
        private const string CINFLAMABLE = @"ZX.Components.CInflamable, TheyAreBillions";
        private const string CBEHAVIOUR_TYPE = @"ZX.Components.CBehaviour, TheyAreBillions";
        private const string CMOVABLE_TYPE = @"ZX.Components.CMovable, TheyAreBillions";

        private const string GIANT_BEHAVIOUR_TYPE = @"ZX.Behaviours.BHZombieGiant, TheyAreBillions";
        private const string MUTANT_BEHAVIOUR_TYPE = @"ZX.Behaviours.BHZombie, TheyAreBillions";

        internal const UInt64 OilSourceType = 14597207313853823957;

        internal const UInt64 TruckAType = 1130949242559706282;
        internal const UInt64 FortressBarLeftType = 1858993070642015232;
        internal const UInt64 FortressBarRightType = 5955209075099213047;
        /*private const string RuinTreasureA_BR_Type = @"5985530356264170826";
        private const string RuinTreasureA_TM_Type = @"257584999789546783";
        private const string RuinTreasureA_AL_Type = @"8971922455791567927";
        private const string RuinTreasureA_DS_Type = @"3137634406804904509";
        private const string RuinTreasureA_VO_Type = @"807600697508101881";
        private const string TensionTowerMediumFlip_Type = @"2617794739528169237";
        private const string TensionTowerMedium_Type = @"4533866769353242870";
        private const string TensionTowerHighFlip_Type = @"2342596987766548617";
        private const string TensionTowerHigh_Type = @"3359149191582161849";
        private const string VOLCANO_Type = @"5660774435759652919";      // Multiple sizes?*/
        //private const string _Type = @"";

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

        internal enum HugeTypes : UInt64
        {
            Giant = 6179780658058987152,
            Mutant = 4885015758634569309
        }

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
        internal static readonly Dictionary<ScalableZombieGroups, SortedSet<ScalableZombieTypes>> scalableZombieTypeGroups;

        internal enum VODTypes : UInt64
        {
            DoomBuildingSmall = 8702552346733362645,
            DoomBuildingMedium = 293812117068830615,
            DoomBuildingLarge = 3441286325348372349
        }
        internal static readonly Dictionary<VODTypes, string> vodSizesNames;

        static LevelEntities()
        {
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

            vodSizesNames = new Dictionary<VODTypes, string> {
                { VODTypes.DoomBuildingSmall, "Dwellings" },
                { VODTypes.DoomBuildingMedium, "Taverns" },
                { VODTypes.DoomBuildingLarge, "City Halls" }
            };
        }

        internal readonly struct ScaledEntity
        {
            internal ScaledEntity( UInt64 b, UInt64? a = null )
            {
                Before = b;
                After = a;
            }
            internal UInt64 Before { get; }
            internal UInt64? After { get; }
        }

        private readonly XElement levelEntitiesItems;
        private readonly SortedDictionary<UInt64, XElement> IDsToItems;
        private readonly SortedDictionary<UInt64, SortedSet<UInt64>> itemTypesToIDs;

        private static XElement getComponents( XElement complex )
        {
            /*return ( from c in complex.Element( "Properties" ).Elements( "Collection" )
                     where (string) c.Attribute( "name" ) == "Components"
                     select c ).Single();*/
            return SaveReader.getFirstPropertyOfTypeNamed( complex, "Collection", "Components" );
        }

        private static XElement getBehaviour( XElement entity )
        {
            XElement currentCBehaviour = getComplexItemOfType( getComponents( entity.Element( "Complex" ) ), CBEHAVIOUR_TYPE );
            return SaveReader.getFirstComplexPropertyNamed( currentCBehaviour, "Behaviour" );
        }

        private static XElement getMovable( XElement entity )
        {
            return getComplexItemOfType( getComponents( entity.Element( "Complex" ) ), CMOVABLE_TYPE );
        }

        private static XAttribute getPosition( XElement entity )
        {
            return SaveReader.getFirstSimplePropertyNamed( entity.Element( "Complex" ), "Position" ).Attribute( "value" );
        }

        private static XElement getComplexItemOfType( XElement components, string type, bool assumeExists = true )
        {
            var i = ( from c in components.Element( "Items" ).Elements( "Complex" )
                      where (string) c.Attribute( "type" ) == type
                      select c );
            return assumeExists ? i.Single() : i.SingleOrDefault();
        }

        private static UInt64 getItemType( XElement entity )
        {
            return (UInt64) SaveReader.getFirstSimplePropertyNamed( entity.Element( "Complex" ), "IDTemplate" ).Attribute( "value" );
        }


        internal LevelEntities( XElement levelComplex )
        {
            levelEntitiesItems = SaveReader.getFirstPropertyOfTypeNamed( levelComplex, "Dictionary", "LevelEntities" ).Element( "Items" );

            IDsToItems = new SortedDictionary<UInt64, XElement>();
            itemTypesToIDs = new SortedDictionary<UInt64, SortedSet<UInt64>>();

            //Console.Write( "Starting parsing level entities..." );
            // Lazy parse these later, on first use?
            foreach( var i in levelEntitiesItems.Elements( "Item" ) )
            {
                trackEntity( i );
            }
            //Console.WriteLine( " Finished." );
            /*Console.WriteLine( "Level entity types count: " + itemTypesToIDs.Count );
            foreach( var k_v in itemTypesToIDs )
            {
                Console.WriteLine( "key: " + k_v.Key + "\tcount: " + k_v.Value.Count ); ;
            }*/
        }


        private void trackEntity( XElement i )
        {
            var id = (UInt64) i.Element( "Simple" ).Attribute( "value" );
            IDsToItems.Add( id, i );

            // At least CUnitGenerator entities lack a type attribute & value

            var itemType = getItemType( i );

            if( !itemTypesToIDs.TryGetValue( itemType, out SortedSet<ulong> typeIDs ) )
            {
                typeIDs = new SortedSet<UInt64>();
                itemTypesToIDs.Add( itemType, typeIDs );
            }
            typeIDs.Add( id );
        }

        private void Add( XElement copy )
        {
            levelEntitiesItems.Add( copy );
            trackEntity( copy );
        }

        private void Remove( UInt64 id )
        {
            if( IDsToItems.TryGetValue( id, out XElement entity ) )
            {
                var entityType = getItemType( entity );

                if( itemTypesToIDs.TryGetValue( entityType, out SortedSet<UInt64> typeIDs ) )
                {
                    typeIDs.Remove( id );
                    // Is OK to leave an empty set of IDs for this type in typesToIDs
                }
                else
                {
                    Console.Error.WriteLine( "Could not remove LevelEntity by type: " + id );
                }
                IDsToItems.Remove( id );

                entity.Remove();
            }
            else
            {
                Console.Error.WriteLine( "Could not remove LevelEntity: " + id );
            }
        }

        internal IEnumerable<XElement> getEntitiesOfTypes( params UInt64[] entityTypes )
        {
            LinkedList<XElement> subset = new LinkedList<XElement>();
            foreach( var entityType in entityTypes )
            {
                if( itemTypesToIDs.TryGetValue( entityType, out SortedSet<UInt64> typeIDs ) )
                {
                    foreach( var id in typeIDs )
                    {
                        subset.AddLast( IDsToItems[id] );
                    }
                }
            }
            return subset;
        }

        internal SortedSet<UInt64> getIDs( UInt64 itemType )
        {
            if( !itemTypesToIDs.TryGetValue( itemType, out SortedSet<UInt64> typeIDs ) )
            {
                return new SortedSet<UInt64>();
            }
            return new SortedSet<UInt64>( typeIDs );
        }

        internal LinkedList<SaveReader.RelativePosition> getPositions( UInt64 itemType, MapData cc )
        {
            var positions = new LinkedList<SaveReader.RelativePosition>();
            if( itemTypesToIDs.TryGetValue( itemType, out SortedSet<UInt64> typeIDs ) )
            {
                foreach( var id in typeIDs )
                {
                    positions.AddLast( new SaveReader.RelativePosition( IDsToItems[id], cc ) );
                }
            }
            return positions;
        }

        internal SortedSet<UInt64> getAllIDs()
        {
            SortedSet<UInt64> keys = new SortedSet<UInt64>();
            foreach( var k in IDsToItems.Keys )
            {
                keys.Add( k );
            }
            return keys;
        }

        internal LinkedList<ScaledEntity> scaleEntities( UInt64 entityType, decimal scale, IDGenerator editor )
        {
            if( scale < 0.0M )
            {
                throw new ArgumentOutOfRangeException( "Scale must not be negative." );
            }

            void duplicateLevelEntity( XElement i, LinkedList<ScaledEntity> dupedEntities )
            {
                XElement iCopy = new XElement( i );     // Duplicate at the same position
                var newID = editor.newID();
                iCopy.Element( "Simple" ).SetAttributeValue( "value", newID );
                SaveReader.getFirstSimplePropertyNamed( iCopy.Element( "Complex" ), "ID" ).SetAttributeValue( "value", newID );

                Add( iCopy );
                dupedEntities.AddLast( new ScaledEntity( (UInt64) i.Element( "Simple" ).Attribute( "value" ), newID ) );
            }

            uint multiples = (uint) scale;              // How many duplicates to certainly make of each entity
            double chance = (double) ( scale % 1 );     // The chance of making 1 more duplicate per entity

            Random rand = new Random();

            var selectedEntities = new LinkedList<ScaledEntity>();

            if( scale < 1.0M )
            {
                // chance is now chance to not remove existing entities
                // selectedEntities will be those removed

                // 0 >= scale < 1
                foreach( var i in getEntitiesOfTypes( entityType ) )
                {
                    if( scale == 0.0M || chance < rand.NextDouble() )
                    {
                        var id = (UInt64) i.Element( "Simple" ).Attribute( "value" );
                        Remove( id );
                        selectedEntities.AddLast( new ScaledEntity( id ) );
                    }
                }
                //Console.WriteLine( "selectedEntities: " + selectedEntities.Count );
            }
            else
            {

                foreach( var i in getEntitiesOfTypes( entityType ) )
                {
                    // First the certain duplications
                    for( uint m = 1; m < multiples; m++ )
                    {
                        duplicateLevelEntity( i, selectedEntities );
                    }
                    // And now the chance-based duplication
                    if( chance >= rand.NextDouble() )   // If the chance is not less than the roll
                    {
                        duplicateLevelEntity( i, selectedEntities );
                    }
                }
                //Console.WriteLine( "selectedEntities: " + selectedEntities.Count );
            }

            return selectedEntities;
        }

        internal void relocateHuge( UInt64 origin, UInt64 destination )
        {
            if( origin == destination )
            {
                return;
            }

            if( !IDsToItems.TryGetValue( origin, out XElement originEntity ) )
            {
                Console.Error.WriteLine( "Could not relocate LevelEntity: " + origin );
                return;
            }
            if( !IDsToItems.TryGetValue( destination, out XElement destinationEntity ) )
            {
                Console.Error.WriteLine( "Could not relocate LevelEntity: " + origin + " to: " + destination );
                return;
            }

            // We're after 3 different values in 2 different <Complex under Components
            // 1
            XElement currentBehaviourTargetPosition = SaveReader.getFirstSimplePropertyNamed( SaveReader.getFirstComplexPropertyNamed( getBehaviour( originEntity ), "Data" ), "TargetPosition" );

            // 2
            XElement currentMovableTargetPosition = SaveReader.getFirstSimplePropertyNamed( getMovable( originEntity ), "TargetPosition" );
            // 3
            XElement currentLastDestinyProcessed = SaveReader.getFirstSimplePropertyNamed( getMovable( originEntity ), "LastDestinyProcessed" );

            string farthestPositionString = (string) getPosition( destinationEntity );

            getPosition( originEntity ).SetValue( farthestPositionString );
            SaveReader.getFirstSimplePropertyNamed( originEntity.Element( "Complex" ), "LastPosition" ).SetAttributeValue( "value", farthestPositionString );
            currentBehaviourTargetPosition?.SetAttributeValue( "value", farthestPositionString );
            currentMovableTargetPosition?.SetAttributeValue( "value", farthestPositionString );
            currentLastDestinyProcessed?.SetAttributeValue( "value", farthestPositionString );
        }

        internal void swapZombieType( UInt64 id )
        {
            if( !IDsToItems.TryGetValue( id, out XElement entity ) )
            {
                Console.Error.WriteLine( "Could not type-swap LevelEntity: " + id );
                return;
            }
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

            XElement complex = entity.Element( "Complex" );
            XElement components = getComponents( complex );
            XElement cLife = getComplexItemOfType( components, CLIFE_TYPE );

            /*XElement path = ( from c in getMovable( entity ).Element( "Properties" ).Elements( "Collection" )
                                where (string) c.Attribute( "name" ) == "Path"
                                select c ).SingleOrDefault();*/
            XElement path = SaveReader.getFirstPropertyOfTypeNamed( getMovable( entity ), "Collection", "Path" );

            string typeString;
            string flags;
            UInt64 typeID;
            string life;
            string behaviour;
            string pathCapacity;
            string size;
            if( (string) complex.Attribute( "type" ) == MUTANT_TYPE )
            {
                typeString = GIANT_TYPE;
                flags = "None";
                typeID = (UInt64) LevelEntities.HugeTypes.Giant;
                life = GIANT_LIFE;
                behaviour = GIANT_BEHAVIOUR_TYPE;
                pathCapacity = "4";
                size = GIANT_SIZE;

                XElement cInflamable = getComplexItemOfType( components, CINFLAMABLE, false );
                cInflamable?.Remove();
            }
            else
            {
                typeString = MUTANT_TYPE;
                flags = "IsOneCellSize";
                typeID = (UInt64) LevelEntities.HugeTypes.Mutant;
                life = MUTANT_LIFE;
                behaviour = MUTANT_BEHAVIOUR_TYPE;
                pathCapacity = "0";
                size = MUTANT_SIZE;

                string cInflamable =
                @"<Complex type=""ZX.Components.CInflamable, TheyAreBillions"">
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
                XElement inflamable = XElement.Parse( cInflamable );
                components.Element( "Items" ).Add( inflamable );
            }

            complex.Attribute( "type" ).SetValue( typeString );
            SaveReader.getFirstSimplePropertyNamed( complex, "Flags" ).Attribute( "value" ).SetValue( flags );
            SaveReader.getFirstSimplePropertyNamed( complex, "IDTemplate" ).Attribute( "value" ).SetValue( typeID );
            SaveReader.getFirstSimplePropertyNamed( cLife, "Life" ).Attribute( "value" ).SetValue( life );
            getBehaviour( entity ).Attribute( "type" ).SetValue( behaviour );
            SaveReader.getFirstSimplePropertyNamed( path, "Capacity" ).Attribute( "value" ).SetValue( pathCapacity );
            SaveReader.getFirstSimplePropertyNamed( complex, "Size" ).Attribute( "value" ).SetValue( size );
        }

        internal void resizeVODs( VODTypes vodTemplate )
        {
            string newType;
            string newLife;
            string newSize;
            switch( vodTemplate )
            {
                default:
                case VODTypes.DoomBuildingSmall:
                    newType = VOD_SMALL_TYPE;
                    newLife = VOD_SMALL_LIFE;
                    newSize = VOD_SMALL_SIZE;
                    break;
                case VODTypes.DoomBuildingMedium:
                    newType = VOD_MEDIUM_TYPE;
                    newLife = VOD_MEDIUM_LIFE;
                    newSize = VOD_MEDIUM_SIZE;
                    break;
                case VODTypes.DoomBuildingLarge:
                    newType = VOD_LARGE_TYPE;
                    newLife = VOD_LARGE_LIFE;
                    newSize = VOD_LARGE_SIZE;
                    break;
            }
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

            IEnumerable<XElement> vodItems = getEntitiesOfTypes( Enum.GetValues( typeof(VODTypes) ).Cast<UInt64>().ToArray() );
            //Console.WriteLine( "vodItems: " + vodItems.Count() );

            foreach( XElement v in vodItems.ToList() )  // no ToList() leads to only removing 1 <item> per save modify cycle?!
            {
                XElement complex = v.Element( "Complex" );
                complex.SetAttributeValue( "type", newType );
                SaveReader.getFirstSimplePropertyNamed( complex, "IDTemplate" ).SetAttributeValue( "value", (UInt64) vodTemplate );
                SaveReader.getFirstSimplePropertyNamed( getComplexItemOfType( getComponents( complex ), CLIFE_TYPE ), "Life" ).SetAttributeValue( "value", newLife );
                SaveReader.getFirstSimplePropertyNamed( complex, "Size" ).SetAttributeValue( "value", newSize );
            }
        }

        internal void scaleVODs( VODTypes vodTemplate, decimal scale, IDGenerator editor )
        {
            scaleEntities( (UInt64) vodTemplate, scale, editor );
        }
    }
}
