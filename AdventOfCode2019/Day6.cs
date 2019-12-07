using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            var input = File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day6.txt");

            return GetTotalOrbits(input);
        }

        public static int GetAnswer2()
        {
            var input = File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day6.txt");
            return GetOrbitTransfers(input, "YOU", "SAN");
        }


        public static int GetTotalOrbits(string input)
        {
            var orbits = Parse(input);
            var orbitMap = OrbitMap(orbits);

            int Count(string planet) => planet == "COM" ? 0 : Count(orbitMap[planet]) + 1;

            return orbitMap.Keys.Sum(Count);
        }


        public static int GetOrbitTransfers(string input, string you, string san)
        {
            var orbits = Parse(input);
            var orbitMap = OrbitMap(orbits);

            ImmutableArray<string> Path(string planet) => planet == "COM" ? ImmutableArray.Create<string>() 
                                                                          : Path(orbitMap[planet]).Add(planet);

            var youPath = Path(orbitMap[you]);
            var santaPath = Path(orbitMap[san]);
            var commonRoot = youPath.Zip(santaPath).TakeWhile(x => x.First == x.Second).Count();
            return youPath.Length - commonRoot + santaPath.Length - commonRoot ;

        }


        private static Dictionary<string, string> OrbitMap(IEnumerable<(string parent, string child)> orbits) 
            => orbits.ToDictionary(o => o.child, o => o.parent);


        public static IEnumerable<(string parent, string child)> Parse(string input)
        {
            var lines = input.Split(Environment.NewLine);
            return lines.Select(o => o.Split(')')).Select(ar => (ar[0], ar[1]));
        }
    }
}
