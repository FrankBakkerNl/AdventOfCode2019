using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day24
    {
        [Result(18400817)]
        public static int GetAnswer1(string[] input) => 
            FindFirstRepeat(new Field(input)).Bits;


        [Result(1944)]
        public static int GetAnswer2(string[] input) =>
            GetCountAfterGenerations(new Field(input), 200); // 1944


        public static Field FindFirstRepeat(Field start)
        {
            var found = new HashSet<int>();
            var current = start;
            do
            {
                current = current.NextGeneration();
            } while (found.Add(current.Bits));

            return current;
        }

        public static int GetCountAfterGenerations(Field start, int generations) => 
            FindLevels(start, generations).Sum(f => f.BugCount);

        public static Field[] FindLevels(Field field, int generations)
        {
            var result = new Field[generations*2+1]; 
            var initial = new Field[generations*2+1];
            initial[0] = field;

            for (int generation = 1; generation <= generations; generation++)
            {
                CreateNexttGen(generation, initial, result);
                Swap(ref initial, ref result);
            }

            return initial;
            //  0
            // -1, 0, 1
            // -2, -1, 0, 1, 2
        }

        private static void CreateNexttGen(int generation, Field[] initial, Field[] result)
        {
            for (int i = 0; i < generation * 2 + 1; i++)
            {
                var left = i > 1 ? initial[i - 2].Bits : 0;
                var current = i > 0 ? initial[i - 1] : new Field(0);
                var right = initial[i].Bits;
                result[i] = current.NextGenerationRecursive(left, right);
            }
        }

        private static void Swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }


        public readonly struct Field
        {
            public int Bits { get; }

            public Field(int bits)
            {
                Bits = bits;
            }

            public Field(string[] input)
            {
                var all = string.Join("", input);
                Bits = all.Select((c, i) => (c, i))
                          .Where(t => t.c == '#')
                          .Select(t => 1 << t.i)
                          .Aggregate(0, (a, b) => a | b);
            }

            public int BugCount
            {
                get
                {
                    int count = 0;
                    for (int i = 0; i < 25; i++)
                    {
                        count += (Bits >> i & 1);
                    }
                    return count;
                }
            }

            public Field NextGeneration()
            {
                int result = Bits;
                for (int i = 0; i < 25; i++)
                {
                    var isAlive = (Bits >> i & 1) != 0;

                    var bitUp    = i < 5      ? 0 : (Bits >> i - 5) & 1;
                    var bitDown  = i > 19     ? 0 : (Bits >> i + 5) & 1;
                    var bitLeft  = i % 5 == 0 ? 0 : (Bits >> i - 1) & 1;
                    var bitRight = i % 5 == 4 ? 0 : (Bits >> i + 1) & 1;

                    var count = bitUp + bitDown + bitLeft + bitRight;

                    // A bug dies (becoming an empty space) unless
                    // there is exactly one bug adjacent to it.
                    if (isAlive && count != 1)
                        result ^= 1 << i;

                    // An empty space becomes infested with a 
                    // bug if exactly one or two bugs are adjacent to it.
                    else if (!isAlive && (count == 1 || count == 2))
                        result ^= 1 << i;
                }

                return new Field(result);
            }


            public Field NextGenerationRecursive(int inner, int outer)
            {

                var topInner = (inner & 1) +
                               (inner >> 1 & 1) +
                               (inner >> 2 & 1) +
                               (inner >> 3 & 1) +
                               (inner >> 4 & 1);

                var leftInner  = (inner & 1) +
                                 (inner >>  5 & 1) +
                                 (inner >> 10 & 1) +
                                 (inner >> 15 & 1) +
                                 (inner >> 20 & 1);

                var bottomInner  = (inner >> 20 & 1) +
                                   (inner >> 21 & 1) +
                                   (inner >> 22 & 1) +
                                   (inner >> 23 & 1) + 
                                   (inner >> 24 & 1);

                var rightInner = (inner >>  4 & 1) +
                                 (inner >>  9 & 1) +
                                 (inner >> 14 & 1) +
                                 (inner >> 19 & 1) +
                                 (inner >> 24 & 1);


                int result = Bits;
                for (int i = 0; i < 25; i++)
                {
                    // skip the center
                    if (i == 12) continue;
                    var isAlive = (Bits >> i & 1) != 0;

                    var bitUp    = i < 5      ? outer >>  7 & 1 : (Bits >> i - 5) & 1;
                    var bitDown  = i > 19     ? outer >> 17 & 1 : (Bits >> i + 5) & 1;
                    var bitLeft  = i % 5 == 0 ? outer >> 11 & 1 : (Bits >> i - 1) & 1;
                    var bitRight = i % 5 == 4 ? outer >> 13 & 1 : (Bits >> i + 1) & 1;

                    // add values for inner level for adjacent cells
                    var add = i switch {7 => topInner, 11 => leftInner, 13 => rightInner, 17 => bottomInner, _=>0};

                    var count = bitUp + bitDown + bitLeft + bitRight + add;

                    // A bug dies (becoming an empty space) unless
                    // there is exactly one bug adjacent to it.
                    if (isAlive && count != 1)
                        result ^= 1 << i;

                    // An empty space becomes infested with a 
                    // bug if exactly one or two bugs are adjacent to it.
                    else if (!isAlive && (count == 1 || count == 2))
                        result ^= 1 << i;
                }

                return new Field(result);
            }

            public override string ToString()
            {
                var bits = Bits;
                return string.Join("", Enumerable.Range(0, 25).Select(i => ((1 << i & bits) != 0 ? "#" : ".") + (i % 5 == 4 ?"|": "")));
            }

            public override int GetHashCode() => Bits;
            public override bool Equals(object obj) => obj is Field f && f.Bits == Bits;
        }
    }
}
