using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day9Test
    {
        [Fact]
        public void EchoTest()
        {
            var program = new [] {109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99};
            var computer = new IntCodeComputer(program);
            computer.Run();
            computer.Output.Should().BeEquivalentTo(program.Select(i=>(BigInteger)i));
        }

        [Fact]
        public void LargeNumberOutputTest()
        {
            var program = new [] {1102,34915192,34915192,7,4,7,99,0};
            var computer = new IntCodeComputer(program);
            computer.Run();
            computer.Output.Take().Should().BeGreaterThan(int.MaxValue);
        }

        [Fact]
        public void LargeNumberDataTest()
        {
            var program = new BigInteger[] {104,1125899906842624,99};
            var computer = new IntCodeComputer(program);
            computer.Run();
            computer.Output.Take().Should().Be(1125899906842624);
        }
    }
}
