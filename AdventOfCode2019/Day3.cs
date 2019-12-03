using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace AdventOfCode2019
{
    class Day3
    {
        public static int GetAnswer1()
        {

            var lines = File.ReadAllLines(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day3.txt");

            return FindFirstIntersection(lines[0], lines[1]);
                

        }

        public static int FindFirstIntersection(string firstWire, string secondWire)
        {
            var firstVisits = GetVisits(firstWire);
            var secondVisits = GetVisits(secondWire);
            var intersections = firstVisits.Intersect(secondVisits);
            return intersections.Select(Distance).Min();
        }

        public static int Distance((int, int) point) => Math.Abs(point.Item1) + Math.Abs(point.Item2);

        public static HashSet<(int, int)> GetVisits(string route)
        {
            var paths = route.Split(',').Select(ParsePath);

            HashSet<(int,int)> visited = new HashSet<(int, int)>();
            int x = 0; 
            int y = 0;
            foreach (var (direction, length) in paths)
            {
                var (xd, yd) = MapDirection(direction);
                for (int i = 0; i < length; i++)
                {
                    x += xd;
                    y += yd;
                    visited.Add((x, y));
                }
            }
            return visited;
        }


        private static (char, int) ParsePath(string s) => (s[0], int.Parse(s.Substring(1)));


        public static (int, int) MapDirection(char direction) =>
            direction switch
            {
                'U' => (1, 0),
                'D' => (-1, 0),
                'R' => (0, 1),
                'L' => (0, -1),
                _ => throw new InvalidOperationException($"Direction {direction} unknown")
            };
    }
}
