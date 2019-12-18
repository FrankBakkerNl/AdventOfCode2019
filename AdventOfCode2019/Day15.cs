using System;
using System.Linq;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    [Test]
    public class Day15
    {
        public static int[] Moves = {1, 2, 3, 4};
        public enum Result{Wall=0, Ok=1, Found=2}

        public static long GetAnswer1(int[] program) // 330
        {
            var cpu = new IntCodeComputer(program);
            Result Step (int move)
            {
                cpu.Run(move);
                return (Result)(int)cpu.ReadOutput();
            }
            return Moves.Min(m => GetStepCount(Step, m));
        }

        public static long GetStepCount(Func<int, Result> step, int move)
        {
            switch (step(move))
            {
                case Result.Found:
                    step(Reverse(move));
                    return 1;
                case Result.Wall:
                    return int.MaxValue;
                default:
                    var result = Moves.Where(m => m != Reverse(move)).Min(m => GetStepCount(step, m)) + 1;
                    step(Reverse(move));
                    return result;
            }
        }

        private static int Reverse(int move) => new[] {0, 2, 1, 4, 3}[move];
    }
}
