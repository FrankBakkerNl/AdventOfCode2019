using System.Linq;
using System.Numerics;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day09
    {
        [Result(2457252183)]
        public static long GetAnswer1(string input)
        {
            var program = input.Split(',').Select(BigInteger.Parse);
            var computer = new IntCodeComputer(program);
            computer.Run(1);
            return (long)computer.ReadOutput();
        }

        [Result(70634)]
        public static long GetAnswer2(string input)
        {
            var program = input.Split(',').Select(BigInteger.Parse);
            var computer = new IntCodeComputer(program);
            computer.Run(2);
            return (long)computer.ReadOutput();
        }
    }
}
