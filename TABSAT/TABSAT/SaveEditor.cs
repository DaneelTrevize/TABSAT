using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TABSAT
{
    class SaveEditor
    {

        public enum ThemeType
        {
            FA,
            BR,
            TM,
            AL,
            DS,
            VO
        }
        public static readonly ReadOnlyDictionary<ThemeType, string> themeTypeNames;
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

        enum CompassDirection
        {
            North,
            East,
            South,
            West
        }

        private const string mutantType = @"ZX.Entities.ZombieMutant, TheyAreBillions";
        private const string giantType = @"ZX.Entities.ZombieGiant, TheyAreBillions";
        private const string cBehaviourType = @"ZX.Components.CBehaviour, TheyAreBillions";
        private const string cMovableType = @"ZX.Components.CMovable, TheyAreBillions";
        private const string mutantProjectImage = @"3097669356589096184";
        private const string giantProjectImage = @"5072922660204167778";

        private XElement data;
        private SortedDictionary<ulong,XElement> mutants;
        private SortedDictionary<ulong, XElement> giants;
        XElement levelComplex;
        private XElement iconsColl;

        private int commandCenterX;
        private int commandCenterY;
        private LinkedList<XElement> mutantCells;
        private XElement farthestMutantIcon;
        private XElement farthestGiantIcon;

        public SaveEditor( string dataFile )
        {
            if( !File.Exists( dataFile ) )
            {
                throw new ArgumentException( "Data file does not exist: " + dataFile );
            }

            data = XElement.Load( dataFile );

            mutants = new SortedDictionary<ulong, XElement>();
            giants = new SortedDictionary<ulong, XElement>();

            //<Complex name="Root" type="ZX.ZXGameState, TheyAreBillions">
            //  < Properties >
            //    <Complex name="LevelState">
            levelComplex = ( from c in data.Element( "Properties" ).Elements( "Complex" )
                                      where (string) c.Attribute( "name" ) == "LevelState"
                                      select c ).First();

            iconsColl = null;

            //      <Properties>
            //        <Simple name="CurrentCommandCenterCell"
            XElement currentCommandCenterCell = ( from s in levelComplex.Element( "Properties" ).Elements( "Simple" )
                                                  where (string) s.Attribute( "name" ) == "CurrentCommandCenterCell"
                                                  select s ).First();
            string xy = (string) currentCommandCenterCell.Attribute( "value" );
            string[] xySplit = xy.Split( ';' );
            commandCenterX = int.Parse( xySplit[0] );
            commandCenterY = int.Parse( xySplit[1] );

            //Console.WriteLine( "CurrentCommandCenterCell: " + commandCenterX + ", " + commandCenterY );

            mutantCells = new LinkedList<XElement>();
            farthestMutantIcon = null;
            farthestGiantIcon = null;
        }

        public void save( string dataFile )
        {
            data.Save( dataFile );
        }

        private void findMutantsAndGiants()
        {
            // Finds the related Items in the LevelEntities Dictionary

            XElement itemsDict = ( from d in levelComplex.Element( "Properties" ).Elements( "Dictionary" )
                                   where (string) d.Attribute( "name" ) == "LevelEntities"
                                   select d ).First();

            //Console.WriteLine( "LevelEntities keyType: " + (string) items.Attribute( "keyType" ) );

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
                XAttribute id = big.Element( "Simple" ).Attribute( "value" );
                XElement complex = big.Element( "Complex" );
                if( (string) complex.Attribute( "type" ) == mutantType )
                {
                    Console.WriteLine( "Mutant:\t\t" + (string) id );
                    mutants.Add( (ulong) id, big );
                }
                else
                {
                    Console.WriteLine( "Giant:\t\t" + (string) id );
                    giants.Add( (ulong) id, big );
                }

                IEnumerable<XElement> positions =
                    from s in complex.Element( "Properties" ).Elements( "Simple" )
                    where (string) s.Attribute( "name" ) == "Position" || (string) s.Attribute( "name" ) == "LastPosition"
                    select s;
                foreach( XElement p in positions )
                {
                    Console.WriteLine( (string) p.Attribute( "name" ) + ":\t" + (string) p.Attribute( "value" ) );
                }
            }

            iconsColl = ( from c in levelComplex.Element( "Properties" ).Elements( "Collection" )
                                   where (string) c.Attribute( "name" ) == "MiniMapIndicators"
                                   select c ).First();
        }

        private void assessDistances()
        {

            //Console.WriteLine( "icons count: " + iconsColl.Element( "Items" ).Elements( "Complex" ).Count() );

            int farthestGiantDistanceSquared = 0;
            int farthestMutantDistanceSquared = 0;
            foreach( XElement c in iconsColl.Element( "Items" ).Elements( "Complex" ) )
            {
                XElement cellSimple = ( from s in c.Element( "Properties" ).Elements( "Simple" )
                                        where (string) s.Attribute( "name" ) == "Cell"
                                        select s ).First();

                string xy = (string) cellSimple.Attribute( "value" );
                Console.WriteLine( "Cell:\t\t" + xy );
                string[] xySplit = xy.Split( ';' );
                int x = int.Parse( xySplit[0] );
                int y = int.Parse( xySplit[1] );
                /*
                // Try to work per compass quadrant?
                CompassDirection dir;
                if( x <= commandCenterX )
                {
                    // West half
                    if( y <= commandCenterY )
                    {
                        // North half
                        dir = ( commandCenterY - y > commandCenterX - x ) ? CompassDirection.North : CompassDirection.West;
                    }
                    else
                    {
                        // South half
                        dir = ( y - commandCenterY > commandCenterX - x ) ? CompassDirection.South : CompassDirection.West;
                    }
                }
                else
                {
                    // East half
                    if( y <= commandCenterY )
                    {
                        // North half
                        dir = ( commandCenterY - y >  x - commandCenterX ) ? CompassDirection.North : CompassDirection.East;
                    }
                    else
                    {
                        // South half
                        dir = ( commandCenterY - y > x - commandCenterX ) ? CompassDirection.South : CompassDirection.East;
                    }
                }
                Console.WriteLine( "Position: " + x + ", " + y + " is: " + dir );
                */

                int distanceSquared = ( x - commandCenterX ) * ( x - commandCenterX ) + ( y - commandCenterY ) * ( y - commandCenterY );

                XElement imageSimple = ( from s in c.Element( "Properties" ).Elements( "Simple" )
                                         where (string) s.Attribute( "name" ) == "IDProjectImage"
                                         select s ).First();
                if( (string) imageSimple.Attribute( "value" ) == mutantProjectImage )
                {
                    Console.WriteLine( "Mutant" );

                    mutantCells.AddLast( cellSimple );

                    if( distanceSquared > farthestMutantDistanceSquared )
                    {
                        farthestMutantDistanceSquared = distanceSquared;
                        farthestMutantIcon = c;
                        Console.WriteLine( "New farthest Mutant: " + x + ", " + y + " distanceSquared: " + farthestMutantDistanceSquared );
                    }
                }
                else
                {
                    Console.WriteLine( "Giant" );

                    if( distanceSquared > farthestGiantDistanceSquared )
                    {
                        farthestGiantDistanceSquared = distanceSquared;
                        farthestGiantIcon = c;
                        Console.WriteLine( "New farthest Giant: " + x + ", " + y + " distanceSquared: " + farthestGiantDistanceSquared );
                    }
                }
            }
        }

        public void removeMutants()
        {
            findMutantsAndGiants();

            LinkedList<XElement> mutantIcons = new LinkedList<XElement>();
            // Find all Mutant minimap icons
            foreach( XElement c in iconsColl.Element( "Items" ).Elements( "Complex" ) )
            {
                XElement imageSimple = ( from s in c.Element( "Properties" ).Elements( "Simple" )
                                         where (string) s.Attribute( "name" ) == "IDProjectImage"
                                         select s ).First();
                if( (string) imageSimple.Attribute( "value" ) == mutantProjectImage )
                {
                    mutantIcons.AddLast( c );
                }
            }

            Console.WriteLine( "Mutants count: " + mutants.Count() );
            Console.WriteLine( "Icons count: " + mutantIcons.Count() );

            // Remove mutants and their icons
            foreach( XElement m in mutants.Values )
            {
                m.Remove();
            }
            foreach( XElement m in mutantIcons )
            {
                m.Remove();
            }

        }

        public void relocateMutants( bool toGiantNotMutant = true )
        {
            findMutantsAndGiants();

            assessDistances();

            XElement farthestIcon = toGiantNotMutant ? farthestGiantIcon : farthestMutantIcon;

            XElement farthestID = ( from c in farthestIcon.Element( "Properties" ).Elements( "Complex" )
                                    where (string) c.Attribute( "name" ) == "EntityRef"
                                    select c ).First().Element( "Properties" ).Element( "Simple" );
            //Console.WriteLine( (string) farthestID.Attribute( "name" ) + ":\t" + (string) farthestID.Attribute( "value" ) );

            string farthestCellString = (string) ( from s in farthestIcon.Element( "Properties" ).Elements( "Simple" )
                                                   where (string) s.Attribute( "name" ) == "Cell"
                                                   select s ).First().Attribute( "value" );
            //Console.WriteLine( "Farthest cell: " + farthestCellString );

            ulong farthestID_ulong = (ulong) farthestID.Attribute( "value" );
            XElement farthestBig = toGiantNotMutant ? giants[farthestID_ulong] : mutants[farthestID_ulong];
            //Console.WriteLine( "Farthest Big: " + farthestBig );

            XElement farthestPosition = ( from s in farthestBig.Element( "Complex" ).Element( "Properties" ).Elements( "Simple" )
                                          where (string) s.Attribute( "name" ) == "Position"
                                          select s ).First();
            string farthestPositionString = (string) farthestPosition.Attribute( "value" );
            //Console.WriteLine( "Farthest position: " + farthestPositionString );

            //Console.WriteLine( "Mutants count: " + mutants.Count() );
            // Use this farthest position value to move all the mutants
            foreach( XElement mutant in mutants.Values )
            {
                tryRelocateMutantItem( mutant, farthestPositionString );
            }
            // And move all the mutant icons
            foreach( XElement cellSimple in mutantCells )
            {
                //Console.WriteLine( "Mutant cell to change: " + cellSimple );
                cellSimple.SetAttributeValue( "value", farthestCellString );
            }
        }

        private void tryRelocateMutantItem( XElement mutant, string farthestPositionString )
        {
            XElement currentPosition = ( from s in mutant.Element( "Complex" ).Element( "Properties" ).Elements( "Simple" )
                                         where (string) s.Attribute( "name" ) == "Position"
                                         select s ).First();

            XElement currentLastPosition = ( from s in mutant.Element( "Complex" ).Element( "Properties" ).Elements( "Simple" )
                                             where (string) s.Attribute( "name" ) == "LastPosition"
                                             select s ).First();

            
            XElement currentComponents = ( from c in mutant.Element( "Complex" ).Element( "Properties" ).Elements( "Collection" )
                                           where (string) c.Attribute( "name" ) == "Components"
                                           select c ).First();
            // We're after 3 different values in 2 different <Complex under Components
            XElement currentCBehaviour = ( from c in currentComponents.Element( "Items" ).Elements( "Complex" )
                                           where (string) c.Attribute( "type" ) == cBehaviourType
                                           select c ).First();

            XElement currentBehaviour = ( from c in currentCBehaviour.Element( "Properties" ).Elements( "Complex" )
                                              where (string) c.Attribute( "name" ) == "Behaviour"
                                          select c ).First();
            XElement currentBehaviourData = ( from c in currentBehaviour.Element( "Properties" ).Elements( "Complex" )
                                              where (string) c.Attribute( "name" ) == "Data"
                                              select c ).First();
            // 1
            XElement currentBehaviourTargetPosition = ( from s in currentBehaviourData.Element( "Properties" ).Elements( "Simple" )
                                               where (string) s.Attribute( "name" ) == "TargetPosition"
                                               select s ).FirstOrDefault();
            
            XElement currentCMovable = ( from c in currentComponents.Element( "Items" ).Elements( "Complex" )
                                         where (string) c.Attribute( "type" ) == cMovableType
                                         select c ).First();
            // 2
            XElement currentMovableTargetPosition = ( from s in currentCMovable.Element( "Properties" ).Elements( "Simple" )
                                               where (string) s.Attribute( "name" ) == "TargetPosition"
                                               select s ).FirstOrDefault();
            // 3
            XElement currentLastDestinyProcessed = ( from s in currentCMovable.Element( "Properties" ).Elements( "Simple" )
                                                      where (string) s.Attribute( "name" ) == "LastDestinyProcessed"
                                                     select s ).FirstOrDefault();
            
            currentPosition.SetAttributeValue( "value", farthestPositionString );
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

        public void removeAllFog()
        {
            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            XElement generatedLevel = ( from c in levelComplex.Element( "Properties" ).Elements( "Complex" )
                                        where (string) c.Attribute( "name" ) == "CurrentGeneratedLevel"
                                        select c ).First();
            //          <Properties>
            //            <Simple name="NCells" value="256" />
            XElement ncells = ( from s in generatedLevel.Element( "Properties" ).Elements( "Simple" )
                                where (string) s.Attribute( "name" ) == "NCells"
                                select s ).First();

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
            string layerFog = Convert.ToBase64String( clearFog );
            //Console.WriteLine( layerFog );

            XElement layerFogSimple = ( from s in levelComplex.Element( "Properties" ).Elements( "Simple" )
                                        where (string) s.Attribute( "name" ) == "LayerFog"
                                        select s ).First();
            layerFogSimple.SetAttributeValue( "value", layerFog );
        }

        public void showFullMap()
        {
            //        <Simple name="ShowFullMap" value="False" />
            XElement ShowFullMap = ( from s in levelComplex.Element( "Properties" ).Elements( "Simple" )
                                     where (string) s.Attribute( "name" ) == "ShowFullMap"
                                     select s ).First();
            ShowFullMap.SetAttributeValue( "value", "True" );
        }

        public void changeTheme( ThemeType theme )
        {
            string themeValue = theme.ToString();
            //Console.WriteLine( "Changing ThemeType to:" + themeValue );

            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            XElement generatedLevel = ( from c in levelComplex.Element( "Properties" ).Elements( "Complex" )
                                        where (string) c.Attribute( "name" ) == "CurrentGeneratedLevel"
                                        select c ).First();
            //          <Properties>
            //            <Complex name="Data">
            //              <Properties>
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            XElement extension = ( from d in generatedLevel.Element( "Properties" ).Elements( "Complex" )
                                   where (string) d.Attribute( "name" ) == "Data"                   // not Random
                                   select d ).First().Element( "Properties" ).Element( "Complex" ); // only one Complex, named Extension
            //                  <Properties>
            //                    <Complex name="MapDrawer">
            XElement mapDrawer = ( from c in extension.Element( "Properties" ).Elements( "Complex" )
                                   where (string) c.Attribute( "name" ) == "MapDrawer"
                                   select c ).First();
            //                      <Properties>
            //                        <Simple name="ThemeType" value=
            XElement levelThemeType = ( from s in mapDrawer.Element( "Properties" ).Elements( "Simple" )
                               where (string) s.Attribute( "name" ) == "ThemeType"
                               select s ).First();

            //<Complex name="Root" type="ZX.ZXGameState, TheyAreBillions">
            //  < Properties >
            //    <Complex name="SurvivalModeParams">
            XElement paramsComplex = ( from c in data.Element( "Properties" ).Elements( "Complex" )
                                       where (string) c.Attribute( "name" ) == "SurvivalModeParams"
                                       select c ).First();
            //      <Properties>
            //        <Simple name="ThemeType"
            XElement paramsThemeType = ( from s in paramsComplex.Element( "Properties" ).Elements( "Simple" )
                                         where (string) s.Attribute( "name" ) == "ThemeType"
                                         select s ).First();

            levelThemeType.SetAttributeValue( "value", themeValue );
            paramsThemeType.SetAttributeValue( "value", themeValue );
        }
    }
}
