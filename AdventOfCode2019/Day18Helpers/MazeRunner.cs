using System;
using System.Collections.Generic;

namespace AdventOfCode2019.Day18Helpers
{
    public readonly struct MazeRunner
    {
        private readonly L2Maze _maze;
        public int Position { get; }
        private readonly uint _keys;
        public int MoveCount { get; }

        public MazeRunner(L2Maze maze, int position, uint keys, int moveCount = 0)
        {
            _maze = maze;
            _keys = keys;
            Position = position;
            MoveCount = moveCount;
        }

        public bool HasAllKeys => _keys == _maze.AllKeys;

        public IEnumerable<MazeRunner> Children()
        {
            foreach (var neighbor in _maze.NeighborLookup[Position])
            {
                if (CanMoveTo(neighbor))
                {
                    yield return Step(neighbor);
                }
            }
        }

        bool CanMoveTo(Neighbor neighbor) => (neighbor.Door & _keys) == neighbor.Door;

        MazeRunner Step(Neighbor neighbor)
        {
            var newPosition = neighbor.Target;
            var newKeys = _keys | neighbor.Key;

            return new MazeRunner(_maze, newPosition, newKeys, MoveCount + neighbor.Length);
        }

        public override bool Equals(object obj)
        {
            var other = (MazeRunner) obj;
            return  Position == other.Position && _keys == other._keys;
        }

        public override int GetHashCode() => HashCode.Combine(Position, _keys);
    }
}