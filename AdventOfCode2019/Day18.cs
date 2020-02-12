using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AdventOfCode2019.Day18Helpers;

namespace AdventOfCode2019
{
    public class Day18
    {
        [Result(5450)]
        public static int GetAnswer1(string[] input) => FindAllKeys(input);

        [Result(2020)]
        public static int GetAnswer2(string[] input) => FindAllKeysMultiMaze(input);

        public static int FindAllKeys(string[] input)
        {
            var maze = new L1Maze(input);
            var l2Maze = new L2Maze(maze);

            var mazeRunner = new MazeRunner(l2Maze, l2Maze.StartPositions[0], 0);

            var allResults = BreadthFirstSearch(mazeRunner);
            return allResults.First(t => t.HasAllKeys).MoveCount;
        }

        private static int FindAllKeysMultiMaze(string[] input)
        {
            var multiMaze = UpdateMazeToMultiMaze(input);
            var mazes = SplitMultiMaze(multiMaze);

            // Calculate the optimal path for each sub-maze separately,
            // assuming all other keys are acquired in time
            return mazes.Sum(part =>
            {
                var l1 = new L1Maze(part);
                var l2 = new L2Maze(l1);
                var allOtherKeys = ~l2.AllKeys; // take all keys except for the ones from this maze
                var runner = new MazeRunner(l2, l2.StartPositions[0], allOtherKeys);
                return BreadthFirstSearch(runner).FirstOrDefault(t => t.HasAllKeys).MoveCount;
            });
        }

        public static IEnumerable<MazeRunner> BreadthFirstSearch(MazeRunner startTrack)
        {
            var tracks = new Heap<MazeRunner>();
            tracks.Add(startTrack, startTrack.MoveCount);
            var visited = new HashSet<object>();

            while (tracks.Any()) 
            {
                var track = tracks.Pop();
                if (!visited.Add(track)) continue;
                yield return track;

                foreach (var nextTrack in track.Children())
                {
                    tracks.Add(nextTrack, nextTrack.MoveCount);
                }
            }
        }

        public static string[] UpdateMazeToMultiMaze(string[] input1)
        {
            var multiMaze = input1.ToArray();
            multiMaze[39] = multiMaze[39].Substring(0, 39) + "@#@" + multiMaze[39].Substring(42, 39);
            multiMaze[40] = multiMaze[40].Replace(".@.", "###");
            multiMaze[41] = multiMaze[41].Substring(0, 39) + "@#@" + multiMaze[41].Substring(42, 39);
            return multiMaze;
        }

        public static string[][] SplitMultiMaze(string[] input) =>
            new[]
            {
                input.Take(41).Select(r => r.Substring(0, 41)).ToArray(),
                input.Take(41).Select(r => r.Substring(40, 41)).ToArray(),
                input.Skip(40).Select(r => r.Substring(0, 41)).ToArray(),
                input.Skip(40).Select(r => r.Substring(40, 41)).ToArray()
            };
    }
}
