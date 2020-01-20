using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day03
    {
        [Result(721)]
        public static int GetAnswer1(string[] lines) => FindFirstIntersection(lines[0], lines[1]);

        [Result(7388)]
        public static int GetAnswer2(string[] lines) => FindClosestIntersection(lines[0], lines[1]);

        public static int FindFirstIntersection(string firstWire, string secondWire)
        {
            var firstVisits = GetVisitsWithDistance(firstWire).Select(c=>c.cell);
            var secondVisits = GetVisitsWithDistance(secondWire).Select(c=>c.cell);
            var intersections = firstVisits.Intersect(secondVisits);
            return intersections.Select(TaxiDistance).Min();
        }


        public static int FindClosestIntersection(string firstWire, string secondWire)
        {
            var firstVisits = GetVisitsWithDistance(firstWire);
            var secondVisits = GetVisitsWithDistance(secondWire);

            return firstVisits.Join(secondVisits, f => f.cell, s => s.cell, (f, s) => f.distance + s.distance).Min();
        }

        public static int TaxiDistance((int x, int y) point) => Math.Abs(point.x) + Math.Abs(point.y);

        public static IEnumerable<((int, int) cell, int distance)> GetVisitsWithDistance(string route)
        {
            var paths = route.Split(',').Select(ParsePath);

            int x = 0; 
            int y = 0;
            int distance = 0;
            foreach (var (direction, length) in paths)
            {
                var (xd, yd) = MapDirection(direction);
                for (int i = 0; i < length; i++)
                {
                    distance++;
                    x += xd;
                    y += yd;
                    yield return ((x, y), distance);
                }
            }
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
