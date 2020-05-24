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
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("addi");

            sim.RunInstruction(instructions[0]);
            Assert.AreEqual(0xAU, tl.Regs.State.x[1]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(0U, tl.Regs.State.x[1]);
        }

        [TestMethod]
        public void SLTI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("slti");

            sim.RunInstruction(instructions[0]);
            Assert.AreEqual(0xAU, tl.Regs.State.x[1]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(1U, tl.Regs.State.x[2]);

            sim.RunInstruction(instructions[2]);
            Assert.AreEqual(0U, tl.Regs.State.x[2]);
        }
    }
}
