using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.RTL;

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

            // compate positive
            sim.RunInstruction(instructions[0]);
            Assert.AreEqual(0xAU, tl.Regs.State.x[1]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(1U, tl.Regs.State.x[2]);

            sim.RunInstruction(instructions[2]);
            Assert.AreEqual(0U, tl.Regs.State.x[2]);

            // compare negative
            sim.RunInstruction(instructions[3]);
            Assert.AreEqual(-10, (int)tl.Regs.State.x[3]);

            sim.RunInstruction(instructions[4]);
            Assert.AreEqual(1U, tl.Regs.State.x[4]);

            sim.RunInstruction(instructions[5]);
            Assert.AreEqual(0U, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void SLTIU()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("sltiu");

            // compate positive
            sim.RunInstruction(instructions[0]);
            Assert.AreEqual(-10, (int)tl.Regs.State.x[1]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(1U, tl.Regs.State.x[2]);

            sim.RunInstruction(instructions[2]);
            Assert.AreEqual(0U, tl.Regs.State.x[2]);
        }

        [TestMethod]
        public void ANDI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("andi");

            // compate positive
            sim.RunInstruction(instructions[0]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(0xFCU, tl.Regs.State.x[2]);
        }

        [TestMethod]
        public void ORI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("ori");

            // compate positive
            sim.RunInstruction(instructions[0]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(0xFFFFFFFFU, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void XORI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("xori");

            // compate positive
            sim.RunInstruction(instructions[0]);

            sim.RunInstruction(instructions[1]);
            Assert.AreEqual(3U, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void SLLI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("slli");

            var data = new RTLBitArray(uint.MaxValue);

            sim.RunInstruction(instructions[0]);
            sim.RunInstruction(instructions[1]);
            Assert.AreEqual((uint)(data << 1), tl.Regs.State.x[2]);

            sim.RunInstruction(instructions[2]);
            Assert.AreEqual((uint)(data << 31), tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void SRLI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("srli");

            var data = new RTLBitArray(uint.MaxValue);

            sim.RunInstruction(instructions[0]);
            sim.RunInstruction(instructions[1]);
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[2]);

            sim.RunInstruction(instructions[2]);
            Assert.AreEqual((uint)(data >> 31), tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void SRAI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("srai");

            var data = new RTLBitArray(uint.MaxValue).Signed();

            sim.RunInstruction(instructions[0]);
            sim.RunInstruction(instructions[1]);
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[2]);

            sim.RunInstruction(instructions[2]);
            Assert.AreEqual((uint)(data >> 31), tl.Regs.State.x[3]);

            sim.RunInstruction(instructions[3]);
            sim.RunInstruction(instructions[4]);
            Assert.AreEqual((uint)(10 >> 1), tl.Regs.State.x[5]);
        }
    }
}
