using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class PCTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void ResetTest()
        {
            var sim = new CPUSimulator();
            var tl = sim.TopLevel;
            Assert.IsFalse(tl.MemRead);
            sim.ClockCycle(new CPUModuleInputs() { BaseAddress = 0xF0000000 });
            Assert.IsTrue(tl.MemRead);
            Assert.AreEqual(0xF0000000, tl.MemAddress);
        }

        [TestMethod]
        public void PCAdvance()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var nop = 19U;

            foreach (var idx in Enumerable.Range(0, 1000))
            {
                Assert.AreEqual((uint)(idx * 4), tl.MemAddress);
                sim.RunInstruction(nop);
            }
        }
    }
}
