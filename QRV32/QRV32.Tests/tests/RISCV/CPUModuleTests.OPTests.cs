using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using Quokka.RTL;

namespace QRV32.Instructions.Tests
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

        [TestMethod]
        public void ANDI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("and");
            sim.RunAll(instructions);
            Assert.AreEqual(0xFCU, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void OR()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("or");
            sim.RunAll(instructions);
            Assert.AreEqual(0xFFFFFFFFU, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void XOR()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("xor");
            sim.RunAll(instructions);
            Assert.AreEqual(3U, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void SLL()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("sll");
            sim.RunAll(instructions);

            var data = new RTLBitArray(uint.MaxValue);
            Assert.AreEqual((uint)(data << 1), tl.Regs.State.x[6]);
            Assert.AreEqual((uint)(data << 31), tl.Regs.State.x[7]);
            Assert.AreEqual((uint)data, tl.Regs.State.x[8]);
            Assert.AreEqual((uint)(data << 1), tl.Regs.State.x[9]);
        }

        [TestMethod]
        public void SRL()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("srl");
            sim.RunAll(instructions);

            var data = new RTLBitArray(uint.MaxValue);
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[6]);
            Assert.AreEqual((uint)(data >> 31), tl.Regs.State.x[7]);
            Assert.AreEqual((uint)data, tl.Regs.State.x[8]);
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[9]);
        }

        [TestMethod]
        public void SRA()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("sra");
            sim.RunAll(instructions);

            var data = new RTLBitArray(uint.MaxValue).Signed();
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[6]);
            Assert.AreEqual((uint)(data >> 31), tl.Regs.State.x[7]);
            Assert.AreEqual((uint)data, tl.Regs.State.x[8]);
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[9]);
            Assert.AreEqual(5U, tl.Regs.State.x[10]);
        }

        /*
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
