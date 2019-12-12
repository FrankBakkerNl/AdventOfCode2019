﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Console;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$")).OrderBy(t=>t.Name);
            foreach (var dayClass in days)
            {
                WriteLine(dayClass.Name);

                var getAnswer1 = dayClass.GetMethod("GetAnswer1");
                if (getAnswer1 != null)
                {
                    WriteLine("1: {0}", getAnswer1.Invoke(null, GetInputArgs(getAnswer1)));
                }

                var getAnswer2 = dayClass.GetMethod("GetAnswer2");
                if (getAnswer2 != null)
                {
                    WriteLine("2: {0}", getAnswer2.Invoke(null, GetInputArgs(getAnswer2)));
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
