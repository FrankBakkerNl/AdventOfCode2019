using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;
using static AdventOfCode2019.Day24;

namespace AdventOfCodeTest2019Test
{
    public class Day24Test
    {

        private string[] TestInput = new string[]
        {
            "....#",
            "#..#.",
            "#..##",
            "..#..",
            "#...."
        };

        [Fact]
        public void TestNextGeneration()
        {
            var start = new Day24.Field(TestInput);
            var gen1 = start.NextGeneration();
            var expected1 = new[]
            {
                "#..#.",
                "####.",
                "###.#",
                "##.##",
                ".##..",
            };

            gen1.Should().Be(new Field(expected1));
            var gen2 = gen1.NextGeneration();
            var expected2 = new Field(new[]
            {
                "#####",
                "....#",
                "....#",
                "...#.",
                "#.###",
            });
            gen2.Should().Be(expected2);

            var gen3 = gen2.NextGeneration();
            var gen4 = gen3.NextGeneration();

            var expected4 = new Field(new[]
            {
                "####.",
                "....#",
                "##..#",
                ".....",
                "##...",
            });
            gen4.Should().Be(expected4);


        }

        [Fact]
        public void FindFirstRepeatTest()
        {
            var start = new Field(TestInput);
            var firstRepeat = FindFirstRepeat(start);

            var expected = new[]
            {
                ".....",
                ".....",
                ".....",
                "#....",
                ".#...",
            };

            firstRepeat.Should().Be(new Field(expected));
            firstRepeat.Bits.Should().Be(2129920);
        }

        [Fact]
        public void FindLevels()
        {
            var result = Day24.FindLevels(new Field(TestInput), 10);
            result.Length.Should().Be(21);
        }

        [Fact]
        public void NextGenerationRecursiveTest()
        {

            var outer = new Field(new[]
            {
                ".....",
                "..#..",
                ".....",
                ".....",
                ".....",
            });

            var current = new Field(new[]
            {
                ".....",
                ".....",
                ".....",
                ".....",
                ".....",
            });

            var inner = new Field(new[]
            {
                "..##.",
                ".....",
                ".....",
                ".....",
                ".....",
            });


            var next = current.NextGenerationRecursive(inner.Bits, outer.Bits);

            var expected = new Field(new[]
            {
                "#####",
                "..#..",
                ".....",
                ".....",
                ".....",
            });
            next.Should().Be(expected);
        }

        [Fact]
        public void GetCountAfterGenerationsTest()
        {
            Day24.GetCountAfterGenerations(new Field(TestInput), 10).Should().Be(99);
        }

    }
}
