using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode2019.Day18Helpers;

namespace AdventOfCode2019
{
    [Test]
    public partial class Day18
    {
        public static int GetAnswer1(string[] input) => FindAllKeys(input); // 5450
        public static int GetAnswer2(string[] input) => FindAllKeysMultiMaze(input); // 2020

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
            var maze = new L1Maze(multiMaze);
            var l2Maze = new L2Maze(maze);
            var mazeRunner = new MultiMazeRunner(l2Maze, l2Maze.StartPositions, 0);

            var allResults = BreadthFirstSearchMulti(mazeRunner);
            return allResults.First(t => t.HasAllKeys).MoveCount;
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

                foreach (var nextTrack in track.Children())
                {
                    yield return nextTrack;
                    tracks.Add(nextTrack, nextTrack.MoveCount);
                }
            }
        }

        public static IEnumerable<MultiMazeRunner> BreadthFirstSearchMulti(MultiMazeRunner startTrack)
        {
            var tracks = new Heap<MultiMazeRunner>();
            tracks.Add(startTrack, startTrack.MoveCount);
            var visited = new HashSet<MultiMazeRunner>();

            while (tracks.Any()) 
            {
                var track = tracks.Pop();
                if (!visited.Add(track)) continue;

                foreach (var nextTrack in track.Children())
                {
                    yield return nextTrack;
                    tracks.Add(nextTrack, nextTrack.MoveCount);
                }
            }
        }

        private static string[] UpdateMazeToMultiMaze(string[] input)
        {
            input[39] = input[39].Substring(0, 39) + "@#@" + input[39].Substring(42, 39);
            input[40] = input[40].Replace(".@.", "###");
            input[41] = input[41].Substring(0, 39) + "@#@" + input[41].Substring(42, 39);
            return input;
        }
    }
}
