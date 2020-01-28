using System.Linq;
using static System.Math;

namespace AdventOfCode2019
{
    public class Day16
    {

        [Result("63483758")]
        public static string GetAnswer1(string input) => MergeOutput(GetValues(ParseInputDigits(input), 100, 0, 8));

        [Result("96099551")]
        public static string GetAnswer2(string input)
        {
            var inputDigits = ParseInputDigits(input);

            // repeat 10.000 times
            var fullInput = Enumerable.Repeat(inputDigits, 10_000).SelectMany(i=>i).ToArray();
            var messageOffset = int.Parse(input.Substring(0, 7));
            var relevantInput = fullInput.Skip(messageOffset).ToArray();
            var values = SimulatePhasesBottom(relevantInput, 100).Take(8).ToArray();
            return MergeOutput(values);
        }


        static int[] GetValues(int[] input, int phases, int start, int outputLength)
        {
            return SimulatePhases(input, phases).Skip(start).Take(outputLength).ToArray();
        }


        public static int[] SimulatePhases(int[] input, int iterations)
        {
            // create one extra buffer and keep swapping them
            var output = new int[input.Length];
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                GetNextPhase(input, output);
                Swap(ref input, ref output);
            }

            return input;
        }

        public static void GetNextPhase(int[] input, int[] output)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var sum = 0;

                var repeatCount = i + 1;
                var patternLength = repeatCount * 4;
                var startSkipCount = repeatCount - 1;

                for (int j = startSkipCount; j < input.Length; j+=patternLength)
                {
                    var end = Min(j + repeatCount, input.Length);
                    for (int k = j; k< end; k++)
                    {
                        sum += input[k];
                    }
                }
                startSkipCount = (repeatCount * 3)-1;

                for (int j = startSkipCount; j < input.Length; j+=patternLength)
                {
                    var end = Min(j + repeatCount, input.Length);
                    for (int k = j; k< end; k++)
                    {
                        sum -= input[k];
                    }
                }
                output[i] = Abs(sum) % 10;
            }
        }


        public static int[] SimulatePhasesBottom(int[] input, int iterations)
        {
            // create one extra buffer and keep swapping them
            var output = new int[input.Length];
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                GetNextPhaseBottom(input, output);
                Swap(ref input, ref output);
            }

            return input;
        }


        public static void GetNextPhaseBottom(int[] input, int[] output)
        {
            var accumulator = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                for (int j = i; j >= i; j--)
                {
                    accumulator += input[j];
                }

                output[i] = accumulator %10;
            }
        }

        static void Swap<T>(ref T x, ref T y)
        {
            T t = x;
            x = y;
            y = t;
        }

        private static int[] ParseInputDigits(string input) => input.Select(i => i - '0').ToArray();
        private static string MergeOutput(int[] result) => new string( result.Select(c => (char) (c + '0')).ToArray());


    }
}

