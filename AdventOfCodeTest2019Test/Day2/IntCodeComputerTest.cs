using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test.Day2
{

    public class IntCodeComputerTest
    {
        [Fact]
        public void Program1Spec()
        {
            //1,0,0,0,99 becomes 2,0,0,0,99 (1 + 1 = 2).
            //2,3,0,3,99 becomes 2,3,0,6,99 (3 * 2 = 6).
            //2,4,4,5,99,0 becomes 2,4,4,5,99,9801 (99 * 99 = 9801).
            //1,1,1,4,99,5,6,0,99 becomes 30,1,1,4,2,5,6,0,99

            Run("1,0,0,0,99").Should().BeEquivalentTo("2,0,0,0,99");
            Run("2,3,0,3,99").Should().BeEquivalentTo("2,3,0,6,99");
            Run("2,4,4,5,99,0").Should().BeEquivalentTo("2,4,4,5,99,9801");
            Run("1,1,1,4,99,5,6,0,99").Should().BeEquivalentTo("30,1,1,4,2,5,6,0,99");
        }

        private static string Run(string input)
        {
            var program = input.Split(',').Select(s=>int.Parse(s)).ToArray();
            var computer = new IntCodeComputer(program );
            computer.Run();
            return string.Join(",", computer._program.Select(s=>s.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
