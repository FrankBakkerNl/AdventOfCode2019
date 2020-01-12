using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    public class Day20Test
    {
        string[] TestMaze1 = new string[]
        {
            "         A           ",
            "         A           ",
            "  #######.#########  ",
            "  #######.........#  ",
            "  #######.#######.#  ",
            "  #######.#######.#  ",
            "  #######.#######.#  ",
            "  #####  B    ###.#  ",
            "BC...##  C    ###.#  ",
            "  ##.##       ###.#  ",
            "  ##...DE  F  ###.#  ",
            "  #####    G  ###.#  ",
            "  #########.#####.#  ",
            "DE..#######...###.#  ",
            "  #.#########.###.#  ",
            "FG..#########.....#  ",
            "  ###########.#####  ",
            "             Z       ",
            "             Z       ",
        };


        string[] TestMaze2 = new []
        {
"                   A               ",
"                   A               ",
"  #################.#############  ",
"  #.#...#...................#.#.#  ",
"  #.#.#.###.###.###.#########.#.#  ",
"  #.#.#.......#...#.....#.#.#...#  ",
"  #.#########.###.#####.#.#.###.#  ",
"  #.............#.#.....#.......#  ",
"  ###.###########.###.#####.#.#.#  ",
"  #.....#        A   C    #.#.#.#  ",
"  #######        S   P    #####.#  ",
"  #.#...#                 #......VT",
"  #.#.#.#                 #.#####  ",
"  #...#.#               YN....#.#  ",
"  #.###.#                 #####.#  ",
"DI....#.#                 #.....#  ",
"  #####.#                 #.###.#  ",
"ZZ......#               QG....#..AS",
"  ###.###                 #######  ",
"JO..#.#.#                 #.....#  ",
"  #.#.#.#                 ###.#.#  ",
"  #...#..DI             BU....#..LF",
"  #####.#                 #.#####  ",
"YN......#               VT..#....QG",
"  #.###.#                 #.###.#  ",
"  #.#...#                 #.....#  ",
"  ###.###    J L     J    #.#.###  ",
"  #.....#    O F     P    #.#...#  ",
"  #.###.#####.#.#####.#####.###.#  ",
"  #...#.#.#...#.....#.....#.#...#  ",
"  #.#####.###.###.#.#.#########.#  ",
"  #...#.#.....#...#.#.#.#.....#.#  ",
"  #.###.#####.###.###.#.#.#######  ",
"  #.#.........#...#.............#  ",
"  #########.###.###.#############  ",
"           B   J   C               ",
"           U   P   P               ",
}
            ;
    
        [Fact]
        public void FindWithTest()
        {
            Day20.L1Maze.FindWith(TestMaze1).Should().Be(5);
        }

        [Fact]
        public void FindPortalsTest()
        {
            var portals = Day20.L1Maze.FindPortals(TestMaze1);
        }

        [Fact]
        public void ParseTestMaze1()
        {
            var maze = new Day20.L1Maze(TestMaze1);
            maze.PortalMap.Count.Should().Be(6);
            maze.Start.Should().Be((9, 2));
            maze.End.Should().Be((13, 16));
            maze.PortalMap[(9, 7)].Should().Be((2,8));
        }


        [Fact]
        public void ParseTestmaze2()
        {
            var maze = new Day20.L1Maze(TestMaze2);
            maze.PortalMap.Count.Should().Be(20);
            maze.Start.Should().Be((19, 2));
            maze.End.Should().Be((1, 17));
        }

        [Fact]
        public void FindShortestMaze1()
        {
            Day20.FindShortestRoute(TestMaze1).Should().Be(23);
        }


        [Fact]
        public void FindShortestMaze2()
        {
            Day20.FindShortestRoute(TestMaze2).Should().Be(58);
        }
    }
}
