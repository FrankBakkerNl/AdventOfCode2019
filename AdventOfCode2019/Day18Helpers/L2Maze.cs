using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day18Helpers
{
    public class L2Maze
    {
        public Neighbor[][] NeighborLookup;

        public uint AllKeys { get; set; }
        public int[] StartPositions { get; set; }

        public L2Maze (L1Maze maze)
        {
            // a root is a start point, key or door
            var roots = maze.StartPositions.Concat(maze.Keys.Keys.Concat(maze.Doors.Keys)).ToList();
            
            // assign an integer value to each root
            var indexMap = roots.Select((root, index) => (root, index)).ToDictionary(t => t.root, t => t.index);

            NeighborLookup = roots.Select(r => FindNeighbors(maze, r, indexMap)).ToArray();
            AllKeys = maze.AllKeys;
            StartPositions = maze.StartPositions.Select(s => indexMap[s]).ToArray();
        }

        Neighbor[] FindNeighbors(L1Maze maze, (int, int) position, Dictionary<(int, int), int> indexMap)
        {
            var runner = new L1MazeRunner(maze, position);
            return FindNearestNodes(runner).Select(r => new Neighbor()
            {
                Target = indexMap[r.Position],
                Length = r.MoveCount,
                Key = maze.Keys.TryGetValue(r.Position, out var k) ? k : 0,
                Door = maze.Doors.TryGetValue(r.Position, out var d) ? d : 0,
            }).ToArray();
        }

        public static IEnumerable<L1MazeRunner> FindNearestNodes(L1MazeRunner startTrack)
        {
            var tracks = new Queue<L1MazeRunner>();
            tracks.Enqueue(startTrack);
            var visited = new HashSet<int>();

            while (tracks.Count!=0)
            {
                var track = tracks.Dequeue();

                foreach (var nextTrack in track.Children())
                {
                    if (visited.Add(nextTrack.EqualsKey))
                    {
                        if (!nextTrack.AtRootNode)
                            tracks.Enqueue(nextTrack);
                        else
                            yield return nextTrack;
                    }
                }
            }
        }
    }

    public struct Neighbor
    {
        public int Target;
        public int Length;
        public uint Key;
        public uint Door;
    }
}