using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode2019
{
    public class InputDataManager
    {
        public static object[] GetInputArgs(MethodInfo method)
        {
            string filename = $@"..\..\..\..\AdventOfCode2019\Data\{method.DeclaringType.Name}.txt";

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
