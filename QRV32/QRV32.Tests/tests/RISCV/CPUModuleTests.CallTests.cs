using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class CallTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void SP()
        {
            var sim = PowerUp();
            var instructions = Inst.FromAsmFile("sp");
            sim.RunAll(instructions);
            // stack pointer is x2
            Assert.AreEqual(0xAU, sim.TopLevel.Regs.State.x[2]);
        }

        [TestMethod]
        public void Call()
        {
            var sim = PowerUp();
            var instructions = Inst.FromAsmFile("call");
            sim.RunAll(instructions);
            Assert.AreEqual(20U, sim.TopLevel.Regs.State.x[11]);
            Assert.AreEqual(30U, sim.TopLevel.Regs.State.x[13]);
            Assert.AreEqual(40U, sim.TopLevel.Regs.State.x[14]);
        }

        [TestMethod]
        public void CallRet()
        {
            var sim = PowerUp();
            var instructions = Inst.FromAsmFile("call_ret");

            var id = new InstructionDecoderModule();
            id.Setup();
            var lines = instructions.Select((i, idx) =>
            {
                id.Cycle(new InstructionDecoderInputs() { Instruction = i });
                return $"{i.ToString("X8")} // {(idx << 2).ToString("X2")} {id.OpTypeCode}";
            });

            sim.RunAll(instructions);
            Assert.AreEqual(1U, sim.TopLevel.Regs.State.x[10]);
            Assert.AreEqual(2U, sim.TopLevel.Regs.State.x[11]);
            Assert.AreEqual(3U, sim.TopLevel.Regs.State.x[12]);
            Assert.AreEqual(4U, sim.TopLevel.Regs.State.x[13]);
            Assert.AreEqual(5U, sim.TopLevel.Regs.State.x[14]);
            Assert.AreEqual(0U, sim.TopLevel.Regs.State.x[15]);
        }
    }
}
