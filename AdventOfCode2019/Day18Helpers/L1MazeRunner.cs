using System.Collections.Generic;

namespace AdventOfCode2019.Day18Helpers
{
    public readonly struct L1MazeRunner
    {
        private readonly L1Maze _maze;
        public (int x, int y) Position { get; }
        private readonly int? _lastMove;
        public int MoveCount { get; }

        public int EqualsKey => Position.x * 100 + Position.y;

        public L1MazeRunner(L1Maze maze, (int, int) position, int?lastMove = null, int moveCount = 0)
        {
            _maze = maze;
            Position = position;
            _lastMove = lastMove;
            MoveCount = moveCount;
        }

        public IEnumerable<L1MazeRunner> Children()
        {
            for (int move = 0; move < 4; move++)
                if (IsMovePossible(move))
                    yield return Step(move);
        }

        public bool AtRootNode => _maze.Doors.ContainsKey(Position) || _maze.Keys.ContainsKey(Position);

        bool IsMovePossible(int move)
        {
            // do not step back
            if (_lastMove.HasValue && (move + 2) % 4 == _lastMove.Value) return false;

            var newPos = GetNewPosition(move);

            return _maze.Corridors.Contains(newPos);
        }

        private (int, int) GetNewPosition(int move)
        {
            return move switch
            {
                0 => (Position.x - 1, Position.y),
                1 => (Position.x,     Position.y + 1),
                2 => (Position.x + 1, Position.y),
                _ => (Position.x,     Position.y - 1),
            };
        }

        L1MazeRunner Step(int move)
        {
            var newPosition = GetNewPosition(move);
            return new L1MazeRunner(_maze, newPosition, move, MoveCount + 1);
        }
    }
}