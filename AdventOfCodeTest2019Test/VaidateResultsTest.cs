using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using AdventOfCode2019;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTest2019Test
{
    class VerifyResultData : TheoryData<string, Type, int, object>
    {
        public VerifyResultData()
        {
            AddTest<Day01>(3287620, 4928567);
            AddTest<Day02>((BigInteger)2890696, 8226);
            AddTest<Day03>(721, 7388);
            AddTest<Day04>(1610, 1104);
            AddTest<Day05>((BigInteger)9219874, (BigInteger)5893654);
            AddTest<Day06>(151345, 391);
            AddTest<Day07>((BigInteger)298586, (BigInteger)9246095);
            AddTest<Day08>(2413, @"
XXX   XX  XXX  XXXX XXX  
X  X X  X X  X    X X  X 
XXX  X    X  X   X  XXX  
X  X X    XXX   X   X  X 
X  X X  X X    X    X  X 
XXX   XX  X    XXXX XXX  "); 
            
            AddTest<Day09>((BigInteger)2457252183, (BigInteger)70634);
            AddTest<Day10>(276, 1321);
            AddTest<Day11>(1964,@"
 #### #  # #### #  #  ##  #### ###  #  #   
 #    # #  #    # #  #  # #    #  # # #    
 ###  ##   ###  ##   #    ###  #  # ##     
 #    # #  #    # #  #    #    ###  # #    
 #    # #  #    # #  #  # #    # #  # #    
 #    #  # #### #  #  ##  #    #  # #  #   
"); //"FKEKCFRK"

            AddTest<Day12>(9127, 353620566035124);
            AddTest<Day13>(348, 16999);
            AddTest<Day14>(899155, 2390226);
            AddTest<Day15>(330, 352);
//            AddTest<Day16>(null, null);
            AddTest<Day17>(3192, (BigInteger)684691);

// skip slow            AddTest<Day18>(5450, 2020);
            AddTest<Day19>(116, 10311666);
            AddTest<Day20>(626, 6912);
//            AddTest<Day21>(null, null);
            AddTest<Day22>((BigInteger)4485, (BigInteger)91967327971097);
            AddTest<Day23>(24555, null);
            AddTest<Day24>(18400817, 1944);
            AddTest<Day25>(352325632);

        }

        private void AddTest<TDay>(object result1 = null, object result2 = null)
        {
            if (result1!=null) Add(typeof(TDay).Name + ".1", typeof(TDay), 1, result1);
            if (result2!=null) Add(typeof(TDay).Name + ".2", typeof(TDay), 2, result2);
        }
    }
    public class ValidateResultsTest
    {
        [Theory()]
        [ClassData(typeof(VerifyResultData))]
        public void VerifyResult(string name, Type dayClass, int part, object expectedResult)
        {
            var methodInfo = dayClass.GetMethod("GetAnswer"+part);

            var input = InputDataManager.GetInputArgs(methodInfo);
            var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(dayClass);

            var result = methodInfo.Invoke(instance, input);
            result.Should().Be(expectedResult);

        }


    }
}
