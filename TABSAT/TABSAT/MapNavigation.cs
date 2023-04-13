﻿using System;
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

        //private const Direction CARDINALS = Direction.NORTH | Direction.EAST | Direction.SOUTH | Direction.WEST;
        //private const Direction ORDINALS = Direction.NORTHEAST | Direction.SOUTHEAST | Direction.SOUTHWEST | Direction.NORTHWEST;
        private static readonly SortedSet<Direction> CARDINALS = new SortedSet<Direction> { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        private static readonly SortedSet<Direction> ORDINALS = new SortedSet<Direction> { Direction.NORTHEAST, Direction.SOUTHEAST, Direction.SOUTHWEST, Direction.NORTHWEST };
        internal const int UNNAVIGABLE = int.MaxValue;

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
                    throw new ArgumentException( "x or y should not be negative." );
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
        /*
        private static void indexToAxes( int index, int res, out int x, out int y )
        {
            x = index / res;
            y = index % res;
        }
        */
        internal static int axesToIndex( int res, int x, int y )
        {
            return ( x * res ) + y;
        }

        internal class FlowGraph
        {
            private readonly int resolution;    // Cells per axis
            private readonly byte[] navigable;
            private readonly SortedDictionary<Position, int> ccDistances;
            private readonly SortedDictionary<Position, Direction> ccDirections;

            internal FlowGraph( int r, byte[] n )
            {
                resolution = r;
                navigable = n;
                ccDistances = new SortedDictionary<Position, int>();
                ccDirections = new SortedDictionary<Position, Direction>();
            }

            internal void floodFromCC( int CCX, int CCY)
            {
                SortedDictionary<Position, Direction> aroundCC = new SortedDictionary<Position, Direction>
                {
                    // 5 cells per side, Corners +/-2 from center co-ords.
                    { new Position( CCX - 3, CCY ), Direction.SOUTHEAST },
                    { new Position( CCX, CCY - 3 ), Direction.SOUTHWEST },
                    { new Position( CCX + 3, CCY ), Direction.NORTHWEST },
                    { new Position( CCX, CCY + 3 ), Direction.NORTHEAST }
                };

                flood( aroundCC, 1, UNNAVIGABLE );

                favourFlow( ORDINALS );
            }

            private void flood( SortedDictionary<Position, Direction> toVisit, int distance, int limit )
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
                    flood( nextToVisit, distance + 1, limit );
                }
            }

            private void recordFlood( Position position, int distance, Direction direction )
            {
                if( ccDistances.TryGetValue( position, out int existing ) )
                {
                    if( existing <= distance )
                    {
                        return;     // New path option isn't better, ignore it
                    }
                }
                ccDistances.Add( position, distance );
                ccDirections.Add( position, direction );
            }

            private void considerFlood( Position position, Direction floodDir, bool checkAdjacent, int range, SortedDictionary<Position, Direction> nextToVisit )
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
                if( ccDistances.TryGetValue( candidatePos, out int distance ) )
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

            private bool IsNavigable( int x, int y )
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
                if( !ccDistances.TryGetValue( position, out int distance ) )
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
            private const int RES_LIMIT = 2;

            private readonly int CornerX;       // North (W/S) corner of covered coords
            private readonly int CornerY;       // North (W/S) corner of covered coords
            private readonly int Resolution;    // Length of sides of covered square. Power of 2.

            private int count;
            private IntQuadTree northQuad;
            private IntQuadTree eastQuad;
            private IntQuadTree southQuad;
            private IntQuadTree westQuad;

            internal IntQuadTree( int x, int y, int res )
            {
                if( x < 0 || y < 0 )
                {
                    throw new ArgumentOutOfRangeException( "x and y coordinates must not be negative." );
                }
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

            internal int getCount( int x, int y, int res )
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
                    return tree == null ? 0 : tree.getCount( x, y, res );
                }
            }

            private bool IsWithinQuad( int x, int y )
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

            private SaveReader.CompassDirection getDir( int x, int y )
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

            internal void Add( int x, int y )
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
                        int newRes = Resolution / 2;
                        int newX = dir == SaveReader.CompassDirection.North || dir == SaveReader.CompassDirection.West ? CornerX : CornerX + newRes;
                        int newY = dir == SaveReader.CompassDirection.North || dir == SaveReader.CompassDirection.East ? CornerY : CornerY + newRes;
                        tree = new IntQuadTree( newX, newY, newRes );
                    }

                    tree.Add( x, y );
                }
            }
        }
    }
}