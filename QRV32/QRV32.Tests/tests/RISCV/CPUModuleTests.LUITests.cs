using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class LUITests : CPUModuleBaseTest
    {
        [TestMethod]
        public void LUI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("lui");
            sim.RunAll(instructions);
            Assert.AreEqual((uint)(0xFF << 12), tl.Regs.State.x[1]);
        }

        [TestMethod]
        public void LI()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("li");
            sim.RunAll(instructions);
            Assert.AreEqual(0x1000U, tl.Regs.State.x[1]);
            Assert.AreEqual(0xFEFEFEFE, tl.Regs.State.x[2]);
            Assert.AreEqual(0xF000FFFF, tl.Regs.State.x[3]);
        }
    }
}
