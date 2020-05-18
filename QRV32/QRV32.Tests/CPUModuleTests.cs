using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.RTL.Simulator;
using System;

namespace QRV32.Tests
{
    class CPUSimulator : RTLSimulator<CPUModule, CPUModuleInputs>
    {
        public CPUSimulator()
        {

        }

        public void RunTillInstructionFetch()
        {
            int counter = 0;

            while (!TopLevel.MemRead)
            {
                if (++counter > 100)
                    throw new Exception("CPU seems to hang");

                switch (TopLevel.State.State)
                {
                    case CPUState.Halt:
                        throw new Exception($"Halted");
                    case CPUState.MEM:
                        // TODO: implement memory operation
                        break;
                    case CPUState.IF:
                        return;
                }

                ClockCycle();
            }
        }
    }

    [TestClass]
    public class CPUModuleTests
    {
        [TestMethod]
        public void ResetTest()
        {
            var sim = new CPUSimulator();            
            var tl = sim.TopLevel;
            Assert.IsFalse(tl.MemRead);
            sim.ClockCycle(new CPUModuleInputs() { BaseAddress = 0xF0000000 });
            Assert.IsTrue(tl.MemRead);
            Assert.AreEqual(0xF0000000, tl.MemAddress);
        }

        [TestMethod]
        public void ADDI()
        {
            var sim = new CPUSimulator();
            var tl = sim.TopLevel;
            sim.ClockCycle();
            // ADDI r1, r0, 10
            sim.ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadValue = 0xA00093 });

            sim.RunTillInstructionFetch();

            Assert.AreEqual((uint)0xA, tl.Regs.State.x[1]);
        }
    }
}
