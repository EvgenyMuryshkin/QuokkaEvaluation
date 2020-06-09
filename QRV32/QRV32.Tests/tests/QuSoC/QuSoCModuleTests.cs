using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using QRV32.Tests;
using System;
using System.IO;
using System.Linq;

namespace QuSoC.Tests
{
    [TestClass]
    public class QuSoCModuleTests : QuSoCModuleBaseTest
    {
        [TestMethod]
        public void HangTest()
        {
            var sim = PowerUp("hang");
            Assert.ThrowsException<Exception>(() => sim.RunToCompletion(100));
        }

        [TestMethod]
        public void LoadTest()
        {
            var sim = PowerUp("load");
            var tl = sim.TopLevel;
            tl.State.BlockRAM[0x40] = 0x80BC80F1;

            sim.RunToCompletion();
            var mem = sim.MemoryDump();

            // expected result is 
            // 80 + BC + 80 + F1 +
            // 80BC + 80F1 +
            // 80BC80F1 =
            // 80BD 854B

            Assert.AreEqual(0x80BC80F1, tl.State.BlockRAM[0x40]);
            Assert.AreEqual(0x80BD854B, tl.State.BlockRAM[0x41]);
        }

        [TestMethod]
        public void StoreTest()
        {
            var sim = PowerUp("store");
            var tl = sim.TopLevel;

            sim.RunToCompletion();

            Assert.AreEqual(0xC00DC0DE, tl.State.BlockRAM[0x40]);
            Assert.AreEqual(0xC0DEC0DE, tl.State.BlockRAM[0x41]);
            Assert.AreEqual(0xC0DEC0DE, tl.State.BlockRAM[0x42]);
        }

        [TestMethod]
        public void BlinkerInfDump()
        {
            var instructions = Inst.FromAsmFile("blinker_inf");
            var sln = Inst.SolutionLocation();
            var payload = JsonConvert.SerializeObject(instructions);
            File.WriteAllText(Path.Combine(sln, "QRV32", "images", "blinker_inf.json"), payload);
        }

        [TestMethod]
        public void Blinker20Test()
        {
            var sim = PowerUp("blinker_20");
            
            // WIP, does not support arrays yet
            //sim.TraceToVCD(PathTools.VCDOutputPath());

            var tl = sim.TopLevel;

            sim.RunToCompletion();

            var memDump = sim.MemoryDump();
            var cpuDump = sim.TopLevel.CPU.ToString();

            Assert.AreEqual(20U, (uint)tl.State.Counter);
        }
    }
}
