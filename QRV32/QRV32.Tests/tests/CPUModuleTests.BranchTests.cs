using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.CPUModuleTests
{

    [TestClass]
    public class BranchTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void BEQ()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("beq");
            sim.RunAll(instructions);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void BNE()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("bne");
            sim.RunAll(instructions);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[3]);
        }

        [TestMethod]
        public void BLT()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("blt");
            sim.RunAll(instructions);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void BLTU()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("bltu");
            sim.RunAll(instructions);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[4]);
        }

        [TestMethod]
        public void BGE()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("bge");
            sim.RunAll(instructions);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[4]);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[5]);
        }

        [TestMethod]
        public void BGEU()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("bgeu");
            sim.RunAll(instructions);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[4]);
            Assert.AreEqual(0xC0FFEEU, tl.Regs.State.x[5]);
        }
    }
}
