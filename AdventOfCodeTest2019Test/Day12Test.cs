using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day12Test
    {
        [Fact]
        public void TestParse()
        {
            var input = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>".Split(Environment.NewLine);

            Day12.Parse(input).Should().BeEquivalentTo(new[]
            {
                new Vector(-1, 0, 2),
                new Vector(2, -10, -7),
                new Vector(4, -8, 8),
                new Vector(3, 5, -1),
            });
        }

        [Fact]
        public void GetNewVelocityTest()
        {
            var current = new Vector(0,0,0);
            var other = new Vector(3,0,-2);
            Day12.GetForce(current, other).Should()
                .BeEquivalentTo(new Vector(1, 0, -1));
        }

        [Fact]
        public void RunTest()
        {
            var input = @"
<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>".Split(Environment.NewLine).Where(s=>!string.IsNullOrEmpty(s)).ToArray();

            var state = Day12.LoadInitialState(input);
            var result = (IEnumerable<(Vector p, Vector v)>) Day12.Step(state).ToList();

            var expected = @"
<x= 2, y=-1, z= 1>
<x= 3, y=-7, z=-4>
<x= 1, y=-7, z= 5>
<x= 2, y= 2, z= 0>".Split(Environment.NewLine).Where(s=>!string.IsNullOrEmpty(s)).ToArray();
            var exp = Day12.Parse(expected);

            result.Select(r=>r.p) .Should().BeEquivalentTo(exp);

        }

        [Fact]
        public void EnergyTest()
        {
            var input = @"
<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>".Split(Environment.NewLine).Where(s=>!string.IsNullOrEmpty(s)).ToArray();
            Day12.GetEnergyAfterSteps(input, 100).Should().Be(1940);

        }
    }
}
