using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019;
using AdventOfCode2019.Day18Helpers;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day18Test
    {
        [Fact]
        public void FindAllKeys()
        {
            var input = new[]
            {
                "########################",
                "#f.D.E.e.C.b.A.@.a.B.c.#",
                "######################.#",
                "#d.....................#",
                "########################",
            };

            var steps  = Day18.FindAllKeys(input);
            steps.Should().Be(86);
        }

        [Fact]
        public void FindAllKeys2()
        {
            var input = new[]
            {
                "...#####################",
                ".@..............ac.GI.b#",
                "..#d#e#f################",
                "###A#B#C################",
                "###g#h#i################",
                "########################",
            };

            var steps  = Day18.FindAllKeys(input);
            steps.Should().Be(81);
        }

        [Fact]
        public void FindAllKeys3()
        {
            var input = new[]
            {
                "#################",
                "#i.G..c...e..H.p#",
                "########.########",
                "#j.A..b...f..D.o#",
                "########@########",
                "#k.E..a...g..B.n#",
                "########.########",
                "#l.F..d...h..C.m#",
                "#################",
            };

            var steps  = Day18.FindAllKeys(input);
            steps.Should().Be(136);
        }


        //[Fact]
        //public void TestTracker()
        //{
        //    var input = new[]
        //    {
        //        "########################",
        //        "#f.D.E.e.C.b.A.@.a.B.c.#",
        //        "######################.#",
        //        "#d.....................#",
        //        "########################",
        //    };

        //    var maze = new Maze(input);
        //    var tracker = new Day18.MazeRunner(maze, maze.StartPosition, 0);

        //    var l1 = tracker.Children().ToArray();
        //    var l21 = l1[0].Children();
        //    var l22 = l1[1].Children();

        //    var l2 = l1.SelectMany(c => c.Children()).ToList();
        //    var l3 = l2.SelectMany(c => c.Children()).ToList();

        //}
    }
}
