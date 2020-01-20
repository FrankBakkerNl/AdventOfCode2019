using System.Linq;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day13
    {
        [Result(348)]
        public int GetAnswer1(string program) //348
        {
            var cpu = new IntCodeComputer(program);
            cpu.Run();
            var output = cpu.ReadAvailableOutput();
            return output.Select((v, i) => (v, i)).Count(t => t.i % 3 ==2 && t.v == 2);
        }

        [Result(16999)]
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
