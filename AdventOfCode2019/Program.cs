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
            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => Regex.IsMatch(t.Name, "Day[0-9][0-9]")).OrderBy(t=>t.Name);
            foreach (var dayClass in days)
            {
                WriteLine(dayClass.Name);

                var getAnswer1 = dayClass.GetMethod("GetAnswer1");
                if (getAnswer1 != null)
                {
                    WriteLine("1: {0}", getAnswer1.Invoke(null, null));
                }

                var getAnswer2 = dayClass.GetMethod("GetAnswer1");
                if (getAnswer2 != null)
                {
                    WriteLine("1: {0}", getAnswer2.Invoke(null, null));
                }
                WriteLine();
            }
        }
    }
}
