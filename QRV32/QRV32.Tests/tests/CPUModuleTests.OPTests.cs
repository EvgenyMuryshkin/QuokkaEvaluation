using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using Quokka.RTL;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class OPTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void ADD()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            sim.RunAll(Inst.FromAsmFile("add"));
            Assert.AreEqual(0U, tl.Regs.State.x[5]);
            Assert.AreEqual(1U, tl.Regs.State.x[6]);
            Assert.AreEqual(uint.MaxValue, tl.Regs.State.x[7]);
            Assert.AreEqual(20U, tl.Regs.State.x[8]);
        }

        [TestMethod]
        public void SUB()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            sim.RunAll(Inst.FromAsmFile("sub"));
            Assert.AreEqual(0U, tl.Regs.State.x[5]);
            Assert.AreEqual(1U, tl.Regs.State.x[6]);
            Assert.AreEqual(uint.MaxValue, tl.Regs.State.x[7]);
            Assert.AreEqual(unchecked((uint)(-20)), tl.Regs.State.x[9]);
        }

        [TestMethod]
        public void SLT()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            sim.RunAll(Inst.FromAsmFile("slt"));
            Assert.AreEqual(0U, tl.Regs.State.x[10]);
            Assert.AreEqual(1U, tl.Regs.State.x[11]);
            Assert.AreEqual(1U, tl.Regs.State.x[12]);
            Assert.AreEqual(0U, tl.Regs.State.x[13]);
        }

        [TestMethod]
        public void SLTU()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            sim.RunAll(Inst.FromAsmFile("sltu"));
            Assert.AreEqual(1U, tl.Regs.State.x[4]);
            Assert.AreEqual(0U, tl.Regs.State.x[5]);
        }

        /*

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
        */
    }
}
