﻿using System.Linq;
using static System.Console;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Day 1:");
            WriteLine(Day1.GetAnswer1());
            WriteLine(Day1.GetAnswer2());
            WriteLine();

            WriteLine("Day 2:");
            WriteLine(Day2.GetAnswer1());
            WriteLine(Day2.GetAnswer2()); // 8226
            WriteLine();

            WriteLine("Day 3:");
            WriteLine(Day3.GetAnswer1()); // 721
            WriteLine(Day3.GetAnswer2()); // 7388
            WriteLine();

            WriteLine("Day 4:");
            WriteLine(Day4.GetAnswer1()); // 1610
            WriteLine(Day4.GetAnswer2());
            WriteLine();

            WriteLine("Day 5:");
            WriteLine(Day5.GetAnswer1()); // 9219874
            WriteLine(Day5.GetAnswer2()); // 5893654
            WriteLine();

            WriteLine("Day 6:");
            WriteLine(Day6.GetAnswer1()); // 151345 
            WriteLine(Day6.GetAnswer2());
            WriteLine();

            WriteLine("Day 7:");
            WriteLine(Day7.GetAnswer1());
            WriteLine(Day7.GetAnswer2());
            WriteLine();

            WriteLine("Day 8:");
            WriteLine(Day8.GetAnswer1());
            WriteLine(Day8.GetAnswer2());
            WriteLine();

            WriteLine("Day 9:"); 
            WriteLine(Day9.GetAnswer1());
            WriteLine(Day9.GetAnswer2()); 
            WriteLine();
        }
    }
}
