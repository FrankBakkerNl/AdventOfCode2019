using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019.VM;
using static System.Linq.Enumerable;

namespace AdventOfCode2019
{
    public class Day19
    {
        public int GetAnswer1(string input)
        {
            bool IsAffected(int x, int y)
            {
                var cpu = new IntCodeComputer(input);
                cpu.Run(x,y);
                return cpu.ReadOutput() == 1;
            }

            return Range(0, 50).SelectMany(i => Range(0, 50).Select(j => (i, j))).Count(s => IsAffected(s.i, s.j));
        }
    }
}
