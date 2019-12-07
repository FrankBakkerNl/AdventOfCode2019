using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day6
    {

        public static int GetAnswer1()
        {
            var orbits = Parse(File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day6.txt"));
            return Map(orbits);
        }

        public static int GetTotalOrbits(string input)
        {
            var orbits = Parse(input);
            return Map(orbits);
        }

        public static int Map(IEnumerable<(string parent, string child)> orbits)
        {
            var orbitMap = orbits.ToDictionary(o => o.child, o => o.parent);

            int Count(string planet) => planet == "COM" ? 0 : Count(orbitMap[planet]) + 1;

            return orbitMap.Keys.Sum(Count);
        }


        public static IEnumerable<(string, string)> Parse(string input)
        {
            var lines = input.Split(Environment.NewLine);
            return lines.Select(o => o.Split(')')).Select(ar => (ar[0], ar[1]));
        }
    }
}
