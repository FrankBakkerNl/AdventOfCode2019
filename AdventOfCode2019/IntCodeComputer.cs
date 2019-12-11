using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019
{
    public class IntCodeComputer
    {
        private BigInteger _programCounter = 0;
        public VirtualMemory Program;
        private BigInteger _relativeBase = 0;

        public BlockingCollection<BigInteger> Input { get; set; } = new BlockingCollection<BigInteger>(new ConcurrentQueue<BigInteger>());
        public BlockingCollection<BigInteger> Output { get; } = new BlockingCollection<BigInteger>(new ConcurrentQueue<BigInteger>());


        public IntCodeComputer(string program) : this(program.Split(',')
            .Select(s=>int.Parse(s.Trim())))
        {}

        public IntCodeComputer(IEnumerable<int> program)
        {
            Program = new VirtualMemory(program.ToArray());
        }

        public IntCodeComputer(IEnumerable<BigInteger> program)
        {
            Program = new VirtualMemory(program.ToArray());
        }

        public IntCodeComputer(IEnumerable<int> program, BlockingCollection<BigInteger> input)
        {
            Program = new VirtualMemory(program.ToArray());
            Input = input;
        }

        public void PipeTo(IntCodeComputer next)
        {
            next.Input = Output;
        }

        public void Run(int input)
        {
            Input.Add(input);
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
                case 9:
                    RelativeBaseOffset();
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
            Store(Input.Take(), 1);
            _programCounter += 2;
        }

        private void WriteOutput()
        {
            Output.Add(GetParam(1));
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


        //public override string ToString()
        //{
        //    var start = string.Join(',', Program.Take(_programCounter));
        //    var rest = string.Join(',', Program.Skip(_programCounter));
        //    return start + ">>" + rest;
        //}
    }

    public class VirtualMemory : IEnumerable<BigInteger>
    {
        public VirtualMemory(IEnumerable<int> load) : this(load.Select(i=>(BigInteger)i))
        {}

        public VirtualMemory(IEnumerable<BigInteger> load)
        {
            _store = load.Select((v, i) => (i, v)).ToDictionary(t => (BigInteger)t.i, t => t.v);
        }

        readonly Dictionary<BigInteger, BigInteger> _store;

        public BigInteger this[BigInteger address]
        {
            get => _store.TryGetValue(address, out var res) ? res : 0;
            set => _store[address] = value;
        }

        public IEnumerator<BigInteger> GetEnumerator() => _store.OrderBy(kv=>kv.Key).Select(kv=>kv.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}