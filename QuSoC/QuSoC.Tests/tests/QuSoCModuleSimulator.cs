using QuSoC;
using Quokka.RTL.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using QRV32.CPU;

namespace QuSoC.Tests
{
    public class QuSoCModuleSimulator : RTLInstanceSimulator<QuSoCModule, QuSoCModuleInputs>
    {
        protected HashSet<uint> InfiniteLoopAddresses = new HashSet<uint>();

        public QuSoCModuleSimulator(uint[] instructions) : base(new QuSoCModule(instructions))
        {
            InfiniteLoopAddresses.AddRange(
                instructions
                .Select((i, idx) => new { i, idx })
                .Where(p => p.i == 0x6F) // j loop code
                .Select(p => (uint)(p.idx * 4))
            );            
        }

        public void RunToCompletion(uint maxClockCycles = 10000)
        {
            RunToCompletion(null, maxClockCycles);
        }

        public void RunToCompletion(Func<bool> keepRunningCheck, uint maxClockCycles = 10000)
        {
            uint clockCycles = 0;
            while (!InfiniteLoopAddresses.Contains(TopLevel.CPU.MemAddress))
            {
                if (keepRunningCheck != null && !keepRunningCheck())
                {
                    break;
                }

                if (clockCycles++ == maxClockCycles)
                    throw new Exception($"Exceeded max allowed clock cycles: {maxClockCycles}");

                if (TopLevel.CPU.State.State == CPUState.Halt)
                    throw new Exception("CPU halted");

                ClockCycle(new QuSoCModuleInputs());
            }
        }

        public List<string> MemoryDump()
        {
            var memDump = TopLevel.State.BlockRAM
                .Select((data, idx) => new { data, idx = idx * 4 })
                .Where(d => d.data != 0)
                .Select(d => $"[{d.idx:X6}]: {d.data:X8}")
                .ToList();

            return memDump;
        }
    }
}
