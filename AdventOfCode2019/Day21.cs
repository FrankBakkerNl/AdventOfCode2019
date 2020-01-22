using System;
using System.Linq;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    
    [Test]
    public class Day21
    {
        [Result(19347868)]
        public long GetAnswer1(int[] input) => GetHullDamage(input, PreProcessScript(WalkScript));

        private static long GetHullDamage(int[] input, string[] script)
        {
            var asc = new AsciiComputer(new IntCodeComputer(input));

            foreach (var line in script)
            {
                asc.Run(line.Trim());
            }

            return asc.ResultCode;
        }


        public const string WalkScript = @"
NOT A T
OR T J
NOT B T
OR T J
NOT C T
OR T J
AND D J
WALK";


        private static string[] PreProcessScript(string script) =>
            script.Split(Environment.NewLine)
                        .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("//")).ToArray();


        public class Simulator
        {

            public bool Jumps(string[] program, string pattern)
            {
                var fields = pattern.Select(c => c == '#').ToArray();

                var state = (T: false, J: false);
                foreach (var line in program.TakeWhile(l => l != "WALK"))
                {
                    state = ProcessLine(line, state.T, state.J, fields);
                }

                return state.J;
            }

            (bool T, bool J) ProcessLine(string line, bool T, bool J, bool[] fields)
            {
                var parts = line.Split(' ');

                bool x = parts[1] switch
                {
                    "A" => fields[0],
                    "B" => fields[1],
                    "C" => fields[2],
                    "D" => fields[3],
                    "T" => T,
                    "J" => J
                };
                bool y = parts[2] switch { "T" => T, "J" => J };

                var result = parts[0] switch
                {
                    "AND" => x && y,
                    "OR" => x || y,
                    "NOT" => !x,
                };

                if (parts[2] == "T")
                {
                    T = result;
                }
                else
                {
                    J = result;
                }

                return (T, J);

            }
        }
    }
}
