using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day18Helpers
{
    public class L1Maze
    {
        public L1Maze(string[] input)
        {
            var fields = input.SelectMany((line, y) => line.Select((c, x) => (p: (x, y), c))).ToArray();

            Corridors = new HashSet<(int, int)>(fields.Where(f => f.c != '#').Select(f => f.p));
            Keys = fields.Where(f => char.IsLower(f.c)).ToDictionary(f => f.p, f => BitForKey(char.ToUpper(f.c)));
            Doors = fields.Where(f => char.IsUpper(f.c)).ToDictionary(f => f.p, f => BitForKey(f.c));
            StartPositions = fields.Where(f => f.c == '@').Select(f => f.p).ToArray();
            StartPosition = StartPositions.First();
            AllKeys = Keys.Values.Aggregate(0u, (a, k) => a | k);
        }

        public uint AllKeys { get; }
        public (int, int) StartPosition { get; }
        public (int, int)[] StartPositions { get; }
        public HashSet<(int, int)> Corridors { get; }

        public Dictionary<(int, int), uint> Doors { get; }
        public Dictionary<(int, int), uint> Keys { get; }

        uint BitForKey(char key) => 1u << key - 'A';

    }
}
