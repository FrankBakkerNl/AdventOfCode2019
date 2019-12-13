using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.VM
{
    public class IntCodeComputer
    {
        private BigInteger _programCounter = 0;
        public VirtualMemory Program;
        private BigInteger _relativeBase = 0;
        private int _ticks;

        private readonly Queue<BigInteger> _inputChannel = new Queue<BigInteger>();
        private readonly Queue<BigInteger> _outputChannel = new Queue<BigInteger>();

        public IntCodeComputer(string program) : this(program.Split(',').Select(s=>BigInteger.Parse(s.Trim())))
        {}

        public IntCodeComputer(IEnumerable<int> program)  : this(program.Select(i=>(BigInteger)i))
        {}

        public IntCodeComputer(IEnumerable<BigInteger> program)
        {
            Program = new VirtualMemory(program.ToArray());
        }

        public void Run()
        { 
            while (Process()) 
            {
              _ticks++;
            }
        }

        public void Run(params BigInteger[] input)
        {
            foreach (var value in input)
            {
                _inputChannel.Enqueue(value);
            }

            Run();
        }

        public BigInteger ReadOutput() => _outputChannel.Dequeue();

        public BigInteger[] ReadAvailableOutput()
        {
            var result = _outputChannel.ToArray();
            _outputChannel.Clear();
            return result;
        }

        public bool Finished { get; private set; }

        public override string ToString() => $"ip:{_programCounter} op:{Program[_programCounter]} ticks:{_ticks}";

        // done, wait for input, produce output
        private bool Process()
        {
            _ticks++;
            int instruction = (int)Program[_programCounter] % 100;
            switch (instruction)
            {
                case 1:
                    Add();
                    return true;
                case 2:
                    Multiply();
                    return true;
                case 3:
                    return ReadInput();
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
                case 9:
                    RelativeBaseOffset();
                    return true;
                case 99:
                    Finished = true;
                    return false;
                default:
                    throw new InvalidOperationException($"Invalid opCode {instruction} at {_programCounter}");
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

        private bool ReadInput()
        {
            if (!_inputChannel.Any()) return false;
            var value = _inputChannel.Dequeue();
            Store(value, 1);
            _programCounter += 2;
            return true;
        }

        private void WriteOutput()
        {
            _outputChannel.Enqueue(GetParam(1));
            _programCounter += 2;
        }

        private void RelativeBaseOffset()
        {
            _relativeBase += GetParam(1);
            _programCounter += 2;
        }

        void Store(BigInteger value, int number)
        {
            Program[ResolveAddress(number)] = value;
        }

        BigInteger GetParam(int number) => Program[ResolveAddress(number)];
        
        BigInteger ResolveAddress(int argumentNumber) =>
            Mode(argumentNumber) switch
            {
                1 => _programCounter + argumentNumber,
                2 => Program[_programCounter + argumentNumber] + _relativeBase, // relative mode
                _ => Program[_programCounter + argumentNumber]
            };

        int Mode(int arg) => GetDigit((int)Program[_programCounter], arg+1);

        int GetDigit(int input, int number) => input / (int) Math.Pow(10, number) % 10;
    }
}