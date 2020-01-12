using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    //[Test]
    public class Day20
    {
        public static int GetAnswer1(string[] input) => FindShortestRoute(input); // 626


        public static void RenderMaze(string[] input)
        {
            Console.Clear();
            Console.WindowHeight = input.Length + 5;

            foreach (var line in input)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(line.Replace("#", "█"));
            }
        }

        public static void RenderVisit((int x,int y)p)
        {
            Console.SetCursorPosition(p.x, p.y);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("█");
        }

        public static void Visualize(string[] input)
        {

            RenderMaze(input);
            var maze = new L1Maze(input);
            var mazeRunner = new MazeRunner(maze.Start, null, 0);
            foreach (var runner in FindEnd(mazeRunner, maze))
            {
                RenderVisit(runner.Position);
                if (runner.Position == maze.End)
                {
                    Console.SetCursorPosition(1, input.Length + 2);
                    Console.WriteLine(runner.MoveCount);
                }
            }
        }

        public static int FindShortestRoute(string[] input)
        {
            var maze = new L1Maze(input);
            var mazeRunner = new MazeRunner(maze.Start, null, 0);

            var endTrack = FindEnd(mazeRunner, maze).First(r => r.Position == maze.End);
            return endTrack.MoveCount;
        }

        public static IEnumerable<MazeRunner> FindEnd(MazeRunner startTrack, L1Maze maze)
        {
            var tracks = new Queue<MazeRunner>();
            tracks.Enqueue(startTrack);
            var visited = new HashSet<(int, int)>();

            while (tracks.Count != 0)
            {
                var track = tracks.Dequeue();

                foreach (var nextTrack in track.Children(maze))
                {
                    if (visited.Add(nextTrack.Position))
                    {
                        tracks.Enqueue(nextTrack);
                        yield return nextTrack;
                    }
                }
            }
        }


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
                // do not step back
               // if (_lastMove.HasValue && (move + 2) % 4 == _lastMove.Value) return false;

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
                    3 => (Position.x, Position.y - 1)
                };
                if (maze.PortalMap.TryGetValue(newPosition, out var ported))
                    newPosition = ported;

                return newPosition;
            }

            MazeRunner Step(int move, L1Maze maze)
            {
                var newPosition = GetNewPosition(move, maze);
                return new MazeRunner(newPosition, move, MoveCount + 1);
            }

        }

        public class L1Maze
        {
            public HashSet<(int, int)> Corridors;
            public Dictionary<(int i, int line), (int i, int line)> PortalMap;
            public (int, int) Start;
            public (int, int) End;


            public L1Maze(string[] input)
            {
                var fields = input.SelectMany((line, y) => line.Select((c, x) => (p: (x, y), c))).ToArray();

                Corridors = new HashSet<(int, int)>(fields.Where(f => f.c == '.').Select(f => f.p));
                var portals = FindPortals(input).ToList();
                var pairs = portals.GroupBy(p => p.key)
                    .Where(g => g.Count() == 2)
                    .Select(pair => (pair.First(), pair.ElementAtOrDefault(1))).ToList();
                PortalMap = pairs.Concat(pairs.Select(p => (p.Item2, p.Item1)))
                    .ToDictionary(p => p.Item1.pIn, p => p.Item2.pOut);

                Start = portals.First(p => p.key == "AA").pOut;
                End = portals.First(p => p.key == "ZZ").pOut;

            }

            public static IEnumerable<(string key, (int i, int line) pIn, (int i, int line) pOut)> FindPortals(string[] input)
            {
                var skipToInner = FindWith(input) + 2;
                var l1 = FindPortalsInLine(input, 0, 0, true);
                var l2 = FindPortalsInLine(input, skipToInner, skipToInner, false);
                var l3 = FindPortalsInLine(input, input.Length - skipToInner - 2, skipToInner, true);
                var l4 = FindPortalsInLine(input, input.Length - 2, skipToInner, false);


                var c1 = FindPortalsInColumn(input, 0, 0, true);
                var c2 = FindPortalsInColumn(input, skipToInner, skipToInner, false);
                var c3 = FindPortalsInColumn(input, input[0].Length - skipToInner - 2, skipToInner, true);
                var c4 = FindPortalsInColumn(input, input[0].Length - 2, skipToInner, false);

                return new[] {l1, l2, l3, l4, c1, c2, c3, c4}.SelectMany(c => c);
            }

            private static IEnumerable<(string key, (int i, int line) pIn, (int i, int line) pOut)> FindPortalsInLine(string[] input, int line,
                int skip, bool top)
            {

                return input[line].Zip(input[line + 1])
                    .Select((t, i) =>
                        (
                            key: $"{t.First}{t.Second}",
                            pIn: (i, top ? line + 1 : line),
                            pOut: (i, top ? line + 2 : line -1)
                            )
                    )
                    .Skip(skip)
                    .SkipLast(skip)
                    .Where(s => s.key.Trim().Length == 2);
            }

            private static IEnumerable<(string key, (int i, int line) pIn, (int i, int line) pOut)> FindPortalsInColumn(string[] input,
                int column, int skip, bool left)
            {
                return input
                    .Select((line, i) =>
                        (
                            key: $"{line[column]}{line[column + 1]}",
                            pIn: (left ? column + 1 : column , i),
                            pOut:(left ? column + 2 : column -1, i)))
                    .Skip(skip)
                    .SkipLast(skip)
                    .Where(s => s.key.Trim().Length == 2);
            }

            public static int FindWith(string[] input)
            {
                var mid = input.Length / 2;
                var midleRow = input[mid];
                return midleRow.Skip(2).TakeWhile(c => c == '#' || c == '.').Count();
            }
        }
    }
}