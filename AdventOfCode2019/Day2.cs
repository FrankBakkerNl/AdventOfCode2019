using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019
{
    class Day2
    {

        public static IEnumerable<int> ReadFile() =>
            File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day2.txt")
                .Split(',')
                .Select(s=>int.Parse(s.Trim()));

        public static BigInteger GetAnswer1()
        {
            var data = ReadFile().ToArray();

            data[1] = 12;
            data[2] = 2;

            var computer = new IntCodeComputer(data);
            computer.Run();
            return computer.Program[0];
        }

        public static int GetAnswer2()
        {
            int target = 19690720;

            for (int i = 60; i < 100; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    var compute = Compute(i, j);
                    if (compute == target)
                    {
                        return i * 100 + j;
                    }
                }
            } // 8226

            return 0;
        }


        public static BigInteger Compute(int noun, int verb)
        {
            var data = ReadFile().ToArray();

            data[1] = noun;
            data[2] = verb;

            var computer = new IntCodeComputer(data);
            computer.Run();
            return computer.Program[0];
        }
    }
}
