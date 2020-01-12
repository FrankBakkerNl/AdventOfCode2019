using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public partial class Day20
    {
        public struct Portal
        {
            public (int x, int y) PIn;
            public (int x, int y) POut;
            public string Key;
            public int levelDelta;
        }

        public class L1Maze
        {
            public HashSet<(int, int)> Corridors;
            public Dictionary<(int x, int y), ((int x, int y) Destination, int levelDelta)> PortalMap;
            public (int, int) Start;
            public (int, int) End;


            public L1Maze(string[] input)
            {
                var fields = input.SelectMany((line, y) => line.Select((c, x) => (p: (x, y), c))).ToArray();

                Corridors = new HashSet<(int, int)>(fields.Where(f => f.c == '.').Select(f => f.p));
                var portals = FindPortals(input).ToList();
                var pairs = portals.GroupBy(p => p.Key)
                    .Where(g => g.Count() == 2)
                    .Select(pair => (from:pair.First(), to:pair.ElementAtOrDefault(1))).ToList();

                PortalMap = pairs.Concat(pairs.Select(p => (from:p.to, to:p.from)))
                    .ToDictionary(p => p.from.PIn, p => (p.to.POut, p.from.levelDelta));

                Start = portals.First(p => p.Key == "AA").POut;
                End = portals.First(p => p.Key == "ZZ").POut;

            }

            public static IEnumerable<Portal> FindPortals(string[] input)
            {
                var skipToInner = FindWith(input) + 2;
                var l1 = FindPortalsInLine(input, 0, 0, true, -1);
                var l2 = FindPortalsInLine(input, skipToInner, skipToInner, false, +1);
                var l3 = FindPortalsInLine(input, input.Length - skipToInner - 2, skipToInner, true, +1);
                var l4 = FindPortalsInLine(input, input.Length - 2, skipToInner, false, -1);


                var c1 = FindPortalsInColumn(input, 0, 0, true, -1);
                var c2 = FindPortalsInColumn(input, skipToInner, skipToInner, false, +1);
                var c3 = FindPortalsInColumn(input, input[0].Length - skipToInner - 2, skipToInner, true, +1);
                var c4 = FindPortalsInColumn(input, input[0].Length - 2, skipToInner, false, -1);

                return new[] {l1, l2, l3, l4, c1, c2, c3, c4}.SelectMany(c => c);
            }

            private static IEnumerable<Portal> FindPortalsInLine(string[] input, int line, int skip, bool top, int levelDelta)
            {

                return input[line].Zip(input[line + 1])
                    .Select((t, i) => new Portal
                        {
                            Key= $"{t.First}{t.Second}",
                            PIn= (i, top ? line + 1 : line),
                            POut= (i, top ? line + 2 : line -1),
                            levelDelta = levelDelta
                        }
                    )
                    .Skip(skip)
                    .SkipLast(skip)
                    .Where(s => s.Key.Trim().Length == 2);
            }

            private static IEnumerable<Portal> FindPortalsInColumn(string[] input, int column, int skip, bool left, int levelDelta)
            {
                return input
                    .Select((line, i) => new Portal
                    {
                        Key= $"{line[column]}{line[column + 1]}",
                        PIn= (left ? column + 1 : column , i),
                        POut=(left ? column + 2 : column -1, i),
                        levelDelta = levelDelta
                    })
                    .Skip(skip)
                    .SkipLast(skip)
                    .Where(s => s.Key.Trim().Length == 2);
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