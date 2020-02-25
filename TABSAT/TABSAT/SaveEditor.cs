using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        internal static readonly ReadOnlyDictionary<ThemeType, string> themeTypeNames;
        static SaveEditor()
        {
            Dictionary<ThemeType, string> ttn = new Dictionary<ThemeType, string>();
            ttn.Add( ThemeType.FA, "Deep Forest" );
            ttn.Add( ThemeType.BR, "Dark Moorland" );
            ttn.Add( ThemeType.TM, "Peaceful Lowlands" );
            ttn.Add( ThemeType.AL, "Frozen Highlands" );
            ttn.Add( ThemeType.DS, "Desert Wasteland" );
            ttn.Add( ThemeType.VO, "Caustic Lands" );
            themeTypeNames = new ReadOnlyDictionary<ThemeType, string>( ttn );
        }

        // TAB cell coordinates are origin top left, before 45 degree rotation clockwise. Positive x is due SE, positive y is due SW.
        enum CompassDirection
        {
            North,
            East,
            South,
            West
        }

        private class HugeZombie
        {
            readonly XElement entityItem;
            public readonly ulong id;
            readonly XElement complex;
            public readonly bool isMutantNotGiant;
            XAttribute entityPositionValue;

            public HugeZombie( XElement i )
            {
                entityItem = i;
                id = (ulong) entityItem.Element( "Simple" ).Attribute( "value" );

                complex = entityItem.Element( "Complex" );
                isMutantNotGiant = ( string) complex.Attribute( "type" ) == mutantType;

                Console.WriteLine( ( isMutantNotGiant ? "Mut" : "Gi" ) + "ant:\t\t" + id );
            }

            private XAttribute getPositionValue()
            {
                if( entityPositionValue == null )
                {
                    entityPositionValue  = getFirstSimplePropertyNamed( complex, "Position" ).Attribute( "value" );
                }
                return entityPositionValue;
            }

            internal void delete()
            {
                entityItem.Remove();
            }

            internal void relocate( HugeZombie other )
            {
                if( this.id == other.id )
                {
                    return;
                }

                XElement currentLastPosition = getFirstSimplePropertyNamed( complex, "LastPosition" );

                XElement currentComponents = ( from c in complex.Element( "Properties" ).Elements( "Collection" )
                                                where (string) c.Attribute( "name" ) == "Components"
                                                select c ).First();
                // We're after 3 different values in 2 different <Complex under Components
                XElement currentCBehaviour = ( from c in currentComponents.Element( "Items" ).Elements( "Complex" )
                                                where (string) c.Attribute( "type" ) == cBehaviourType
                                                select c ).First();
                
                XElement currentBehaviour = getFirstComplexPropertyNamed( currentCBehaviour, "Behaviour" );
                XElement currentBehaviourData = getFirstComplexPropertyNamed( currentBehaviour, "Data" );
                // 1
                XElement currentBehaviourTargetPosition = getFirstSimplePropertyNamed( currentBehaviourData, "TargetPosition" );
                
                XElement currentCMovable = ( from c in currentComponents.Element( "Items" ).Elements( "Complex" )
                                                where (string) c.Attribute( "type" ) == cMovableType
                                                select c ).First();
                
                // 2
                XElement currentMovableTargetPosition = getFirstSimplePropertyNamed( currentCMovable, "TargetPosition" );
                // 3
                XElement currentLastDestinyProcessed = getFirstSimplePropertyNamed( currentCMovable, "LastDestinyProcessed" );
                
                string farthestPositionString = (string) other.getPositionValue();
                
                getPositionValue().SetValue( farthestPositionString );
                currentLastPosition.SetAttributeValue( "value", farthestPositionString );
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
        }

        private class HugeZombieIcon
        {
            readonly SaveEditor editor;
            readonly XElement icon;
            public readonly ulong id;
            public readonly bool isMutantNotGiant;
            XAttribute iconCellValue;
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

                XElement imageSimple = getFirstSimplePropertyNamed( icon, "IDProjectImage" );
                isMutantNotGiant = (string) imageSimple.Attribute( "value" ) == mutantProjectImage;

                dir = null;

                Console.WriteLine( ( isMutantNotGiant ? "Mut" : "Gi" ) + "ant Icon:\t" + id );
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

                Console.WriteLine( ( isMutantNotGiant ? "Mut" : "Gi" ) + "ant:\t\t" + id + ",\tPosition: " + x + ", " + y + "\tis: " + dir + ",\tdistanceSquared: " + distanceSquared );
            }

            internal void relocate( HugeZombieIcon otherIcon )
            {
                if( this.id == otherIcon.id )
                {
                    return;
                }

                getCellValue().SetValue( otherIcon.getCellValue().Value );
                dir = null;     // no need to trigger assessPosition() yet

                Console.WriteLine( "Mutant icon: " + id + " relocated to: " + getCellValue().Value );
            }

            internal void delete()
            {
                icon.Remove();
            }
        }

        private class HugeZombieIconComparer : IComparer<HugeZombieIcon>
        {
            public int Compare( HugeZombieIcon a, HugeZombieIcon b )
            {
                return b.getDistanceSquared().CompareTo( a.getDistanceSquared() );  // b.CompareTo( a ) for reversed order
            }
        }
        private static readonly IComparer<HugeZombieIcon> iconDistanceComparer = new HugeZombieIconComparer();

        private const string mutantType = @"ZX.Entities.ZombieMutant, TheyAreBillions";
        private const string giantType = @"ZX.Entities.ZombieGiant, TheyAreBillions";
        private const string cBehaviourType = @"ZX.Components.CBehaviour, TheyAreBillions";
        private const string cMovableType = @"ZX.Components.CMovable, TheyAreBillions";
        private const string mutantProjectImage = @"3097669356589096184";
        //private const string giantProjectImage = @"5072922660204167778";

        private string dataFile;
        private XElement data;
        XElement levelComplex;

        private int commandCenterX;
        private int commandCenterY;
        private SortedDictionary<ulong, HugeZombie> mutants;
        private SortedDictionary<ulong, HugeZombie> giants;
        private LinkedList<HugeZombieIcon> mutantIcons;
        private LinkedList<HugeZombieIcon> giantIcons;

        private static XElement getFirstSimplePropertyNamed( XElement c, string name )
        {
            return ( from s in c.Element( "Properties" ).Elements( "Simple" )
                     where (string) s.Attribute( "name" ) == name
                     select s ).FirstOrDefault();
        }

        private static XElement getFirstComplexPropertyNamed( XElement c, string name )
        {
            return ( from s in c.Element( "Properties" ).Elements( "Complex" )
                     where (string) s.Attribute( "name" ) == name
                     select s ).FirstOrDefault();   // Avoid exception risk of First()?
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

            Console.WriteLine( "CurrentCommandCenterCell: " + commandCenterX + ", " + commandCenterY );

            mutantIcons = new LinkedList<HugeZombieIcon>();
            giantIcons = new LinkedList<HugeZombieIcon>();
        }

        internal void save()    // Doesn't take parameter to save modified file to a different location?
        {
            data.Save( dataFile );
        }

        private void findMutantsAndGiants()
        {
            // Finds the related Items in the LevelEntities Dictionary
            XElement itemsDict = ( from d in levelComplex.Element( "Properties" ).Elements( "Dictionary" )
                                   where (string) d.Attribute( "name" ) == "LevelEntities"
                                   select d ).First();

            //Console.WriteLine( "Items count: " + items.Descendants( "Item" ).Count() );

            IEnumerable<XElement> bigBoys =
                from i in itemsDict.Element( "Items" ).Elements( "Item" )
                where
                    ( from big in i.Elements( "Complex" )
                      where (string) big.Attribute( "type" ) == mutantType
                      || (string) big.Attribute( "type" ) == giantType
                      select big ).Any()
                select i;

            //Console.WriteLine( "bigBoys count: " + bigBoys.Count() );
            foreach( XElement big in bigBoys )
            {
                HugeZombie z = new HugeZombie( big );
                ( z.isMutantNotGiant ? mutants : giants ).Add( z.id, z );
            }

            XElement iconsColl = ( from c in levelComplex.Element( "Properties" ).Elements( "Collection" )
                                   where (string) c.Attribute( "name" ) == "MiniMapIndicators"
                                   select c ).First();

            // Find all Mutant minimap icons
            foreach( XElement c in iconsColl.Element( "Items" ).Elements( "Complex" ) )
            {
                HugeZombieIcon z = new HugeZombieIcon( this, c );
                if( z.isMutantNotGiant )
                {
                    mutantIcons.AddLast( z );
                }
                else
                {
                    giantIcons.AddLast( z );
                }
            }

            Console.WriteLine( "Mutants count: " + mutants.Count() +", icons count: " + mutantIcons.Count() );
            Console.WriteLine( "Giants count: " + giants.Count() + ", icons count: " + giantIcons.Count() );
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

        internal void relocateMutants( bool toGiantNotMutant, bool perDirection )
        {
            findMutantsAndGiants();

            if( !mutants.Any() )
            {
                Console.WriteLine( "No Mutants to relocate." );
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
                mutantsPerDirection[mutant.getDirection()].Add(  mutant );
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
                                Console.WriteLine( "No Giants to relocate Mutants onto in direction: " + d + ", using global farthest Giant." );
                                relocateTo = globalFarthestGiant;
                                if( relocateTo == null )
                                {
                                    Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
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
                    Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
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
                HugeZombie farthest = (farthestIcon.isMutantNotGiant ? mutants : giants)[farthestIcon.id];
                mutant.relocate( farthest );
            }
        }


        internal void removeFog( int radius = 0 )
        {
            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            XElement generatedLevel = getFirstComplexPropertyNamed( levelComplex, "CurrentGeneratedLevel" );
            //          <Properties>
            //            <Simple name="NCells" value="256" />
            XElement ncells = getFirstSimplePropertyNamed( generatedLevel, "NCells" );

            string cells = (string) ncells.Attribute( "value" );
            int size;
            if( ! Int32.TryParse( cells, out size ) )
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

            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            XElement generatedLevel = getFirstComplexPropertyNamed( levelComplex, "CurrentGeneratedLevel" );
            //          <Properties>
            //            <Complex name="Data">
            //              <Properties>
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            XElement extension = getFirstComplexPropertyNamed( generatedLevel, "Data" ).Element( "Properties" ).Element( "Complex" ); // only one Complex, named Extension
            //                  <Properties>
            //                    <Complex name="MapDrawer">
            XElement mapDrawer = getFirstComplexPropertyNamed( extension, "MapDrawer" );
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
