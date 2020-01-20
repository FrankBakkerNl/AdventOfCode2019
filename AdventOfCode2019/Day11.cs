using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day11
    {
        enum Heading {up, right, down, left};

        [Result(1964)]
        public int GetAnswer1(string input) => GetPainting(input, 0).Count;

        [Result(@"
 #### #  # #### #  #  ##  #### ###  #  #   
 #    # #  #    # #  #  # #    #  # # #    
 ###  ##   ###  ##   #    ###  #  # ##     
 #    # #  #    # #  #    #    ###  # #    
 #    # #  #    # #  #  # #    # #  # #    
 #    #  # #### #  #  ##  #    #  # #  #   
")]//"FKEKCFRK"
        public string GetAnswer2(string input)
        {
            var map = GetPainting(input, 1);
            return Render(map);
        }

        public string Render(Dictionary<(int x, int y), int> tiles)
        {
            var whites = new HashSet<(int, int)>(tiles.Where(t => t.Value == 1).Select(t => t.Key));

            var hight = tiles.Max(t => t.Key.y);
            var with = tiles.Max(t => t.Key.x);
            var sb = new StringBuilder();
            sb.AppendLine();
            for (int i = 0; i <= hight; i++)
            {
                for (int j = 0; j <= with; j++)
                {
                    sb.Append(whites.Contains((j, i)) ? "#" : " ");
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }

        private Dictionary<(int x, int y), int> GetPainting(string input, int initialColor)
        {
            var cpu = new IntCodeComputer(input);
            var map = new Dictionary<(int x, int y), int>();

            var currentPosition = (0, 0);
            map[currentPosition] = initialColor;
            var currentHeading = Heading.up;

            cpu.Run(initialColor);
            while (!cpu.Finished)
            {
                var newColor = cpu.ReadOutput();
                var instruction = cpu.ReadOutput();

                map[currentPosition] = (int)newColor;
                var (newHeading, newPosition) = NewPosition(currentPosition, currentHeading, (int)instruction);
                
                currentHeading = newHeading;
                currentPosition = newPosition;

                var color = map.TryGetValue(currentPosition, out var value) ? value : 0;
                cpu.Run(color);
            }
            return map;
        }

        private (Heading heading, (int, int) position) NewPosition((int x, int y) position, Heading heading, int instruction)
        {
            var newHeading = Turn(heading, instruction);
            return (newHeading, newHeading switch
            {
                Heading.up => (position.x, position.y-1),
                Heading.down => (position.x, position.y+1),
                Heading.left => (position.x-1, position.y ),
                Heading.right => (position.x+1, position.y),
                _ => throw new InvalidOperationException()
            });
        }

        private Heading Turn(Heading heading, int instruction)
        {
            var delta = instruction == 0 ? -1 : +1;
            return (Heading) (((int)heading + delta + 4) % 4);
        }
    }
}
