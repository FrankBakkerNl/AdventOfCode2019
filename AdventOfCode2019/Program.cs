using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode2019.Arcade;
using static System.Console;

namespace AdventOfCode2019
{
    class TestAttribute : Attribute{ }

    public class Program
    {
        static void Main(string[] args)
        {
            var totalTime = Stopwatch.StartNew();
            //RunParalell();
            RunSequential();
            WriteLine($"Total: {totalTime.Elapsed}");

        }

        private static void RunSequential()
        {
            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$"))
                .OrderBy(t => t.Name).ToList();

            var testDays = days.Where(d => d.GetCustomAttribute<TestAttribute>() != null).ToList();
            if (testDays.Any()) days = testDays;

            foreach (var dayClass in days)
            {
                PrintAnswer(dayClass, 1);
                PrintAnswer(dayClass, 2);
                WriteLine();
            }
        }


        static void RunParalell()
        {
            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$")).OrderBy(t=>t.Name).ToList();
            var answers = days.SelectMany(d => new[] {(d, 1), (d, 2)}).AsParallel().Select(t => (t, GetAnswer(t.d, t.Item2)))
                .ToList();
            foreach (var answer in answers)
            {
                WriteLine($"{answer.t.d.Name}-{answer.t.Item2}: " );
                WriteLine(answer.Item2);
            }
        }

        public static object GetAnswer(Type dayClass, int part)
        {
            var methodInfo = dayClass.GetMethod("GetAnswer"+part);
            if (methodInfo== null) return null;
            var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(dayClass);
            var input = InputDataManager.GetInputArgs(methodInfo);
            return methodInfo.Invoke(instance, input);
        }

        public static void PrintAnswer(Type dayClass, int part)
        {
            var methodInfo = dayClass.GetMethod("GetAnswer"+part);

            if (methodInfo != null)
            {
                var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(dayClass);
                var input = InputDataManager.GetInputArgs(methodInfo);

                var sw = Stopwatch.StartNew();
                WriteLine($"{dayClass.Name}-{part}: " );
                WriteLine(methodInfo.Invoke(instance, input));

                var swElapsed = sw.Elapsed;
                if (sw.Elapsed > TimeSpan.FromSeconds(1)) Console.ForegroundColor = ConsoleColor.Red;
                else if (sw.Elapsed > TimeSpan.FromMilliseconds(100)) Console.ForegroundColor = ConsoleColor.Yellow;
                WriteLine(swElapsed);
                ForegroundColor = ConsoleColor.Gray;
            }
        }

    }
}
