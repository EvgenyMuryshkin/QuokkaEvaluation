using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Tests;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class CSRTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void CSRRW_mvendorid()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrw_mvendorid");
            sim.RunAll(instructions);
            Assert.AreEqual(0U, tl.Regs.State.x[1]);
        }

        [TestMethod]
        public void CSRRW_marchid()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrw_marchid");
            sim.RunAll(instructions);
            Assert.AreEqual(0x0FA57DB9U, tl.Regs.State.x[1]);
        }

        [TestMethod]
        public void CSRRW_mimpid()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrw_mimpid");
            sim.RunAll(instructions);
            Assert.AreEqual(0x01010101U, tl.Regs.State.x[1]);
        }

        [TestMethod]
        public void CSRRW_mhartid()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrw_mhartid");
            sim.RunAll(instructions);
            Assert.AreEqual(0x0U, tl.Regs.State.x[1]);
        }

        [TestMethod]
        public void CSRRW_mtvec()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrw_mtvec");
            sim.RunAll(instructions);
            Assert.AreEqual(0xF0000004, tl.Regs.State.x[1]);
            Assert.AreEqual(0xF0000004, tl.Regs.State.x[3]);
            Assert.AreEqual(0xF0000004, tl.State.CSR[7]);
        }
    }
}
