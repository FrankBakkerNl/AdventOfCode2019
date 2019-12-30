﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using AdventOfCode2019.VM;
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

            Run("1,0,0,0,99").Should().Be("2,0,0,0,99");
            Run("2,3,0,3,99").Should().Be("2,3,0,6,99");
            Run("2,4,4,5,99,0").Should().Be("2,4,4,5,99,9801");
            Run("1,1,1,4,99,5,6,0,99").Should().Be("30,1,1,4,2,5,6,0,99");
        }



        [Fact]
        public void OutputInputTest()
        {
            var intCodeComputer = new IntCodeComputer("3,0,4,0,99");

            intCodeComputer.Run(42);
            intCodeComputer.ReadOutput().Should().Be(42);;
        }


        [Fact]
        public void ParamModeTest()
        {
            Run("1002,4,3,4,33").Should().Be("1002,4,3,4,99");
        }

        [Fact]
        public void JumpTests()
        {
            // 3,9,8,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
            RunIO("3,9,8,9,10,9,4,9,99,-1, 8", 7).Should().Be(0);
            RunIO("3,9,8,9,10,9,4,9,99,-1, 8", 8).Should().Be(1);
            RunIO("3,9,8,9,10,9,4,9,99,-1, 8", 9).Should().Be(0);

            // 3,9,7,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is less than 8; output 1 (if it is) or 0 (if it is not).
            RunIO("3,9,7,9,10,9,4,9,99,-1,8", 7).Should().Be(1);
            RunIO("3,9,7,9,10,9,4,9,99,-1,8", 8).Should().Be(0);
            RunIO("3,9,7,9,10,9,4,9,99,-1,8", 9).Should().Be(0);

            // 3,3,1108,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
            RunIO("3,3,1108,-1,8,3,4,3,99", 7).Should().Be(0);
            RunIO("3,3,1108,-1,8,3,4,3,99", 8).Should().Be(1);
            RunIO("3,3,1108,-1,8,3,4,3,99", 9).Should().Be(0);

            // 3,3,1107,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is less than 8; output 1 (if it is) or 0 (if it is not).
            RunIO("3,3,1107,-1,8,3,4,3,99", 7).Should().Be(1);
            RunIO("3,3,1107,-1,8,3,4,3,99", 8).Should().Be(0);
            RunIO("3,3,1107,-1,8,3,4,3,99", 9).Should().Be(0);

            RunIO("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0).Should().Be(0);
            RunIO("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 5).Should().Be(1);

            RunIO("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 0).Should().Be(0);
            RunIO("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 5).Should().Be(1);
        }


        [Fact]
        public void SelfTest()
        {
            var program = "3,225,1,225,6,6,1100,1,238,225,104,0,101,67,166,224,1001,224,-110,224,4,224,102,8,223,223,1001,224,4,224,1,224,223,223,2,62,66,224,101,-406,224,224,4,224,102,8,223,223,101,3,224,224,1,224,223,223,1101,76,51,225,1101,51,29,225,1102,57,14,225,1102,64,48,224,1001,224,-3072,224,4,224,102,8,223,223,1001,224,1,224,1,224,223,223,1001,217,90,224,1001,224,-101,224,4,224,1002,223,8,223,1001,224,2,224,1,223,224,223,1101,57,55,224,1001,224,-112,224,4,224,102,8,223,223,1001,224,7,224,1,223,224,223,1102,5,62,225,1102,49,68,225,102,40,140,224,101,-2720,224,224,4,224,1002,223,8,223,1001,224,4,224,1,223,224,223,1101,92,43,225,1101,93,21,225,1002,170,31,224,101,-651,224,224,4,224,102,8,223,223,101,4,224,224,1,223,224,223,1,136,57,224,1001,224,-138,224,4,224,102,8,223,223,101,2,224,224,1,223,224,223,1102,11,85,225,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,1107,226,226,224,102,2,223,223,1006,224,329,1001,223,1,223,1007,226,677,224,1002,223,2,223,1005,224,344,101,1,223,223,108,677,677,224,1002,223,2,223,1006,224,359,101,1,223,223,1008,226,226,224,1002,223,2,223,1005,224,374,1001,223,1,223,108,677,226,224,1002,223,2,223,1006,224,389,101,1,223,223,7,226,226,224,102,2,223,223,1006,224,404,101,1,223,223,7,677,226,224,1002,223,2,223,1005,224,419,101,1,223,223,107,226,226,224,102,2,223,223,1006,224,434,1001,223,1,223,1008,677,677,224,1002,223,2,223,1005,224,449,101,1,223,223,108,226,226,224,102,2,223,223,1005,224,464,1001,223,1,223,1108,226,677,224,1002,223,2,223,1005,224,479,1001,223,1,223,8,677,226,224,102,2,223,223,1006,224,494,1001,223,1,223,1108,677,677,224,102,2,223,223,1006,224,509,1001,223,1,223,1007,226,226,224,1002,223,2,223,1005,224,524,1001,223,1,223,7,226,677,224,1002,223,2,223,1005,224,539,1001,223,1,223,8,677,677,224,102,2,223,223,1005,224,554,1001,223,1,223,107,226,677,224,1002,223,2,223,1006,224,569,101,1,223,223,1107,226,677,224,102,2,223,223,1005,224,584,1001,223,1,223,1108,677,226,224,102,2,223,223,1006,224,599,1001,223,1,223,1008,677,226,224,102,2,223,223,1006,224,614,101,1,223,223,107,677,677,224,102,2,223,223,1006,224,629,1001,223,1,223,1107,677,226,224,1002,223,2,223,1005,224,644,101,1,223,223,8,226,677,224,102,2,223,223,1005,224,659,1001,223,1,223,1007,677,677,224,102,2,223,223,1005,224,674,1001,223,1,223,4,223,99,226";;
            var computer = new IntCodeComputer(program);
            computer.Run(1);
            computer.ReadAvailableOutput()[..^1].Should().AllBeEquivalentTo(BigInteger.Zero);
        }

        [Fact]
        public async void DoneAfterInputTest()
        {
            var program = "1,255, 255, 300, 3, 1000, 3, 1001, 99";;
            var computer = new IntCodeComputer(program);
            computer.Run(3, 5);
            computer.Finished.Should().BeTrue();
        }

        private static string Run(string program)
        {
            var computer = new IntCodeComputer(program );
            computer.Run();
            return string.Join(",", computer.Program.Dump.Select(s=>s.ToString(CultureInfo.InvariantCulture)));
        }

        private static BigInteger RunIO(string program, int input)
        {
            var computer = new IntCodeComputer(program );
            computer.Run(input);
            return computer.ReadOutput();
        }
    }
}
