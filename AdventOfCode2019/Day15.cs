using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day15
    {
        public static int[] Moves = { 1, 2, 3, 4 };
        public enum Result { Wall = 0, Ok = 1, OxygenTank = 2 }

        public static long GetAnswer1(int[] program) // 330
        {
            var cpu = new IntCodeComputer(program);
            Result Step(int move)
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
                case Result.OxygenTank:
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

        private static int Reverse(int move) => new[] { 0, 2, 1, 4, 3 }[move];


        public static int GetAnswer2(int[] program)
        {
            var cpu = new IntCodeComputer(program);
            var startTrack = new Tracker(cpu);

            // First move to the OxygenTank
            var oxygenTrack = BreadthFirstSearch(startTrack).First(t => t.Result == Result.OxygenTank).ClearMoves();

            // Then find the longest path from there
            return BreadthFirstSearch(oxygenTrack).Max(t=>t.StepCount);
        }

        public static IEnumerable<Tracker> BreadthFirstSearch(Tracker startTrack)
        {
            var tracks = new Queue<Tracker>(new [] {startTrack });

            while (tracks.Any())
            {
                var track = tracks.Dequeue();

                foreach (var nextTrack in track.Children())
                {
                    yield return nextTrack;
                    tracks.Enqueue(nextTrack);
                }
            }
        }

        public class Tracker
        {
            /// The BreadthFirst search requires us to back to a previously state when needed
            /// therefore we clone the CPU state in the Tracker
            private readonly IntCodeComputer _cpu;
            private int? _lastMove;
            public int StepCount { get; private set; }

            public Tracker(IntCodeComputer cpu)
            {
                _cpu = cpu;
            }

            public IEnumerable<Tracker> Children() => MoveOptions.Select(Step).Where(t=>t.Result!=Result.Wall);

            private IEnumerable<int> MoveOptions => _lastMove.HasValue ? Moves.Where(m => m != Reverse(_lastMove.Value)) 
                                                                       : Moves;

            private Tracker Step(int move)
            {
                var newState = _cpu.CloneState();
                newState.Run(move);
                return new Tracker(newState)
                {
                    _lastMove = move,
                    StepCount = StepCount+1,
                    Result = (Result)(int)newState.ReadOutput()
                };
            }

            public Tracker ClearMoves() => new Tracker(_cpu.CloneState());

            public Result Result { get; private set; }
        }
    }
}
