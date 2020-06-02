using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class ETests : CPUModuleBaseTest
    {
        [TestMethod]
        public void ECall()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("ecall");
            sim.RunAll(instructions);
            Assert.AreEqual(2, sim.ECalls.Count());
            Assert.AreEqual(1U, sim.ECalls[0]);
            Assert.AreEqual(10U, sim.ECalls[1]);
            Assert.AreEqual(1U, tl.Regs.State.x[1]);
            Assert.AreEqual(2U, tl.Regs.State.x[2]);
        }

        [TestMethod]
        public void EBreak()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("ebreak");
            sim.RunAll(instructions);
            Assert.AreEqual(2, sim.DebuggerCalls);
            Assert.AreEqual(1U, tl.Regs.State.x[1]);
            Assert.AreEqual(2U, tl.Regs.State.x[2]);
        }
    }
}
