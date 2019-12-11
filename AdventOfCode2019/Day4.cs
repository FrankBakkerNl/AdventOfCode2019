using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day4
    {
        public static int GetAnswer1() => 
            GetAllBetween(183564, 657474)
            .Where(HasRepeatingDigit)
            .Count();

        public static int GetAnswer2() => 
            GetAllBetween(183564, 657474)
                .Where(HasDoubleDigit)
                .Count();

        public static IEnumerable<int> GetAllBetween(int lower, int upper)
        {
            for (int i = NextValidNumber(lower-1); i < upper; i = NextValidNumber(i))
            {
                yield return i;
            }
        }

        public static int NextValidNumber(int input)
        {
            var lowerBound = 0;
            var result = 0;

            foreach (var digit in GetDigits(input + 1))
            {
                var current = Math.Max(lowerBound, digit);
                lowerBound = current;
                result = result * 10 + current;
            }

            return result;
        }


        private static IEnumerable<int> GetDigits(int input) => input.ToString().Select(c => c - '0');

        private static bool HasRepeatingDigit(int input) => GetDigits(input).GroupBy(d=>d).Any(g=>g.Count()>=2);

        private static bool HasDoubleDigit(int input) => GetDigits(input).GroupBy(d=>d).Any(g=>g.Count()==2);
    }
}
