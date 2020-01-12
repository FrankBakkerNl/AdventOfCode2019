using System.Collections.Generic;

namespace AdventOfCode2019
{
    public partial class Day20
    {
        public struct MultiLevelMazeRunner
        {
            public (int x, int y) Position;
            public int Level;
            public int MoveCount { get; }

            public MultiLevelMazeRunner((int, int) position, int level, int moveCount = 0)
            {
                Position = position;
                MoveCount = moveCount;
                Level = level;
            }

            public IEnumerable<MultiLevelMazeRunner> Children(L1Maze maze)
            {
                for (int move = 0; move < 4; move++)
                {
                    var newPos = GetNewPosition(move, maze);
                    if (newPos.l >=0 && maze.Corridors.Contains(newPos.Item1))
                    {
                        yield return new MultiLevelMazeRunner(newPos.p, newPos.l, MoveCount + 1);
                    }
                }
            }

            private ((int, int) p,int l) GetNewPosition(int move, L1Maze maze)
            {
                var newPosition = move switch
                {
                    0 => (Position.x - 1, Position.y),
                    1 => (Position.x, Position.y + 1),
                    2 => (Position.x + 1, Position.y),
                    3 => (Position.x, Position.y - 1),
                };
                var newLevel = Level;
                if (maze.PortalMap.TryGetValue(newPosition, out var ported))
                {
                     newPosition = ported.Destination;
                    newLevel = newLevel + ported.levelDelta;
                }

                return (newPosition, newLevel);
            }
        }
    }
}