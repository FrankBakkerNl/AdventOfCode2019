using System;
using System.Linq;
using AdventOfCode2019.VM;
using static System.Linq.Enumerable;

namespace AdventOfCode2019
{
    public class Day19
    {
        [Result(116)]
        public int GetAnswer1(string input)
        {
            bool IsAffected(int x, int y)
            {
                var cpu = new IntCodeComputer(input);
                cpu.Run(x,y);
                return cpu.ReadOutput() == 1;
            }

            return Range(0, 50).SelectMany(i => Range(0, 50).Select(j => (i, j))).Count(s => IsAffected(s.i, s.j));
        }

        [Result(10311666)]
        public int GetAnswer2(int[] input)
        { 
            var analyser = new BeamAnalyzer(input);

            bool IsAffected(int x, int y) => analyser.IsAffected(x, y);
            return FindBoxSize(100, IsAffected);
        }

        public delegate bool Affected(int x, int y);

        public static int FindBoxSize(int size, Affected isAffected)
        {
            var (col, row) = FindBoxStart(size, isAffected);
            return col * 10000 + row;
        }

        public static (int, int) FindBoxStart(int size, Affected isAffected)
        {
            var (lower, upper) = FindInitialBounds(i=>HasDiagonalSize(isAffected, i, size -2), guess: size * 10);
            var diagoinalRow = BinarySearchWhile(i=>HasDiagonalSize(isAffected, i, size -2), lower, upper);

            while (!HasDiagonalSize(isAffected, diagoinalRow, size))
            {
                diagoinalRow++;
            }

            var startCol = ProbeLeftColOfBeam(isAffected, diagoinalRow, 0, true);
            return (startCol, diagoinalRow - size +1);
        }


        public static int ProbeLeftColOfBeam(Affected isAffected, int startRow, int startCol, bool target)
        {
            // Very naive way to find the left of the beam, could be optimized
            var col = startCol;
            while (isAffected(col, startRow) != target)
            {
                col++;
            }
            return col;
        }

        public static bool HasDiagonalSize(Affected isAffected, int startRow, int size)
        {
            var col= ProbeLeftColOfBeam(isAffected, startRow, 0, true);
            return DiagonalFits(isAffected, startRow, col, size);
        }

        public static bool DiagonalFits(Affected isAffected, int startRow, int startCol, int size) => 
            isAffected(startCol + size -1, startRow - size +1);


        private static (int lower, int upper) FindInitialBounds(Func<int, bool> produce, int guess)
        {
            var lower = 1;
            while (true)
            {
                if (produce(guess))
                {
                    return (lower, guess);
                }
                lower = guess;
                guess *= 2;
            }
        }


        private static int BinarySearchWhile(Func<int, bool> produce, int lowerBound, int upperBound)
        {
            while (upperBound - lowerBound > 1 )
            {
                var tryValue = (lowerBound + upperBound) / 2;
                if (produce(tryValue))
                {
                    upperBound = tryValue;
                }
                else
                {
                    lowerBound = tryValue;
                }
            }
            return lowerBound;
        }

        public class BeamAnalyzer
        {
            private IntCodeComputer baseCpu;
            public BeamAnalyzer(int[] input)
            {
                baseCpu = new IntCodeComputer(input);
            }

            public bool IsAffected(int x, int y)
            {
                var cpu = baseCpu.CloneState();
                cpu.Run(x, y);
                var isAffected = cpu.ReadOutput() == 1;
                return isAffected;
            }
        }
    }
}
