using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day10Test
    {
        [Fact]
        public void VectorAlign1()
        {
            Day10.NormalizeVector((1, 1)).Should().Be((1, 1));
            Day10.NormalizeVector((6, 9)).Should().Be((2, 3));
            Day10.NormalizeVector((5, 5)).Should().Be((1, 1));
            Day10.NormalizeVector((0, 5)).Should().Be((0, 1));
            Day10.NormalizeVector((0, -5)).Should().Be((0, -1));
        }

        [Fact]
        public void Test2()
        {
            var input = @"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####";

            Day10.FindMaxVisible(input).Should().Be(33);

        }

        [Fact]
        public void LargeTest()
        {
            var input = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

            Day10.FindMaxVisible(input).Should().Be(210);
        }

        [Fact]
        public void SortVecorstest()
        {
//            var input = new[] {(0,-2),(1,-1),(-2,-2),(3, 1), (3, 3), (1, 2)};
            var input = new[] {
                ( 0,-2), ( 1,-2),(-2,-2),
                (-2, 1), (-2, 0),(-2,-1), (-2, -2),
                (-1,-2), ( 0,-2), (1,-2), (2,-2), 
                ( 2,-1), ( 2, 0), (2,1),(2,2),
                ( 1, 2), (0,2),(-1,2),(-2,2),
                (-2, 1), (-2, 0), (-2,-1), (-2, -2)
            };
            Day10.SortVectors(input.Reverse()).Should().BeEquivalentTo(input);
        }


        [Fact]
        public void GetVaporizeOrderTestBasic()
        {
            var input = @"#####
#...#
#.#.#
#...#
#####";
            var order = Day10.GetVaporizeOrder(input).ToList();
            var display = ShowOrder(new HashSet<(int, int)>(order), 5, 5);
        }


        private string ShowOrder(IEnumerable<(int, int)> order, int with, int hight)
        {
            var lookupOrder = order.Select((c, i) => (c, i)).ToLookup(t => t.c, t => (int?)t.i);
            var sb = new StringBuilder((with+2) * hight);
            for (int i = 0; i < hight; i++)
            {
                for (int j = 0; j < with; j++)
                {
                    var number = lookupOrder[(j, i)].FirstOrDefault();
                    sb.Append(number?.ToString("000 ") ?? "  . ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        [Fact]
        public void GetVaporizeOrderTest()
        {
            var input = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";
            var order = Day10.GetVaporizeOrder(input).ToList();
            var display = ShowOrder(order, 20, 20);
            //The 1st asteroid to be vaporized is at 11,12.
            //The 2nd asteroid to be vaporized is at 12,1.
            //The 3rd asteroid to be vaporized is at 12,2.
            //The 10th asteroid to be vaporized is at 12,8.
            //The 20th asteroid to be vaporized is at 16,0.
            //The 50th asteroid to be vaporized is at 16,9.
            //The 100th asteroid to be vaporized is at 10,16.
            //The 199th asteroid to be vaporized is at 9,6.
            //The 200th asteroid to be vaporized is at 8,2.
            //The 201st asteroid to be vaporized is at 10,9.
            //The 299th and final asteroid to be vaporized is at 11,1.
            order[0].Should().Be((11, 12));
            order[1].Should().Be((12,1));
            order[2].Should().Be((12,2));
            order[9].Should().Be((12,8));
            order[19].Should().Be((16,0));
            order[49].Should().Be((16,9));
            order[19].Should().Be((16,0));
            order[99].Should().Be((10,16));
            order[198].Should().Be((9,6));
            order[199].Should().Be((8,2));
            order[200].Should().Be((10,9));
            order[298].Should().Be((11,1));

        }

    }
}
