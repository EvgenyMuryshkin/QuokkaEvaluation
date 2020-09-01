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
    public class SBoxTest
    {
        [TestMethod]
        public void Test()
        {
            var sim = new RTLSimulator<SBoxTestModule, SBoxTestModuleInputs>();
            sim.ClockCycle();
            sim.ClockCycle(new SBoxTestModuleInputs() { LSBData = 0x03020100, LSBDataReady = true });
            sim.ClockCycle(new SBoxTestModuleInputs() { LSBData = 0x07060504, LSBDataReady = true });
            sim.ClockCycle(new SBoxTestModuleInputs() { LSBData = 0x0B0A0908, LSBDataReady = true });
            sim.ClockCycle(new SBoxTestModuleInputs() { LSBData = 0x0F0E0D0C, LSBDataReady = true });
            Assert.IsFalse(sim.TopLevel.LBSResultReady);

            var parts = new List<uint>();
            for (var i = 0; i < 4; i++)
            {
                sim.ClockCycle();
                Assert.IsTrue(sim.TopLevel.LBSResultReady);
                parts.Add(sim.TopLevel.LSBResult);
            }

            var calculated = SBoxModule.GetData();
            var bytes = parts.SelectMany(p => BitConverter.GetBytes(p)).ToList();
            for (var idx = 0; idx < 16; idx++)
            {
                Assert.AreEqual(calculated[idx], bytes[idx], $"Value at {idx} is wrong");
            }
        }
    }
}
