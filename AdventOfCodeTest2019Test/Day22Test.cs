using System.Numerics;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;
using static AdventOfCode2019.Day22;

namespace AdventOfCodeTest2019Test
{
    public class Day22Test
    {
        [Fact]
        public void ParseInstructionTest()
        {
            ParseInstruction("deal into new stack").Item1
                .Should().Be(Operation.DealIntoNewStack);
            ParseInstruction("cut -2")
                .Should().Be((Operation.Cut, -2));
            ParseInstruction("deal with increment 7")
                .Should().Be((Operation.DealWithIncrement, 7));
        }

        [Fact]
        public void Shuffle1Test()
        {
            var input = new[]
            {
                "deal with increment 7",
                "deal into new stack",
                "deal into new stack",
            };
            ShuffleDeck(10, input).Should().Be("0 3 6 9 2 5 8 1 4 7");
            ReverseShuffleDeck(10, input).Should().Be("0 3 6 9 2 5 8 1 4 7");
        }

        [Fact]
        public void Shuffle2Test()
        {
            var input = new[]
            {
                "cut 6",
                "deal with increment 7",
                "deal into new stack", 
            };
            ShuffleDeck(10, input).Should().Be("3 0 7 4 1 8 5 2 9 6");
            ReverseShuffleDeck(10, input).Should().Be("3 0 7 4 1 8 5 2 9 6");
        }
        [Fact]
        public void Shuffle3Test()
        {
            var input = new[]
            {
                "deal with increment 7",
                "deal with increment 9",
                "cut -2",
            }; 
            ShuffleDeck(10, input).Should().Be("6 3 0 7 4 1 8 5 2 9");
            ReverseShuffleDeck(10, input).Should().Be("6 3 0 7 4 1 8 5 2 9");
        }

        [Fact]
        public void Shuffle4Test()
        {
            var input = new[]
            {
                "deal into new stack",
                "cut -2",
                "deal with increment 7",
                "cut 8",
                "cut -4",
                "deal with increment 7",
                "cut 3",
                "deal with increment 9",
                "deal with increment 3",
                "cut -1",
            };
            ShuffleDeck(10, input).Should().Be("9 2 5 8 1 4 7 0 3 6");
            ReverseShuffleDeck(10, input).Should().Be("9 2 5 8 1 4 7 0 3 6");
        }

        [Fact]
        public void LoopTest()
        {
            var deckSize = (BigInteger) 119_315_717_514_047;

            var input = new[]
            {
                "deal into new stack",
                "cut -2",
                "deal with increment 7",
                "cut 8",
                "cut -4",
                "deal with increment 7",
                "cut 3",
                "deal with increment 9",
                "deal with increment 3",
                "cut -1",
            };

            var loopCount = 10211;
            var initialPosition = 2019;
            BigInteger lastPos = initialPosition;
            for (int i = 0; i < loopCount; i++)
            {
                lastPos = FindNewPosition(input, deckSize, lastPos);
            }

            FindOriginalPosition(input, deckSize, lastPos, loopCount).Should().Be(initialPosition);
        }

        [Theory]
        [InlineData("deal into new stack",   "9 8 7 6 5 4 3 2 1 0")]
        [InlineData("cut 3",                 "3 4 5 6 7 8 9 0 1 2")]
        [InlineData("cut -4",                "6 7 8 9 0 1 2 3 4 5")]
        [InlineData("deal with increment 3", "0 7 4 1 8 5 2 9 6 3")]
        public void BasicShuffles(string instruction, string result)
        {
            ShuffleDeck(10, instruction).Should().Be(result);
            ReverseShuffleDeck(10, instruction).Should().Be(result);
        }
       
        private string ShuffleDeck(int deckSize, params string[] instructions)
        {
            var newDeck = new long[deckSize];
            for (int i = 0; i < deckSize; i++)
            {
                var position = Day22.FindNewPosition(instructions, deckSize, i);
                newDeck[(int)position] = i;
            }
            return string.Join(" ", newDeck);
        }

        private string ReverseShuffleDeck(int deckSize, params string[] instructions)
        {
            var newDeck = new long[deckSize];
            for (int i = 0; i < deckSize; i++)
            {
                var original = (int)Day22.FindOriginalPosition(instructions, deckSize, i, 1);
                newDeck[i] = original;
            }
            return string.Join(" ", newDeck);
        }
    }
}
