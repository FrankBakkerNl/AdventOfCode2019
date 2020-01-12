using System.Collections.Generic;

namespace AdventOfCode2019
{
    public partial class Day20
    {
        public struct MazeRunner
        {
            public (int x, int y) Position;
            private readonly int? _lastMove;
            public int MoveCount { get; }

            public MazeRunner((int, int) position, int? lastMove = null, int moveCount = 0)
            {
                Position = position;
                _lastMove = lastMove;
                MoveCount = moveCount;
            }

            public IEnumerable<MazeRunner> Children(L1Maze maze)
            {
                for (int move = 0; move < 4; move++)
                    if (IsMovePossible(move, maze))
                        yield return Step(move, maze);

            }

            bool IsMovePossible(int move, L1Maze maze)
            {
                var newPos = GetNewPosition(move, maze);
                return maze.Corridors.Contains(newPos);
            }

            private (int, int) GetNewPosition(int move, L1Maze maze)
            {
                var newPosition = move switch
                {
                    0 => (Position.x - 1, Position.y),
                    1 => (Position.x, Position.y + 1),
                    2 => (Position.x + 1, Position.y),
                    3 => (Position.x, Position.y - 1),
                };
                if (maze.PortalMap.TryGetValue(newPosition, out var ported))
                    newPosition = ported.Destination;

                return newPosition;
            }

            MazeRunner Step(int move, L1Maze maze)
            {
                var newPosition = GetNewPosition(move, maze);
                return new MazeRunner(newPosition, move, MoveCount + 1);
            }

        }
    }
}