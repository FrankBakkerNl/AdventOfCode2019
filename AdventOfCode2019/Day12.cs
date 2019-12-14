using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Math;

namespace AdventOfCode2019
{
    public class Day12
    {
        public static int GetAnswer1(string[] input) => GetEnergyAfterSteps(input, 1000);

        public static int GetEnergyAfterSteps(string[] input, int steps)
        {
            var state = LoadInitialState(input);
            for (var i = 0; i < steps; i++)
            {
                state = Step(state).ToList();
            }
            return state.Sum(Energy);
        }

        public static IList<(Vector p, Vector)> LoadInitialState(string[] input)
        {
            var positions = Parse(input);
            return positions.Select(p => (p, new Vector())).ToList();
        }

        public static int Energy((Vector p, Vector v) t) => Energy(t.p) * Energy(t.v);

        public static int Energy(Vector v) => Abs(v.x) + Abs(v.y) + Abs(v.z);

        public static IEnumerable<(Vector p, Vector v)> Step(IList<(Vector p, Vector velocity)> state) =>
            from moon in state
            let newVelocity = state.Aggregate(moon.velocity, (velocity, other) => velocity.Add(GetForce(moon.p, other.p)))
            select (moon.p.Add(newVelocity), newVelocity);

        public static Vector GetForce(Vector current, Vector other) =>
            new Vector(GetForce(current.x, other.x), 
                       GetForce(current.y, other.y), 
                       GetForce(current.z, other.z));

        private static int GetForce(int x, int y ) => x > y ? -1 :
                                                      x < y ?  1 : 0;

        public static IEnumerable<Vector> Parse(string[] input) => input.Select(Parse).ToArray();

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
