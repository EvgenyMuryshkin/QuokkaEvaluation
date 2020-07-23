using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.CPUModuleTests
{

    [TestClass]
    public class AUIPCTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void AUIPC()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("auipc");
            sim.RunAll(instructions);
            Assert.AreEqual((uint)(0x1008), tl.Regs.State.x[1]);
            Assert.AreEqual((uint)(0x10000C), tl.Regs.State.x[2]);
            Assert.AreEqual((uint)(0xFFFFF010), tl.Regs.State.x[3]);
        }
    }
}
