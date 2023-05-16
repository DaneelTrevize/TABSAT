using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TABSAT
{
    interface IDGenerator
    {
        UInt64 newID();
    }

    internal class MiniMapIcons
    {
        // Refactor into SaveReader..?

        private const string GIANT_PROJECT_IMAGE = @"5072922660204167778";
        private const string MUTANT_PROJECT_IMAGE = @"3097669356589096184";

        private readonly SortedDictionary<UInt64, XElement> iconsItems;

        private static XAttribute getID( XElement icon )
        {
            //                 <Complex name="EntityRef">
            //                   <Properties >
            //                     <Simple name = "IDEntity"
            return SaveReader.getValueAttOfSimpleProp( SaveReader.getFirstComplexPropertyNamed( icon, "EntityRef" ), "IDEntity" );
        }

        internal MiniMapIcons( XElement levelComplex )
        {
            iconsItems = new SortedDictionary<UInt64, XElement>();
            foreach( var icon in SaveReader.getFirstPropertyOfTypeNamed( levelComplex, "Collection", "MiniMapIndicators" ).Element( "Items" ).Elements( "Complex" ) )
            {
                // Check it's not for ZXMiniMapIndicatorInfectedSwarn or something else
                if( (string) icon.Attribute( "type" ) == @"ZX.ZXMiniMapIndicatorInfo, TheyAreBillions" )
                {
                    iconsItems.Add( (UInt64) getID( icon ), icon );
                }
            }
        }

        internal void Remove( UInt64 id )
        {
            if( iconsItems.TryGetValue( id, out XElement icon ) )
            {
                iconsItems.Remove( id );
                icon.Remove();
            }
            else
            {
                Console.WriteLine( "Icon not found for HugeZombie id:" + id );
            }
        }

        internal void Duplicate( UInt64 oldID, UInt64 newID )
        {
            if( iconsItems.TryGetValue( oldID, out XElement icon ) )
            {
                XElement copy = new XElement( icon );
                getID( icon ).SetValue( newID );
                iconsItems.Add( newID, copy );
            }
            else
            {
                Console.WriteLine( "Icon not found for HugeZombie id:" + oldID );
            }
        }

        internal void swapZombieType( UInt64 id )
        {
            // replace the <Complex type="ZX.ZXMiniMapIndicatorInfo...><Properties> minimap icon's <Simple name="IDProjectImage" value=> and <Simple name="Title" value=> & "Text"
            if( iconsItems.TryGetValue( id, out XElement icon ) )
            {
                //<Simple name="IDProjectImage">
                XAttribute image = SaveReader.getValueAttOfSimpleProp( icon, "IDProjectImage" );

                string project_image;
                string text;
                string title;
                if( (string) image == MUTANT_PROJECT_IMAGE )
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

                image.SetValue( project_image );
                SaveReader.getValueAttOfSimpleProp( icon, "Text" ).SetValue( text );
                SaveReader.getValueAttOfSimpleProp( icon, "Title" ).SetValue( title );
            }
        }

        internal void relocate( UInt64 fromID, UInt64 toID )
        {
            if( fromID == toID )
            {
                return;
            }

            if( iconsItems.TryGetValue( toID, out XElement toIcon ) )
            {
                if( iconsItems.TryGetValue( fromID, out XElement fromIcon ) )
                {
                    SaveReader.getValueAttOfSimpleProp( fromIcon, "Cell" ).SetValue( SaveReader.getValueAttOfSimpleProp( toIcon, "Cell" ).Value );

                    //Console.WriteLine( "Icon: " + fromID + " relocated to: " + getCell( toIcon ).Value );
                }
            }
        }
    }

    internal class SaveEditor : SaveReader, IDGenerator
    {
        private const UInt64 FIRST_NEW_ID = 0x8000000000000000;

        private readonly MiniMapIcons icons;
        private UInt64 nextNewID;


        public SaveEditor( string filesPath ) : base( filesPath )
        {
            icons = new MiniMapIcons( levelComplex );

            nextNewID = 0;
        }


        internal void save()    // Doesn't take parameter to save modified file to a different location?
        {
            data.Save( dataFile );
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

            //uniqueIDs.UnionWith( entities.getIDs() );
            uniqueIDs = entities.getAllIDs();
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
            var inactiveItems = from zombieType in getInactiveZombieItems()
                            select zombieType.Element( "Collection" ).Element( "Items" );
            //Console.WriteLine( "zombieTypes: " + inactiveItems.Count() );
            var inactiveIDs = from c in inactiveItems.Elements( "Complex" )
                          let s = c.Element( "Properties" ).Elements( "Simple" )
                          from a in s
                          where (string) a.Attribute( "name" ) == "A"
                          select (string) a.Attribute( "value" );
            //Console.WriteLine( "inactiveIDs: " + inactiveIDs.Count() );
            addIDs( inactiveIDs );
            //Console.WriteLine( "Unique IDs: " + uniqueIDs.Count );

            /*
             *          <Collection name="ExtraEntities" >
                          <Items>
                            <Complex>
                              <Properties>
                                <Simple name="ID" value="
             */
            var extrasIDs = from c in getExtraEntities().Elements( "Complex" )
                            select getValueAttOfSimpleProp( c, "ID" ).Value;
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

        public UInt64 newID()
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

        internal void scalePopulation( in decimal scale, in bool scaleIdle, in bool scaleActive )
        {
            if( scale < 0.0M )
            {
                throw new ArgumentOutOfRangeException( "Scale must not be negative." );
            }

            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            SortedDictionary<LevelEntities.ScalableZombieTypes, decimal> scalableZombieTypeFactors = new SortedDictionary<LevelEntities.ScalableZombieTypes, decimal> {
                { LevelEntities.ScalableZombieTypes.ZombieWeakA, scale },
                { LevelEntities.ScalableZombieTypes.ZombieWeakB, scale },
                { LevelEntities.ScalableZombieTypes.ZombieWeakC, scale },
                { LevelEntities.ScalableZombieTypes.ZombieWorkerA, scale },
                { LevelEntities.ScalableZombieTypes.ZombieWorkerB, scale },
                { LevelEntities.ScalableZombieTypes.ZombieMediumA, scale },
                { LevelEntities.ScalableZombieTypes.ZombieMediumB, scale },
                { LevelEntities.ScalableZombieTypes.ZombieDressedA, scale },
                { LevelEntities.ScalableZombieTypes.ZombieStrongA, scale },
                { LevelEntities.ScalableZombieTypes.ZombieVenom, scale },
                { LevelEntities.ScalableZombieTypes.ZombieHarpy, scale }
            };
            scalePopulation( scalableZombieTypeFactors, scaleIdle, scaleActive );
        }

        internal void scalePopulation( SortedDictionary<LevelEntities.ScalableZombieGroups, decimal> scalableZombieGroupFactors, in bool scaleIdle, in bool scaleActive )
        {
            SortedDictionary<LevelEntities.ScalableZombieTypes, decimal> scalableZombieTypeFactors = new SortedDictionary<LevelEntities.ScalableZombieTypes, decimal>();

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

                if( LevelEntities.scalableZombieTypeGroups.TryGetValue( k_v.Key, out SortedSet<LevelEntities.ScalableZombieTypes> groupTypes ) )
                {
                    foreach( var t in groupTypes )
                    {
                        scalableZombieTypeFactors.Add( t, scale );
                    }
                }
            }

            scalePopulation( scalableZombieTypeFactors, scaleIdle, scaleActive );
        }

        private void scalePopulation( SortedDictionary<LevelEntities.ScalableZombieTypes, decimal> scalableZombieTypeFactors, in bool scaleIdle, in bool scaleActive )
        {
            // First handle zombies from and idle since the map was generated, found in LevelFastSerializedEntities
            if( scaleIdle )
            {
                var zombieTypes = getInactiveZombieItems();
                foreach( var typeItem in zombieTypes )
                {
                    UInt64 zombieTypeInt = Convert.ToUInt64( typeItem.Element( "Simple" ).Attribute( "value" ).Value );
                    //Console.WriteLine( "zombieTypeInt: " + zombieTypeInt );
                    if( !Enum.IsDefined( typeof( LevelEntities.ScalableZombieTypes ), zombieTypeInt ) )
                    {
                        Console.Error.WriteLine( "Unexpected Serialized TypeID: " + zombieTypeInt );
                        continue;   // Can't scale this type
                    }

                    LevelEntities.ScalableZombieTypes zombieType = (LevelEntities.ScalableZombieTypes) zombieTypeInt;
                    if( !scalableZombieTypeFactors.ContainsKey( zombieType ) )
                    {
                        continue;   // Won't be scaling this type
                    }

                    var collection = typeItem.Element( "Collection" );
                    decimal scale = scalableZombieTypeFactors[zombieType];
                    scaleInactiveZombies( collection, scale );
                }
            }

            // Next handle aggro'd and spawn-generated zombies, found in LevelEntities
            if( scaleActive )
            {
                foreach( var k_v in scalableZombieTypeFactors )
                {
                    var zombieType = k_v.Key;
                    var scale = k_v.Value;
                    entities.scaleEntities( (UInt64) zombieType, scale, this );
                }
            }
        }

        private void scaleInactiveZombies( XElement collection, in decimal scale )
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
                /*( from s in zCopy.Element( "Properties" ).Elements( "Simple" )
                  where (string) s.Attribute( "name" ) == "A"
                  select s ).Single()*/
                getValueAttOfSimpleProp( zCopy, "A" ).SetValue( newID() );
                copiedZombies.AddLast( zCopy );
            }

            Random rand = new Random();

            int multiples = (int) scale;                // How many duplicates to certainly make of each zombie
            double chance = (double) ( scale % 1 );     // The chance of making 1 more duplicate per zombie
            //Console.WriteLine( "multiples: " + multiples + ", chance: " + chance );

            // Get the capacity for each ZombieType
            /*
             *             <Item>
             *               <Simple />
             *               <Collection >
             *                 <Properties>
             *                   <Simple name="Capacity" value=
             * 
             */
            var cap = /*( from s in col.Element( "Properties" ).Elements( "Simple" )
                         where (string) s.Attribute( "name" ) == "Capacity"
                         select s ).Single()*/getValueAttOfSimpleProp( collection, "Capacity" );
            //Console.WriteLine( zombieType + ": " + cap.Value );

            if( scale < 1.0M )
            {
                // chance is now chance to not remove existing zombies

                if( scale == 0.0M )
                {
                    // No need to iterate and count
                    collection.Element( "Items" ).RemoveNodes();

                    cap.SetValue( 0 );
                }
                else
                {
                    // 0 > scale < 1
                    var removedZombies = new LinkedList<XElement>();

                    foreach( var com in collection.Element( "Items" ).Elements( "Complex" ) )
                    {
                        if( chance < rand.NextDouble() )
                        {
                            // Removing within foreach doen't work, without .ToList(). Might as well collect candidates so we also have an O(1) count for later too
                            removedZombies.AddLast( com );
                        }
                    }
                    //Console.WriteLine( "removedZombies: " + removedZombies.Count );

                    foreach( var i in removedZombies )
                    {
                        i.Remove();
                    }

                    // Update capacity count
                    UInt64 newCap = (UInt64) ( Convert.ToInt32( cap.Value ) - removedZombies.Count );   // Assume actual UInt64 Capacity value is positive Int32 size and no less than Count removed
                    cap.SetValue( newCap );
                    //Console.WriteLine( "newCap: " + newCap );
                }
            }
            else
            {
                var selectedZombies = new LinkedList<XElement>();

                foreach( var com in collection.Element( "Items" ).Elements( "Complex" ) )
                {
                    // First the certain duplications
                    for( int m = 1; m < multiples; m++ )
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
                    collection.Element( "Items" ).Add( i );
                }

                // Update capacity count
                UInt64 newCap = Convert.ToUInt64( cap.Value ) + (UInt64) selectedZombies.Count;
                cap.SetValue( newCap );
                //Console.WriteLine( "newCap: " + newCap );
            }
        }

        internal void scaleHugePopulation( in bool giantsNotMutants, in decimal scale )
        {
            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            var selectedZombies = entities.scaleEntities( (UInt64) (giantsNotMutants ? LevelEntities.HugeTypes.Giant : LevelEntities.HugeTypes.Mutant), scale, this );

            if( scale < 1.0M )
            {
                foreach( var s in selectedZombies )
                {
                    // Find and remove their minimap icons
                    icons.Remove( s.Before );
                }
            }
            else
            {
                foreach( var s in selectedZombies )
                {
                    // Find and duplicate their minimap icon
                    icons.Duplicate( s.Before, (UInt64) s.After );
                }
            }
        }
        
        internal void replaceHugeZombies( bool toGiantNotMutant )
        {
            var mutants = entities.getIDs( (UInt64) LevelEntities.HugeTypes.Mutant );
            if( toGiantNotMutant && !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to replace." );
                return;
            }
            var giants = entities.getIDs( (UInt64) LevelEntities.HugeTypes.Giant );
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
            var fromSet = toGiantNotMutant ? mutants : giants;
            foreach( var id in fromSet )
            {
                entities.swapZombieType( id );

                icons.swapZombieType( id );
            }
        }

        private class RelativePosition
        {
            public readonly UInt64 ID;
            public readonly CompassDirection Direction;
            public readonly float distanceSquared;

            internal RelativePosition( XElement i, in int CCX, in int CCY )
            {
                // The below value extractions are best relocated into LevelEntities? EntityDescriptors under UnitsGenerationPack under LevelEvents under LevelState also have ID and Position.
                // LevelEvents is also duplicated under Extension under Data under CurrentGeneratedLevel also under LevelState.
                ID = (UInt64) i.Element( "Simple" ).Attribute( "value" );
                extractCoordinates( i, out int x, out int y );

                if( x <= CCX )
                {
                    // North or West
                    Direction = y <= CCY ? CompassDirection.North : CompassDirection.West;
                }
                else
                {
                    // East or South
                    Direction = y <= CCY ? CompassDirection.East : CompassDirection.South;
                }

                distanceSquared = (( x - CCX ) * ( x - CCX )) + (( y - CCY ) * ( y - CCY ));

                //Console.WriteLine( "id: " + id + "\tPosition: " + x + ", " + y + "\tis: " + dir + ",\tdistanceSquared: " + distanceSquared );
            }
        }
        
        private class DistanceComparer : IComparer<RelativePosition>
        {
            public int Compare( RelativePosition a, RelativePosition b )
            {
                return b.distanceSquared.CompareTo( a.distanceSquared );    // b.CompareTo( a ) for reversed order
            }
        }
        
        internal void relocateMutants( bool toGiantNotMutant, bool perDirection )
        {
            var mutants = entities.getEntitiesOfType( (UInt64) LevelEntities.HugeTypes.Mutant );
            if( !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to relocate." );
                return;
            }
            var giants = entities.getEntitiesOfType( (UInt64) LevelEntities.HugeTypes.Giant );

            void relocateMutants( ICollection<RelativePosition> movingMutants, in UInt64 farthestID )
            {
                foreach( RelativePosition z in movingMutants )
                {
                    entities.relocateHuge( z.ID, farthestID );

                    // Also see if both HugeZombies have had icons generated, for 1 to be repositioned to the other
                    icons.relocate( z.ID, farthestID );
                }
            }

            // Globally, or per direction, we'll have a list of huge zombie to find the farthest of, and a list of mutants to relocate, as well as their corresponding icons?

            var mutantsPerDirection = new SortedDictionary<CompassDirection,List<RelativePosition>>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                mutantsPerDirection.Add( d, new List<RelativePosition>( mutants.Count() ) );
            }
            var giantsPerDirection = new SortedDictionary<CompassDirection,List<RelativePosition>>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                giantsPerDirection.Add( d, new List<RelativePosition>( giants.Count() ) );
            }
            var farthestMutantShortlist = new List<RelativePosition>();
            var farthestGiantShortlist = new List<RelativePosition>();


            // Split huge zombies by direction
            foreach( var mutant in mutants )
            {
                RelativePosition p = new RelativePosition( mutant, commandCenterX, commandCenterY );
                mutantsPerDirection[p.Direction].Add( p );
            }
            foreach( var giant in giants )
            {
                RelativePosition p = new RelativePosition( giant, commandCenterX, commandCenterY );
                giantsPerDirection[p.Direction].Add( p );
            }

            // Sort each direction, take the farthest, form a new shortlist and sort that for the overall farthest
            IComparer<RelativePosition> relativePositionDistanceComparer = new DistanceComparer();

            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                var mutantsInDirection = mutantsPerDirection[d];
                if( mutantsInDirection.Count > 0 )
                {
                    mutantsInDirection.Sort( relativePositionDistanceComparer );
                    farthestMutantShortlist.Add( mutantsInDirection.First() );
                }
            }
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                var giantsInDirection = giantsPerDirection[d];
                if( giantsInDirection.Count > 0 )
                {
                    giantsInDirection.Sort( relativePositionDistanceComparer );
                    farthestGiantShortlist.Add( giantsInDirection.First() );
                }
            }

            farthestMutantShortlist.Sort( relativePositionDistanceComparer );
            farthestGiantShortlist.Sort( relativePositionDistanceComparer );
            RelativePosition globalFarthestMutant = farthestMutantShortlist.FirstOrDefault();
            RelativePosition globalFarthestGiant = farthestGiantShortlist.FirstOrDefault();
            

            if( perDirection )
            {
                // Get sorted mutant group per direction, also giants if toGiantNotMutant
                // Get farthest group mutant, or giant if toGiantNotMutant, or global farthest (could be either) if no local giant
                // relocate all group mutants to chosen farthest

                foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
                {
                    var mutantsInDirection = mutantsPerDirection[d];
                    if( mutantsInDirection.Count > 0 )
                    {
                        // There are mutants in this direction to relocation

                        UInt64 relocateToID;
                        // Should and could we use a Giant?
                        if( toGiantNotMutant )
                        {
                            var giantsInDirection = giantsPerDirection[d];
                            if( giantsInDirection.Count > 0 )
                            {
                                relocateToID = giantsInDirection.First().ID;
                            }
                            else
                            {
                                //Console.WriteLine( "No Giants to relocate Mutants onto in direction: " + d + ", using global farthest Mutant, or Giant if one exists." );
                                relocateToID = globalFarthestMutant.ID;
                                if( globalFarthestGiant != null )
                                {
                                    //Console.WriteLine( "Using farthest Giant." );
                                    relocateToID = globalFarthestGiant.ID;
                                }
                            }
                        }
                        else
                        {
                            relocateToID = mutantsInDirection.First().ID;
                        }

                        relocateMutants( mutantsInDirection, relocateToID );
                    }
                }
            }
            else
            {
                // Should and could we use a Giant?
                RelativePosition farthest = toGiantNotMutant ? globalFarthestGiant : globalFarthestMutant;
                if( farthest == null )
                {
                    //Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
                    farthest = globalFarthestMutant;
                }

                var movingMutants = new List<RelativePosition>( entities.getIDs( (UInt64) LevelEntities.HugeTypes.Mutant ).Count );
                foreach( var m in mutantsPerDirection.Values )
                {
                    movingMutants.AddRange( m );
                }
                relocateMutants( movingMutants, farthest.ID );
            }

        }
        
        internal void resizeVODs( LevelEntities.VODTypes vodType )
        {
            entities.resizeVODs( vodType );
        }

        internal void stackVODbuildings( LevelEntities.VODTypes vodType, decimal scale )
        {
            // Could use some add/remove entity delegates, to refactor this and zombie type scaling? Except zombie type collections can be RemoveNodes()'d per type, while level entities are all mixed in 1 collection.

            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            entities.scaleVODs( vodType, scale, this );
        }

        internal void removeFog( in uint radius = 0, in bool withinNotBeyond = true )
        {
            int rawLength = 4 * cellsCount * cellsCount;        // 4 bytes just to store 00 00 00 FF or 00 00 00 00, yuck

            // Sadly we can't use String.Create<TState>(Int32, TState, SpanAction<Char,TState>) to avoid duplicate allocation prior to creating the final string

            byte[] clearFog = new byte[rawLength];  // Defaulting to 00 00 00 00, good if we want less than 50% fog

            // Should we not modify the existing LayerFog data instead of potentially adding more fog than before..?

            if( withinNotBeyond )
            {
                if( radius > 0 )
                {
                    setFogCircle( commandCenterX, commandCenterY, cellsCount, clearFog, radius, withinNotBeyond );
                }
                // Else no fog, already done thanks to default byte array values.
            }
            else
            {
                if( radius > 0 )
                {
                    setFogCircle( commandCenterX, commandCenterY, cellsCount, clearFog, radius, withinNotBeyond );
                }/*
                else
                {
                    // Add fog everywhere. Never used?
                    for( int x = 0; x < cellsCount; x++ )
                    {
                        for( int y = 0; y < cellsCount; y++ )
                        {
                            //setFog( cellsCount, clearFog, x, y );
                        }
                    }
                }*/
            }

            string layerFog = Convert.ToBase64String( clearFog );
            //Console.WriteLine( layerFog );
            
            getValueAttOfSimpleProp( levelComplex, "LayerFog" ).SetValue( layerFog );
        }

        private static void setFogCircle( in int commandCenterX, in int commandCenterY, in int size, byte[] clearFog, in uint r, in bool clearWithinNotBeyond )
        {
            int radius = (int) r;   // Just assume it'll fit

            void setFog( in int s, byte[] f, in int x, in int y )
            {
                int wordIndex = ( ( y * s ) + x ) * 4;  // This indexing is backwards, because fog is oddly reversed, as well as in the opposite byte of the word
                f[wordIndex + 3] = 0xFF;
            }

            void setFogLine( in int s, byte[] f, in int xStart, in int y, in int xEnd )
            {
                for( int x = xStart; x < xEnd; x++ )
                {
                    setFog( s, f, x, y );
                }
            }

            int beforeCCx = commandCenterX - radius;
            int beforeCCy = commandCenterY - radius;
            int afterCCx = commandCenterX + radius;
            int afterCCy = commandCenterY + radius;

            if( clearWithinNotBeyond )
            {
                // Fill outside the circle with 00 00 00 FF, aka step 4 bytes at a time. Centered on CC

                // Quick fill in "thirds" of before, adjacent, after (CC +- radius)
                for( int x = 0; x < size; x++ )
                {
                    // 1/3 before, NorthEast
                    for( int y = 0; y < beforeCCy; y++ )
                    {
                        setFog( size, clearFog, x, y );
                    }
                    // 1/3 after, SouthWest
                    for( int y = afterCCy; y < size; y++ )
                    {
                        setFog( size, clearFog, x, y );
                    }
                }

                for( int y = beforeCCy; y < afterCCy; y++ )
                {
                    // 1/9 adjacent before, NorthWest
                    for( int x = 0; x < beforeCCx; x++ )
                    {
                        setFog( size, clearFog, x, y );
                    }
                    // 1/9 adjacent after, SouthEast
                    for( int x = afterCCx; x < size; x++ )
                    {
                        setFog( size, clearFog, x, y );
                    }
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

                if( clearWithinNotBeyond )
                {
                    // SE to S to SW
                    setFogLine( size, clearFog, commandCenterX + xFromCC, commandCenterY + yFromCC, radiusAfterCommandCenterX );

                    // SW to W to NW
                    setFogLine( size, clearFog, radiusBeforeCommandCenterX, commandCenterY + yFromCC, commandCenterX - xFromCC );

                    // NW to N to NE
                    setFogLine( size, clearFog, radiusBeforeCommandCenterX, commandCenterY - yFromCC, commandCenterX - xFromCC );

                    // NE to E to SE
                    setFogLine( size, clearFog, commandCenterX + xFromCC, commandCenterY - yFromCC, radiusAfterCommandCenterX );
                }
                else
                {
                    // Before and after CC
                    setFogLine( size, clearFog, commandCenterX - xFromCC, commandCenterY - yFromCC, commandCenterX + xFromCC );
                    setFogLine( size, clearFog, commandCenterX - xFromCC, commandCenterY + yFromCC, commandCenterX + xFromCC );
                }

                yFromCC += 1;
            }
        }

        internal void showFullMap()
        {
            //        <Simple name="ShowFullMap" value="False" />
            getValueAttOfSimpleProp( levelComplex, "ShowFullMap" ).SetValue( "True" );
        }

        internal void addExtraSupplies( uint food, uint energy, uint workers )
        {
            bool addValue( XAttribute extra, uint add )
            {
                if( !Int32.TryParse( extra.Value, out int value ) )
                {
                    return false;
                }

                value += (int) add;
                extra.SetValue( value );
                return true;
            }

            if( food > 0 )
            {
                if( !addValue( getValueAttOfSimpleProp( levelComplex, "CommandCenterExtraFood" ), food ) )
                {
                    Console.Error.WriteLine( "Unable to get the current CC Food supply value." );
                }
            }
            if( energy > 0 )
            {
                if( !addValue( getValueAttOfSimpleProp( levelComplex, "CommandCenterExtraEnergy" ), energy ) )
                {
                    Console.Error.WriteLine( "Unable to get the current CC Energy supply value." );
                }
            }
            if( workers > 0 )
            {
                if( !addValue( getValueAttOfSimpleProp( levelComplex, "CommandCenterExtraWorkers" ), workers ) )
                {
                    Console.Error.WriteLine( "Unable to get the current CC Workers supply value." );
                }
            }
        }

        internal void giftEntities( LevelEntities.GiftableTypes giftable, uint count )
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
                        if( !Int32.TryParse( secondSimpleValue.Value, out int existing ) )
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

            int storesCapacity = (1 + entities.getIDs( (UInt64) LevelEntities.GiftableTypes.WareHouse ).Count ) * RESOURCE_STORE_CAPACITY;   // "1+" assumes base CC storage, no mayor +25/+50 upgrades

            // Update CC stored values, per resource
            if( gold )
            {
                getValueAttOfSimpleProp( levelComplex, "Gold" ).SetValue( storesCapacity * GOLD_STORAGE_FACTOR );
            }
            if( wood )
            {
                getValueAttOfSimpleProp( levelComplex, "Wood" ).SetValue( storesCapacity );
            }
            if( stone )
            {
                getValueAttOfSimpleProp( levelComplex, "Stone" ).SetValue( storesCapacity );
            }
            if( iron )
            {
                getValueAttOfSimpleProp( levelComplex, "Iron" ).SetValue( storesCapacity );
            }
            if( oil )
            {
                getValueAttOfSimpleProp( levelComplex, "Oil" ).SetValue( storesCapacity );
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
            getValueAttOfSimpleProp( getMapDrawer(), "ThemeType" ).SetValue( themeValue );

            //<Complex name="Root" type="ZX.ZXGameState, TheyAreBillions">
            //  < Properties >
            //    <Complex name="SurvivalModeParams">
            XElement paramsComplex = getFirstComplexPropertyNamed( data, "SurvivalModeParams" );
            //      <Properties>
            //        <Simple name="ThemeType"
            getValueAttOfSimpleProp( paramsComplex, "ThemeType" ).SetValue( themeValue );
        }

        private XElement getSwarmByName( XElement collectionContainer, string name )
        {
            /*
             * The LevelEvents Collection appears to be duplicated (w.r.t. Generators) at different depths in a single save file,
             * a single method works from both given the correct starting element.
             */
            XElement eventsItems = getFirstPropertyOfTypeNamed( collectionContainer, "Collection", "LevelEvents" ).Element( "Items" );
            return ( from c in eventsItems.Elements( "Complex" )
                     where (string) getValueAttOfSimpleProp( c, "Name" ) == name
                     select c ).FirstOrDefault();
        }

        internal void fasterSwarms()
        {
            void setTimes( XElement c, SwarmTimings times )
            {
                if( c != null )
                {
                    getValueAttOfSimpleProp( c, "StartTimeH" ).SetValue( times.startTime );
                    getValueAttOfSimpleProp( c, "GameTimeH" ).SetValue( times.gameTime );
                    getValueAttOfSimpleProp( c, "RepeatTimeH" ).SetValue( times.repeatTime );
                    getValueAttOfSimpleProp( c, "StartNotifyTimeH" ).SetValue( times.notifyTime );
                }
            }

            void setTimesPair( string name, SwarmTimings times )
            {
                setTimes( getSwarmByName( levelComplex, name ), times );
                setTimes( getSwarmByName( getDataExtension(), name ), times );
            }

            var finish = GameFinish.Day50;
            var timings = swarmTimings[finish];
            setTimesPair( LEVEL_EVENT_GAME_WON_NAME, timings.Won );
            setTimesPair( SWARM_FINAL_NAME, timings.Final );
            setTimesPair( SWARM_EASY_NAME, timings.Easy );
            setTimesPair( SWARM_HARD_NAME, timings.Hard );
            setTimesPair( SWARM_ROAMING_WEAK_NAME, timings.Weak );
            setTimesPair( SWARM_ROAMING_MEDIUM_NAME, timings.Medium );
        }

        internal void setSwarms( bool earlierNotLater, SwarmDirections directions )
        {
            void setSwarm( XElement collectionContainer, string s, string g )
            {
                var complex = getSwarmByName( collectionContainer, s );
                if( complex != null )
                {
                    getValueAttOfSimpleProp( complex, "Generators" ).SetValue( g );
                }
            }

            string swarmName = earlierNotLater ? SWARM_EASY_NAME : SWARM_HARD_NAME;

            string generators;
            switch( directions )
            {
                default:
                case SwarmDirections.ONE:
                    generators = GENERATORS_1_DIRECTION;
                    break;
                case SwarmDirections.TWO:
                    generators = GENERATORS_2_DIRECTIONS;
                    break;
                case SwarmDirections.ALL_BUT_ONE:
                    generators = GENERATORS_3_DIRECTIONS;
                    break;
                case SwarmDirections.ALL:
                    generators = GENERATORS_4_DIRECTIONS;
                    break;
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
            setSwarm( levelComplex, swarmName, generators );

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
             *                            <Simple name="Name" value=
             *                            <Simple name="Generators" value=
             */
            // getFirstPropertyOfTypeNamed( getDataExtension(), "Collection", "LevelEvents" ).Element( "Items" );
            setSwarm( getDataExtension(), swarmName, generators );

        }

        internal void disableMayors()
        {
            //                 <Complex name="Extension" type="ZX.GameSystems.ZXLevelExtension, TheyAreBillions">
            //                  <Properties>
            //                    <Simple name="AllowMayors" value="True" />
            getValueAttOfSimpleProp( getDataExtension(), "AllowMayors" ).SetValue( "False" );
        }

        internal void removeReclaimables()
        {
            entities.removeReclaimables();
            // No need to update/reset SaveReader's Positions structures, as instance isn't reused after modification..?
        }
    }
}
