using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day01
    {

        public static int GetAnswer1(string input)
        {
            return input.Split(Environment.NewLine).Select(int.Parse).Select(Fuel).Sum();
        }

        public static int GetAnswer2(string input)
        {
            return input.Split(Environment.NewLine).Select(int.Parse).Select(CumelativeFuel).Sum();
        }

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

        static int Fuel(int mass) => (mass / 3) - 2;
    }
}
