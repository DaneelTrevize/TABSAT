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

        private const Direction CARDINALS = Direction.NORTH | Direction.EAST | Direction.SOUTH | Direction.WEST;
        private const Direction ORDINALS = Direction.NORTHEAST | Direction.SOUTHEAST | Direction.SOUTHWEST | Direction.NORTHWEST;
        //private Direction ALL = Direction.NORTH | Direction.NORTHEAST | Direction.EAST | Direction.SOUTHEAST | Direction.SOUTH | Direction.SOUTHWEST | Direction.WEST | Direction.NORTHWEST;

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
                    throw new ArgumentException( "Unimplemented Direction: " + d );
            }
        }

        internal class Position : IComparable
        {
            internal readonly int x;
            internal readonly int y;

            internal Position( int x, int y )
            {
                if( x < 0 || y < 0 )
                {
                    throw new ArgumentException( "x and y should be positive." );
                }
                this.x = x;
                this.y = y;
            }

            internal Position TryMoveDirection( Direction d )      // Doesn't account for 45degree rotation..?
            {
                int newX = x;
                int newY = y;
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
                        throw new ArgumentException( "Unimplemented Direction: " + d );
                }

                try
                {
                    return new Position( newX, newY );
                }
                catch
                {
                    return null;
                }
            }

            public int CompareTo( object obj )
            {
                if( obj == null ) return 1;

                Position other = obj as Position;
                if( other == null )
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

        private static void indexToAxes( int index, int res, out int x, out int y )
        {
            x = index / res;
            y = index % res;
        }

        internal static int axesToIndex( int res, int x, int y )
        {
            return ( x * res ) + y;
        }

        internal class FlowGraph
        {
            private readonly int res;
            private readonly byte[] navigable;
            private readonly SortedDictionary<Position, int> ccDistances;
            private readonly SortedDictionary<Position, Direction> ccDirections;

            internal FlowGraph( int r, byte[] n )
            {
                res = r;
                navigable = n;
                ccDistances = new SortedDictionary<Position, int>();
                ccDirections = new SortedDictionary<Position, Direction>();
            }

            internal void floodFromCC( int CCX, int CCY)
            {
                SortedDictionary<Position, Direction> aroundCC = new SortedDictionary<Position, Direction>();

                // 5 cells per side, Corners +/-2 from center co-ords.
                aroundCC.Add( new Position( CCX - 3, CCY ), Direction.SOUTHEAST );
                aroundCC.Add( new Position( CCX, CCY - 3 ), Direction.SOUTHWEST );
                aroundCC.Add( new Position( CCX + 3, CCY ), Direction.NORTHWEST );
                aroundCC.Add( new Position( CCX, CCY + 3 ), Direction.NORTHEAST );

                flood( aroundCC, 1, int.MaxValue );

                favourFlow( ORDINALS );
            }

            private void flood( SortedDictionary<Position, Direction> toVisit, int range, int limit )
            {
                var nextToVisit = new SortedDictionary<Position, Direction>();

                foreach( var k_v in toVisit )
                {
                    var position = k_v.Key;
                    var fromDir = k_v.Value;
                    recordPosition( position, range, fromDir );

                    foreach( Direction forwardDir in Enum.GetValues( typeof( Direction ) ) )
                    {
                        if( ( CARDINALS & forwardDir ) == forwardDir )
                        {
                            considerFlood( position, forwardDir, true, range, nextToVisit );
                        }
                    }
                    foreach( Direction forwardDir in Enum.GetValues( typeof( Direction ) ) )
                    {
                        if( ( ORDINALS & forwardDir ) == forwardDir )
                        {
                            considerFlood( position, forwardDir, false, range, nextToVisit );
                        }
                    }
                }

                // Are we done flood filling the map?
                if( range + 1 <= limit && nextToVisit.Any() )
                {
                    flood( nextToVisit, range + 1, limit );
                }
            }

            private void recordPosition( Position position, int distance, Direction direction )
            {
                int existing;
                if( ccDistances.TryGetValue( position, out existing ) )
                {
                    if( existing <= distance )
                    {
                        return;     // New path option isn't better, ignore it
                    }
                }
                ccDistances.Add( position, distance );
                ccDirections.Add( position, direction );
            }

            private void considerFlood( Position position, Direction forwardDir, bool checkAdjacent, int range, SortedDictionary<Position, Direction>  nextToVisit )
            {
                var candidatePos = TryMove( position, forwardDir );
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

                // Might the current position be a shorter path to an adjacent navigable?
                if( ccDistances.TryGetValue( candidatePos, out int distance ) )
                {
                    if( distance < range + 1 )
                    {
                        return;
                    }
                }
                
                if( !nextToVisit.ContainsKey( candidatePos ) )
                {
                    nextToVisit.Add( candidatePos, Reverse( forwardDir ) );
                }
                //nextToVisit[candidatePos] = Reverse( forwardDir );
            }

            private Position TryMove( Position fromPosition, Direction direction )
            {
                var candidatePos = fromPosition.TryMoveDirection( direction );
                if( candidatePos == null )  // Unimplemented direction
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
                return p.x >= 0 && p.y >= 0 && p.x < res && p.y < res;
            }

            private bool IsNavigable( int x, int y )
            {
                int i = axesToIndex( res, x, y );
                return navigable[i] == 0x00;  // != SaveReader.NAVIGABLE_BLOCKED
            }

            private void favourFlow( Direction favoured )
            {
                var changesToApply = new SortedDictionary<Position, Direction>();

                foreach( var k_v in ccDirections )
                {
                    var position = k_v.Key;
                    var direction = k_v.Value;
                    if( ( favoured & direction ) == direction )
                    {
                        continue;           // Already favoured, test another position
                    }

                    foreach( Direction candidateDirection in Enum.GetValues( typeof( Direction ) ) )
                    {
                        if( ( favoured & candidateDirection ) == candidateDirection )   // An actual candidate favoured direction
                        {
                            var resultingPosition = TryMove( position, candidateDirection );
                            if( resultingPosition == null )
                            {
                                continue;   // direction isn't navigable
                            }
                            int candidateDistance = getDistance( resultingPosition );
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

            internal int getDistance( Position position )
            {
                int distance;
                if( !ccDistances.TryGetValue( position, out distance ) )
                {
                    distance = int.MaxValue;
                }
                return distance;
            }

            internal Direction? getDirection( Position position )
            {
                Direction direction;
                if( !ccDirections.TryGetValue( position, out direction ) )
                {
                    return null;
                }
                return direction;
            }
        }
    }
}
