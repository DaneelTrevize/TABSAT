using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TABSAT
{
    internal class SaveEditor
    {

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
            Sawmill = 6484699889268923215,
            Quarry = 4012164333689948063,
            AdvancedQuarry = 6574833960938744452,
            OilPlatform = 15110117066074335339,
            HunterCottage = 706050193872584208,
            FishermanCottage = 13910727858942983852,
            Farm = 7709119203238641805,
            // AdvancedFarm
            WareHouse = 13640414733981798546,
            Market = 5507471650351043258,
            Bank = 5036892806562984913,
            TentHouse = 17301104073651661026,
            StoneHouse = 17389931916361639317,
            // TrapStakes                               // WoodTraps
            // TrapBlades                               // IronTraps
            WallWood = 16980392503923994773,
            WallStone = 7684920400170855714,
            // GateWood
            WatchTowerWood = 11206202837167900273,
            WoodWorkshop = 2943963846200136989,
            StoneWorkshop = 11153810025740407576,
            SoldiersCenter = 17945382406851792953,
            AdvancedUnitCenter = 8857617519118038933,
            LookoutTower = 9352245195514814739,
            RadarTower = 10083572309367106690,
            Ballista = 1621013738552581284,
            ShockingTower = 7671446590444700196,
            Executor = 782017986530656774,
            // TheCrystalPalace = 7936948209186953569,
            // TheSpire = 6908380734610266301,
            // TheTransmutator = 5872990212787919747,
            // TheAcademy = 8274629648718325688,
            // TheVictorious = 6953739609864588774
        }
        internal static readonly Dictionary<GiftableTypes, string> giftableTypeNames;

        static SaveEditor()
        {
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
            gtn.Add( GiftableTypes.Sawmill, "Sawmill" );
            gtn.Add( GiftableTypes.Quarry, "Quarry" );
            gtn.Add( GiftableTypes.AdvancedQuarry, "Advanced Quarry" );
            gtn.Add( GiftableTypes.OilPlatform, "Oil Platform" );
            gtn.Add( GiftableTypes.HunterCottage, "Hunter Cottage" );
            gtn.Add( GiftableTypes.FishermanCottage, "Fisherman Cottage" );
            gtn.Add( GiftableTypes.Farm, "Farm" );
            gtn.Add( GiftableTypes.WareHouse, "Warehouse" );
            gtn.Add( GiftableTypes.Market, "Market" );
            gtn.Add( GiftableTypes.Bank, "Bank" );
            gtn.Add( GiftableTypes.TentHouse, "Tent" );
            gtn.Add( GiftableTypes.StoneHouse, "Stone House" );
            gtn.Add( GiftableTypes.WallWood, "Wood Wall" );
            gtn.Add( GiftableTypes.WallStone, "Stone Wall" );
            gtn.Add( GiftableTypes.WatchTowerWood, "Wood Tower" );
            gtn.Add( GiftableTypes.WoodWorkshop, "Wood Workshop" );
            gtn.Add( GiftableTypes.StoneWorkshop, "Stone Workshop" );
            gtn.Add( GiftableTypes.SoldiersCenter, "Soldiers' Center" );
            gtn.Add( GiftableTypes.AdvancedUnitCenter, "Engineering Center" );
            gtn.Add( GiftableTypes.LookoutTower, "Lookout Tower" );
            gtn.Add( GiftableTypes.RadarTower, "Radar Tower" );
            gtn.Add( GiftableTypes.Ballista, "Ballista" );
            gtn.Add( GiftableTypes.Executor, "Executor" );
            giftableTypeNames = new Dictionary<GiftableTypes, string>( gtn );
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

        private const string MUTANT_TYPE = @"ZX.Entities.ZombieMutant, TheyAreBillions";
        private const string GIANT_TYPE = @"ZX.Entities.ZombieGiant, TheyAreBillions";
        private const string MUTANT_BEHAVIOUR_TYPE = @"ZX.Behaviours.BHZombie, TheyAreBillions";
        private const string GIANT_BEHAVIOUR_TYPE = @"ZX.Behaviours.BHZombieGiant, TheyAreBillions";
        private const string VOD_SMALL_TYPE = @"ZX.Entities.DoomBuildingSmall, TheyAreBillions";
        private const string VOD_MEDIUM_TYPE = @"ZX.Entities.DoomBuildingMedium, TheyAreBillions";
        private const string VOD_LARGE_TYPE = @"ZX.Entities.DoomBuildingLarge, TheyAreBillions";

        private const string VOD_LARGE_ID_TEMPLATE = @"3441286325348372349";
        private const string VOD_SMALL_ID_TEMPLATE = @"8702552346733362645";
        private const string VOD_LARGE_LIFE = @"4000";
        private const string VOD_SMALL_LIFE = @"400";
        private const string VOD_LARGE_SIZE = @"4;4";
        private const string VOD_SMALL_SIZE = @"2;2";
        private const string MUTANT_ID_TEMPLATE = @"4885015758634569309";
        private const string GIANT_ID_TEMPLATE = @"6179780658058987152";
        private const string MUTANT_PROJECT_IMAGE = @"3097669356589096184";
        private const string GIANT_PROJECT_IMAGE = @"5072922660204167778";
        private const string GIANT_LIFE = @"10000";
        private const string MUTANT_LIFE = @"4000";
        private const string GIANT_SIZE = @"1.6;1.6";
        private const string MUTANT_SIZE = @"0.8;0.8";

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

        private readonly int commandCenterX;
        private readonly int commandCenterY;
        private readonly SortedDictionary<ulong, HugeZombie> mutants;
        private readonly SortedDictionary<ulong, HugeZombie> giants;
        private readonly LinkedList<HugeZombieIcon> mutantIcons;
        private readonly LinkedList<HugeZombieIcon> giantIcons;

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
        }

        internal void save()    // Doesn't take parameter to save modified file to a different location?
        {
            data.Save( dataFile );
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


        private void findMutantsAndGiants()
        {
            IEnumerable<XElement> bigBoys =
                from i in getLevelEntitiesItems().Elements( "Item" )
                where
                    ( from big in i.Elements( "Complex" )
                      where (string) big.Attribute( "type" ) == MUTANT_TYPE
                      || (string) big.Attribute( "type" ) == GIANT_TYPE
                      select big ).Any()
                select i;

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

        internal void removeVODs()
        {
            IEnumerable<XElement> vodItems =
                (from i in getLevelEntitiesItems().Elements( "Item" )
                where
                    ( from vod in i.Elements( "Complex" )
                      where (string) vod.Attribute( "type" ) == VOD_SMALL_TYPE
                      || (string) vod.Attribute( "type" ) == VOD_MEDIUM_TYPE
                      || (string) vod.Attribute( "type" ) == VOD_LARGE_TYPE
                      select vod ).Any()
                select i);
            
            //Console.WriteLine( "vodItems: " + vodItems.Count() );
            foreach( XElement v in vodItems.ToList() )  // no ToList() leads to only removing 1 <item> per save modify cycle?!
            {
                v.Remove();
            }
        }

        internal void resizeVODs( bool largerNotSmaller )
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

            IEnumerable<XElement> vodItems =
                ( from i in getLevelEntitiesItems().Elements( "Item" )
                  where
                      ( from vod in i.Elements( "Complex" )
                        where ( largerNotSmaller && (string) vod.Attribute( "type" ) == VOD_SMALL_TYPE )
                        || (string) vod.Attribute( "type" ) == VOD_MEDIUM_TYPE
                        || (!largerNotSmaller && ( string) vod.Attribute( "type" ) == VOD_LARGE_TYPE )
                        select vod ).Any()
                  select i );
            
            foreach( XElement v in vodItems.ToList() )  // no ToList() leads to only removing 1 <item> per save modify cycle?!
            {
                XElement complex = v.Element( "Complex" );
                complex.SetAttributeValue( "type", largerNotSmaller ? VOD_LARGE_TYPE : VOD_SMALL_TYPE );
                getFirstSimplePropertyNamed( complex, "IDTemplate" ).SetAttributeValue( "value", largerNotSmaller ? VOD_LARGE_ID_TEMPLATE : VOD_SMALL_ID_TEMPLATE );
                getFirstSimplePropertyNamed( getComplexItemOfType( getComponents( complex ), CLIFE_TYPE ), "Life" ).SetAttributeValue( "value", largerNotSmaller ? VOD_LARGE_LIFE : VOD_SMALL_LIFE );
                getFirstSimplePropertyNamed( complex, "Size" ).SetAttributeValue( "value", largerNotSmaller ? VOD_LARGE_SIZE : VOD_SMALL_SIZE );
            }
        }

        internal void disableMayors()
        {
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            //                  <Properties>
            //                    <Simple name="AllowMayors" value="True" />
            XElement allowMayors = getFirstSimplePropertyNamed( getDataExtension(), "AllowMayors" );
            allowMayors.SetAttributeValue( "value", "False" );
        }

        internal void giftEntities( GiftableTypes giftable, int count )
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
            giftItem.Add( simple  );

            simple = new XElement( "Simple" );
            simple.SetAttributeValue( "value", count );
            giftItem.Add( simple );

            items.Add( giftItem );
        }

        internal void removeFog( int radius = 0 )
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

        private void circularFog( int size, byte[] clearFog, int radius )
        {
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

        internal void changeTheme( ThemeType theme )
        {
            string themeValue = theme.ToString();
            //Console.WriteLine( "Changing ThemeType to:" + themeValue );

            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            //                  <Properties>
            //                    <Complex name="MapDrawer">
            XElement mapDrawer = getFirstComplexPropertyNamed( getDataExtension(), "MapDrawer" );
            //                      <Properties>
            //                        <Simple name="ThemeType" value=
            XElement levelThemeType = getFirstSimplePropertyNamed( mapDrawer, "ThemeType" );

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
    }
}
