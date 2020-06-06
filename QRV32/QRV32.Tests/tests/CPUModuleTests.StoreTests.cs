using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Tests;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class StoreTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void SB()
        {
            var sim = PowerUp();
            var instructions = Inst.FromAsmFile("sb");
            sim.RunAll(instructions);
            Assert.AreEqual(0xBADC0FFE, sim.MemoryBlock[0x40]);
        }

        [TestMethod]
        public void SH()
        {
            var sim = PowerUp();
            var instructions = Inst.FromAsmFile("sh");
            sim.RunAll(instructions);
            Assert.AreEqual(0xBADC0FFE, sim.MemoryBlock[0x40]);
        }

        [TestMethod]
        public void SW()
        {
            var sim = PowerUp();
            var instructions = Inst.FromAsmFile("sw");
            sim.RunAll(instructions);
            Assert.AreEqual(0xBADC0FFEU, sim.MemoryBlock[0x40]);
            Assert.AreEqual(0x0A11C001U, sim.MemoryBlock[0x3F]);
        }
    }
}
