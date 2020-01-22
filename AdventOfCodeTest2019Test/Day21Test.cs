using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day21Test
    {
        [Fact]
        public void SimulatorTest()
        {
            var scriptLines = Day21.WalkScript
                .Split(Environment.NewLine)
                .Where(l=>!string.IsNullOrEmpty(l) && !l.StartsWith("//")).ToArray();

            new Day21.Simulator().Jumps(scriptLines, ".###").Should().BeTrue();
            new Day21.Simulator().Jumps(scriptLines, "..##").Should().BeTrue();
            new Day21.Simulator().Jumps(scriptLines, "...#").Should().BeTrue();
            new Day21.Simulator().Jumps(scriptLines, ".#.#").Should().BeTrue();

            new Day21.Simulator().Jumps(scriptLines, ".##.").Should().BeFalse();
            new Day21.Simulator().Jumps(scriptLines, "#.#.").Should().BeFalse();
            new Day21.Simulator().Jumps(scriptLines, "##..").Should().BeFalse();
            new Day21.Simulator().Jumps(scriptLines, "####").Should().BeFalse();

        }
    }
}
