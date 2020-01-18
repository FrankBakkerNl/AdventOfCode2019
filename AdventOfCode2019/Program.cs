using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode2019.Arcade;
using static System.Console;

namespace AdventOfCode2019
{
    class TestAttribute : Attribute{ }

    class Program
    {
        static void Main(string[] args)
        {
            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$")).OrderBy(t=>t.Name).ToList();

            var testDays = days.Where(d => d.GetCustomAttribute<TestAttribute>() != null).ToList();
            if (testDays.Any()) days = testDays;

            foreach (var dayClass in days)
            {
                WriteLine(dayClass.Name);

                GetAnswer(dayClass, 1);
                GetAnswer(dayClass, 2);

                WriteLine();
            }
        }

        public static void GetAnswer(Type dayClass, int part)
        {
            var methodInfo = dayClass.GetMethod("GetAnswer"+part);

            if (methodInfo != null)
            {
                var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(dayClass);
                var input = InputDataManager.GetInputArgs(methodInfo);
                var sw = Stopwatch.StartNew();
                Write($"part {part}: " );
                WriteLine(methodInfo.Invoke(instance, input));
                WriteLine(sw.Elapsed);
            }
        }

    }
}
