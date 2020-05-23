using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;

namespace QRV32.Tests
{
    [TestClass]
    public class CPUModuleTests : CPUModuleBaseTest
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

            var instructions = FromAsmFile("addi");

            // ADDI r1, r0, 10
            sim.RunInstruction(instructions[0]);
            Assert.AreEqual((uint)0xA, tl.Regs.State.x[1]);

            // ADDI r1, r1, FF6 (-10)
            sim.RunInstruction(instructions[1]);
            Assert.AreEqual((uint)0, tl.Regs.State.x[1]);
        }
    }
}
