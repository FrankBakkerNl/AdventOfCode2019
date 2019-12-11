using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public class Day7
    {

        private static string Input => File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day7.txt");


        public static int GetAnswer1() => FindMax(Input.Split(',').Select(int.Parse).ToArray());

        public static int GetAnswer2() => FindMaxFeedBack(Input.Split(',').Select(int.Parse).ToArray());


        public static int FindMax(int[] program)
        {
            return InputCombinations(5).Max(c => GetThrusterSignal(program, c));
        }

        static readonly int[] PossibleInputs = Enumerable.Range(0, 5).ToArray();

        public static IEnumerable<ImmutableArray<int>> InputCombinations(int number)
        {
            if (number == 1) return PossibleInputs.Select(v=>ImmutableArray.Create(v));
            return InputCombinations(number - 1).SelectMany(v1 => PossibleInputs.Except(v1).Select(v1.Add));
        }

        public static int GetThrusterSignal(int[] program, IEnumerable<int> phaseSettings)
        {
            int lastResult = 0;
            foreach (var phaseSetting in phaseSettings)
            {
                var amp = new IntCodeComputer(program);

                amp.Input.Add(phaseSetting);
                amp.Input.Add(lastResult);
                amp.Run();
                lastResult = amp.Output.Take();
            }

            return lastResult;
        }



        public static int FindMaxFeedBack(int[] program) 
            => InputCombinationsFeedback(5).Max(c => GetThrusterSignalFeedback(program, c));

        static readonly int[] PossibleInputsFeedback = Enumerable.Range(5, 5).ToArray();

        public static IEnumerable<ImmutableArray<int>> InputCombinationsFeedback(int number)
        {
            if (number == 1) return PossibleInputsFeedback.Select(v=>ImmutableArray.Create(v));
            return InputCombinationsFeedback(number - 1).SelectMany(v1 => PossibleInputsFeedback.Except(v1).Select(v1.Add));
        }

        public static int GetThrusterSignalFeedback(int[] program, IEnumerable<int> phaseSettings)
        {
            phaseSettings = phaseSettings.ToArray();
            var previousOutput = new BlockingCollection<int>(new ConcurrentQueue<int>());

            var amps = new List<IntCodeComputer>();
            foreach (var _ in phaseSettings)
            {
                var amp = new IntCodeComputer(program, previousOutput);
                amps.Add(amp);
                previousOutput = amp.Output;
            }

            // feedback last output to first input
            amps[0].Input = previousOutput;

            foreach (var (phaseSetting, amp) in phaseSettings.Zip(amps))
            {
                amp.Input.Add(phaseSetting);
            }

            amps[0].Input.Add(0);
            var tasks = amps.Select(a => Task.Run(a.Run)).ToArray();
            Task.WaitAll(tasks.ToArray());

            return previousOutput.Take();
        }

    }
}
