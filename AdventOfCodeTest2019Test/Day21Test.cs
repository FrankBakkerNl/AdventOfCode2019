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
        public void WalkScriptTest()
        {
            var scriptLines = Day21.WalkScript;

            new Day21.Simulator().Jumps(scriptLines, ".###").Should().BeTrue();
            new Day21.Simulator().Jumps(scriptLines, "..##").Should().BeTrue();
            new Day21.Simulator().Jumps(scriptLines, "...#").Should().BeTrue();
            new Day21.Simulator().Jumps(scriptLines, ".#.#").Should().BeTrue();

            new Day21.Simulator().Jumps(scriptLines, ".##.").Should().BeFalse();
            new Day21.Simulator().Jumps(scriptLines, "#.#.").Should().BeFalse();
            new Day21.Simulator().Jumps(scriptLines, "##..").Should().BeFalse();
            new Day21.Simulator().Jumps(scriptLines, "####").Should().BeFalse();

        }
        [Theory]
        [InlineData(".######")]

        public void RunScript_ShouldJump(string pattern)
        {
            new Day21.Simulator().Jumps(Day21.RunScript, pattern).Should().BeTrue();
        }

        [Theory]
        [InlineData("##.....")]
        public void RunScript_ShouldNotJump(string pattern)
        {
            new Day21.Simulator().Jumps(Day21.RunScript, pattern).Should().BeFalse();
        }

    }
}
