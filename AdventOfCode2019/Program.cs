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

        private static void GetAnswer(Type dayClass, int part)
        {
            var methodInfo = dayClass.GetMethod("GetAnswer"+part);

            if (methodInfo != null)
            {
                var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(dayClass);
                var input = GetInputArgs(methodInfo);
                var sw = Stopwatch.StartNew();
                Write($"part {part}: " );
                WriteLine(methodInfo.Invoke(instance, input));
                WriteLine(sw.Elapsed);
            }
        }

        private static object[] GetInputArgs(MethodInfo method)
        {
            string filename = $@"..\..\..\Data\{method.DeclaringType.Name}.txt";

            var param = method.GetParameters().FirstOrDefault();
            if (param == null)
            {
                return null;
            }

            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, "");
            }


            if (param.ParameterType == typeof(string)) return new object [] {File.ReadAllText(filename)};

            if (param.ParameterType == typeof(string[])) return new object [] {File.ReadAllLines(filename)};

            if (param.ParameterType== typeof(int[]))
                return new object[] {File.ReadAllText(filename).Split(",").Select(int.Parse).ToArray()};

            if (param.ParameterType== typeof(long[]))
                return new object[] {File.ReadAllText(filename).Split(",").Select(long.Parse).ToArray()};

            throw new InvalidOperationException($"Unable to map input data for {method.DeclaringType.Name}.{method.Name}");
        }
    }
}
