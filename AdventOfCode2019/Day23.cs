﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2019.VM;

namespace AdventOfCode2019
{
    public class Day23
    {
        public long GetAnswer1(long[] input)
        {
            var cpus = new IntCodeComputer[50];
            for (int i = 0; i < 50; i++)
            {
                cpus[i] = new IntCodeComputer(input);
                cpus[i].Run(i);
            }

            while (true)
            {
                var packets = cpus.Where(c=>c.IsOutputAvailable).Select(ReadPacket).ToArray();
                foreach (var packet in packets)
                {
                    if (packet.addr == 255) return packet.Y;

                    cpus[packet.addr].Run(packet.x, packet.Y);
                }

                var recipients = packets.Select(p => p.addr).Distinct();

                var noInputCpus= cpus.Select((cpu, i)=>(cpu,i))
                    .Where(t=>!recipients.Contains(t.i)).Select(t=>t.cpu);
                foreach (var cpu in noInputCpus)
                {
                    cpu.Run(-1);
                }
            }
        }

        (int addr, long x, long Y) ReadPacket(IntCodeComputer cpu) => 
            ((int)cpu.ReadOutput(), (long)cpu.ReadOutput(), (long)cpu.ReadOutput());
    }
}