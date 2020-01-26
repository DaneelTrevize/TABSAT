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
        private const string mutantProjectImage = @"3097669356589096184";
        private const string giantProjectImage = @"5072922660204167778";

        private XElement data;
        private SortedDictionary<ulong,XElement> mutants;
        private SortedDictionary<ulong, XElement> giants;
        XElement levelComplex;
        private int commandCenterX;
        private int commandCenterY;
        private LinkedList<XElement> mutantCells;
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
            //      <Properties>
            //        <Simple name="CurrentCommandCenterCell"
            XElement currentCommandCenterCell = ( from s in levelComplex.Element( "Properties" ).Elements( "Simple" )
                                                  where (string) s.Attribute( "name" ) == "CurrentCommandCenterCell"
                                                  select s ).First();
            string xy = (string) currentCommandCenterCell.Attribute( "value" );
            string[] xySplit = xy.Split( ';' );
            commandCenterX = int.Parse( xySplit[0] );
            commandCenterY = int.Parse( xySplit[1] );

            Console.WriteLine( "CurrentCommandCenterCell: " + commandCenterX + ", " + commandCenterY );

            mutantCells = new LinkedList<XElement>();
            farthestGiantIcon = null;
        }

        public void save( string dataFile )
        {
            data.Save( dataFile );
        }

        private void findMutantsAndGiants()
        {
            XElement itemsDict = levelComplex.Element( "Properties" ).Element( "Dictionary" );

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

            XElement iconsColl = ( from c in levelComplex.Element( "Properties" ).Elements( "Collection" )
                                   where (string) c.Attribute( "name" ) == "MiniMapIndicators"
                                   select c ).First();

            //Console.WriteLine( "icons count: " + iconsColl.Element( "Items" ).Elements( "Complex" ).Count() );

            int farthestDistanceSquared = 0;
            foreach( XElement i in iconsColl.Element( "Items" ).Elements( "Complex" ) )
            {
                XElement cellSimple = ( from s in i.Element( "Properties" ).Elements( "Simple" )
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
                XElement imageSimple = ( from s in i.Element( "Properties" ).Elements( "Simple" )
                                         where (string) s.Attribute( "name" ) == "IDProjectImage"
                                         select s ).First();
                if( (string) imageSimple.Attribute( "value" ) == mutantProjectImage )
                {
                    Console.WriteLine( "Mutant" );
                    
                    mutantCells.AddLast( cellSimple );
                }
                else
                {
                    Console.WriteLine( "Giant" );

                    int distanceSquared = ( x - commandCenterX ) * ( x - commandCenterX ) + ( y - commandCenterY ) * ( y - commandCenterY );
                    if( distanceSquared > farthestDistanceSquared )
                    {
                        farthestDistanceSquared = distanceSquared;
                        farthestGiantIcon = i;
                        Console.WriteLine( "New farthest Giant: " + x + ", " + y + " distanceSquared: " + farthestDistanceSquared );
                    }
                }
            }

        }

        public void relocateMutants()
        {
            findMutantsAndGiants();

            XElement farthestID = ( from c in farthestGiantIcon.Element( "Properties" ).Elements( "Complex" )
                                    where (string) c.Attribute( "name" ) == "EntityRef"
                                    select c ).First().Element( "Properties" ).Element( "Simple" );
            Console.WriteLine( (string) farthestID.Attribute( "name" ) + ":\t" + (string) farthestID.Attribute( "value" ) );

            XElement farthestIconCell = ( from s in farthestGiantIcon.Element( "Properties" ).Elements( "Simple" )
                                          where (string) s.Attribute( "name" ) == "Cell"
                                          select s ).First();
            string farthestCellString = (string) farthestIconCell.Attribute( "value" );
            //Console.WriteLine( "Farthest Giant cell: " + farthestCellString );

            XElement farthestGiant = giants[(ulong) farthestID.Attribute( "value" )];
            //Console.WriteLine( "Farthest Giant: " + farthestGiant );

            XElement farthestPosition = ( from s in farthestGiant.Element( "Complex" ).Element( "Properties" ).Elements( "Simple" )
                                          where (string) s.Attribute( "name" ) == "Position"
                                          select s ).First();
            string farthestPositionString = (string) farthestPosition.Attribute( "value" );
            //Console.WriteLine( "Farthest Giant position: " + farthestPositionString );

            // Use this farthest position value to move all the mutants
            foreach( XElement mutant in mutants.Values )
            {
                XElement currentPosition = ( from s in farthestGiant.Element( "Complex" ).Element( "Properties" ).Elements( "Simple" )
                                             where (string) s.Attribute( "name" ) == "Position"
                                             select s ).First();
                XElement currentLastPosition = ( from s in farthestGiant.Element( "Complex" ).Element( "Properties" ).Elements( "Simple" )
                                                 where (string) s.Attribute( "name" ) == "LastPosition"
                                                 select s ).First();
                currentPosition.SetAttributeValue( "value", farthestPositionString );
                currentLastPosition.SetAttributeValue( "value", farthestPositionString );
            }
            // And move all the mutant icons
            foreach( XElement cellSimple in mutantCells )
            {
                //Console.WriteLine( "Mutant cell to change: " + cellSimple );
                cellSimple.SetAttributeValue( "value", farthestCellString );
            }
        }

        public void changeTheme( ThemeType theme )
        {
            string themeValue = theme.ToString();
            //Console.WriteLine( "Changing ThemeType to:" + themeValue );

            //      <Properties>
            //        <Complex name = "CurrentGeneratedLevel" >
            XElement generatedLevel = ( from d in levelComplex.Element( "Properties" ).Elements( "Complex" )
                                        where (string) d.Attribute( "name" ) == "CurrentGeneratedLevel"
                                        select d ).First();
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
