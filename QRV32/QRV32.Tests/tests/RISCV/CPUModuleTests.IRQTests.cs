using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Tests;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class IRQTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void Call()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("irq_call");

            sim.inputsModifier = (inputs, context) =>
            {
                // let it run for 100 cycles, then raise IRQ
                if (context.Clock >= 100)
                {
                    inputs.IRQ = true;
                }
            };

            sim.RunAll(instructions);

            // should be stuck and end label
            Assert.AreEqual(tl.Regs.State.x[11], (uint)tl.State.PC);
            Assert.AreEqual(0xFFU, tl.Regs.State.x[12]);
            Assert.AreEqual(0x1FFU, tl.Regs.State.x[13]);
        }

        [TestMethod]
        public void CallRet()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;

            var instructions = Inst.FromAsmFile("irq_call_ret");

            sim.inputsModifier = (inputs, context) =>
            {
                // let it run for 100 cycles, then raise IRQ
                if (context.Clock >= 100)
                {
                    inputs.IRQ = true;
                }
            };

            sim.RunAll(instructions);

            // should be stuck and end label
            Assert.AreEqual(tl.Regs.State.x[11], (uint)tl.State.PC);
            Assert.AreEqual(0xFFU, tl.Regs.State.x[12]);
            Assert.AreEqual(0x1FFU, tl.Regs.State.x[13]);
        }
    }
}
