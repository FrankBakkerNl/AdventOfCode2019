using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2019
{
    public class IntCodeComputer
    {
        private int _programCounter = 0;
        public int[] Program;

        public int Input { get; set; }
        public List<int> Output { get; } = new List<int>();


        public IntCodeComputer(string program) : this(program.Split(',')
            .Select(s=>int.Parse(s.Trim())))
        {
        }

        public IntCodeComputer(IEnumerable<int> program)
        {
            Program = program.ToArray();
        }

        public void Run(int input)
        {
            Input = input;
            Run();
        }

        public void Run()
        {
            while (Process())
            {
            }
        }

        public bool Process()
        {
            int instruction = Program[_programCounter] % 100;
            switch (instruction)
            {
                case 1:
                    Add();
                    break;
                case 2:
                    Multiply();
                    break;
                case 3:
                    ReadInput();
                    break;
                case 4:
                    WriteOutput();
                    break;
                case 99:
                    return false;
                default:
                    throw new InvalidOperationException($"Invalid opcode {instruction} at {_programCounter}");
            }

            return true;
        }

        private void Add()
        {
            Store(GetParam(1) + GetParam(2), 3);
            _programCounter += 4;
        }

        private void Multiply()
        {
            Store(GetParam(1) * GetParam(2), 3);
            _programCounter += 4;
        }

        private void ReadInput()
        {
            Store(Input, 1);
            _programCounter += 2;
        }

        private void WriteOutput()
        {
            Output.Add(GetParam(1));
            _programCounter += 2;
        }

        void Store(int value, int number)
        {
            Program[Program[_programCounter + number]] = value;
        }

        int GetParam(int number) => Mode(number) == 1 ? Program[_programCounter + number] 
                                                      : Program[Program[_programCounter + number]];

        int Mode(int arg) => GetDigit(Program[_programCounter], arg+1);

        int GetDigit(int input, int number) => input / (int) Math.Pow(10, number) % 10;


    }
}