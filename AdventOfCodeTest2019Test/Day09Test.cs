using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using AdventOfCode2019;
using AdventOfCode2019.VM;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day09Test
    {
        [Fact]
        public void EchoTest()
        {
            var program = new [] {109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99};
            var computer = new IntCodeComputer(program);

            //computer.Output.AsIEnumerableAsync().Result.Should().BeEquivalentTo(program.Select(i=>(BigInteger)i));
            computer.Run();
            computer.ReadAvailableOutput().Should().BeEquivalentTo(program.Select(i=>(BigInteger)i));
        }

        [Fact]
        public void LargeNumberOutputTest()
        {
            var program = new [] {1102,34915192,34915192,7,4,7,99,0};
            var computer = new IntCodeComputer(program);
            computer.Run();
            computer.ReadOutput().Should().BeGreaterThan(int.MaxValue);
        }

        [Fact]
        public void LargeNumberDataTest()
        {
            var program = new BigInteger[] {104,1125899906842624,99};
            var computer = new IntCodeComputer(program);
            computer.Run();
            computer.ReadOutput().Should().Be(1125899906842624);
        }
    }
}
