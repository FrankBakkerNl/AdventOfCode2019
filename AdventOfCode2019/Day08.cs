using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day08
    {
        private static int ImageWith = 25;
        private static int ImageHight = 6;

        [Result(2413)]
        public static int GetAnswer1(string input) => GetChecksum(input.Select(c=>int.Parse(c.ToString())).ToArray(), ImageWith*ImageHight);

        [Result(@"
XXX   XX  XXX  XXXX XXX  
X  X X  X X  X    X X  X 
XXX  X    X  X   X  XXX  
X  X X    XXX   X   X  X 
X  X X  X X    X    X  X 
XXX   XX  X    XXXX XXX  ")]
        public static string GetAnswer2(string input) => Environment.NewLine +  Decode(input.Select(c=>int.Parse(c.ToString())).ToArray(), ImageWith, ImageHight);

        public static int GetChecksum(int[] input, int size)
        {
            var segments = Segment(input, size);
            var grouped = segments.Select(s => s.ToLookup(d => d));
            var layer = grouped.MinBy(g => g[0].Count());

            //the number of 1 digits multiplied by the number of 2 digits?
            return layer[1].Count() * layer[2].Count();
        }

        public static string Decode(int[] input, int with, int height)
        {
            var size = with * height;
            var layers = Segment(input, size);
            var combined = Enumerable.Range(0, size).Select(i => FirstNonTransparent(layers.Select(l => l[i])));
            var characters = combined.Select(i => i == 1 ? 'X' : ' ').ToArray();
            var lines = Segment(characters, with).Select(ch => new string(ch));
            return string.Join(Environment.NewLine, lines);
        }

        private static int FirstNonTransparent(IEnumerable<int> input)
            => input.SkipWhile(i => i == 2).First();

        private static IEnumerable<T[]> Segment<T>(T[] input, int size)
        {
            var layers = input.Length / size;

            return Enumerable.Range(0, layers).Select(l => new Span<T>(input, l * size, size).ToArray());
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
