﻿using System;
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

        private static string Input => File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day07.txt");


        public static BigInteger GetAnswer1(int[]input) => FindMax(input);

        public static BigInteger GetAnswer2(int[]input) => FindMaxFeedBack(input);


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
            BigInteger lastResult = 0;
            foreach (var phaseSetting in phaseSettings)
            {
                var amp = new IntCodeComputer(program);

                amp.WriteInput(phaseSetting);
                amp.WriteInput(lastResult);
                amp.Run();
                lastResult = amp.ReadOutput();
            }

            return lastResult;
        }



        public static BigInteger FindMaxFeedBack(int[] program) 
            => InputCombinationsFeedback(5).Max(c => GetThrusterSignalFeedback(program, c).Result);

        static readonly int[] PossibleInputsFeedback = Enumerable.Range(5, 5).ToArray();

        public static IEnumerable<ImmutableArray<int>> InputCombinationsFeedback(int number)
        {
            if (number == 1) return PossibleInputsFeedback.Select(v=>ImmutableArray.Create(v));
            return InputCombinationsFeedback(number - 1).SelectMany(v1 => PossibleInputsFeedback.Except(v1).Select(v1.Add));
        }

        public static async Task<BigInteger> GetThrusterSignalFeedback(int[] program, IEnumerable<int> phaseSettings)
        {
            phaseSettings = phaseSettings.ToArray();

            var amps = phaseSettings.Select(_ => new IntCodeComputer(program)).ToList();

            // feedback last output to first input
            var previousAmp = amps.Last();
            foreach (var (phaseSetting, amp) in phaseSettings.Zip(amps))
            {
                previousAmp.PipeTo(amp);
                await amp.WriteInputAsync(phaseSetting);
                previousAmp = amp;
            }

            await amps[0].WriteInputAsync(0);
            var tasks = amps.Select( a => a.RunAsync()).ToArray();
            await Task.WhenAll(tasks.ToArray());

            return await amps.Last().ReadOutputAsync();
        }
    }
}
