using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    [Test]
    public class Day25
    {
        public int GetAnswer1(long[] program)
        {
            var cpu = new IntCodeComputer(program.Select(i => (BigInteger) i));
            var asciiComputer =  new AsciiComputer(cpu);
            GetAllItemsAndGoToSecurity(asciiComputer);
            return Force(asciiComputer);

        }

        public void Repl(long[] program)
        {

            while (true)
            {
                Console.WriteLine("****** Start New Game! ******");
                var cpu = new IntCodeComputer(program.Select(i => (BigInteger) i));
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

        private void GetAllItemsAndGoToSecurity(AsciiComputer cpu)
        {
            foreach (var command in GetAllItemsScript)
            {
                //Console.WriteLine(">>" + command);
                var output = cpu.Run(command);
                //Console.Write(output);
            }
        }


        public string[] GetAllItemsScript = new[]
        {
            "east",
            "take loom",
            "south",
            "take ornament",
            "west",
            "north",
            "take candy cane",
            "south",
            "east",
            "north",
            "east",
            "take fixed point",
            "north",
            "take spool of cat6",
            "north",
            "take weather machine",
            "south",
            "west",
            "take shell",
            "east",
            "south",
            "west",
            "west",
            "north",
            "take wreath",
            "north",
            "east"
        };

    }

    public class AsciiComputer
    {
        private readonly IntCodeComputer _cpu;

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
            return new string(_cpu.ReadAvailableOutput().Select(i => (char) (int) i).ToArray());
        }
    }
}
