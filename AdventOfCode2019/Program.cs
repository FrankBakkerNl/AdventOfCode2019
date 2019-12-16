using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            new ConsoleGame().Run();

            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$")).OrderBy(t=>t.Name).ToList();

            var testDays = days.Where(d => d.GetCustomAttribute<TestAttribute>() != null).ToList();
            if (testDays.Any()) days = testDays;

            foreach (var dayClass in days)
            {
                WriteLine(dayClass.Name);

                var getAnswer1 = dayClass.GetMethod("GetAnswer1");

                if (getAnswer1 != null)
                {
                    var instance = getAnswer1.IsStatic ? null : Activator.CreateInstance(dayClass);
                    WriteLine("1: {0}", getAnswer1.Invoke(instance, GetInputArgs(getAnswer1)));
                }

                var getAnswer2 = dayClass.GetMethod("GetAnswer2");
                if (getAnswer2 != null)
                {
                    var instance = getAnswer2.IsStatic ? null : Activator.CreateInstance(dayClass);
                    WriteLine("2: {0}", getAnswer2.Invoke(instance, GetInputArgs(getAnswer2)));
                }
                WriteLine();
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

            throw new InvalidOperationException($"Unable to map input data for {method.DeclaringType.Name}.{method.Name}");
        }
    }
}
