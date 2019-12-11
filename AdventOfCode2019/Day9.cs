using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019
{
    class Day9
    {
        private static string Input => File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day9.txt");

        public static BigInteger GetAnswer1()
        {
            var program = Input.Split(',').Select(BigInteger.Parse);
            var computer = new IntCodeComputer(program);
            computer.Input.Add(1);
            computer.Run();
            return computer.Output.Take();
        }

        public static BigInteger GetAnswer2()
        {
            var program = Input.Split(',').Select(BigInteger.Parse);
            var computer = new IntCodeComputer(program);
            computer.Input.Add(2);
            computer.Run();
            return computer.Output.Take();
        }

    }
}
