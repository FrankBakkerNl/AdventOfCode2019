using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode2019.Day18Helpers;

namespace AdventOfCode2019
{
    [Test]
    public partial class Day20
    {
        [Result(626)]
        public static int GetAnswer1_(string[] input) => FindShortestRoute(input); // 626

        [Result(6912)]
        public static int GetAnswer2(string[] input) => FindShortestRouteMultiLevel(input); // 6912

        public static void RenderMaze(string[] input)
        {
            Console.Clear();

            foreach (var line in input)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(line.Replace("#", "█"));
            }
        }

        //public static void RenderVisit((int x,int y)p, int level)
        //{

        //    Console.SetCursorPosition(p.x, p.y);
        //    Console.ForegroundColor = (ConsoleColor)((level+1 )%15);
        //    Console.Write("█");
        //}

        //public static int Visualize(string[] input)
        //{

        //    RenderMaze(input);
        //    var maze = new L1Maze(input);
        //    var l2 = new L2MazeRunner(maze);
        //    var mazeRunner = new L2MazeRunner(0, 0);
        //    foreach (var runner in SearchMultiLevelMaze(mazeRunner, l2))
        //    {
        //        RenderVisit(runner.Position, runner.Level);
        //            //Console.ReadKey(true);
        //        if (runner.Position == 1 && runner.Level == 0)
        //        {
        //            Console.SetCursorPosition(1, input.Length + 2);
        //            Console.WriteLine(runner.MoveCount);
        //            return runner.MoveCount;
        //        }
        //    }

        //    return 0;
        //}

        public static int FindShortestRoute(string[] input)
        {
            var maze = new L1Maze(input);
            var mazeRunner = new L1MazeRunner(maze.Start, 0);

            var endTrack = SearchMaze(mazeRunner, maze).First(r => r.Position == maze.End);
            return endTrack.MoveCount;
        }

        public static IEnumerable<L1MazeRunner> SearchMaze(L1MazeRunner startTrack, L1Maze maze)
        {
            var tracks = new Queue<L1MazeRunner>();
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


        public static int FindShortestRouteMultiLevel(string[] input)
        {
            var maze = new L1Maze(input);
            var l2 = new L2Maze(maze);
            var mazeRunner = new L2MazeRunner(0, 0);

            var all = SearchMultiLevelMaze(mazeRunner, l2).TakeWhile(r => r.MoveCount < 6913).ToList();

            return SearchMultiLevelMaze(mazeRunner, l2).FirstOrDefault(r => r.Position == 1 && r.Level == 0)
                       .MoveCount;
        }

        public static IEnumerable<L2MazeRunner> SearchMultiLevelMaze(L2MazeRunner startTrack, L2Maze maze)
        {
            var tracks = new Heap<L2MazeRunner>();
            tracks.Add(startTrack, startTrack.MoveCount);
            var visited = new HashSet<( int, int)>();

            while (tracks.Any())
            {
                var track = tracks.Pop();

                foreach (var nextTrack in track.Children(maze))
                {
                    if (visited.Add((nextTrack.Position, nextTrack.Level)))                    {
                        tracks.Add(nextTrack, nextTrack.MoveCount);
                        yield return nextTrack;
                    }
                }
            }
        }

        public struct Neighbor
        {
            public int Target;
            public int LevelDelta;
            public int Length;
        }


        public struct L2MazeRunner
        {
            public int Position { get; }
            public int Level { get; }
            public int MoveCount { get; }

            public L2MazeRunner(int position, int level = 0, int moveCount = 0)
            {
                Position = position;
                Level = level;
                MoveCount = moveCount;
            }

            public IEnumerable<L2MazeRunner> Children(L2Maze maze)
            {
                foreach (var neighbor in maze.NeighborLookup[Position])
                {
                    var newLevel = Level + neighbor.LevelDelta;
                    if (newLevel < 0) continue;
                    yield return new L2MazeRunner(neighbor.Target, newLevel, MoveCount + neighbor.Length);
                }
            }
        }

        public class L2Maze
        {
            public Neighbor[][] NeighborLookup;
            public Dictionary<(int, int), int> IndexLookup { get; }

            public L2Maze(L1Maze l1Maze)
            {
                var roots = new[] {l1Maze.Start, l1Maze.End}.Concat(l1Maze.PortalMap.Values.Select(d => d.Destination)).ToList();

                IndexLookup = roots.Select((r, i) => (r, i)).ToDictionary(t => t.r, t => t.i);

                NeighborLookup = roots.Select(root =>

                {
                    var start = new L1MazeRunner(root, 0);
                    return NavigateL1Maze(start, l1Maze)
                        .Where(state => l1Maze.PortalMap.ContainsKey(state.Position))
                        .Select(state => new Neighbor()
                        {
                            Length = state.MoveCount,
                            LevelDelta = l1Maze.PortalMap[state.Position].levelDelta,
                            Target = IndexLookup[l1Maze.PortalMap[state.Position].Destination]
                        }).ToArray();

                }).ToArray();
            }

            private static IEnumerable<L1MazeRunner> NavigateL1Maze(L1MazeRunner start, L1Maze l1Maze)
            {
                var open = new Queue<L1MazeRunner>();
                open.Enqueue(start);
                var closed = new HashSet<(int,int)>();

                while (open.Count!=0)
                {
                    var current = open.Dequeue();
                    yield return current;

                    foreach (var child in current.Children(l1Maze))
                    {
                        if (closed.Add(child.Position)) open.Enqueue(child);
                    }
                }
            }
        }
    }
}