using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class IntCodeComputer
    {
        private int _programCounter = 0;
        public int[] Program;

        public Queue<int> Input { get; set; } = new Queue<int>();
        public List<int> Output { get; } = new List<int>();


        public IntCodeComputer(string program) : this(program.Split(',')
            .Select(s=>int.Parse(s.Trim())))
        {}

        public IntCodeComputer(IEnumerable<int> program)
        {
            Program = program.ToArray();
        }

        public void Run(int input)
        {
            Input.Enqueue(input);
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
                    return true;
                case 2:
                    Multiply();
                    return true;
                case 3:
                    ReadInput();
                    return true;
                case 4:
                    WriteOutput();
                    return true;
                case 5:
                    JumpIfTrue();
                    return true;
                case 6:
                    JumpIfFalse();
                    return true;
                case 7:
                    LessThan();
                    return true;
                case 8:
                    Equals();
                    return true;
                case 99:
                    return false;
                default:
                    throw new InvalidOperationException($"Invalid opcode {instruction} at {_programCounter}");
            }
        }

        private void JumpIfTrue()
        {
            if (GetParam(1) != 0)
            {
                _programCounter = GetParam(2);
            }
            else
            {
                _programCounter += 3;
            }
        }

        private void LessThan()
        {
            Store(GetParam(1) < GetParam(2) ? 1 : 0, 3);
            _programCounter += 4;
        }

        private void Equals()
        {
            Store(GetParam(1) == GetParam(2) ? 1 : 0, 3);
            _programCounter += 4;
        }


        private void JumpIfFalse()
        {
            if (GetParam(1) == 0)
            {
                _programCounter = GetParam(2);
            }
            else
            {
                _programCounter += 3;
            }
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
            Store(Input.Dequeue(), 1);
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


        public override string ToString()
        {
            var start = string.Join(',', Program.Take(_programCounter));
            var rest = string.Join(',', Program.Skip(_programCounter));
            return start + ">>" + rest;
        }
    }
}