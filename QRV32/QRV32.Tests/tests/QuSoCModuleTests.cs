using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.RTL.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRV32.Tests
{
    [TestClass]
    public class QuSoCModuleTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void BlinkieTest()
        {
            var sim = new RTLSimulator<QuSoCModule, QuSoCModuleInputs>();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("blinkie");
            instructions.CopyTo(tl.State.BlockRAM, 0);

            var infiniteLoops = new HashSet<uint>(
                instructions
                .Select((i, idx) => new { i, idx })
                .Where(p => p.i == 0x6F) // j loop code
                .Select(p => (uint)(p.idx * 4))
            );

            while(!infiniteLoops.Contains(tl.CPU.MemAddress))
            {
                if (tl.CPU.State.State == CPUState.Halt)
                    throw new Exception("CPU halted");

                sim.ClockCycle(new QuSoCModuleInputs());
            }
        }
    }
}
