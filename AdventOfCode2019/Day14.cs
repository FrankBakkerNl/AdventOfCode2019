using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    using LookupTable = IDictionary<string, (long quantity, IEnumerable<(long quantity, string element)> inputs)>;
    using StockStatus = Dictionary<string, long>;

    public class Day14
    {
        [Result(899155)]
        public static long GetAnswer1(string input) => GetOreRequiredForFuel(1, CreateLookup(input));
        [Result(2390226)]
        public static long GetAnswer2(string input) => FindFuelForOre (CreateLookup(input), 1_000_000_000_000);

        public static long FindFuelForOre(LookupTable formulas, long oreStock)
        {
            var oreRequiredForOneFuel = GetOreRequiredForFuel(1, formulas);

            var (lower, upper) = FindInitialBounds(l => GetOreRequiredForFuel(l, formulas), oreStock, oreStock / oreRequiredForOneFuel);
            return BinarySearch(l => GetOreRequiredForFuel(l, formulas), oreStock, lower, upper);
        }

        private static (long lower, long upper) FindInitialBounds(Func<long, long> produce, long target, long guess)
        {
            var lower = 1L;
            while (true)
            {
                var result = produce(guess);
                if (result > target)
                {
                    return (lower, guess);
                }
                lower = guess;
                guess *= 2;
            }
        }


        private static long BinarySearch(Func<long, long> produce, long targetValue, long lowerBound, long upperBound)
        {
            while (upperBound - lowerBound > 1 )
            {
                var tryValue = (lowerBound + upperBound) / 2L;
                var tempResult = produce(tryValue);
                if (tempResult >= targetValue)
                {
                    upperBound = tryValue;
                }
                else
                {
                    lowerBound = tryValue;
                }
            }
            return lowerBound;
        }

        public static long GetOreRequiredForFuel(long fuelCount, LookupTable formulas)
        {
            var leftOvers = formulas.Keys.ToDictionary(k => k, k => 0L);
            leftOvers["ORE"] = 0;
            return GetOreRequiredFor(fuelCount, "FUEL", leftOvers, formulas);
        }

        public static long GetOreRequiredFor(long requiredQuantity, string element, StockStatus leftOvers, LookupTable formulas)
        {
            if (element == "ORE")
            {
                return requiredQuantity;
            }
            var (buildQuantum, inputs) = formulas[element];

            var fromLeftovers = Math.Min(requiredQuantity, leftOvers[element]);
            var buildQuantity = requiredQuantity - fromLeftovers;

            var quantumsToProduce = (buildQuantity + buildQuantum - 1) / buildQuantum;

            var leftOver = quantumsToProduce * buildQuantum - buildQuantity;
            leftOvers[element] += leftOver - fromLeftovers;

            return inputs.Sum(i => GetOreRequiredFor(i.quantity * quantumsToProduce, i.element, leftOvers, formulas));
        }

        public static LookupTable CreateLookup(string input)
        {
            var parsed = input.Split(Environment.NewLine).Select(Parse);
            return parsed.ToDictionary(l => l.result.element,
                l => (l.result.quantity, l.inputs));
        }

        public static (IEnumerable<(long quantity, string element)> inputs, (long quantity, string element) result)
            Parse(string line)
        {
            // '7 A, 1 B => 1 C'
            var (left, right) = line.Split("=>").AsTuple2();
            var inputs = left.Trim().Split(",").Select(term => ParseTerm(term)).ToArray();
            var output = ParseTerm(right);
            return (inputs, output);
        }

        private static (long, string) ParseTerm(string term)
        {
            // '7 A'
            var (factor, element) = term.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).AsTuple2();
            return (long.Parse(factor), element);
        }
    }

    static class HelpersD14
    {
        public static (T, T) AsTuple2<T>(this T[] i) => (i[0], i[1]);
    }
}
