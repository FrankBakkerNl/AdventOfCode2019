using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2019
{
    public class Day22
    {
        public static BigInteger GetAnswer1(string[] instructions) => FindNewPosition(instructions, 10007, 2019); 
        // 4485

        public static BigInteger FullDeckSize = 119_315_717_514_047;
        public static BigInteger LoopCount = 101_741_582_076_661;

        public static BigInteger GetAnswer2(string[] instructions) 
            => FindOriginalPosition(instructions, FullDeckSize, 2020, LoopCount);
        // 91967327971097

        private static readonly (BigInteger a, BigInteger b) NoOpShuffle = (1, 0);

        public static BigInteger FindNewPosition(string[] instructions, BigInteger deckSize, BigInteger initialPosition)
        {
            var parsed = instructions.Select(ParseInstruction);
            var (a, b) = parsed.Select(p => GetABForOperation(p, deckSize))
                .Aggregate(NoOpShuffle, MergeFactors);

            return ModuloShift(initialPosition * a + b, deckSize);
        }

        public enum Operation { DealIntoNewStack, Cut, DealWithIncrement }

         public static (Operation, long) ParseInstruction(string line)
         {
             var match = Regex.Match(line, "(?<i>[a-z ]*[a-z]) ?(?<n>-?[0-9]+)?");

             var instruction = match.Groups["i"].Value switch
             {
                 "deal into new stack" => Operation.DealIntoNewStack,
                 "deal with increment" => Operation.DealWithIncrement,
                 "cut" => Operation.Cut
             };

             var param = match.Groups["n"].Success ? long.Parse(match.Groups["n"].Value) : 0;
             return (instruction, param);
         }

         /// <summary>
         /// Each shuffle can be expressed as a function y = ax+b % deckSize,
         /// here we find just the A and B for an operation
         /// </summary>
         public static (BigInteger a, BigInteger b) GetABForOperation( 
             (Operation type, BigInteger p) instruction, BigInteger deckSize)
         {
             var values = instruction.type switch
             {
                 Operation.DealIntoNewStack =>  (a: -BigInteger.One,       b: deckSize - 1L), // y = -x + ds - 1
                 Operation.Cut =>               (a: BigInteger.One,        b: deckSize - instruction.p), // y = x + ds - p
                 Operation.DealWithIncrement => (a: instruction.p,         b: 0L) // y = p * x
             };
             return ModuloShift(values, deckSize);
         }

         /// <summary>
         /// Combines the A and B components of two y=ax+b functions into the A and B one new combined function
         /// Such that the resulting function will be F2(F1))
         /// </summary>
         public static (BigInteger a, BigInteger b) MergeFactors((BigInteger a, BigInteger b) f1, (BigInteger a, BigInteger b) f2) =>
             (a: f2.a * f1.a, 
              b: f2.a * f1.b + f2.b);
         /* we add two functions together like this
          * F1(x) = a1 * x + b1
          * F2(x) = a2 * x + b2
          * Fr(x) = F2   (F1(x)      )
          * Fr(x) = a2 * (a1 * x + b1) + b2
          * Fr(x) = a2 * a1 * x + a2 * b1 + b2
          *         |  a  | * x + |    b     |
          */

        public static BigInteger FindOriginalPosition(string[] instructions, BigInteger deckSize, BigInteger finalPosition, BigInteger loopCount)
        {
            var parsed = instructions.Select(ParseInstruction);
            var factors = parsed.Select(p => GetABForOperation(p, deckSize))
                .Aggregate(NoOpShuffle, MergeFactors);

            var (a, b) = MultiplyFactors(factors, loopCount, deckSize);

            return ReverseLookup(finalPosition, a, b, deckSize);
        }

        public static (BigInteger, BigInteger) MultiplyFactors((BigInteger a, BigInteger b) initialFactors, BigInteger loopCount, BigInteger deckSize)
        {
            // We need to apply F(F(F(x))) MANY times (101_741_582_076_661)
            // Instead of adding the function one by one we keep on duplicating it
            // so we get the results for all the squares 1, 2, 4, 8, 16
            // then we add up all required squares to the result

            var current = initialFactors;
            var result = (a: BigInteger.One, b: BigInteger.Zero);

            // loop fast by duplicating each time so we loop just around 47 times 
            for (BigInteger i = 1; i <= loopCount; i *= 2)
            {
                // add the current to the result if the loopCount contains the bit for this iteration
                if ((loopCount & i) != 0)
                {
                    result = ModuloShift(MergeFactors(current, result), deckSize);
                }
                // Merge the current with itself so we get the values for the next square number
                current = ModuloShift(MergeFactors(current, current), deckSize);
            }

            return result;
        }

        public static BigInteger ReverseLookup(BigInteger finalPosition, BigInteger a, BigInteger b, BigInteger deckSize)
        {
            // We need the inverse of Y = (A * X + B) % deckSize

            // subtract b first and shift back
            var offSetFinalPosition = ModuloShift(finalPosition - b, deckSize);

            // for the reverse lookup we need the Multiplicative Inverse of A
            var inverseA = MultiplicativeInverse(a, deckSize);
            var initialPosition = ModuloShift(offSetFinalPosition * inverseA, deckSize);

            // double check the result
            Debug.Assert( ModuloShift(initialPosition * a + b, deckSize) == finalPosition);
            return initialPosition;
        }

        public static BigInteger MultiplicativeInverse(BigInteger x, BigInteger modulus)
        {
            // Thanks to Eric https://ericlippert.com/2013/11/14/a-practical-use-of-multiplicative-inverses/
            // for explaining Multiplicative Inverse
            return ExtendedEuclideanDivision(x, modulus).Item1 % modulus;
        }

        public static (BigInteger, BigInteger)
            ExtendedEuclideanDivision(BigInteger a, BigInteger b)
        {
            if (a < 0)
            {
                var (s, t) = ExtendedEuclideanDivision(-a, b);
                return (-s, t);
            }
            if (b < 0)
            {
                var (s, t) = ExtendedEuclideanDivision(a, -b);
                return (s, -t);
            }
            if (b == 0)
                return (1, 0);
            else
            {
                var (s, t) = ExtendedEuclideanDivision(b, a % b);
                return (t, s - a / b * t);
            }
        }

        public static BigInteger ModuloShift(BigInteger pos, BigInteger deckSize)
        {
            pos %= deckSize;
            return pos >= 0 ? pos : pos + deckSize;
        }

        static (BigInteger a, BigInteger b) ModuloShift((BigInteger a, BigInteger b) input, BigInteger deckSize) =>
            (ModuloShift(input.a, deckSize), ModuloShift(input.b, deckSize));
    }
}
