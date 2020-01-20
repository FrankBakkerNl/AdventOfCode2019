using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day17
    {
        [Result(3192)]
        public static int GetAnswer1(string input)
        {
            var map = GetMap(new IntCodeComputer(input));
            return map.scaffolds
                .Where(s => IsIntersection(s, map.scaffolds))
                .Sum(s => s.Item1 * s.Item2);
        }

        private static bool IsIntersection((int r, int c) i, HashSet<(int, int)> scaffolds) =>
            scaffolds.IsSupersetOf(new[]
            {
                (i.r - 1, i.c),
                (i.r + 1, i.c),
                (i.r,     i.c - 1),
                (i.r,     i.c + 1),
            });
        

        private static (HashSet<(int, int)> scaffolds, (int, int) startPos, int heading) GetMap(IntCodeComputer cpu)
        {
            var facingChars = "^>v<";
            cpu.Run();
            int row = 0;
            int col = 0;
            var scaffolds = new HashSet<(int, int)>();
            var startPos = (0, 0);
            var heading = 0;

            while (cpu.IsOutputAvailable)
            {
                var value = cpu.ReadOutput();

                var character = (char) (int) value;
                if (character == '#')
                {
                    scaffolds.Add((row, col));
                }
                if (facingChars.Contains(character))
                {
                    startPos = (row, col);
                    heading = facingChars.IndexOf(character);
                }
                col++;
                if (value == 10)
                {
                    row++;
                    col = 0;
                }
            }

            return (scaffolds, startPos, heading);
        }

        [Result(684691)]
        public static long GetAnswer2(string input)
        {
            var (scaffolds, startPos, heading) = GetMap(new IntCodeComputer(input));
            var fullPath = FindPath(scaffolds, startPos, heading);
            var lines = SplitProgram(fullPath);

            var cpu = new IntCodeComputer(input);
            cpu.Program[0] = 2;
            foreach (var line in lines)
            {
                InputLine(cpu, line);
            }
            InputLine(cpu, "n");
            return (long)cpu.ReadAvailableOutput().Last();
        }

        private static string FindPath(HashSet<(int, int)> scaffolds, (int, int) start, int heading)
        {
            var sb = new StringBuilder();
            var headings = new[]
            {
                (-1, 0), 
                ( 0, 1), 
                ( 1, 0), 
                ( 0,-1)
            };

            var current = start;
            int currentPath = 0;

            while (true)
            {
                var headingDelta = headings[heading];
                var next = Add(current, headingDelta);
                if (scaffolds.Contains(next))
                {
                    currentPath++;
                    current = next;
                }
                else
                {
                    sb.Append(","+ currentPath);
                    currentPath = 0;
                    
                    var left = (heading + 3) % 4;
                    var right = (heading + 1) % 4;
                    if (scaffolds.Contains(Add(current, headings[left])))
                    {
                        sb.Append(",L");
                        heading = left;
                    }
                    else if (scaffolds.Contains(Add(current, headings[right])))
                    {
                        sb.Append(",R");
                        heading = right;
                    }
                    else
                    {
                        return sb.ToString(3, sb.Length-3);
                    }
                }
            }
        }

        private static string[] SplitProgram(string full)
        {
            var parts = FindParts(full, new string[0]);
            var (_, main) = Match(full, parts);
            return parts.Prepend(main.TrimEnd(',')).ToArray();
        }

        private static string[] FindParts(string input, string[] parts)
        {
            var (remainder, _) = Match(input, parts);
            if (remainder.Length == 0) return parts;
            if (parts.Length == 3) return null;

            int partLength = 20;
            while (partLength > 0)
            {
                var (head, tail) = TakeMax(remainder, partLength);
                if (head.Length == 0) return null;
                var newParts = FindParts(tail, parts.Append(head).ToArray());
                if (newParts != null) return newParts;
                partLength = head.Length - 1;
            }
            return null;
        }


        private static (string head, string tail) TakeMax(string input, int max)
        {
            var commaIndex = (input + ",").Substring(0, max + 1).LastIndexOf(',');
            if (commaIndex < 0) return ("", input);
            return (input.Substring(0, commaIndex), RemoveLeft(input, commaIndex));
        }

        private static (string tail, string main) Match(string input, string[] parts)
        {
            if (input.Length == 0)
            {
                return ("", "");
            }

            foreach (var (part, letter) in parts.Zip("ABC"))
            {
                if (input.StartsWith(part))
                {
                    var (tail, main) = Match(RemoveLeft(input, part.Length), parts);
                    return (tail, letter + "," + main);
                }
            }

            return (input, "");
        }

        private static string RemoveLeft(string input, int length)
            => input.Substring(length, input.Length - length).TrimStart(',');

        private static void InputLine(IntCodeComputer cpu, string line)
        {
            cpu.Run(line.Select(c => (BigInteger) c).ToArray());
            cpu.Run(10);
        }

        private static (int, int) Add((int, int) x, (int, int ) y) => (x.Item1 + y.Item1, x.Item2 + y.Item2);
    }
}
