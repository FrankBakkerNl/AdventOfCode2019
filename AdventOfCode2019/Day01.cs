using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day01
    {

        public static int GetAnswer1()
        {
            return ReadFile.Select(s => int.Parse(s)).Select(Fuel).Sum();
        }

        public static int GetAnswer2()
        {
            return ReadFile.Select(s => int.Parse(s)).Select(CumelativeFuel).Sum();
        }


        public static IEnumerable<string> ReadFile =>
            File.ReadLines(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day01.txt");



        public static int CumelativeFuel(int mass)
        {
            int totalFuel = 0;
            var fuel = Fuel(mass);

            while (fuel > 0)
            {
                totalFuel += fuel;
                mass = fuel;
                fuel = Fuel(mass);
            }

            return totalFuel;
        }

        static int Fuel(int mass)
        {
            return (mass / 3) - 2;
        }
    }
}
