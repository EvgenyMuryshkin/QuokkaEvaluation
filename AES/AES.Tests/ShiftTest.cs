using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.Core.Tools;
using Quokka.RTL.Simulator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AES.Tests
{
    [TestClass]
    public class ShiftTest
    {
        [TestMethod]
        public void Test()
        {
            var sim = new RTLSimulator<ShiftTestModule, ShiftTestModuleInputs>();
            sim.ClockCycle();
            sim.ClockCycle(new ShiftTestModuleInputs() { LSBData = 0x03020100, LSBDataReady = true });
            sim.ClockCycle(new ShiftTestModuleInputs() { LSBData = 0x07060504, LSBDataReady = true });
            sim.ClockCycle(new ShiftTestModuleInputs() { LSBData = 0x0B0A0908, LSBDataReady = true });
            sim.ClockCycle(new ShiftTestModuleInputs() { LSBData = 0x0F0E0D0C, LSBDataReady = true });
            Assert.IsFalse(sim.TopLevel.LBSResultReady);

            var parts = new List<uint>();
            for (var i = 0; i < 4; i++)
            {
                sim.ClockCycle();
                Assert.IsTrue(sim.TopLevel.LBSResultReady);
                parts.Add(sim.TopLevel.LSBResult);
            }

            Assert.AreEqual(0x00030201U, parts[0]);
            Assert.AreEqual(0x04070605U, parts[1]);
            Assert.AreEqual(0x080B0A09U, parts[2]);
            Assert.AreEqual(0x0C0F0E0DU, parts[3]);
        }        
    }
}
