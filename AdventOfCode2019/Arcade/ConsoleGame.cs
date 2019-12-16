using System;
using System.IO;
using AdventOfCode2019.VM;

namespace AdventOfCode2019.Arcade
{
    class ConsoleGame
    {
        static char[] characters = new[] {' ', '█', 'δ', '═', '*'};

        public void Run()
        {
            var filename = $@"..\..\..\Data\day13.txt";
            var program = File.ReadAllText(filename);

            while (true)
            {
                PlayGame(program);
                if (Console.ReadKey().Key == ConsoleKey.Escape) break;
            }
        }

        public void PlayGame(string program)
        {
            Console.Clear();
            Console.CursorVisible = false;
            var cpu = new IntCodeComputer(program) {Program = {[0] = 2}};
            cpu.Run();

            while (cpu.IsOutputAvailable)
            {
                var instruction = (x:(int)cpu.ReadOutput(), y:(int)cpu.ReadOutput(), v:(int)cpu.ReadOutput());
                if (instruction is (-1, 0, _ ))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Score: {instruction.v}");
                }
                else
                {
                    Console.SetCursorPosition(instruction.x, instruction.y+2);
                    Console.Write(characters[instruction.v]);
                }
                if (!cpu.IsOutputAvailable)
                {
                    var key = Console.ReadKey().Key;
                    var joystick = key switch
                    {
                        ConsoleKey.LeftArrow => -1,
                        ConsoleKey.RightArrow => +1,
                        _ => 0
                    };
                    cpu.Run(joystick);
                }
            }
        }
    }
}
