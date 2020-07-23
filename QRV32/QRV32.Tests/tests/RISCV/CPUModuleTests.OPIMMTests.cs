using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using Quokka.RTL;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class OPIMMTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void ADDI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("addi");
            sim.RunAll(instructions);
            Assert.AreEqual(0xAU, tl.Regs.State.x[1]);
            Assert.AreEqual(0x1U, tl.Regs.State.x[2]);
        }

        [TestMethod]
        public void SLTI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("slti");
            sim.RunAll(instructions);
            // compate positive
            Assert.AreEqual(0xAU, tl.Regs.State.x[1]);
            Assert.AreEqual(1U, tl.Regs.State.x[2]);
            Assert.AreEqual(0U, tl.Regs.State.x[3]);
            // compare negative
            Assert.AreEqual(-10, (int)tl.Regs.State.x[4]);
            Assert.AreEqual(1U, tl.Regs.State.x[5]);
            Assert.AreEqual(0U, tl.Regs.State.x[6]);
        }

        [TestMethod]
        public void SLTIU()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("sltiu");
            sim.RunAll(instructions);
            // compate positive
            Assert.AreEqual(-10, (int)tl.Regs.State.x[1]);
            Assert.AreEqual(1U, tl.Regs.State.x[2]);
            Assert.AreEqual(0U, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void ANDI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("andi");
            sim.RunAll(instructions);

            Assert.AreEqual(0xFCU, tl.Regs.State.x[2]);
        }

        [TestMethod]
        public void ORI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("ori");
            sim.RunAll(instructions);
            Assert.AreEqual(0xFFFFFFFFU, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void XORI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("xori");
            sim.RunAll(instructions);
            Assert.AreEqual(3U, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void SLLI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("slli");
            sim.RunAll(instructions);

            var data = new RTLBitArray(uint.MaxValue);
            Assert.AreEqual((uint)(data << 1), tl.Regs.State.x[2]);
            Assert.AreEqual((uint)(data << 31), tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void SRLI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("srli");
            sim.RunAll(instructions);

            var data = new RTLBitArray(uint.MaxValue);
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[2]);
            Assert.AreEqual((uint)(data >> 31), tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void SRAI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("srai");
            sim.RunAll(instructions);

            var data = new RTLBitArray(uint.MaxValue).Signed();
            Assert.AreEqual((uint)(data >> 1), tl.Regs.State.x[2]);
            Assert.AreEqual((uint)(data >> 31), tl.Regs.State.x[3]);
            Assert.AreEqual((uint)(10 >> 1), tl.Regs.State.x[5]);
        }
    }
}
