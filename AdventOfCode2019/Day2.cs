using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Day2
    {

        public static IEnumerable<int> ReadFile() =>
            File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day2.txt")
                .Split(',')
                .Select(s=>int.Parse(s.Trim()));

        public static int Run()
        {
            var data = ReadFile().ToArray();

            data[1] = 12;
            data[2] = 2;

            var computer = new IntCodeComputer(data);
            computer.Run();
            return computer._program[0];
        }

    }


    public class IntCodeComputer
    {

        private int _programCounter = 0;
        public int[] _program;

        public IntCodeComputer(IEnumerable<int> program)
        {
            _program = program.ToArray();
        }



        public void Run()
        {
            while (Process())
            {
                _programCounter += 4;
            }
        }

        public bool Process()
        {
            int instruction = _program[_programCounter];
            switch (instruction)
            {
                case 1:
                    Add();
                    break;
                case 2:
                    Multiply();
                    break;
                case 99:
                    return false;
                default:
                    throw new InvalidOperationException($"Invalid opcode {instruction}");
            }

            return true;
        }

        private void Add()
        {
            var newValue = _program[_program[_programCounter + 1]] + _program[_program[_programCounter + 2]];
            var address = _program[_programCounter + 3];
            _program[address] = newValue;
        }
        private void Multiply()
        {
            var newValue = _program[_program[_programCounter + 1]] * _program[_program[_programCounter + 2]];
            var address = _program[_programCounter + 3];
            _program[address] = newValue;
        }


    }
}
