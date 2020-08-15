using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;

namespace QRV32.Instructions.Tests
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
            Assert.AreEqual(0xF0000004, tl.State.CSR[(byte)CSRAddr.mtvec]);
        }

        [TestMethod]
        public void CSR_RSI_RCI_mstatus()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csr_rsi_rci_mstatus");
            sim.RunAll(instructions);
            Assert.AreEqual(0U, tl.Regs.State.x[2]);
            Assert.AreEqual(0x8U, tl.Regs.State.x[3]);
            Assert.AreEqual(0U, tl.State.CSR[(byte)CSRAddr.mstatus]);
        }

        [TestMethod]
        public void CSRRW_mie()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrw_mie");
            sim.RunAll(instructions);
            Assert.AreEqual(0xFFFFFFFF, tl.State.CSR[(byte)CSRAddr.mie]);
        }

        [TestMethod]
        public void CSRRS_mie()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrs_mie");
            sim.RunAll(instructions);
            Assert.AreEqual(0x80000101, tl.State.CSR[(byte)CSRAddr.mie]);
            Assert.AreEqual(0x1U, tl.Regs.State.x[14]);
            Assert.AreEqual(0x101U, tl.Regs.State.x[15]);
            Assert.AreEqual(0x80000101, tl.Regs.State.x[16]);
        }

        [TestMethod]
        public void CSRRC_mie()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("csrrc_mie");
            sim.RunAll(instructions);
            Assert.AreEqual(0x7FFFFEFEU, tl.State.CSR[(byte)CSRAddr.mie]);
            Assert.AreEqual(0xFFFFFFFF, tl.Regs.State.x[14]);
            Assert.AreEqual(0xFFFFFFFE, tl.Regs.State.x[15]);
            Assert.AreEqual(0xFFFFFEFE, tl.Regs.State.x[16]);
            Assert.AreEqual(0x7FFFFEFEU, tl.Regs.State.x[17]);
        }
    }
}
