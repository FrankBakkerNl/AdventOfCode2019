using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.VM
{
    enum OpCode
    {
        Add=1,
        Multiply=2,
        Input=3,
        Output = 4,
        JumpIfTrue = 5,
        JumpIfFalse = 6,
        LessThan = 7,
        Equals = 8,
        RelativeBaseOffset = 9,
        Stop = 99
    }

    public class IntCodeComputer
    {
        private BigInteger _programCounter = 0;
        public VirtualMemory Program;
        private BigInteger _relativeBase = 0;
        private int _ticks;

        private readonly Queue<BigInteger> _inputChannel = new Queue<BigInteger>();
        private readonly Queue<BigInteger> _outputChannel = new Queue<BigInteger>();

        public IntCodeComputer(string program) : this(program.Split(',').Select(s=>BigInteger.Parse(s.Trim()))) {}

        public IntCodeComputer(IEnumerable<int> program)  : this(program.Select(i=>(BigInteger)i)) {}
        public IntCodeComputer(IEnumerable<long> program)  : this(program.Select(i=>(BigInteger)i)) {}

        public IntCodeComputer(IEnumerable<BigInteger> program)
        {
            Program = new VirtualMemory(program.ToArray());
        }

        private IntCodeComputer(IntCodeComputer copyFrom)
        {
            _inputChannel = new Queue<BigInteger>(copyFrom._inputChannel);
            _outputChannel = new Queue<BigInteger>(copyFrom._outputChannel);
            _programCounter = copyFrom._programCounter;
            _relativeBase = copyFrom._relativeBase;
            _ticks = copyFrom._ticks;
            Program = copyFrom.Program.Clone();
        }

        public IntCodeComputer CloneState()
        {
            return new IntCodeComputer(this);
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
        public bool IsOutputAvailable => _outputChannel.Any();

        public BigInteger[] ReadAvailableOutput()
        {
            var result = _outputChannel.ToArray();
            _outputChannel.Clear();
            return result;
        }

        public bool Finished { get; private set; }

        public override string ToString() => $"ip:{_programCounter} op:{Program[_programCounter]} ticks:{_ticks}";

        private bool Process()
        {
            var instruction = (OpCode)(int)(Program[_programCounter] % 100);

            return instruction switch
            {
                OpCode.Add =>                Execute((x,y) => x + y),
                OpCode.Multiply =>           Execute((x,y) => x * y),
                OpCode.Input =>              ReadInput(),
                OpCode.Output =>             Execute(x=>_outputChannel.Enqueue(x)),
                OpCode.JumpIfTrue =>         Execute(JumpIfTrue),
                OpCode.JumpIfFalse =>        Execute(JumpIfFalse),
                OpCode.LessThan =>           Execute((x,y) => x < y ? 1 : 0),
                OpCode.Equals =>             Execute((x,y) => x == y ? 1 : 0),
                OpCode.RelativeBaseOffset => Execute(x => { _relativeBase += x; }),
                OpCode.Stop =>               Stop(),
                _ => throw new InvalidOperationException($"Invalid opCode {instruction} at {_programCounter}"),
            };
        }

        private bool Execute(Action<BigInteger> action)
        {
            var param = GetParam(1);
            _programCounter += 2;
            action(param);
            return true;
        }

        private bool Execute(Action<BigInteger, BigInteger> action)
        {
            var p1 = GetParam(1);
            var p2 = GetParam(2);
            _programCounter += 3;
            action(p1, p2);
            return true;
        }

        private bool Execute(Func<BigInteger, BigInteger, BigInteger> func)
        {
            var p1 = GetParam(1);
            var p2 = GetParam(2);
            var addrRes = ResolveAddress(3);
            _programCounter += 4;
            Program[addrRes] = func(p1, p2);
            return true;
        }
        
        private bool Stop()
        {
            Finished = true;
            return false;
        }

        private void JumpIfTrue(BigInteger x, BigInteger y)
        {
            if (x != 0)
            {
                _programCounter = y;
            }
        }

        private void JumpIfFalse(BigInteger x, BigInteger y)
        {
            if (x == 0)
            {
                _programCounter = y;
            }
        }

        private bool ReadInput()
        {
            if (!_inputChannel.Any()) return false;

            var value = _inputChannel.Dequeue();
            Store(value, 1);
            _programCounter += 2;
            return true;
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