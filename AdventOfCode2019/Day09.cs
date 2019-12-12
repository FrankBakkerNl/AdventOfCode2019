using System.Linq;
using System.Numerics;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    class Day09
    {
        public static BigInteger GetAnswer1(string input)
        {
            var program = input.Split(',').Select(BigInteger.Parse);
            var computer = new IntCodeComputer(program);
            computer.WriteInput(1);
            computer.Run();
            return computer.ReadOutput();
        }

        public static BigInteger GetAnswer2(string input)
        {
            var program = input.Split(',').Select(BigInteger.Parse);
            var computer = new IntCodeComputer(program);
            computer.WriteInput(2);
            computer.Run();
            return computer.ReadOutput();
        }
    }
}
