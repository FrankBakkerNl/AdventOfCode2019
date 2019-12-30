using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    [Test]
    public class Day17
    {
        public static int GetAnswer1(string input)
        {
            var cpu = new IntCodeComputer(input);
            cpu.Run();
            int row = 0;
            int col = 0;
            HashSet<(int, int)> scaffolds = new HashSet<(int, int)>();
            while (cpu.IsOutputAvailable)
            {
                var value = (char) (int) cpu.ReadOutput();
                if (value == '#')
                {
                    scaffolds.Add((row, col));
                }

                col++;
                if (value == 10)
                {
                    row++;
                    col = 0;
                }
                Console.Write(value);
            }



            return FindIntersections(scaffolds);
        }

        private static int FindIntersections(HashSet<(int, int)> scaffolds)
        {
            int intersections = 0;
            foreach (var (row, col) in scaffolds)
            {
                if (scaffolds.IsSupersetOf(new[]
                {
                    (row - 1, col),
                    (row + 1, col),
                    (row , col -1),
                    (row , col +1),
                })) intersections+= row*col;
            }

            return intersections;
        }
    }
}
