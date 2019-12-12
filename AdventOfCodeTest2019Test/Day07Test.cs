using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day07Test
    {
        [Fact]
        public void GetThrusterSignalTest()
        {
            Day07.GetThrusterSignal(new[] {3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0},
                new[] {4, 3, 2, 1, 0}).Should().Be(43210);
        }

        [Fact]
        public void FindMaxTest()
        {
            Day07.FindMax(new[] {3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0}).Should().Be(43210);

            Day07.FindMax(new[] {3,23,3,24,1002,24,10,24,1002,23,-1,23,
                101,5,23,23,1,24,23,23,4,23,99,0,0}).Should().Be(54321);

            Day07.FindMax(new[] {3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,
                1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0}).Should().Be(65210);

        }

        [Fact]
        public void GetThrusterSignalFeedbackTest()
        {
            Day07.GetThrusterSignalFeedback(new[] {3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,
                    27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5},
                new[] {9,8,7,6,5}).Result.Should().Be(139629729);
        }

        [Fact]
        public void FindMaxFeedbackTest()
        {
            Day07.FindMaxFeedBack(new[] {3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,
                    27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5}
                ).Should().Be(139629729);
        }
    }
}
