using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TABSAT
{
    interface IDGenerator
    {
        UInt64 newID();
    }

    internal class MiniMapIcons
    {
        private const string GIANT_PROJECT_IMAGE = @"5072922660204167778";
        private const string MUTANT_PROJECT_IMAGE = @"3097669356589096184";

        private readonly SortedDictionary<UInt64, XElement> iconsItems;

        private static XAttribute getID( XElement icon )
        {
            //                 <Complex name="EntityRef">
            //                   <Properties >
            //                     <Simple name = "IDEntity"
            return SaveReader.getFirstComplexPropertyNamed( icon, "EntityRef" ).Element( "Properties" ).Element( "Simple" ).Attribute( "value" );   // Could use getFirstSimplePropertyNamed( ..., "IDEntity" )?
        }

        private static XAttribute getCell( XElement icon )
        {
            return SaveReader.getFirstSimplePropertyNamed( icon, "Cell" ).Attribute( "value" );
        }

        internal MiniMapIcons( XElement levelComplex )
        {
            iconsItems = new SortedDictionary<ulong, XElement>();
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
            XElement icon;
            if( iconsItems.TryGetValue( id, out icon ) )
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
            XElement icon;
            if( iconsItems.TryGetValue( oldID, out icon ) )
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
            XElement icon;
            if( iconsItems.TryGetValue( id, out icon ) )
            {
                //<Simple name="IDProjectImage">
                XAttribute image = SaveReader.getFirstSimplePropertyNamed( icon, "IDProjectImage" ).Attribute( "value" );

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
                SaveReader.getFirstSimplePropertyNamed( icon, "Text" ).Attribute( "value" ).SetValue( text );
                SaveReader.getFirstSimplePropertyNamed( icon, "Title" ).Attribute( "value" ).SetValue( title );
            }
        }

        internal void relocate( UInt64 fromID, UInt64 toID )
        {
            if( fromID == toID )
            {
                return;
            }

            XElement toIcon;
            if( iconsItems.TryGetValue( toID, out toIcon ) )
            {
                XElement fromIcon;
                if( iconsItems.TryGetValue( fromID, out fromIcon ) )
                {
                    getCell( fromIcon ).SetValue( getCell( toIcon ).Value );

                    //Console.WriteLine( "Icon: " + fromID + " relocated to: " + getCell( toIcon ).Value );
                }
            }
        }
    }

    internal class SaveEditor : SaveReader, IDGenerator
    {
        private const UInt64 FIRST_NEW_ID = 0x8000000000000000;

        internal class MapPosition
        {
            public readonly UInt64 ID;
            public readonly CompassDirection Direction;
            public readonly float distanceSquared;

            public MapPosition( XElement i, MapData cc )
            {
                ID = (UInt64) i.Element( "Simple" ).Attribute( "value" );
                float x;
                float y;
                extractCoordinates( getFirstSimplePropertyNamed( i.Element( "Complex" ), "Position" ), out x, out y );

                if( x <= cc.CCX() )
                {
                    // North or West
                    Direction = y <= cc.CCY() ? CompassDirection.North : CompassDirection.West;
                }
                else
                {
                    // East or South
                    Direction = y <= cc.CCY() ? CompassDirection.East : CompassDirection.South;
                }

                distanceSquared = ( x - cc.CCX() ) * ( x - cc.CCX() ) + ( y - cc.CCY() ) * ( y - cc.CCY() );

                //Console.WriteLine( "id: " + id + "\tPosition: " + x + ", " + y + "\tis: " + dir + ",\tdistanceSquared: " + distanceSquared );
            }
        }

        private class DistanceComparer : IComparer<MapPosition>
        {
            public int Compare( MapPosition a, MapPosition b )
            {
                return b.distanceSquared.CompareTo( a.distanceSquared );    // b.CompareTo( a ) for reversed order
            }
        }
        private static readonly IComparer<MapPosition> mapPositionDistanceComparer = new DistanceComparer();

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
            var extrasIDs = from c in getExtraEntities().Elements( "Complex" )
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

            SortedDictionary<ScalableZombieTypes, decimal> scalableZombieTypeFactors = new SortedDictionary<ScalableZombieTypes, decimal> {
                { ScalableZombieTypes.ZombieWeakA, scale },
                { ScalableZombieTypes.ZombieWeakB, scale },
                { ScalableZombieTypes.ZombieWeakC, scale },
                { ScalableZombieTypes.ZombieWorkerA, scale },
                { ScalableZombieTypes.ZombieWorkerB, scale },
                { ScalableZombieTypes.ZombieMediumA, scale },
                { ScalableZombieTypes.ZombieMediumB, scale },
                { ScalableZombieTypes.ZombieDressedA, scale },
                { ScalableZombieTypes.ZombieStrongA, scale },
                { ScalableZombieTypes.ZombieVenom, scale },
                { ScalableZombieTypes.ZombieHarpy, scale }
            };
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

                if( scalableZombieTypeGroups.TryGetValue( k_v.Key, out SortedSet<ScalableZombieTypes> groupTypes ) )
                {
                    foreach( var t in groupTypes )
                    {
                        scalableZombieTypeFactors.Add( t, scale );
                    }
                }
            }

            scalePopulation( scalableZombieTypeFactors );
        }

        private void scalePopulation( SortedDictionary<ScalableZombieTypes, decimal> scalableZombieTypeFactors )
        {
            // Aggro'd zombies (also?) occur in LevelEntities, won't be removed just by removing from LevelFastSerializedEntities

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
                getFirstSimplePropertyNamed( zCopy, "A" ).SetAttributeValue( "value", newID() );
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
                return /*( from s in col.Element( "Properties" ).Elements( "Simple" )
                         where (string) s.Attribute( "name" ) == "Capacity"
                         select s ).Single()*/getFirstSimplePropertyNamed( col, "Capacity" ).Attribute( "value" );
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
                var cap = getCapacity( col );
                //Console.WriteLine( zombieType + ": " + cap.Value );

                if( scale < 1.0M )
                {
                    // chance is now chance to not remove existing zombies
                    // selectedZombies will be those removed

                    if( scale == 0.0M )
                    {
                        // No need to iterate and count
                        col.Element( "Items" ).RemoveNodes();

                        cap.SetValue( 0 );
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
                        col.Element( "Items" ).Add( i );
                    }

                    // Update capacity count
                    UInt64 newCap = Convert.ToUInt64( cap.Value ) + (UInt64) selectedZombies.Count;
                    cap.SetValue( newCap );
                    //Console.WriteLine( "newCap: " + newCap );

                    selectedZombies.Clear();
                }
            }
        }

        internal void scaleHugePopulation( bool giantsNotMutants, decimal scale )
        {
            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            var selectedZombies = entities.scaleEntities( giantsNotMutants ? LevelEntities.GIANT_ID_TEMPLATE : LevelEntities.MUTANT_ID_TEMPLATE, scale, this );

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
            var mutants = entities.getIDs( LevelEntities.MUTANT_ID_TEMPLATE );
            if( toGiantNotMutant && !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to replace." );
                return;
            }
            var giants = entities.getIDs( LevelEntities.GIANT_ID_TEMPLATE );
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
        
        internal void relocateMutants( bool toGiantNotMutant, bool perDirection )
        {
            var mutants = entities.getPositions( LevelEntities.MUTANT_ID_TEMPLATE, this );
            if( !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to relocate." );
                return;
            }
            var giants = entities.getPositions( LevelEntities.GIANT_ID_TEMPLATE, this );

            void relocateMutants( ICollection<MapPosition> movingMutants, MapPosition farthest )
            {
                foreach( MapPosition z in movingMutants )
                {
                    entities.relocate( z.ID, farthest.ID );

                    // Also see if both HugeZombies have had icons generated, for 1 to be repositioned to the other
                    icons.relocate( z.ID, farthest.ID );
                }
            }

            // Globally, or per direction, we'll have a list of huge zombie to find the farthest of, and a list of mutants to relocate, as well as their corresponding icons?

            var mutantsPerDirection = new SortedDictionary<CompassDirection,List<MapPosition>>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                mutantsPerDirection.Add( d, new List<MapPosition>( mutants.Count ) );
            }
            var giantsPerDirection = new SortedDictionary<CompassDirection,List<MapPosition>>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                giantsPerDirection.Add( d, new List<MapPosition>( giants.Count ) );
            }
            var farthestMutantShortlist = new List<MapPosition>();
            var farthestGiantShortlist = new List<MapPosition>();


            // Split huge zombies by direction
            foreach( var mutant in mutants )
            {
                mutantsPerDirection[mutant.Direction].Add( mutant );
            }
            foreach( var giant in giants )
            {
                giantsPerDirection[giant.Direction].Add( giant );
            }

            // Sort each direction, take the farthest, form a new shortlist and sort that for the overall farthest
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                var mutantsInDirection = mutantsPerDirection[d];
                if( mutantsInDirection.Count > 0 )
                {
                    mutantsInDirection.Sort( mapPositionDistanceComparer );
                    farthestMutantShortlist.Add( mutantsInDirection.First() );
                }
            }
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                var giantsInDirection = giantsPerDirection[d];
                if( giantsInDirection.Count > 0 )
                {
                    giantsInDirection.Sort( mapPositionDistanceComparer );
                    farthestGiantShortlist.Add( giantsInDirection.First() );
                }
            }

            farthestMutantShortlist.Sort( mapPositionDistanceComparer );
            farthestGiantShortlist.Sort( mapPositionDistanceComparer );
            MapPosition globalFarthestMutant = farthestMutantShortlist.FirstOrDefault();
            MapPosition globalFarthestGiant = farthestGiantShortlist.FirstOrDefault();
            

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

                        MapPosition relocateTo;
                        // Should and could we use a Giant?
                        if( toGiantNotMutant )
                        {
                            var giantsInDirection = giantsPerDirection[d];
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
                MapPosition farthest = toGiantNotMutant ? globalFarthestGiant : globalFarthestMutant;
                if( farthest == null )
                {
                    //Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
                    farthest = globalFarthestMutant;
                }

                var movingMutants = new List<MapPosition>( entities.getIDs( LevelEntities.MUTANT_ID_TEMPLATE ).Count );
                foreach( var m in mutantsPerDirection.Values )
                {
                    movingMutants.AddRange( m );
                }
                relocateMutants( movingMutants, farthest );
            }

        }
        
        internal void resizeVODs( VodSizes vodSize )
        {
            entities.resizeVODs( vodSize );
        }

        internal void stackVODbuildings( VodSizes size, decimal scale )
        {
            // Could use some add/remove entity delegates, to refactor this and zombie type scaling? Except zombie type collections can be RemoveNodes()'d per type, while level entities are all mixed in 1 collection.

            if( scale == 1.0M )
            {
                return;     // Nothing needs be done
            }

            UInt64 vodTemplate;
            switch( size )
            {
                default:
                case VodSizes.SMALL:
                    vodTemplate = LevelEntities.VOD_SMALL_ID_TEMPLATE;
                    break;
                case VodSizes.MEDIUM:
                    vodTemplate = LevelEntities.VOD_MEDIUM_ID_TEMPLATE;
                    break;
                case VodSizes.LARGE:
                    vodTemplate = LevelEntities.VOD_LARGE_ID_TEMPLATE;
                    break;
            }

            entities.scaleVODs( vodTemplate, scale, this );
        }

        internal void removeFog( uint radius = 0 )
        {
            int rawLength = 4 * cellsCount * cellsCount;        // 4 bytes just to store 00 00 00 FF or 00 00 00 00, yuck

            // Sadly we can't use String.Create<TState>(Int32, TState, SpanAction<Char,TState>) to avoid duplicate allocation prior to creating the final string

            byte[] clearFog = new byte[rawLength];  // Defaulting to 00 00 00 00, good if we want less than 50% fog

            if( radius > 0 )
            {
                circularFog( cellsCount, clearFog, radius );
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
                int wordIndex = ( ( y * s ) + x ) * 4;  // This indexing is backwards, because fog is reversed in some manner...
                f[wordIndex + 3] = 0xFF;
            }

            void setFogLine( int s, byte[] f, int xStart, int y, int xEnd )
            {
                for( int x = xStart; x < xEnd; x++ )
                {
                    setFog( s, f, x, y );
                }
            }

            int commandCenterX = (int) CCX();
            int commandCenterY = (int) CCY();
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

            int storesCapacity = (1 + entities.getIDs( (UInt64) GiftableTypes.WareHouse ).Count ) * RESOURCE_STORE_CAPACITY;   // "1+" assumes base CC storage, no mayor +25/+50 upgrades

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

        private XElement getSwarmByName( XElement collectionContainer, string name )
        {
            /*
             * The LevelEvents Collection appears to be duplicated (w.r.t. Generators) at different depths in a single save file,
             * a single method works from both given the correct starting element.
             */
            XElement eventsItems = getFirstPropertyOfTypeNamed( collectionContainer, "Collection", "LevelEvents" ).Element( "Items" );
            return ( from c in eventsItems.Elements( "Complex" )
                     where (string) getFirstSimplePropertyNamed( c, "Name" ).Attribute( "value" ) == name
                     select c ).FirstOrDefault();
        }

        internal void fasterSwarms()
        {
            void setTimes( XElement c, SwarmTimings times )
            {
                if( c != null )
                {
                    getFirstSimplePropertyNamed( c, "StartTimeH" ).SetAttributeValue( "value", times.startTime );
                    getFirstSimplePropertyNamed( c, "GameTimeH" ).SetAttributeValue( "value", times.gameTime );
                    getFirstSimplePropertyNamed( c, "RepeatTimeH" ).SetAttributeValue( "value", times.repeatTime );
                    getFirstSimplePropertyNamed( c, "StartNotifyTimeH" ).SetAttributeValue( "value", times.notifyTime );
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
                    getFirstSimplePropertyNamed( complex, "Generators" ).SetAttributeValue( "value", g );
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
            XElement allowMayors = getFirstSimplePropertyNamed( getDataExtension(), "AllowMayors" );
            allowMayors.SetAttributeValue( "value", "False" );
        }
    }
}
