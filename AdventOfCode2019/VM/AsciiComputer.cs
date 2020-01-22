using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.VM
{
    public class AsciiComputer
    {
        private readonly IntCodeComputer _cpu;
        public long ResultCode { get; private set; }

        public AsciiComputer(IntCodeComputer cpu)
        {
            _cpu = cpu;
        }

        public string Run()
        {
            _cpu.Run();
            return new string(_cpu.ReadAvailableOutput().Select(i => (char) (int) i).ToArray());
        }

        public string Run(string command)
        {
            _cpu.Run(command.Select(i => ((BigInteger) (int) i)).ToArray());
            _cpu.Run(10);
            var output = _cpu.ReadAvailableOutput();
            ResultCode = (long)output.LastOrDefault();

            return new string(output.Where(o=>o<256).Select(i => (char) (int) i).ToArray());
        }
    }
}