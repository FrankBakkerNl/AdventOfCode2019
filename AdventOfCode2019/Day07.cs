using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day07
    {
        [Result(298586)]
        public static long GetAnswer1(int[]input) => (long)FindMax(input);

        [Result(9246095)]
        public static long GetAnswer2(int[]input) => (long)FindMaxFeedBack(input); // 9246095


        public static BigInteger FindMax(int[] program)
        {
            return InputCombinations(5).Max(c => GetThrusterSignal(program, c));
        }

        static readonly int[] PossibleInputs = Enumerable.Range(0, 5).ToArray();

        public static IEnumerable<ImmutableArray<int>> InputCombinations(int number)
        {
            if (number == 1) return PossibleInputs.Select(v=>ImmutableArray.Create(v));
            return InputCombinations(number - 1).SelectMany(v1 => PossibleInputs.Except(v1).Select(v1.Add));
        }

        public static BigInteger GetThrusterSignal(int[] program, IEnumerable<int> phaseSettings)
        {
            var lastResult = new BigInteger[]{0};
            foreach (var phaseSetting in phaseSettings)
            {
                var amp = new IntCodeComputer(program);
                
                amp.Run(phaseSetting, lastResult[0]);
                lastResult = amp.ReadAvailableOutput();
            }

            return lastResult.First();
        }

        public static BigInteger FindMaxFeedBack(int[] program) 
            => InputCombinationsFeedback(5).Max(c => GetThrusterSignalFeedback(program, c));

        static readonly int[] PossibleInputsFeedback = Enumerable.Range(5, 5).ToArray();

        public static IEnumerable<ImmutableArray<int>> InputCombinationsFeedback(int number)
        {
            if (number == 1) return PossibleInputsFeedback.Select(v=>ImmutableArray.Create(v));
            return InputCombinationsFeedback(number - 1).SelectMany(v1 => PossibleInputsFeedback.Except(v1).Select(v1.Add));
        }

        public static BigInteger GetThrusterSignalFeedback(int[] program, IEnumerable<int> phaseSettings)
        {
            phaseSettings = phaseSettings.ToArray();

            var amps = phaseSettings.Select(_ => new IntCodeComputer(program)).ToList();

            // feedback each last output to first input
            foreach (var (phaseSetting, amp) in phaseSettings.Zip(amps))
            {
                amp.Run(phaseSetting);
            }

            var lastResult = new BigInteger[]{0};
            while (!amps.Last().Finished)
            {
                foreach (var amp in amps)
                {
                    amp.Run(lastResult);
                    lastResult = amp.ReadAvailableOutput();
                }
            }

            return lastResult[0];
        }
    }
}
