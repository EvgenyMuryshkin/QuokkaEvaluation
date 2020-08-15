using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Tests;
using System.Linq;

namespace QRV32.Instructions.Tests
{
    [TestClass]
    public class LoadTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void LB()
        {
            var sim = PowerUp();
            sim.MemoryBlock[0x40] = 0xF4030201;

            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lb");
            sim.RunAll(instructions);
            Assert.AreEqual(0x1U, tl.Regs.State.x[2]);
            Assert.AreEqual(0x2U, tl.Regs.State.x[3]);
            Assert.AreEqual(0x3U, tl.Regs.State.x[4]);
            Assert.AreEqual(0xFFFFFFF4U, tl.Regs.State.x[5]);
        }

        [TestMethod]
        public void LBU()
        {
            var sim = PowerUp();
            sim.MemoryBlock[0x40] = 0xF4030201;

            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lbu");
            sim.RunAll(instructions);
            Assert.AreEqual(0x1U, tl.Regs.State.x[2]);
            Assert.AreEqual(0x2U, tl.Regs.State.x[3]);
            Assert.AreEqual(0x3U, tl.Regs.State.x[4]);
            Assert.AreEqual(0xF4U, tl.Regs.State.x[5]);
        }

        [TestMethod]
        public void LH()
        {
            var sim = PowerUp();
            sim.MemoryBlock[0x3F] = 0xE4030201U;
            sim.MemoryBlock[0x40] = 0xF4030201U;
            sim.MemoryBlock[0x41] = 0x07030201U;

            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lh");
            sim.RunAll(instructions);
            Assert.AreEqual(0x0201U, tl.Regs.State.x[2]);
            Assert.AreEqual(0xFFFFF403U, tl.Regs.State.x[3]);
            Assert.AreEqual(0xFFFFE403U, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void LHU()
        {
            var sim = PowerUp();
            sim.MemoryBlock[0x3F] = 0xE4030201U;
            sim.MemoryBlock[0x40] = 0xF4030201U;
            sim.MemoryBlock[0x41] = 0x07030201U;

            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lhu");
            sim.RunAll(instructions);
            Assert.AreEqual(0x0201U, tl.Regs.State.x[2]);
            Assert.AreEqual(0xF403U, tl.Regs.State.x[3]);
            Assert.AreEqual(0xE403U, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void LW()
        {
            var sim = PowerUp();
            sim.MemoryBlock[0x3F] = 0xE4030201U;
            sim.MemoryBlock[0x40] = 0xF4030201U;
            sim.MemoryBlock[0x41] = 0x07030201U;

            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lw");
            sim.RunAll(instructions);
            Assert.AreEqual(0xE4030201U, tl.Regs.State.x[2]);
            Assert.AreEqual(0xF4030201U, tl.Regs.State.x[3]);
            Assert.AreEqual(0x07030201U, tl.Regs.State.x[4]);
        }
    }
}
