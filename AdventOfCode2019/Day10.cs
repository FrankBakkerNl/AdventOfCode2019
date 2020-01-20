using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static System.Math;

namespace AdventOfCode2019
{
    public class Day10
    {
        [Result(276)]
        public static int GetAnswer1(string input) => FindMaxVisible(input);

        [Result(1321)]
        public static int GetAnswer2(string input)
        {
            var (x, y) = GetVaporizeOrder(input).ElementAt(199);
            return x * 100 + y;
        }

        public static int FindMaxVisible(string input)
        {
            var asteroidLocations = ParseAsteroidLocations(input).ToImmutableArray();

            int NumberVisible((int x, int y) current) => asteroidLocations.GroupBy(other => GetNormalizedVector(other, current)).Count() - 1;

            return asteroidLocations.Max(NumberVisible);
        }

        public static IEnumerable<(int x, int y)> GetVaporizeOrder(string input)
        {
            var asteroidLocations = ParseAsteroidLocations(input).ToImmutableArray();

            int NumberVisible((int x, int y) current) => asteroidLocations.GroupBy(other => GetNormalizedVector(other, current)).Count() - 1;

            var laserPosition = asteroidLocations.MaxBy(NumberVisible);

            var targets = asteroidLocations.Remove(laserPosition);

            var sortedFireVectors = targets
                .GroupBy(target => GetNormalizedVector(target, laserPosition))
                .OrderBy(g => Angle(g.Key))
                .Select(queue => ImmutableQueue.Create(queue.OrderBy(t=>Distance(t, laserPosition)).ToArray()));

            return Shoot(sortedFireVectors);
        }

        private static IEnumerable<(int, int)> Shoot(IEnumerable<ImmutableQueue<(int,int)>> queues)
        {
            // Remove one item from each queue, we need the item and the remaining queue
            var result = queues.Select(q => (newQueue: q.Dequeue(out var target), target)).ToImmutableArray();

            return result.Any() 
                ? result.Select(r => r.target).Union(Shoot(result.Select(r => r.newQueue).Where(q => !q.IsEmpty))) 
                : Enumerable.Empty<(int, int)>();
        }

        public static int Distance((int x, int y) first, (int x, int y) second)
        {
            var (a, b) = GetVector(first, second);
            return Abs(a * a) + Abs(b * b);
        }

        private static double Angle((int x, int y) v) => -Atan2(v.x, v.y);

        public static IEnumerable<(int, int)> SortVectors(IEnumerable<(int x, int y)> vectors) 
            => vectors.OrderBy(v => Atan2(v.x,-v.y));

        public static IEnumerable<(int x, int y)> ParseAsteroidLocations(string input) 
            => input.Split(Environment.NewLine)
                .SelectMany((line, y) => line.Select((character, x) => (character, position:(x, y) )))
                .Where(t => t.character == '#').Select(t => t.position);

        private static (int x, int y) GetNormalizedVector((int x, int y) other, (int x, int y) current) 
            => NormalizeVector(GetVector(other, current));

        private static (int, int) GetVector((int x, int y) c1, (int x, int y) c2) => (c1.x - c2.x, c1.y - c2.y);

        public static (int x, int y)NormalizeVector((int x, int y) v)
        {
            if (v == (0, 0)) return (0, 0);

            var gcd = Gcd(Abs(v.x), Abs(v.y));
            return (v.x / gcd, v.y / gcd);
        }

        private static int Gcd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            return a == 0 ? b : a;
        }
    }

    static class Day10Extensions
    {
        public static T MaxBy<T>(this IEnumerable<T> input, Func<T, int> selector)
        {
            T result = default;
            var maxValue = int.MinValue;
            foreach (var i in input)
            {
                var currentValue = selector(i);
                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                    result = i;
                }
            }
            return result;
        }
    }
}
