using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode2019.VM
{
    public class IntCodeComputer
    {
        private BigInteger _programCounter = 0;
        public VirtualMemory Program;
        private BigInteger _relativeBase = 0;

        private Channel<BigInteger> _inputChannel = Channel.CreateUnbounded<BigInteger>();
        private readonly Channel<BigInteger> _outputChannel = Channel.CreateUnbounded<BigInteger>();

        public IntCodeComputer(string program) : this(program.Split(',')
            .Select(s=>BigInteger.Parse(s.Trim())))
        {}

        public IntCodeComputer(IEnumerable<int> program)
        {
            Program = new VirtualMemory(program.ToArray());
        }

        public IntCodeComputer(IEnumerable<BigInteger> program)
        {
            Program = new VirtualMemory(program.ToArray());
        }

        public ValueTask<BigInteger> ReadOutputAsync() => _outputChannel.Reader.ReadAsync();

        public ValueTask WriteInputAsync(BigInteger value) => _inputChannel.Writer.WriteAsync(value);

        public BigInteger ReadOutput() => ReadOutputAsync().AsTask().Result;

        public void WriteInput(BigInteger value) => WriteInputAsync(value).AsTask().Wait();

        public IAsyncEnumerable<BigInteger> Output => _outputChannel.Reader.ReadAllAsync();

        public void PipeTo(IntCodeComputer next)
        {
            next._inputChannel = _outputChannel;
        }

        public void Run(int input)
        {
            WriteInput(input);
            Run();
        }

        public void Run()
        {
            if (!RunAsync().IsCompleted)
            {
                throw new InvalidOperationException($"Program cannot finish, blocked at instruction {_programCounter}:{Program[_programCounter]}");
            }
        }

        public async Task RunAsync()
        {
            while (await Process())
            {}
            _outputChannel.Writer.Complete();
        }

        public async Task<bool> Process()
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
                    await ReadInput();
                    return true;
                case 4:
                    await WriteOutput();
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

        private async Task ReadInput()
        {
            var value = await _inputChannel.Reader.ReadAsync();
            Store(value, 1);
            _programCounter += 2;
        }

        private async Task WriteOutput()
        {
            await _outputChannel.Writer.WriteAsync(GetParam(1));
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