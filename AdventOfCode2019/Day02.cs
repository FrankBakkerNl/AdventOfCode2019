using System;
using System.Linq;
using System.Numerics;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day02
    {
        [Result(2890696)]
        public static long GetAnswer1(int[] data)
        {
            data[1] = 12;
            data[2] = 2;

            var computer = new IntCodeComputer(data);
            computer.Run();
            return (long)computer.Program[0];
        }

        [Result(8226)]
        public static int GetAnswer2(int[] data)
        {
            int target = 19690720;
            var domain = Enumerable.Range(0,100);
            
            var options = domain.SelectMany(i => domain.Select(j=>(i, j)));

            var (noun, verb) = options.FirstOrDefault(o=>Compute(data, o.i, o.j) == target);

            return noun * 100 + verb;
        }

        public static BigInteger Compute(int[] data, int noun, int verb)
        {
            data[1] = noun;
            data[2] = verb;

            var computer = new IntCodeComputer(data);
            computer.Run();
            return computer.Program[0];
        }
    }
}
