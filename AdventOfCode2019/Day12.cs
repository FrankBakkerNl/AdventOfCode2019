using System.Linq;
using System.Text.RegularExpressions;
using static System.Math;

namespace AdventOfCode2019
{
    public class Day12
    {
        public static int GetAnswer1(string[] input) => GetEnergyAfterSteps(input, 1000);

        public static long GetAnswer2(string[] input) // 353620566035124
        {
            var state = Parse(input);
            var dimensions = Transpose(state);

            // Find the length of the cycle of each dimension (x, y, z) separately
            var results = dimensions.Select(FindCycleLength).ToArray();

            // Now a full cycle is the first time all cycles meet, the lowest common multiple
            return Lcm(results[0], Lcm(results[1], results[2]));
        }

        public static int GetEnergyAfterSteps(string[] input, int steps)
        {
            var state = LoadInitialState(input);
            for (var i = 0; i < steps; i++)
            {
                state = Step(state);
            }
            return state.Sum(Energy);
        }

        public static int Energy((Vector p, Vector v) t) => Energy(t.p) * Energy(t.v);

        public static int Energy(Vector v) => Abs(v.x) + Abs(v.y) + Abs(v.z);


        private static int FindCycleLength((int, int)[] startState)
        {
            var state = startState;
            var iteration = 0;
            do
            {
                iteration++;
                state = Step(state);
            } while (!state.SequenceEqual(startState));
            return iteration;
        }

        private static long Gcd(long a, long b) => a % b == 0 ? b : Gcd(b, a % b);
        private static long Lcm(long a, long b) => a * b / Gcd(a, b);

        public static (Vector p, Vector v)[] Step((Vector p, Vector v)[] allMoons) =>
            (from moon in allMoons
                let newVelocity =  allMoons.Aggregate(moon.v, (velocity, other) => velocity.Add(GetForce(moon.p, other.p)))
                select (moon.p.Add(newVelocity), newVelocity))
            .ToArray();

        public static (int p, int v)[] Step((int p, int v)[] allMoons) =>
            (from moon in allMoons
                let newVelocity = moon.v + allMoons.Sum(other => GetForce(moon.p, other.p))
                select (moon.p + newVelocity, newVelocity))
            .ToArray();

        public static (int, int)[][]Transpose(Vector[] input) => new[]
        {
            input.Select(i => (i.x,0)).ToArray(),
            input.Select(i => (i.y,0)).ToArray(),
            input.Select(i => (i.z,0)).ToArray()
        };

        public static (Vector p, Vector)[] LoadInitialState(string[] input)
        {
            var positions = Parse(input);
            return positions.Select(p => (p, new Vector())).ToArray();
        }

        public static Vector GetForce(Vector current, Vector other) =>
            new Vector(GetForce(current.x, other.x), 
                       GetForce(current.y, other.y), 
                       GetForce(current.z, other.z));

        private static int GetForce(int x, int y ) => x > y ? -1 :
                                                      x < y ?  1 : 0;

        public static Vector[] Parse(string[] input) => input.Select(Parse).ToArray();

        public static Vector Parse(string input)
        {
            var match = Regex.Match(input, "x= *(?<x>-?[0-9]+), y= *(?<y>-?[0-9]+), z= *(?<z>-?[0-9]+)");
            return new Vector(int.Parse(match.Groups["x"].Value),
                              int.Parse(match.Groups["y"].Value),
                              int.Parse(match.Groups["z"].Value));
        }
    }

    public struct Vector
    {
        public int x, y, z; 

        public Vector(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector Add(Vector other) => new Vector(x + other.x, y + other.y, z + other.z);
        public override string ToString() => $"({x},{y},{z})";
    }
}
