using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Tests;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class LoadTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void LB()
        {
            var sim = PowerUp();
            sim.MemoryBlock[0x100] = 0xF4030201;

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
            sim.MemoryBlock[0x100] = 0xF4030201;

            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lbu");
            sim.RunAll(instructions);
            Assert.AreEqual(0x1U, tl.Regs.State.x[2]);
            Assert.AreEqual(0x2U, tl.Regs.State.x[3]);
            Assert.AreEqual(0x3U, tl.Regs.State.x[4]);
            Assert.AreEqual(0xF4U, tl.Regs.State.x[5]);
        }
    }
}
