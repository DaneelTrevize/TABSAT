using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TABSAT
{
    internal class LevelEntities
    {
        internal const string GIANT_TYPE = @"ZX.Entities.ZombieGiant, TheyAreBillions";
        internal const string MUTANT_TYPE = @"ZX.Entities.ZombieMutant, TheyAreBillions";
        internal const string VOD_SMALL_TYPE = @"ZX.Entities.DoomBuildingSmall, TheyAreBillions";
        internal const string VOD_MEDIUM_TYPE = @"ZX.Entities.DoomBuildingMedium, TheyAreBillions";
        internal const string VOD_LARGE_TYPE = @"ZX.Entities.DoomBuildingLarge, TheyAreBillions";
        internal const string WAREHOUSE_TYPE = @"ZX.Entities.WareHouse, TheyAreBillions";
        internal const string OILPOOL_TYPE = @"ZX.Entities.OilSource, TheyAreBillions";
        //@"ZX.Entities.Raven, TheyAreBillions"  @"12735209386004068058"
        //@"ZX.Entities.PickableEnergy, TheyAreBillions"  @"5768965425817495539"
        //@"ZX.Entities.PickableFood, TheyAreBillions"  @"3195137037877540492"
        //@"ZX.Entities.CommandCenter, TheyAreBillions"  @"3153977018683405164"
        //@"ZX.Entities.ExplosiveBarrel, TheyAreBillions"  @"4963903858315893432"
        //@"ZX.Entities.PickableWood, TheyAreBillions"  @"526554950743885365"
        //@"ZX.Entities.PickableGold, TheyAreBillions"  @"18025200598184898750"
        //???  @"1040" @"ZX.Components.CUnitGenerator"
        //@"ZX.Entities., TheyAreBillions"  @""
        //@"ZX.Entities., TheyAreBillions"  @""
        //@"ZX.Entities., TheyAreBillions"  @""
        //@"ZX.Entities., TheyAreBillions"  @""
        //@"ZX.Entities., TheyAreBillions"  @""
        //@"ZX.Entities., TheyAreBillions"  @""

        private const string CLIFE_TYPE = @"ZX.Components.CLife, TheyAreBillions";
        private const string CINFLAMABLE = @"ZX.Components.CInflamable, TheyAreBillions";
        private const string CBEHAVIOUR_TYPE = @"ZX.Components.CBehaviour, TheyAreBillions";
        private const string CMOVABLE_TYPE = @"ZX.Components.CMovable, TheyAreBillions";

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
        private readonly SortedDictionary<UInt64, XElement> entityItems;
        private readonly SortedDictionary<string, SortedSet<UInt64>> typesToIDs;

        private static XElement getComplex( XElement entity )
        {
            return entity.Element( "Complex" );
        }

        private static XElement getComponents( XElement entity )
        {
            return SaveEditor.getComponents( getComplex( entity ) );
        }

        private static XElement getBehaviour( XElement entity )
        {
            XElement currentCBehaviour = SaveEditor.getComplexItemOfType( getComponents( entity ), CBEHAVIOUR_TYPE );
            return SaveEditor.getFirstComplexPropertyNamed( currentCBehaviour, "Behaviour" );
        }

        private static XElement getMovable( XElement entity )
        {
            return SaveEditor.getComplexItemOfType( getComponents( entity ), CMOVABLE_TYPE );
        }

        private static XAttribute getPosition( XElement entity )
        {
            return SaveEditor.getFirstSimplePropertyNamed( getComplex( entity ), "Position" ).Attribute( "value" );
        }

        internal LevelEntities( XElement levelComplex )
        {
            levelEntitiesItems = SaveEditor.getFirstPropertyOfTypeNamed( levelComplex, "Dictionary", "LevelEntities" ).Element( "Items" );

            entityItems = new SortedDictionary<UInt64, XElement>();
            typesToIDs = new SortedDictionary<string, SortedSet<UInt64>>();

            //Console.Write( "Starting parsing level entities..." );
            // Lazy parse these later, on first use?
            foreach( var i in levelEntitiesItems.Elements( "Item" ) )
            {
                trackEntity( i );
            }
            //Console.WriteLine( " Finished." );
            /*Console.WriteLine( "Level entity types count: " + typesToIDs.Count );
            foreach( var k_v in typesToIDs )
            {
                Console.WriteLine( "key: " + k_v.Key + "\tcount: " + k_v.Value.Count ); ;
            }*/
        }

        private void trackEntity( XElement i )
        {
            var id = (UInt64) i.Element( "Simple" ).Attribute( "value" );
            entityItems.Add( id, i );

            var type = (string) i.Element( "Complex" ).Attribute( "type" );
            if( type == null )
            {
                type = "CUnitGenerator?";      // At least CUnitGenerator entities lack a type attribute & value, use a placeholder
            }

            SortedSet<UInt64> typeIDs;
            if( !typesToIDs.TryGetValue( type, out typeIDs ) )
            {
                typeIDs = new SortedSet<UInt64>();
                typesToIDs.Add( type, typeIDs );
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
            XElement entity;
            if( entityItems.TryGetValue( id, out entity ) )
            {
                var type = (string) entity.Element( "Complex" ).Attribute( "type" );
                SortedSet<UInt64> typeIDs;
                if( typesToIDs.TryGetValue( type, out typeIDs ) )
                {
                    typeIDs.Remove( id );
                    // Is OK to leave an empty set of IDs for this type in typesToIDs
                }
                else
                {
                    Console.Error.WriteLine( "Could not remove LevelEntity by type: " + id );
                }
                entityItems.Remove( id );

                entity.Remove();
            }
            else
            {
                Console.Error.WriteLine( "Could not remove LevelEntity: " + id );
            }
        }

        private IEnumerable<XElement> getEntitiesOfTypes( params string[] types )
        {
            LinkedList<XElement> subset = new LinkedList<XElement>();
            foreach( var type in types )
            {
                SortedSet<UInt64> typeIDs;
                if( typesToIDs.TryGetValue( type, out typeIDs ) )
                {
                    foreach( var id in typeIDs )
                    {
                        subset.AddLast( entityItems[id] );
                    }
                }
            }
            return subset;
        }

        internal SortedSet<UInt64> getIDs( string type )
        {
            SortedSet<UInt64> typeIDs;
            if( !typesToIDs.TryGetValue( type, out typeIDs ) )
            {
                return new SortedSet<UInt64>();
            }
            return new SortedSet<UInt64>( typeIDs );
        }

        internal LinkedList<SaveEditor.MapPosition> getPositions( string type, CommandCenterPosition cc )
        {
            var positions = new LinkedList<SaveEditor.MapPosition>();
            SortedSet<UInt64> typeIDs;
            if( typesToIDs.TryGetValue( type, out typeIDs ) )
            {
                foreach( var id in typeIDs )
                {
                    positions.AddLast( new SaveEditor.MapPosition( entityItems[id], cc ) );
                }
            }
            return positions;
        }

        internal SortedSet<UInt64> getIDs()
        {
            SortedSet<UInt64> keys = new SortedSet<UInt64>();
            foreach( var k in entityItems.Keys )
            {
                keys.Add( k );
            }
            return keys;
        }

        internal LinkedList<ScaledEntity> scaleEntities( string entityType, decimal scale, IDGenerator editor )
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
                SaveEditor.getFirstSimplePropertyNamed( iCopy.Element( "Complex" ), "ID" ).SetAttributeValue( "value", newID );

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

        internal void relocate( UInt64 origin, UInt64 destination )
        {
            if( origin == destination )
            {
                return;
            }

            XElement originEntity;
            if( !entityItems.TryGetValue( origin, out originEntity ) )
            {
                Console.Error.WriteLine( "Could not relocate LevelEntity: " + origin );
                return;
            }
            XElement destinationEntity;
            if( !entityItems.TryGetValue( destination, out destinationEntity ) )
            {
                Console.Error.WriteLine( "Could not relocate LevelEntity: " + origin + " to: " + destination );
                return;
            }

            // We're after 3 different values in 2 different <Complex under Components
            // 1
            XElement currentBehaviourTargetPosition = SaveEditor.getFirstSimplePropertyNamed( SaveEditor.getFirstComplexPropertyNamed( getBehaviour( originEntity ), "Data" ), "TargetPosition" );

            // 2
            XElement currentMovableTargetPosition = SaveEditor.getFirstSimplePropertyNamed( getMovable( originEntity ), "TargetPosition" );
            // 3
            XElement currentLastDestinyProcessed = SaveEditor.getFirstSimplePropertyNamed( getMovable( originEntity ), "LastDestinyProcessed" );

            string farthestPositionString = (string) getPosition( destinationEntity );

            getPosition( originEntity ).SetValue( farthestPositionString );
            SaveEditor.getFirstSimplePropertyNamed( getComplex( originEntity ), "LastPosition" ).SetAttributeValue( "value", farthestPositionString );
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

        internal void swapZombieType( UInt64 id )
        {
            XElement entity;
            if( entityItems.TryGetValue( id, out entity ) )
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

                XElement complex = entity.Element( "Complex" );
                XElement components = getComponents( entity );
                XElement cLife = SaveEditor.getComplexItemOfType( components, CLIFE_TYPE );

                XElement path = ( from c in getMovable( entity ).Element( "Properties" ).Elements( "Collection" )
                                    where (string) c.Attribute( "name" ) == "Path"
                                    select c ).SingleOrDefault();

                string type;
                string flags;
                string template;
                string life;
                string behaviour;
                string pathCapacity;
                string size;
                if( (string) complex.Attribute( "type" ) == MUTANT_TYPE )
                {
                    type = GIANT_TYPE;
                    flags = "None";
                    template = GIANT_ID_TEMPLATE;
                    life = GIANT_LIFE;
                    behaviour = GIANT_BEHAVIOUR_TYPE;
                    pathCapacity = "4";
                    size = GIANT_SIZE;

                    XElement cInflamable = SaveEditor.getComplexItemOfType( components, CINFLAMABLE, false );
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

                complex.Attribute( "type" ).SetValue( type );
                SaveEditor.getFirstSimplePropertyNamed( complex, "Flags" ).Attribute( "value" ).SetValue( flags );
                SaveEditor.getFirstSimplePropertyNamed( complex, "IDTemplate" ).Attribute( "value" ).SetValue( template );
                SaveEditor.getFirstSimplePropertyNamed( cLife, "Life" ).Attribute( "value" ).SetValue( life );
                getBehaviour( entity ).Attribute( "type" ).SetValue( behaviour );
                SaveEditor.getFirstSimplePropertyNamed( path, "Capacity" ).Attribute( "value" ).SetValue( pathCapacity );
                SaveEditor.getFirstSimplePropertyNamed( complex, "Size" ).Attribute( "value" ).SetValue( size );
            }
            else
            {
                Console.Error.WriteLine( "Could not type-swap LevelEntity: " + id );
            }
        }

        internal void resizeVODs( SaveEditor.VodSizes vodSize )
        {
            string newType;
            string newIDTemplate;
            string newLife;
            string newSize;
            switch( vodSize )
            {
                default:
                case SaveEditor.VodSizes.SMALL:
                    newType = VOD_SMALL_TYPE;
                    newIDTemplate = VOD_SMALL_ID_TEMPLATE;
                    newLife = VOD_SMALL_LIFE;
                    newSize = VOD_SMALL_SIZE;
                    break;
                case SaveEditor.VodSizes.MEDIUM:
                    newType = VOD_MEDIUM_TYPE;
                    newIDTemplate = VOD_MEDIUM_ID_TEMPLATE;
                    newLife = VOD_MEDIUM_LIFE;
                    newSize = VOD_MEDIUM_SIZE;
                    break;
                case SaveEditor.VodSizes.LARGE:
                    newType = VOD_LARGE_TYPE;
                    newIDTemplate = VOD_LARGE_ID_TEMPLATE;
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

            IEnumerable<XElement> vodItems = getEntitiesOfTypes( VOD_SMALL_TYPE, VOD_MEDIUM_TYPE, VOD_LARGE_TYPE );
            //Console.WriteLine( "vodItems: " + vodItems.Count() );

            foreach( XElement v in vodItems.ToList() )  // no ToList() leads to only removing 1 <item> per save modify cycle?!
            {
                XElement complex = v.Element( "Complex" );
                complex.SetAttributeValue( "type", newType );
                SaveEditor.getFirstSimplePropertyNamed( complex, "IDTemplate" ).SetAttributeValue( "value", newIDTemplate );
                SaveEditor.getFirstSimplePropertyNamed( SaveEditor.getComplexItemOfType( SaveEditor.getComponents( complex ), CLIFE_TYPE ), "Life" ).SetAttributeValue( "value", newLife );
                SaveEditor.getFirstSimplePropertyNamed( complex, "Size" ).SetAttributeValue( "value", newSize );
            }
        }

        internal void scaleVODs( string vodType, decimal scale, IDGenerator editor )
        {
            scaleEntities( vodType, scale, editor );
        }
    }
}
