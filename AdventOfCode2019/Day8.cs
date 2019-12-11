using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Day8
    {
        private static string Input => File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day8.txt");

        public static int GetAnswer1() => GetChecksum(Input.Select(c=>int.Parse(c.ToString())).ToArray(), 25*6);

        public static int GetAnswer2() => 0;

        public static int GetChecksum(int[] input, int size)
        {
            var layers = input.Length / size;

            var segments = Enumerable.Range(0, layers).Select(l => new Span<int>(input, l * size, size).ToArray());
            var grouped = segments.Select(s => s.ToLookup(d => d));
            var layer = grouped.MinBy(g => g[0].Count());

            //the number of 1 digits multiplied by the number of 2 digits?
            return layer[1].Count() * layer[2].Count();
        }
    }

    static class Day8Extensions
    {
        public static T MinBy<T>(this IEnumerable<T> input, Func<T, int> selector)
        {
            T result = default;
            var minValue = int.MaxValue;
            foreach (var i in input)
            {
                var currentValue = selector(i);
                if (currentValue < minValue)
                {
                    minValue = currentValue;
                    result = i;
                }
            }
            return result;
        }
    }
}
