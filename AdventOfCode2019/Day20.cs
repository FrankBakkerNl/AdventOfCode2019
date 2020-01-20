using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019
{
    public partial class Day20
    {
        [Result(626)]
        public static int GetAnswer1(string[] input) => FindShortestRoute(input); // 626

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

        public static void RenderVisit((int x,int y)p, int level)
        {

            Console.SetCursorPosition(p.x, p.y);
            Console.ForegroundColor = (ConsoleColor)((level+1 )%15);
            Console.Write("█");
        }

        public static int Visualize(string[] input)
        {

            RenderMaze(input);
            var maze = new L1Maze(input);
            var mazeRunner = new MultiLevelMazeRunner(maze.Start, 0, 0);
            foreach (var runner in SearchMultiLevelMaze(mazeRunner, maze))
            {
                RenderVisit(runner.Position, runner.Level);
                    //Console.ReadKey(true);
                if (runner.Position == maze.End && runner.Level == 0)
                {
                    Console.SetCursorPosition(1, input.Length + 2);
                    Console.WriteLine(runner.MoveCount);
                    return runner.MoveCount;
                }
            }

            return 0;
        }

        public static int FindShortestRoute(string[] input)
        {
            var maze = new L1Maze(input);
            var mazeRunner = new MazeRunner(maze.Start, null, 0);

            var endTrack = SearchMaze(mazeRunner, maze).First(r => r.Position == maze.End);
            return endTrack.MoveCount;
        }

        public static IEnumerable<MazeRunner> SearchMaze(MazeRunner startTrack, L1Maze maze)
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


        public static int FindShortestRouteMultiLevel(string[] input)
        {
            var maze = new L1Maze(input);
            var mazeRunner = new MultiLevelMazeRunner(maze.Start, 0, 0);
            return SearchMultiLevelMaze(mazeRunner, maze).FirstOrDefault(r => r.Position == maze.End && r.Level == 0)
                       .MoveCount;
        }

        public static IEnumerable<MultiLevelMazeRunner> SearchMultiLevelMaze(MultiLevelMazeRunner startTrack, L1Maze maze)
        {
            var tracks = new Queue<MultiLevelMazeRunner>();
            tracks.Enqueue(startTrack);
            var visited = new HashSet<(int, int, int)>();

            while (tracks.Count != 0)
            {
                var track = tracks.Dequeue();

                foreach (var nextTrack in track.Children(maze))
                {
                    if (visited.Add((nextTrack.Position.x, nextTrack.Position.y, nextTrack.Level)))                    {
                        tracks.Enqueue(nextTrack);
                        yield return nextTrack;
                    }
                }
            }
        }
    }
}