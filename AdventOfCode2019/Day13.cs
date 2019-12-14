using System.Linq;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    class Day13
    {
        public int GetAnswer1_skip(string program)
        {
            var cpu = new IntCodeComputer(program);
            cpu.Run();
            var output = cpu.ReadAvailableOutput();
            return Enumerable.Range(0, output.Length / 3).Select(i => output[i * 3 + 2]).Count(i => i == 2);
        }

        public int GetAnswer2(string program)
        {
            var cpu = new IntCodeComputer(program);
            cpu.Program[0] = 2;
            cpu.Run();

            var ballX = 0;
            var peddleX = 0;
            var score = 0;

            while (cpu.IsOutputAvailable)
            {
                var instruction = (x:(int)cpu.ReadOutput(), y:(int)cpu.ReadOutput(), v:(int)cpu.ReadOutput());
                switch (instruction)
                {
                    case (-1, 0, _): // keep score
                        score = instruction.v;
                        break;
                    case (_, _, 3): // draw peddle
                        peddleX = instruction.x;
                        break;
                    case (_, _, 4): // draw Ball 
                        ballX = instruction.x;
                        break;
                }
                if (!cpu.IsOutputAvailable)
                {
                    var joystick = peddleX > ballX ? -1 :
                                   peddleX < ballX ?  1 : 0;
                    cpu.Run(joystick);
                }
            }
            return score;
        }
    }
}
