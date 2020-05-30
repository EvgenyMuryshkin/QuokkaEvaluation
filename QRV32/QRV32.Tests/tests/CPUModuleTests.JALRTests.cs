﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QRV32.CPUModuleTests
{
    [TestClass]
    public class JALRTests : CPUModuleBaseTest
    {
        [TestMethod]
        public void JALR()
        {
            var sim = PowerUp();
            var tl = sim.TopLevel;
            var instructions = Inst.FromAsmFile("jalr");
            sim.RunAll(instructions);

            Assert.AreEqual(0x8U, tl.Regs.State.x[2]);
            // 4(PC) + 10(x1) + 256(imm)
            Assert.AreEqual(270U, tl.PC.PC);
        }
    }
}
