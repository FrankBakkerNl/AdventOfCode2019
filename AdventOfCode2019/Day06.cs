using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day06
    {
        private static string Input => File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day06.txt");

        public static int GetAnswer1() => GetTotalOrbits(Input);

        public static int GetAnswer2() => GetOrbitTransfers(Input, "YOU", "SAN");

        public static int GetTotalOrbits(string input)
        {
            var orbitMap = CreateOrbitMap(input);

            int Count(string planet) => planet == "COM" ? 0 : Count(orbitMap[planet]) + 1;

            return orbitMap.Keys.Sum(Count);
        }

        public static int GetOrbitTransfers(string input, string you, string san)
        {
            var orbitMap = CreateOrbitMap(input);

            ImmutableArray<string> Path(string planet) => planet == "COM" ? ImmutableArray.Create<string>() 
                                                                          : Path(orbitMap[planet]).Add(planet);

            var youPath = Path(orbitMap[you]);
            var santaPath = Path(orbitMap[san]);

            var commonRoot = youPath.Zip(santaPath).TakeWhile(x => x.First == x.Second).Count();
            return youPath.Length - commonRoot + santaPath.Length - commonRoot ;
        }

        private static Dictionary<string, string> CreateOrbitMap(string input) 
            => input.Split(Environment.NewLine)
                    .Select(o => o.Split(')'))
                    .ToDictionary(ar => ar[1], ar => ar[0]);
    }
}
