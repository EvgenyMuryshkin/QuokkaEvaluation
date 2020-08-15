using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.Instructions.Tests
{
    [TestClass]
    public class JALTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void JAL()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("jal");
            sim.RunAll(instructions);

            Assert.AreEqual(0xCU, tl.Regs.State.x[1]);
            Assert.AreEqual(0x100U, (uint)tl.State.PC);
        }

        [TestMethod]
        public void J()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("j");
            sim.RunAll(instructions);

            Assert.IsTrue(tl.Regs.State.x.All(x => x == 0));
            Assert.AreEqual(0x100U, (uint)tl.State.PC);
        }
    }
}
