using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TABSAT
{
    internal class LevelEntities
    {
        //@"ZX.Entities.Raven, TheyAreBillions"  @"12735209386004068058"
        //@"ZX.Entities.CommandCenter, TheyAreBillions"  @"3153977018683405164"
        //private const UInt64 CommandCenterType = 3153977018683405164;
        //@"ZX.Entities.ExplosiveBarrel, TheyAreBillions"  @"4963903858315893432"

        //@"ZX.Entities.MapSign, TheyAreBillions"  @"8008600744737996051"               SignWoodSmallA
        //@"ZX.Entities.GenericInteractive, TheyAreBillions"  @"3895270953278513917"    Console
        //@"ZX.Entities.GenericInteractive, TheyAreBillions"  @"4750080934714817242"    Communicator
        //@"ZX.Entities., TheyAreBillions"  @""

        //@"1040" @"ZX.Components.CUnitGenerator"

        private const string CLIFE_TYPE = @"ZX.Components.CLife, TheyAreBillions";
        private const string CINFLAMABLE = @"ZX.Components.CInflamable, TheyAreBillions";
        private const string CBEHAVIOUR_TYPE = @"ZX.Components.CBehaviour, TheyAreBillions";
        private const string CMOVABLE_TYPE = @"ZX.Components.CMovable, TheyAreBillions";

        internal enum HugeTypes : UInt64
        {
            Giant = 6179780658058987152,
            Mutant = 4885015758634569309
        }
        protected class HugeData
        {
            internal readonly string Type;
            internal readonly string Flags;
            internal readonly string Behaviour;
            internal readonly string Life;
            internal readonly string PathCapacity;
            internal readonly string Size;

            internal HugeData( string t, string f, string b, string l, string p, string s )
            {
                Type = t;
                Flags = f;
                Behaviour = b;
                Life = l;
                PathCapacity = p;
                Size = s;
            }
        }
        protected static readonly Dictionary<HugeTypes, HugeData> hugeTypesData;

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
        internal enum ScalableZombieGroups : byte
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

        protected class VODData
        {
            internal readonly string Type;
            internal readonly string Life;
            internal readonly string Size;

            internal VODData( string t, string l, string s )
            {
                Type = t;
                Life = l;
                Size = s;
            }
        }
        //internal static readonly Dictionary<VODTypes, string> vodSizesNames;    // Refactor with vodTypesData if still needed, making Name a field of VODData..?
        protected static readonly Dictionary<VODTypes, VODData> vodTypesData;

        internal static readonly SortedSet<UInt64> joinableTypes;

        internal enum PickableTypes : UInt64
        {
            PickableEnergy = 5768965425817495539,
            PickableFood = 3195137037877540492,
            PickableGold = 18025200598184898750,
            PickableWood = 526554950743885365,
            PickableStone = 3280721770111095074,
            PickableIron = 13398426522155325346,
            PickableOil = 6915758403222462548
        }

        static LevelEntities()
        {
            hugeTypesData = new Dictionary<HugeTypes, HugeData> {
                { HugeTypes.Giant, new HugeData( @"ZX.Entities.ZombieGiant, TheyAreBillions", @"None", @"ZX.Behaviours.BHZombieGiant, TheyAreBillions", @"10000", @"4", @"1.6;1.6" ) },
                { HugeTypes.Mutant, new HugeData( @"ZX.Entities.ZombieMutant, TheyAreBillions", @"IsOneCellSize", @"ZX.Behaviours.BHZombie, TheyAreBillions", @"4000", @"0", @"0.8;0.8" ) }
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
            /*
            vodSizesNames = new Dictionary<VODTypes, string> {
                { VODTypes.DoomBuildingSmall, "Dwellings" },
                { VODTypes.DoomBuildingMedium, "Taverns" },
                { VODTypes.DoomBuildingLarge, "City Halls" }
            };*/
            vodTypesData = new Dictionary<VODTypes, VODData> {
                { VODTypes.DoomBuildingSmall, new VODData( @"ZX.Entities.DoomBuildingSmall, TheyAreBillions", @"400", @"2;2" ) },
                { VODTypes.DoomBuildingMedium, new VODData (@"ZX.Entities.DoomBuildingMedium, TheyAreBillions", @"1500", @"3;3" ) },
                { VODTypes.DoomBuildingLarge, new VODData (@"ZX.Entities.DoomBuildingLarge, TheyAreBillions", @"4000", @"4;4" ) }
            };

            joinableTypes = new SortedSet<UInt64> {
                { (UInt64) GiftableTypes.RadarTower },
                { (UInt64) GiftableTypes.Executor },
                { (UInt64) GiftableTypes.ShockingTower }
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
        private readonly SortedDictionary<UInt64, SortedSet<UInt64>> joinableTypesToIDs;

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
            return SaveReader.getValueAttOfSimpleProp( entity.Element( "Complex" ), "Position" );
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
            return (UInt64) SaveReader.getValueAttOfSimpleProp( entity.Element( "Complex" ), "IDTemplate" );
        }


        internal LevelEntities( XElement levelComplex )
        {
            levelEntitiesItems = SaveReader.getFirstPropertyOfTypeNamed( levelComplex, "Dictionary", "LevelEntities" ).Element( "Items" );

            IDsToItems = new SortedDictionary<UInt64, XElement>();
            itemTypesToIDs = new SortedDictionary<UInt64, SortedSet<UInt64>>();
            joinableTypesToIDs = new SortedDictionary<UInt64, SortedSet<UInt64>>();
            foreach( var joinable in joinableTypes )
            {
                joinableTypesToIDs.Add( joinable, new SortedSet<UInt64>() );
            }

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

            if( !itemTypesToIDs.TryGetValue( itemType, out SortedSet<UInt64> typeIDs ) )
            {
                typeIDs = new SortedSet<UInt64>();
                itemTypesToIDs.Add( itemType, typeIDs );
            }
            typeIDs.Add( id );

            // Is it a map-spawned building that can be reclaimed?
            if( joinableTypesToIDs.TryGetValue( itemType, out SortedSet<UInt64> joinableIDs ) )
            {
                var team = (string) SaveReader.getValueAttOfSimpleProp( i.Element( "Complex" ), "Team" );
                if( "NeutralJoinable".Equals( team ) )
                {
                    joinableIDs.Add( id );
                }
            }
        }

        private void Add( XElement copy )
        {
            levelEntitiesItems.Add( copy );
            trackEntity( copy );
        }

        private void Remove( in UInt64 id )
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

                // What about entity references in swarms..?

                // What about entities assigned to group shortcuts? We don't try to remove such friendly entities.
                /*
                  <SingleArray name="RefEntitySelectionGroups" elementType="System.Collections.Generic.List`1[[DXVision.DXEntityRef, DXVision]], mscorlib">
                    <Items>
                      <Collection elementType="DXVision.DXEntityRef, DXVision">
                        <Items>
                          <Complex>
                            <Properties>
                              <Simple name="IDEntity" value="..." />
                */
            }
            else
            {
                Console.Error.WriteLine( "Could not remove LevelEntity: " + id );
            }
        }

        internal IEnumerable<XElement> getEntitiesOfType( in UInt64 entityType )
        {
            LinkedList<XElement> subset = new LinkedList<XElement>();
            if( itemTypesToIDs.TryGetValue( entityType, out SortedSet<UInt64> typeIDs ) )
            {
                foreach( var id in typeIDs )
                {
                    subset.AddLast( IDsToItems[id] );
                }
            }
            return subset;
        }

        internal IEnumerable<XElement> getNeutralJoinablesOfType( in UInt64 entityType )
        {
            LinkedList<XElement> subset = new LinkedList<XElement>();
            if( joinableTypesToIDs.TryGetValue( entityType, out SortedSet<UInt64> joinableIDs ) )
            {
                foreach( var id in joinableIDs )
                {
                    subset.AddLast( IDsToItems[id] );
                }
            }
            return subset;
        }

        internal SortedSet<UInt64> getIDs( in UInt64 itemType )
        {
            if( !itemTypesToIDs.TryGetValue( itemType, out SortedSet<UInt64> typeIDs ) )
            {
                return new SortedSet<UInt64>();
            }
            return new SortedSet<UInt64>( typeIDs );
        }

        internal SortedSet<UInt64> getAllIDs()
        {
            SortedSet<UInt64> keys = new SortedSet<UInt64>();
            foreach( var k in IDsToItems.Keys )
            {
                keys.Add( k );
            }

            // What about infection nest smoke references..?
            /*
              <Complex type="ZX.Entities.DoomBuilding..., TheyAreBillions">
                <Properties>
                  <Collection name="Components" elementType="DXVision.DXComponent, DXVision">
                    <Items>
                      <Complex type="ZX.Components.CInfectionNest, TheyAreBillions">
                        <Properties>
                          <Complex name="InfectionSmokeRef">
                            <Properties>
                              <Simple name="IDEntity" value="..." />
            */

            return keys;
        }

        internal LinkedList<ScaledEntity> scaleEntities( in UInt64 entityType, in byte scale, IDGenerator editor, SaveReader.ItemInArea inArea )
        {
            var selectedEntities = new LinkedList<ScaledEntity>();

            if( scale == 100U )
            {
                // Nothing needs be done
                return selectedEntities;
            }

            void duplicateLevelEntity( XElement i, LinkedList<ScaledEntity> dupedEntities )
            {
                XElement iCopy = new XElement( i );         // Duplicate at the same position
                var newID = editor.newID();
                iCopy.Element( "Simple" ).SetAttributeValue( "value", newID );
                SaveReader.getValueAttOfSimpleProp( iCopy.Element( "Complex" ), "ID" ).SetValue( newID );

                Add( iCopy );
                dupedEntities.AddLast( new ScaledEntity( (UInt64) i.Element( "Simple" ).Attribute( "value" ), newID ) );
            }

            Random rand = new Random();

            uint multiples = scale / 100U;                  // How many duplicates to certainly make of each zombie
            double chance = ( scale % 100U ) / 100;         // The chance of making 1 more duplicate per zombie

            IEnumerable<XElement> items = getEntitiesOfType( entityType );
            foreach( var item in items )
            {
                // First test that their position is in the affected area
                if( !inArea( item, true ) )
                {
                    continue;
                }
                // item is in the affected area, we continue here rather than loop to another immediately

                if( scale < 100U )
                {
                    // chance is now chance to not remove existing entities
                    // selectedEntities will be those removed

                    // 0 >= scale < 100
                    if( scale == 0U || chance < rand.NextDouble() )
                    {
                        var id = (UInt64) item.Element( "Simple" ).Attribute( "value" );
                        Remove( id );
                        selectedEntities.AddLast( new ScaledEntity( id ) );
                    }
                }
                else
                {
                    // First the certain duplications
                    for( uint m = 1; m < multiples; m++ )
                    {
                        duplicateLevelEntity( item, selectedEntities );
                    }
                    // And now the chance-based duplication
                    if( chance >= rand.NextDouble() )   // If the chance is not less than the roll
                    {
                        duplicateLevelEntity( item, selectedEntities );
                    }
                }
            }
            //Console.WriteLine( "selectedEntities: " + selectedEntities.Count );

            return selectedEntities;
        }

        internal bool relocateHuge( in XElement fromEntity, in XElement toEntity )
        {
            if( fromEntity == toEntity )
            {
                return false;
            }

            // We're after 3 different values in 2 different <Complex under Components
            // 1
            XElement currentBehaviourTargetPosition = SaveReader.getFirstSimplePropertyNamed( SaveReader.getFirstComplexPropertyNamed( getBehaviour( fromEntity ), "Data" ), "TargetPosition" );

            // 2
            XElement currentMovableTargetPosition = SaveReader.getFirstSimplePropertyNamed( getMovable( fromEntity ), "TargetPosition" );
            // 3
            XElement currentLastDestinyProcessed = SaveReader.getFirstSimplePropertyNamed( getMovable( fromEntity ), "LastDestinyProcessed" );

            string toPositionString = (string) getPosition( toEntity );

            getPosition( fromEntity ).SetValue( toPositionString );
            SaveReader.getValueAttOfSimpleProp( fromEntity.Element( "Complex" ), "LastPosition" ).SetValue( toPositionString );
            currentBehaviourTargetPosition?.SetAttributeValue( "value", toPositionString );
            currentMovableTargetPosition?.SetAttributeValue( "value", toPositionString );
            currentLastDestinyProcessed?.SetAttributeValue( "value", toPositionString );

            return true;
        }

        internal bool swapZombieType( in UInt64 id, SaveReader.InArea inArea )
        {
            if( !IDsToItems.TryGetValue( id, out XElement entity ) )
            {
                Console.Error.WriteLine( "Could not type-swap LevelEntity: " + id );
                return false;
            }

            SaveReader.extractCoordinates( entity, out ushort x, out ushort y );
            if( !inArea( x, y ) )
            {
                return false;
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

            UInt64 targetType;
            HugeData targetHugeData;
            if( getIDs( (UInt64) HugeTypes.Mutant ).Contains( id ) )
            {
                targetType = (UInt64) HugeTypes.Giant;
                targetHugeData = hugeTypesData[HugeTypes.Giant];

                XElement cInflamable = getComplexItemOfType( components, CINFLAMABLE, false );
                cInflamable?.Remove();
            }
            else
            {
                targetType = (UInt64) HugeTypes.Mutant;
                targetHugeData = hugeTypesData[HugeTypes.Mutant];

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

            complex.Attribute( "type" ).SetValue( targetHugeData.Type );
            SaveReader.getValueAttOfSimpleProp( complex, "Flags" ).SetValue( targetHugeData.Flags );
            SaveReader.getValueAttOfSimpleProp( complex, "IDTemplate" ).SetValue( targetType );
            SaveReader.getValueAttOfSimpleProp( cLife, "Life" ).SetValue( targetHugeData.Life );
            getBehaviour( entity ).Attribute( "type" ).SetValue( targetHugeData.Behaviour );
            SaveReader.getValueAttOfSimpleProp( path, "Capacity" ).SetValue( targetHugeData.PathCapacity );
            SaveReader.getValueAttOfSimpleProp( complex, "Size" ).SetValue( targetHugeData.Size );

            // Also move ID from old itemTypesToIDs index type to new
            changeItemType( id, (UInt64) ( targetType == (UInt64) HugeTypes.Mutant ? HugeTypes.Giant : HugeTypes.Mutant ), targetType );

            return true;
        }

        private void changeItemType( in UInt64 id, in UInt64 oldType, in UInt64 newType )
        {
            if( itemTypesToIDs.TryGetValue( oldType, out SortedSet<UInt64> oldTypeIDs ) )
            {
                oldTypeIDs.Remove( id );

                if( !itemTypesToIDs.TryGetValue( newType, out SortedSet<UInt64> newTypeIDs ) )
                {
                    newTypeIDs = new SortedSet<UInt64>();
                    itemTypesToIDs.Add( newType, newTypeIDs );
                }
                newTypeIDs.Add( id );
            }
        }

        internal void resizeVODs( in VODTypes targetVodType )
        {
            var newVodData = vodTypesData[targetVodType];
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
                              <Complex type="ZX.Components.CInfectionNest, TheyAreBillions">
                                <Properties>
                                  <Simple name="NUnitsGenerated" value="0" />
             */
            foreach( VODTypes fromVodType in Enum.GetValues( typeof( VODTypes ) ) )
            {
                IEnumerable<XElement> vodItems = getEntitiesOfType( (UInt64) fromVodType );
                //Console.WriteLine( "vodItems: " + vodItems.Count() );

                if( fromVodType == targetVodType )
                {
                    continue;   // No need to modify these entities
                }

                foreach( XElement v in vodItems.ToList() )  // no ToList() leads to only removing 1 <item> per save modify cycle?!
                {
                    XElement complex = v.Element( "Complex" );
                    complex.SetAttributeValue( "type", newVodData.Type );
                    SaveReader.getValueAttOfSimpleProp( complex, "IDTemplate" ).SetValue( (UInt64) targetVodType );
                    SaveReader.getValueAttOfSimpleProp( getComplexItemOfType( getComponents( complex ), CLIFE_TYPE ), "Life" ).SetValue( newVodData.Life );
                    SaveReader.getValueAttOfSimpleProp( complex, "Size" ).SetValue( newVodData.Size );

                    // Also move ID from old itemTypesToIDs index type to new
                    var id = (UInt64) v.Element( "Simple" ).Attribute( "value" );
                    changeItemType( id, (UInt64) fromVodType, (UInt64) targetVodType );
                }
            }
        }

        internal void removeReclaimables()
        {
            foreach( PickableTypes pickableType in Enum.GetValues( typeof( PickableTypes ) ) )
            {
                var positions = new LinkedList<MapNavigation.Position>();
                IEnumerable<XElement> pickableItems = getEntitiesOfType( (UInt64) pickableType );
                foreach( var pickable in pickableItems )
                {
                    var id = (UInt64) pickable.Element( "Simple" ).Attribute( "value" );
                    Remove( id );
                }
            }

            foreach( var k_v in joinableTypesToIDs )
            {
                var joinableIDs = k_v.Value;
                foreach( var id in joinableIDs )
                {
                    Remove( id );
                }
                joinableIDs.Clear();    // Not currently needed as instances aren't reused after modifications
            }
        }
    }
}
