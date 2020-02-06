using System;
using System.Collections.Generic;
using System.Linq;

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
        public VirtualMemory Program;
        private long _programCounter;
        private long _relativeBase;
        private int _ticks;
        private int _argumentModes;

        private readonly Queue<long> _inputChannel = new Queue<long>();
        private readonly Queue<long> _outputChannel = new Queue<long>();

        public IntCodeComputer(string program) : this(program.Split(',').Select(s=>long.Parse(s.Trim()))) {}

        public IntCodeComputer(IEnumerable<int> program)  : this(program.Select(i=>(long)i)) {}

        public IntCodeComputer(IEnumerable<long> program)
        {
            Program = new VirtualMemory(program);
        }

        private IntCodeComputer(IntCodeComputer copyFrom)
        {
            _inputChannel = new Queue<long>(copyFrom._inputChannel);
            _outputChannel = new Queue<long>(copyFrom._outputChannel);
            _programCounter = copyFrom._programCounter;
            _relativeBase = copyFrom._relativeBase;
            _ticks = copyFrom._ticks;
            Program = copyFrom.Program.Clone();
        }

        public IntCodeComputer CloneState() => new IntCodeComputer(this);

        public void Run()
        { 
            while (Process()) _ticks++;

            // last operation was Stop or blocking Input so reset _programCounter
            _programCounter--;
        }

        public void Run(params long[] input)
        {
            foreach (var value in input)
            {
                _inputChannel.Enqueue(value);
            }

            Run();
        }

        public long ReadOutput() => _outputChannel.Dequeue();
        public bool IsOutputAvailable => _outputChannel.Any();

        public long[] ReadAvailableOutput()
        {
            var result = _outputChannel.ToArray();
            _outputChannel.Clear();
            return result;
        }

        public bool Finished => Program[_programCounter] == 99;

        public override string ToString() => $"ip:{_programCounter} op:{Program[_programCounter]} ticks:{_ticks}";

        private bool Process()
        {
            var instruction = (int)(Program[_programCounter++]);
            _argumentModes = instruction / 100;

            return (OpCode)(instruction %100) switch
            {
                OpCode.Add =>                Execute( (x, y) => x + y),
                OpCode.Multiply =>           Execute( (x, y) => x * y),
                OpCode.Input =>              ReadInput(),
                OpCode.Output =>             Execute( x=>_outputChannel.Enqueue(x)),
                OpCode.JumpIfTrue =>         Execute( (x, y) => { if (x != 0) _programCounter = y;}),
                OpCode.JumpIfFalse =>        Execute( (x, y) => { if (x == 0) _programCounter = y;}),
                OpCode.LessThan =>           Execute( (x, y) => x < y ? 1 : 0),
                OpCode.Equals =>             Execute( (x, y) => x == y ? 1 : 0),
                OpCode.RelativeBaseOffset => Execute( x => { _relativeBase += x; }),
                OpCode.Stop =>               false,
                _ => throw new InvalidOperationException($"Invalid opCode {instruction} at {_programCounter-1}"),
            };
        }

        private bool Execute(Action<long> action)
        {
            action(GetParam());
            return true;
        }

        private bool Execute(Action<long, long> action)
        {
            action(GetParam(), GetParam());
            return true;
        }

        private bool Execute(Func<long, long, long> func)
        {
            SetParam(func(GetParam(), GetParam()));
            return true;
        }
        
        private bool ReadInput()
        {
            if (!_inputChannel.Any()) return false;

            SetParam(_inputChannel.Dequeue());
            return true;
        }

        long GetParam() => Program[ResolveAddress()];
        void SetParam(long val) => Program[ResolveAddress()] = val;
        
        long ResolveAddress()
        {
            var mode = _argumentModes % 10;
            _argumentModes /= 10;

            return mode switch
            {
                0 => Program[_programCounter++],                 // position mode
                1 => _programCounter++,                          // immediate mode
                2 => Program[_programCounter++] + _relativeBase, // relative mode
                _ => throw new InvalidOperationException($"Invalid argument mode {mode}")
            };
        }
    }
}