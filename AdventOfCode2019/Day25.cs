using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day25
    {
        [Result(352325632)]
        public int GetAnswer1(long[] program)
        {
            var cpu = new IntCodeComputer(program);
            var asciiComputer =  new AsciiComputer(cpu);

            return WallFollower(asciiComputer);
        }

        public void Repl(long[] program)
        {

            while (true)
            {
                Console.WriteLine("****** Start New Game! ******");
                var cpu = new IntCodeComputer(program);
                var asciiComputer =  new AsciiComputer(cpu);

                Console.Write(asciiComputer.Run());
                while (!cpu.Finished)
                {
                    var input = Console.ReadLine();
                    if (input == "exit") return;
                    Console.Write(asciiComputer.Run(input));
                }
            }
        }

        public int WallFollower(AsciiComputer cpu)
        {
            //cpu.EchoInput = true;
            //cpu.EchoOutput = true;
            var gameOutput = cpu.Run();

            var facing = Directions.north;
            bool securityVisited = false;

            while (true)
            {
                TakeAllItems(cpu, gameOutput);
                if (gameOutput.Contains("Security Checkpoint"))
                {
                    if (!securityVisited)
                    {
                        // first time here, turn around to get all items
                        facing = (Directions) (((int) facing + 2) % 4);
                        securityVisited = true;
                    }
                    else
                    {
                        return Force(cpu);
                    }
                }
                else
                {
                    var doors = GetDoors(gameOutput);
                    facing = TakeLeft(facing, doors);
                }

                gameOutput = cpu.Run(facing.ToString());
                //Console.ReadKey(true);
            }
        }

        private static readonly string[] UnsafeItems = {"molten lava", "photons", "escape pod", "infinite loop", "giant electromagnet"};

        public void TakeAllItems(AsciiComputer cpu, string gameOutput)
        {
            var itemsHere = GetList(gameOutput, "Items here:");
            foreach (var item in itemsHere.Except(UnsafeItems))
            {
                 cpu.Run("take " + item);
            }
        }

        public Directions[] GetDoors(string gameOutput) =>
            GetList(gameOutput, "Doors here lead:")
                .Select(d => Enum.TryParse(d, out Directions res) ? res : Directions.north).ToArray();

        public enum Directions {north, east, south, west}

        Directions TakeLeft(Directions heading, Directions[] doors)
        {
            // turn left
            var newHeading = ((int)heading + 3) % 4;
            while (!doors.Contains((Directions)newHeading))
            {
                newHeading = (newHeading + 1) % 4;
            }

            return (Directions) newHeading;
        }

        private int Force(AsciiComputer cpu)
        {
            var all = GetInventory(cpu);
            int code = 0;
            ForceEntry(cpu, all.ToImmutableArray(), ref code);
            return code;
        }

        private bool ForceEntry(AsciiComputer cpu, ImmutableArray<string> inventory, ref int code)
        {
            // simple brute force try all combinations
            foreach (var item in inventory)
            {
                cpu.Run("drop " + item);
                var res = TryGetCode(cpu, ref code);
                if (res == 0) return true;
                if (res > 0)
                {
                    if (ForceEntry(cpu, inventory.Remove(item), ref code))
                        return true;
                }

                cpu.Run("take " + item);
            }

            code = 0;
            return false;
        }


        private string[] GetInventory(AsciiComputer cpu)
        {
            var result = cpu.Run("inv");
            return GetList(result, "Items in your inventory:");
        }


        private string[] GetList(string output, string listName)
        {
            var lines = output.Split('\n');
            if (lines.Any(l=>l.Contains("Alert!")))
            {
                lines = lines.SkipWhile(l => !l.Contains("Alert")).ToArray();
            }

            return lines.SkipWhile(l => l != listName).Skip(1).TakeWhile(l => l.StartsWith('-')).Select(l => l.Substring(2))
                .ToArray();
        }

        private int TryGetCode(AsciiComputer cpu, ref int code)
        {
            var result =  cpu.Run("south");
            if (!result.Contains("Alert!"))
            {
                code = int.Parse(Regex.Match(result, "\\d+").Value);
                return 0;
            }

            if (result.Contains("lighter")) return 1;
            if (result.Contains("heavier")) return -1;

            throw new InvalidOperationException("unexpected result");
        }
    }
}
