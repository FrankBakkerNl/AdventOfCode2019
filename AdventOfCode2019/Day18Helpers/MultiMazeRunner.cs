using System;
using System.Collections.Generic;

namespace AdventOfCode2019.Day18Helpers
{
    public readonly struct MultiMazeRunner
    {
        private const int BitsPerAgent = 6;
        private const uint BitMask = 0b111111;

        private readonly L2Maze _maze;
        public readonly uint PositionsPattern;
        public readonly uint Keys;

        public int MoveCount { get; }

        public bool HasAllKeys => Keys == _maze.AllKeys;

        public MultiMazeRunner(L2Maze maze, uint positionsPattern, uint keys, int moveCount)
        {
            _maze = maze;
            PositionsPattern = positionsPattern;
            Keys = keys;
            MoveCount = moveCount;
        }

        public MultiMazeRunner(L2Maze maze, int[] positions, uint keys)
        {
            _maze = maze;
            PositionsPattern =
                (uint)positions[0] |
                (uint)positions[1] << BitsPerAgent |
                (uint)positions[2] << (BitsPerAgent * 2)|
                (uint)positions[3] << (BitsPerAgent * 3);
            Keys = keys;
            MoveCount = 0;
        }

        public IEnumerable<MultiMazeRunner> Children()
        {
            for (int agent = 0; agent < 4; agent++)
            {
                // Extract the position for this agent from the bit pattern
                var position = (PositionsPattern >> (BitsPerAgent * agent)) & BitMask;
                foreach (var neighbor in _maze.NeighborLookup[position])
                {
                    if (CanMoveTo(neighbor))
                    {
                        yield return Step(agent, neighbor);
                    }
                }
            }
        }

        private bool CanMoveTo(Neighbor neighbor) => (neighbor.Door & Keys) == neighbor.Door;

        private MultiMazeRunner Step(int agent, Neighbor neighbor)
        {
            var newPositionPattern = GetNewPositionPattern(PositionsPattern, agent, (uint)neighbor.Target);

            // merge keys with possibly new found key
            var newKeys = Keys | neighbor.Key;

            return new MultiMazeRunner(_maze, newPositionPattern, newKeys, MoveCount + neighbor.Length);
        }

        private static uint GetNewPositionPattern(uint currentPattern, int agent, uint newPosition)
        {
            var shift = BitsPerAgent * agent;
            var positionMask = BitMask << shift;
            var newPositionShifted = newPosition << shift;

            // clear the bits for this agent and overwrite with new position
            return (currentPattern & ~positionMask) | newPositionShifted;
        }

        public override bool Equals(object obj)
        {
            var other = (MultiMazeRunner) obj;
            return PositionsPattern == other.PositionsPattern && Keys == other.Keys;
        }

        public override int GetHashCode() => HashCode.Combine(PositionsPattern, Keys);
    }
}