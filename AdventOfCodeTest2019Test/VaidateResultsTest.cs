using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    class VerifyResultData : TheoryData<string, MethodInfo, object>
    {
        public VerifyResultData()
        {
            var days = typeof(AdventOfCode2019.Program).Assembly.GetTypes().Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$")).OrderBy(t=>t.Name).ToList();
            foreach (var type in days)
            {
                AddTests(type, 1);
                AddTests(type, 2);
            }
        }

        private void AddTests(Type dayClass, int part)
        {
            var methodInfo = dayClass.GetMethod("GetAnswer"+part);
            var expectedResult = methodInfo?.GetCustomAttributes<ResultAttribute>().FirstOrDefault()?.Result;
            if (expectedResult != null)
            {
                Add($"{dayClass.Name}.{part}", methodInfo, expectedResult);
            }

        }
    }
    public class ValidateResultsTest
    {
        [Theory]
        [ClassData(typeof(VerifyResultData))]
        public void VerifyResult(string name, MethodInfo methodInfo, object expectedResult)
        {
            var input = InputDataManager.GetInputArgs(methodInfo);
            var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(methodInfo.DeclaringType);

            var result = methodInfo.Invoke(instance, input);
            result.Should().Be(expectedResult);
        }
    }
}
