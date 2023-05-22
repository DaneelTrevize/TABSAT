using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

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

        internal void relocate( XElement from, XElement to )
        {
            UInt64 fromID = (UInt64) from.Element( "Simple" ).Attribute( "value" );
            UInt64 toID = (UInt64) to.Element( "Simple" ).Attribute( "value" );
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

            // EntityDescriptors under UnitsGenerationPack under LevelEvents under LevelState also have ID and Position.
            // LevelEvents is also duplicated under Extension under Data under CurrentGeneratedLevel also under LevelState.

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

        internal void scalePopulation( in byte scale, in bool scaleIdle, in bool scaleActive, ItemInArea inArea )
        {
            if( scale == 100 )
            {
                return;     // Nothing needs be done
            }

            SortedDictionary<LevelEntities.ScalableZombieTypes, byte> scalableZombieTypeFactors = new SortedDictionary<LevelEntities.ScalableZombieTypes, byte> {
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
            scalePopulation( scalableZombieTypeFactors, scaleIdle, scaleActive, inArea );
        }

        internal void scalePopulation( SortedDictionary<LevelEntities.ScalableZombieGroups, byte> scalableZombieGroupFactors, in bool scaleIdle, in bool scaleActive, ItemInArea inArea )
        {
            SortedDictionary<LevelEntities.ScalableZombieTypes, byte> scalableZombieTypeFactors = new SortedDictionary<LevelEntities.ScalableZombieTypes, byte>();

            foreach( var k_v in scalableZombieGroupFactors )
            {
                byte scale = k_v.Value;

                if( LevelEntities.scalableZombieTypeGroups.TryGetValue( k_v.Key, out SortedSet<LevelEntities.ScalableZombieTypes> groupTypes ) )
                {
                    foreach( var t in groupTypes )
                    {
                        scalableZombieTypeFactors.Add( t, scale );
                    }
                }
            }

            scalePopulation( scalableZombieTypeFactors, scaleIdle, scaleActive, inArea );
        }

        private void scalePopulation( SortedDictionary<LevelEntities.ScalableZombieTypes, byte> scalableZombieTypeFactors, in bool scaleIdle, in bool scaleActive, ItemInArea inArea )
        {
            // First handle zombies from-and-idle-since the map was generated, found in LevelFastSerializedEntities
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
                    byte scale = scalableZombieTypeFactors[zombieType];
                    scaleInactiveZombies( collection, scale, inArea );
                }
            }

            // Next handle aggro'd and spawn-generated zombies, found in LevelEntities
            if( scaleActive )
            {
                foreach( var k_v in scalableZombieTypeFactors )
                {
                    var zombieType = k_v.Key;
                    var scale = k_v.Value;
                    entities.scaleEntities( (UInt64) zombieType, scale, this, inArea );
                }
            }
        }

        private void scaleInactiveZombies( XElement collection, in byte scale, ItemInArea inArea )
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

            if( scale == 100 )
            {
                // Nothing needs be done
                return;
            }

            void duplicateZombie( XElement com, LinkedList<XElement> copiedZombies )
            {
                XElement zCopy = new XElement( com );       // Duplicate at the same position
                getValueAttOfSimpleProp( zCopy, "A" ).SetValue( newID() );
                copiedZombies.AddLast( zCopy );
            }

            Random rand = new Random();

            uint multiples = scale / 100U;                  // How many duplicates to certainly make of each zombie
            double chance = ( scale % 100U ) / 100;         // The chance of making 1 more duplicate per zombie
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
            var cap = getValueAttOfSimpleProp( collection, "Capacity" );
            //Console.WriteLine( zombieType + ": " + cap.Value );

            // By having to iterate each zombie to test their position, we lose an optimisation opportunity when scale == 0M to just Remove() and replace the type Items node..?

            var selectedZombies = new LinkedList<XElement>();
            foreach( var com in collection.Element( "Items" ).Elements( "Complex" ) )
            {
                // First test that their position is in the affected area
                if( !inArea( com, false ) )
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
                        // Removing within foreach doen't work, without .ToList(). Might as well collect candidates so we also have an O(1) count for later too
                        selectedZombies.AddLast( com );
                    }
                }
                else
                {
                    // First the certain duplications
                    for( uint m = 1; m < multiples; m++ )
                    {
                        duplicateZombie( com, selectedZombies );
                    }
                    // And now the chance-based duplication
                    if( chance >= rand.NextDouble() )   // If the chance is not less than the roll
                    {
                        duplicateZombie( com, selectedZombies );
                    }
                }
            }
            //Console.WriteLine( "selectedZombies: " + selectedZombies.Count );

            if( scale < 100 )
            {
                foreach( var i in selectedZombies )
                {
                    i.Remove();
                }

                // Update capacity count
                UInt64 newCap = (UInt64) ( Convert.ToInt32( cap.Value ) - selectedZombies.Count );   // Assume actual UInt64 Capacity value is positive Int32 size and no less than Count removed
                cap.SetValue( newCap );
                //Console.WriteLine( "newCap: " + newCap );
            }
            else
            {
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

        internal void scaleEntities( UInt64 type, in byte scale, ItemInArea inArea, in bool haveIcons = false )
        {
            // Could refactor with scalePopulation( scaleIdle = false, scaleActive = true ) ..? Need to rename the method and those parameters though.

            var selectedZombies = entities.scaleEntities( type, scale, this, inArea );

            if( !haveIcons )
            {
                return;
            }
            if( scale < 100 )
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
        
        internal void replaceHugeZombies( bool toGiantNotMutant, InArea inArea )
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
                if( entities.swapZombieType( id, inArea ) )
                {
                    icons.swapZombieType( id );
                }
            }
        }
        
        internal void relocateMutants( bool toGiantNotMutant, bool perDirection, InArea inArea )
        {
            var mutants = entities.getEntitiesOfType( (UInt64) LevelEntities.HugeTypes.Mutant );
            if( !mutants.Any() )
            {
                //Console.WriteLine( "No Mutants to relocate." );
                return;
            }
            var giants = entities.getEntitiesOfType( (UInt64) LevelEntities.HugeTypes.Giant );
            
            // Per direction, we'll have a list of huge zombies, a map of the farthest, and a list of mutants in the filtered area to relocate (as well as their corresponding icons).

            var mutantsPerDirection = new SortedDictionary<CompassDirection,List<XElement>>();
            var mutantsInAreaPerDirection = new SortedDictionary<CompassDirection, List<XElement>>();
            var giantsPerDirection = new SortedDictionary<CompassDirection, List<XElement>>();

            var farthestMutantPerDirection = new SortedDictionary<CompassDirection, XElement>();
            var farthestMutantDistancePerDirection = new SortedDictionary<CompassDirection, uint>();
            var farthestGiantPerDirection = new SortedDictionary<CompassDirection, XElement>();
            var farthestGiantDistancePerDirection = new SortedDictionary<CompassDirection, uint>();
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                mutantsPerDirection.Add( d, new List<XElement>( mutants.Count() ) );
                mutantsInAreaPerDirection.Add( d, new List<XElement>( mutants.Count() ) );
                giantsPerDirection.Add( d, new List<XElement>( giants.Count() ) );
            }

            // Split huge zombies by direction
            foreach( var mutant in mutants )
            {
                extractCoordinates( mutant, out ushort x, out ushort y );
                var position = new MapNavigation.Position( x, y );
                var direction = compassDirectionToCC( position );
                mutantsPerDirection[direction].Add( mutant );

                if( inArea( x, y ) )
                {
                    mutantsInAreaPerDirection[direction].Add( mutant );
                }

                var distance = squaredDistanceToCC( position );
                if( !farthestMutantDistancePerDirection.TryGetValue( direction, out uint farthestDistance ) || distance > farthestDistance )
                {
                    farthestMutantDistancePerDirection[direction] = distance;
                    farthestMutantPerDirection[direction] = mutant;
                }
            }
            foreach( var giant in giants )
            {
                extractCoordinates( giant, out ushort x, out ushort y );
                var position = new MapNavigation.Position( x, y );
                var direction = compassDirectionToCC( position );
                giantsPerDirection[direction].Add( giant );

                var distance = squaredDistanceToCC( position );
                if( !farthestGiantDistancePerDirection.TryGetValue( direction, out uint farthestDistance ) || distance > farthestDistance )
                {
                    farthestGiantDistancePerDirection[direction] = distance;
                    farthestGiantPerDirection[direction] = giant;
                }
            }

            // Find global farthest huge zombie of each type
            XElement farthestMutant = null;
            uint farthestMutantDistance = uint.MaxValue;
            XElement farthestGiant = null;
            uint farthestGiantDistance = uint.MaxValue;
            foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
            {
                if( farthestMutantDistancePerDirection.TryGetValue( d, out uint mutantDistance ) && ( farthestMutant == null || mutantDistance < farthestMutantDistance ) )
                {
                    farthestMutant = farthestMutantPerDirection[d];
                    farthestMutantDistance = mutantDistance;
                }
                if( farthestGiantDistancePerDirection.TryGetValue( d, out uint giantDistance ) && ( farthestGiant == null || giantDistance < farthestGiantDistance ) )
                {
                    farthestGiant = farthestGiantPerDirection[d];
                    farthestGiantDistance = giantDistance;
                }
            }

            void relocateMutants( ICollection<XElement> movingMutants, in XElement farthest )
            {
                foreach( var m in movingMutants )
                {
                    if( entities.relocateHuge( m, farthest ) )
                    {
                        // Also see if both HugeZombies have had icons generated, for 1 to be repositioned to the other
                        icons.relocate( m, farthest );
                    }
                }
            }

            if( perDirection )
            {
                // Get sorted mutant group per direction, also giants if toGiantNotMutant
                // Get farthest group mutant, or giant if toGiantNotMutant, or global farthest (could be either) if no local giant
                // relocate all group mutants to chosen farthest

                foreach( CompassDirection d in Enum.GetValues( typeof( CompassDirection ) ) )
                {
                    var mutantsInAreaAndDirection = mutantsInAreaPerDirection[d];
                    if( mutantsInAreaAndDirection.Count > 0 )
                    {
                        // There are mutants in this direction and area filter to relocate

                        XElement relocateTo;
                        // Should and could we use a Giant?
                        if( toGiantNotMutant )
                        {
                            var giantsInDirection = giantsPerDirection[d];
                            if( giantsInDirection.Count > 0 )
                            {
                                relocateTo = farthestGiantPerDirection[d];
                            }
                            else
                            {
                                //Console.WriteLine( "No Giants to relocate Mutants onto in direction: " + d + ", using global farthest Mutant, or Giant if one exists." );
                                // Could consider farthestMutantPerDirection[d]..?

                                relocateTo = farthestMutant;
                                if( farthestGiant != null )
                                {
                                    //Console.WriteLine( "Using farthest Giant." );
                                    relocateTo = farthestGiant;
                                }
                            }
                        }
                        else
                        {
                            relocateTo = farthestMutantPerDirection[d];     // There must be at least one in this direction as we are relocating some (that also passed the InArea test)
                        }

                        relocateMutants( mutantsInAreaAndDirection, relocateTo );
                    }
                }
            }
            else
            {
                // Should and could we use a Giant?
                XElement farthest = toGiantNotMutant ? farthestGiant : farthestMutant;
                if( farthest == null )
                {
                    //Console.WriteLine( "No Giants to relocate Mutants onto, using farthest Mutant." );
                    farthest = farthestMutant;
                }

                var movingMutants = new List<XElement>();   // Start capacity at entities.getIDs( (UInt64) LevelEntities.HugeTypes.Mutant ).Count ?
                foreach( var m in mutantsInAreaPerDirection.Values )
                {
                    movingMutants.AddRange( m );
                }
                relocateMutants( movingMutants, farthest );
            }

        }
        /*
        internal void resizeVODs( LevelEntities.VODTypes vodType )
        {
            entities.resizeVODs( vodType );
        }
        */

        internal void removeFog( InArea inArea )    // Naive implementation, considering each coordinate individually
        {
            int rawLength = 4 * cellsCount * cellsCount;        // 4 bytes just to store 00 00 00 FF or 00 00 00 00, yuck

            // Sadly we can't use String.Create<TState>(Int32, TState, SpanAction<Char,TState>) to avoid duplicate allocation prior to creating the final string

            byte[] noFog = new byte[rawLength];  // Defaulting to 00 00 00 00, good if we want less than 50% fog

            // Should we not modify the existing LayerFog data instead of potentially adding more fog than before..?

            for( ushort x = 0; x < cellsCount; x++ )
            {
                for( ushort y = 0; y < cellsCount; y++ )
                {
                    if( !inArea( x, y ) )
                    {
                        setFog( cellsCount, noFog, x, y );
                    }
                }
            }

            string layerFog = Convert.ToBase64String( noFog );
            //Console.WriteLine( layerFog );

            getValueAttOfSimpleProp( levelComplex, "LayerFog" ).SetValue( layerFog );
        }

        private static void setFog( in ushort s, byte[] f, in ushort x, in ushort y )
        {
            var wordIndex = ( ( y * s ) + x ) * 4;      // This indexing is backwards, because fog is oddly reversed, as well as in the opposite byte of the word
            f[wordIndex + 3] = 0xFF;
        }

        internal void removeFog( in byte radius = 0, in bool removeWithinNotBeyond = false )
        {
            int rawLength = 4 * cellsCount * cellsCount;        // 4 bytes just to store 00 00 00 FF or 00 00 00 00, yuck

            // Sadly we can't use String.Create<TState>(Int32, TState, SpanAction<Char,TState>) to avoid duplicate allocation prior to creating the final string

            byte[] noFog = new byte[rawLength];  // Defaulting to 00 00 00 00, good if we want less than 50% fog

            // Should we not modify the existing LayerFog data instead of potentially adding more fog than before..?

            if( removeWithinNotBeyond )
            {
                if( radius > 0 )
                {
                    setFogArea( ccPosition, cellsCount, noFog, radius, true );
                }
                // Else no fog, already done thanks to default byte array values.
            }
            else
            {
                if( radius > 0 )
                {
                    setFogArea( ccPosition, cellsCount, noFog, radius, false );
                }/*
                else
                {
                    // Add fog everywhere. Never used?
                    for( ushort x = 0; x < cellsCount; x++ )
                    {
                        for( ushort y = 0; y < cellsCount; y++ )
                        {
                            //setFog( cellsCount, noFog, x, y );
                        }
                    }
                }*/
            }

            string layerFog = Convert.ToBase64String( noFog );
            //Console.WriteLine( layerFog );
            
            getValueAttOfSimpleProp( levelComplex, "LayerFog" ).SetValue( layerFog );
        }

        private static void setFogArea( in MapNavigation.Position ccPosition, in ushort size, byte[] noFog, in byte radius, in bool fogBeyondNotWithin )
        {

            void setFogLine( in ushort s, byte[] f, in ushort xStart, in ushort y, in ushort xEnd )
            {
                for( ushort x = xStart; x < xEnd; x++ )
                {
                    setFog( s, f, x, y );
                }
            }

            ushort beforeCCx = (ushort) ( ccPosition.x - radius );
            ushort beforeCCy = (ushort) ( ccPosition.y - radius );
            ushort afterCCx = (ushort) ( ccPosition.x + radius );
            ushort afterCCy = (ushort) ( ccPosition.y + radius );

            if( fogBeyondNotWithin )
            {
                // Fill outside the circle with 00 00 00 FF, aka step 4 bytes at a time. Centered on CC

                // Quick fill in "thirds" of before, adjacent, after (CC +- radius)
                for( ushort x = 0; x < size; x++ )
                {
                    // 1/3 before, NorthEast
                    for( ushort y = 0; y < beforeCCy; y++ )
                    {
                        setFog( size, noFog, x, y );
                    }
                    // 1/3 after, SouthWest
                    for( ushort y = afterCCy; y < size; y++ )
                    {
                        setFog( size, noFog, x, y );
                    }
                }

                for( ushort y = beforeCCy; y < afterCCy; y++ )
                {
                    // 1/9 adjacent before, NorthWest
                    for( ushort x = 0; x < beforeCCx; x++ )
                    {
                        setFog( size, noFog, x, y );
                    }
                    // 1/9 adjacent after, SouthEast
                    for( ushort x = afterCCx; x < size; x++ )
                    {
                        setFog( size, noFog, x, y );
                    }
                }
            }
            // Central 1/9 left to do

            // In calculating the xFromCC to the circle edge in a single anticlockwise 45degree octant between y=0 and y=x, we would have all the values to duplicate the octant & its mirror in all 4 corners.

            // We'll just skip messing with Bresenham and use square roots, for a 90degree arc quadrant, and mirror 4 times.
            uint radiusSquared = (uint) radius * radius;
            ushort radiusBeforeCommandCenterX = (ushort) (ccPosition.x - radius);
            ushort radiusAfterCommandCenterX = (ushort) (ccPosition.x + radius);

            ushort xFromCC;// = radius;
            ushort yFromCC = 0;
            while( yFromCC <= radius )
            {
                xFromCC = Convert.ToUInt16( Math.Sqrt( radiusSquared - (yFromCC * yFromCC) ) );

                if( fogBeyondNotWithin )
                {
                    // SE to S to SW
                    setFogLine( size, noFog, (ushort) (ccPosition.x + xFromCC), (ushort) (ccPosition.y + yFromCC), radiusAfterCommandCenterX );

                    // SW to W to NW
                    setFogLine( size, noFog, radiusBeforeCommandCenterX, (ushort) (ccPosition.y + yFromCC ), (ushort) (ccPosition.x - xFromCC) );

                    // NW to N to NE
                    setFogLine( size, noFog, radiusBeforeCommandCenterX, (ushort) (ccPosition.y - yFromCC), (ushort) ( ccPosition.x - xFromCC) );

                    // NE to E to SE
                    setFogLine( size, noFog, (ushort) (ccPosition.x + xFromCC), (ushort) (ccPosition.y - yFromCC), radiusAfterCommandCenterX );
                }
                else
                {
                    // Before and after CC
                    setFogLine( size, noFog, (ushort) ( ccPosition.x - xFromCC), (ushort) ( ccPosition.y - yFromCC), (ushort) ( ccPosition.x + xFromCC) );
                    setFogLine( size, noFog, (ushort) ( ccPosition.x - xFromCC), (ushort) ( ccPosition.y + yFromCC), (ushort) ( ccPosition.x + xFromCC) );
                }

                yFromCC += 1;
            }
        }

        internal void showFullMap()
        {
            //        <Simple name="ShowFullMap" value="False" />
            getValueAttOfSimpleProp( levelComplex, "ShowFullMap" ).SetValue( "True" );
        }

        internal void addExtraSupplies( ushort food, ushort energy, ushort workers )
        {
            bool addValue( XAttribute extra, ushort add )
            {
                if( !UInt16.TryParse( extra.Value, out ushort value ) )
                {
                    return false;
                }

                value += add;
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
