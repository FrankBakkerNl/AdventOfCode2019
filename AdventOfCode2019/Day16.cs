using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using AdventOfCode2019.VM;
using static System.Math;

namespace AdventOfCode2019
{
    [Test]
    public class Day16
    {

        [Result(63483758)]
        public static string GetAnswer1(string input) => MergeOutput( GetValues(ParseInputDigits(input), 100, 0, 8));

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
                Console.WriteLine(iteration);
                GetNextPhase(input, output);
                var temp = output;
                output = input;
                input = temp;
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
                    for (int repeat = 0; repeat < repeatCount && j+repeat < input.Length; repeat++)
                    {
                        sum += input[j+repeat];
                    }
                }
                startSkipCount = (repeatCount * 3)-1;

                for (int j = startSkipCount; j < input.Length; j+=patternLength)
                {
                    for (int repeat = 0; repeat < repeatCount && j+repeat < input.Length; repeat++)
                    {
                        sum -= input[j+repeat];
                    }
                }
                output[i] = Abs(sum) % 10;
            }
        }

        private static int[] ParseInputDigits(string input) => input.Select(i => i - '0').ToArray();
        private static string MergeOutput(int[] result) => new string( result.Select(c => (char) (c + '0')).ToArray());


    }
}

