using System.Collections.Generic;

namespace AdventOfCode2019
{
    public partial class Day20
    {
        public struct L1MazeRunner
        {
            public (int x, int y) Position;
            public int MoveCount { get; }

            public L1MazeRunner((int, int) position, int moveCount = 0)
            {
                Position = position;
                MoveCount = moveCount;
            }

            public IEnumerable<L1MazeRunner> Children(L1Maze maze)
            {
                for (int move = 0; move < 4; move++)
                {
                    var newPos = GetNewPosition(move);
                    if (maze.Corridors.Contains(newPos) || maze.PortalMap.ContainsKey(newPos))
                    {
                        yield return new L1MazeRunner(newPos, MoveCount + 1);
                    }
                }
            }

            private (int, int) GetNewPosition(int move) =>
                move switch
                {
                    0 => (Position.x - 1, Position.y),
                    1 => (Position.x, Position.y + 1),
                    2 => (Position.x + 1, Position.y),
                    _ => (Position.x, Position.y - 1),
                };
        }
    }
}