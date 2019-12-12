using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day06Test
    {
        [Fact]
        public void SampleShouldReturn42()
        {
            const string example = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L";
            Day06.GetTotalOrbits(example).Should().Be(42);
        }

        [Fact]
        public void Sample2OrbitTransfer()
        {
            const string example = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN";


            Day06.GetOrbitTransfers(example, "YOU", "SAN").Should().Be(4);
        }
    }
}
