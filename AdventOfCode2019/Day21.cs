using System;
using System.Linq;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day21
    {
        [Result(19347868)]
        public long GetAnswer1(int[] input) => GetHullDamage(input, WalkScript);

        [Result(1142479667)]
        public long GetAnswer2(int[] input) => GetHullDamage(input, RunScript);

        private static long GetHullDamage(int[] input, string[] script)
        {
            var cpu = new IntCodeComputer(input);
            var asc = new AsciiComputer(cpu);

            foreach (var line in script)
            {
                var r = asc.Run(line.Trim());
                //Console.Write(r);
            }

            //Console.WriteLine(cpu.ToString());
            return asc.ResultCode;
        }


        public static string[] WalkScript = PreProcessScript(@"
NOT A T
OR T J
NOT B T
OR T J
NOT C T
OR T J
AND D J
WALK");


        public static string[] RunScript = PreProcessScript(@"
NOT B T
OR T J -- if B is non ground
NOT C T
OR T J -- or C is non ground
AND D J -- and D is ground

NOT A T
AND E T
OR T J

-- only if E or G is ground
NOT E T
NOT T T
OR H T
AND T J


NOT A T
OR T J -- always if A is non ground
RUN
");

        public static string[] PreProcessScript(string script) =>
            script.Split(Environment.NewLine).Select(NormalizeInstruction)
                        .Where(l => !string.IsNullOrEmpty(l)).ToArray();

        static string NormalizeInstruction(string instruction)
        {
            var commentStart= instruction.IndexOf('-');
            return (commentStart >= 0 ? instruction.Substring(0, commentStart) : instruction).ToUpper().Trim();
        }


        public class Simulator
        {
            public bool Jumps(string[] program, string pattern)
            {
                var fields = pattern.PadRight(10, '.').Select(c => c == '#').ToArray();

                var finalState = program.TakeWhile(l => l != "WALK" && l != "RUN")
                    .Aggregate((T: false, J: false), (state, line) => 
                        ProcessLine(line, state.T, state.J, fields));

                return finalState.J;
            }

            (bool T, bool J) ProcessLine(string line, bool t, bool j, bool[] fields)
            {
                var parts = line.Split(' ');
                var arg1 = parts[1][0];

                var val1 = arg1 <= 'I' ? fields[arg1 - 'A'] : arg1 == 'T' ? t : j;

                ref var ref2 = ref parts[2][0] == 'T' ? ref t : ref j;

                ref2 = parts[0] switch
                {
                    "AND" => val1 && ref2,
                    "OR"  => val1 || ref2,
                    "NOT" => !val1,
                    _ => ref2,
                };
                return (t, j);
            }
        }
    }
}
