using System;
using System.Collections.Generic;
using System.Linq;

namespace TABSAT
{
    class MapNavigation
    {
        [Flags]
        internal enum Direction : byte
        {
            NORTH = 0b_0000_0001,
            NORTHEAST = 0b_0000_0010,
            EAST = 0b_0000_0100,
            SOUTHEAST = 0b_0000_1000,
            SOUTH = 0b_0001_0000,
            SOUTHWEST = 0b_0010_0000,
            WEST = 0b_0100_0000,
            NORTHWEST = 0b_1000_0000
        }

        internal const byte ALL_DIRECTIONS = 0xFF;

        //private const Direction CARDINALS = Direction.NORTH | Direction.EAST | Direction.SOUTH | Direction.WEST;
        //private const Direction ORDINALS = Direction.NORTHEAST | Direction.SOUTHEAST | Direction.SOUTHWEST | Direction.NORTHWEST;
        private static readonly SortedSet<Direction> CARDINALS = new SortedSet<Direction> { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        private static readonly SortedSet<Direction> ORDINALS = new SortedSet<Direction> { Direction.NORTHEAST, Direction.SOUTHEAST, Direction.SOUTHWEST, Direction.NORTHWEST };
        internal const ushort UNNAVIGABLE = ushort.MaxValue;

        private static Direction Reverse( Direction d )
        {
            switch( d )
            {
                case Direction.NORTH:
                    return Direction.SOUTH;
                case Direction.NORTHEAST:
                    return Direction.SOUTHWEST;
                case Direction.EAST:
                    return Direction.WEST;
                case Direction.SOUTHEAST:
                    return Direction.NORTHWEST;
                case Direction.SOUTH:
                    return Direction.NORTH;
                case Direction.SOUTHWEST:
                    return Direction.NORTHEAST;
                case Direction.WEST:
                    return Direction.EAST;
                case Direction.NORTHWEST:
                    return Direction.SOUTHEAST;
                default:
                    throw new NotImplementedException( "Unimplemented Direction: " + d );
            }
        }

        internal class Position : IComparable
        {
            // Could be bytes, unsigned 8bit ints..?
            internal readonly ushort x;
            internal readonly ushort y;

            internal Position( ushort x, ushort y )
            {
                this.x = x;
                this.y = y;
            }

            internal Position TryMoveDirection( Direction d )      // Doesn't account for 45degree rotation..?
            {
                ushort newX = x;
                ushort newY = y;
                switch( d )
                {
                    case Direction.NORTH:
                        newX -= 1;
                        newY -= 1;
                        break;
                    case Direction.NORTHEAST:
                        newY -= 1;
                        break;
                    case Direction.EAST:
                        newX += 1;
                        newY -= 1;
                        break;
                    case Direction.SOUTHEAST:
                        newX += 1;
                        break;
                    case Direction.SOUTH:
                        newX += 1;
                        newY += 1;
                        break;
                    case Direction.SOUTHWEST:
                        newY += 1;
                        break;
                    case Direction.WEST:
                        newX -= 1;
                        newY += 1;
                        break;
                    case Direction.NORTHWEST:
                        newX -= 1;
                        break;
                    default:
                        throw new NotImplementedException( "Unimplemented Direction: " + d );
                }

                if( newX < 0 || newY < 0 )
                {
                    return null;
                }
                return new Position( newX, newY );
            }

            public int CompareTo( object obj )
            {
                if( obj == null ) return 1;

                if( !( obj is Position other ) )
                {
                    throw new ArgumentException( "Object is not a Position." );
                }
                else
                {
                    int c = x.CompareTo( other.x );
                    if( c == 0 )
                    {
                        c = y.CompareTo( other.y );
                    }
                    return c;
                }
            }
        }

        internal class FlowGraph
        {
            private readonly ushort resolution;   // Cells per axis
            private readonly byte[] navigable;
            private readonly SortedDictionary<Position, ushort> ccDistances;
            private readonly SortedDictionary<Position, Direction> ccDirections;

            internal FlowGraph( ushort r, byte[] n )
            {
                resolution = r;
                navigable = n;
                ccDistances = new SortedDictionary<Position, ushort>();
                ccDirections = new SortedDictionary<Position, Direction>();
            }

            internal void floodFromCC( in Position cc )
            {
                SortedDictionary<Position, Direction> aroundCC = new SortedDictionary<Position, Direction>
                {
                    // 5 cells per side, Corners +/-2 from center co-ords.
                    { new Position( (ushort) (cc.x - 3), cc.y ), Direction.SOUTHEAST },
                    { new Position( cc.x, (ushort) (cc.y - 3) ), Direction.SOUTHWEST },
                    { new Position( (ushort) (cc.x + 3), cc.y ), Direction.NORTHWEST },
                    { new Position( cc.x, (ushort) (cc.y + 3) ), Direction.NORTHEAST }
                };

                flood( aroundCC, 1, UNNAVIGABLE );

                favourFlow( ORDINALS );
            }

            private void flood( SortedDictionary<Position, Direction> toVisit, ushort distance, ushort limit )
            {
                foreach( var k_v in toVisit )
                {
                    recordFlood( k_v.Key, distance, k_v.Value );
                }

                // Are we done flood filling the map?
                if( distance >= limit )
                {
                    return;
                }

                var nextToVisit = new SortedDictionary<Position, Direction>();

                foreach( var k_v in toVisit )
                {
                    var position = k_v.Key;
                    var reversedFromDir = k_v.Value;    // Used for not flooding back the way we came. No 2 extra Reverse()s needed

                    foreach( Direction floodDir in CARDINALS )
                    {
                        if( floodDir != reversedFromDir )
                        {
                            considerFlood( position, floodDir, true, distance, nextToVisit );
                        }
                    }
                    foreach( Direction floodDir in ORDINALS )
                    {
                        if( floodDir != reversedFromDir )
                        {
                            considerFlood( position, floodDir, false, distance, nextToVisit );
                        }
                    }
                }

                if( nextToVisit.Any() )
                {
                    flood( nextToVisit, (ushort) (distance + 1), limit );
                }
            }

            private void recordFlood( Position position, ushort distance, Direction direction )
            {
                if( ccDistances.TryGetValue( position, out ushort existing ) )
                {
                    if( existing <= distance )
                    {
                        return;     // New path option isn't better, ignore it
                    }
                }
                ccDistances.Add( position, distance );
                ccDirections.Add( position, direction );
            }

            private void considerFlood( Position position, Direction floodDir, bool checkAdjacent, ushort range, SortedDictionary<Position, Direction> nextToVisit )
            {
                var candidatePos = TryMove( position, floodDir );
                if( candidatePos == null )
                {
                    return;
                }

                // Check movement isn't block in both adjacent directions
                if( checkAdjacent )
                {
                    if( !IsNavigable( position.x, candidatePos.y ) && !IsNavigable( candidatePos.x, position.y ) )
                    {
                        return;
                    }
                }

                // Test we don't already have a new candidate route to this position before we look up any known distance
                if( nextToVisit.ContainsKey( candidatePos ) )
                {
                    return;
                }

                // Might the current position be a shorter path to an adjacent navigable?
                if( ccDistances.TryGetValue( candidatePos, out ushort distance ) )
                {
                    if( distance <= range )
                    {
                        return;
                    }
                }

                nextToVisit.Add( candidatePos, Reverse( floodDir ) );
                //nextToVisit[candidatePos] = Reverse( floodDir );
            }

            private Position TryMove( Position fromPosition, Direction direction )
            {
                var candidatePos = fromPosition.TryMoveDirection( direction );
                if( candidatePos == null )  // Unimplemented direction or negative coordinate
                {
                    return null;
                }

                // Does moving this direction from this position fit on the map?
                if( !IsWithinMap( candidatePos ) )
                {
                    return null;
                }

                // Is this a valid position for navigating?
                if( !IsNavigable( candidatePos.x, candidatePos.y ) )
                {
                    return null;
                }

                return candidatePos;
            }

            private bool IsWithinMap( Position p )
            {
                return /*p.x >= 0 && p.y >= 0 &&*/ p.x < resolution && p.y < resolution;
            }

            private bool IsNavigable( ushort x, ushort y )
            {
                int i = axesToIndex( resolution, x, y );
                return navigable[i] == 0x00;  // != SaveReader.NAVIGABLE_BLOCKED
            }

            private void favourFlow( SortedSet<Direction> favoured )
            {
                var changesToApply = new SortedDictionary<Position, Direction>();

                foreach( var k_v in ccDirections )
                {
                    var position = k_v.Key;
                    var direction = k_v.Value;
                    if( favoured.Contains( direction ) )
                    {
                        continue;           // Already favoured, test another position
                    }

                    foreach( Direction candidateDirection in Enum.GetValues( typeof( Direction ) ) )
                    {
                        if( favoured.Contains( candidateDirection ) )   // An actual candidate favoured direction
                        {
                            var resultingPosition = TryMove( position, candidateDirection );
                            if( resultingPosition == null )
                            {
                                continue;   // direction isn't navigable
                            }
                            uint candidateDistance = getDistance( resultingPosition );
                            if( candidateDistance < getDistance( position ) )           // Is towards the CC
                            {
                                //ccDirections[position] = candidateDirection;
                                changesToApply.Add( position, candidateDirection );
                                break;      // Test another position
                            }
                        }
                    }
                }

                foreach( var k_v in changesToApply )
                {
                    ccDirections[k_v.Key] = k_v.Value;
                }
            }

            internal ushort getDistance( Position position )
            {
                if( !ccDistances.TryGetValue( position, out ushort distance ) )
                {
                    distance = UNNAVIGABLE;
                }
                return distance;
            }

            internal Direction? getDirection( Position position )
            {
                if( !ccDirections.TryGetValue( position, out Direction direction ) )
                {
                    return null;
                }
                return direction;
            }
        }

        internal class IntQuadTree
        {
            private const ushort RES_LIMIT = 2;

            private readonly ushort CornerX;        // North (W/S) corner of covered coords
            private readonly ushort CornerY;        // North (W/S) corner of covered coords
            private readonly ushort Resolution;     // Length of sides of covered square. Power of 2.

            private ushort count;
            private IntQuadTree northQuad;
            private IntQuadTree eastQuad;
            private IntQuadTree southQuad;
            private IntQuadTree westQuad;

            internal IntQuadTree( ushort x, ushort y, ushort res )
            {
                if( res == 0 || ( res & (res - 1)) != 0 )
                {
                    throw new ArgumentOutOfRangeException( "res must be a positive power of 2." );
                }
                CornerX = x;
                CornerY = y;
                Resolution = res;

                count = 0;
                northQuad = null;
                eastQuad = null;
                southQuad = null;
                westQuad = null;
            }

            internal ushort getCount( ushort x, ushort y, ushort res )
            {
                if( !IsWithinQuad( x, y ) )
                {
                    throw new ArgumentOutOfRangeException( "x or y not within quad." );
                }

                // Are we deep enough?
                if( Resolution == res )
                {
                    return count;
                }
                else
                {
                    // Can we skip going deeper?
                    if( count == 0 )
                    {
                        return 0;
                    }
                    // Find the corresponding quad and recurse
                    IntQuadTree tree = SubTree( getDir( x, y ) );
                    if( tree == null )
                    {
                        return 0;
                    }
                    else
                    {
                        return tree.getCount( x, y, res );
                    }
                }
            }

            private bool IsWithinQuad( ushort x, ushort y )
            {
                if( x < CornerX || x >= CornerX + Resolution )
                {
                    return false;
                }
                if( y < CornerY || y >= CornerY + Resolution )
                {
                    return false;
                }
                return true;
            }

            private SaveReader.CompassDirection getDir( ushort x, ushort y )
            {
                if( x - CornerX < Resolution / 2 )
                {
                    // N or W
                    return ( y - CornerY < Resolution / 2 ) ? SaveReader.CompassDirection.North : SaveReader.CompassDirection.West;
                }
                else
                {
                    // E or S
                    return ( y - CornerY < Resolution / 2 ) ? SaveReader.CompassDirection.East : SaveReader.CompassDirection.South;
                }
            }

            private ref IntQuadTree SubTree( SaveReader.CompassDirection dir )
            {
                ref IntQuadTree tree = ref southQuad;
                switch( dir )
                {
                    case SaveReader.CompassDirection.North:
                        tree = ref northQuad;
                        break;
                    case SaveReader.CompassDirection.East:
                        tree = ref eastQuad;
                        break;
                    case SaveReader.CompassDirection.South:
                        tree = ref southQuad;
                        break;
                    case SaveReader.CompassDirection.West:
                        tree = ref westQuad;
                        break;
                }
                return ref tree;
            }

            internal void Add( ushort x, ushort y )
            {
                if( !IsWithinQuad( x, y ) )
                {
                    throw new ArgumentOutOfRangeException( "x or y not within quad." );
                }

                count += 1;

                // Should we expand the tree?
                if( Resolution > RES_LIMIT )
                {
                    SaveReader.CompassDirection dir = getDir( x, y );
                    ref IntQuadTree tree = ref SubTree( dir );

                    if( tree == null )
                    {
                        ushort newRes = (ushort) (Resolution / 2);
                        ushort newX = dir == SaveReader.CompassDirection.North || dir == SaveReader.CompassDirection.West ? CornerX : (ushort) (CornerX + newRes);
                        ushort newY = dir == SaveReader.CompassDirection.North || dir == SaveReader.CompassDirection.East ? CornerY : (ushort) (CornerY + newRes);
                        tree = new IntQuadTree( newX, newY, newRes );
                    }

                    tree.Add( x, y );
                }
            }
        }

        internal static bool containsDirection( in byte sections, in Direction direction )
        {
            return ( sections & (byte) direction ) != 0;
        }
        /*
        private static void indexToAxes( int index, ushort res, out ushort x, out ushort y )
        {
            x = index / res;
            y = index % res;
        }
        */
        internal static int axesToIndex( ushort res, ushort x, ushort y )
        {
            return ( x * res ) + y;
        }
    }
}
