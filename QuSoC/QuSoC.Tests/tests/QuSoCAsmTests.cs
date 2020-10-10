using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.Public.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuSoC.Tests
{
    [TestClass]
    public class QuSoCAsmTests : QuSoCModuleBaseTest
    {
        [TestMethod]
        public void ArraysDisasm()
        {
            var firmwareTools = new FirmwareTools(AppPath("Arrays"));
            var disassembler = new Disassembler();

            FileTools.WriteAllText(firmwareTools.FirmwareAsmFile, disassembler.Disassemble(firmwareTools.Instructions()));
        }

        [TestMethod]
        public void HangTest()
        {
            var sim = PowerUp("hang");
            Assert.ThrowsException<Exception>(() => sim.RunToCompletion(100));
        }

        [TestMethod]
        public void InterruptTest()
        {
            var sim = PowerUp("interrupt");
            //Assert.ThrowsException<Exception>(() => sim.RunToCompletion(100));
        }

        [TestMethod]
        public void PushPopTest()
        {
            var sim = PowerUp("push_pop");
            sim.RunToCompletion();

            var memDump = sim.MemoryDump();
            var cpuDump = sim.TopLevel.CPU.ToString();

            var regs = sim.TopLevel.CPU.Regs;

            Assert.AreEqual(2U, regs.State.x[10]);
            Assert.AreEqual(1U, regs.State.x[11]);
            Assert.AreEqual(0x100U, regs.State.x[2]);
        }

        [TestMethod]
        public void LoadTest()
        {
            var sim = PowerUp("load");
            var tl = sim.TopLevel;
            tl.InstructionsRAM.State.BlockRAM[0x40] = 0x80BC80F1;

            sim.RunToCompletion();
            var mem = sim.MemoryDump();

            // expected result is 
            // 80 + BC + 80 + F1 +
            // 80BC + 80F1 +
            // 80BC80F1 =
            // 80BD 854B

            Assert.AreEqual(0x80BC80F1, tl.InstructionsRAM.State.BlockRAM[0x40]);
            Assert.AreEqual(0x80BD854B, tl.InstructionsRAM.State.BlockRAM[0x41]);
        }

        [TestMethod]
        public void StoreTest()
        {
            var sim = PowerUp("store");
            var tl = sim.TopLevel;

            sim.RunToCompletion();

            Assert.AreEqual(0xC00DC0DE, tl.InstructionsRAM.State.BlockRAM[0x40]);
            Assert.AreEqual(0xC0DEC0DE, tl.InstructionsRAM.State.BlockRAM[0x41]);
            Assert.AreEqual(0xC0DEC0DE, tl.InstructionsRAM.State.BlockRAM[0x42]);
        }

        [TestMethod]
        public void BlinkerSimTest()
        {
            var sim = PowerUp("blinker_sim");
            
            // WIP, does not support arrays yet
            //sim.TraceToVCD(PathTools.VCDOutputPath());

            var tl = sim.TopLevel;

            sim.RunToCompletion();

            var memDump = sim.MemoryDump();
            var cpuDump = sim.TopLevel.CPU.ToString();

            Assert.AreEqual(20U, (uint)tl.Counter);
        }

        [TestMethod]
        public void UARTSim()
        {
            var sim = PowerUp("uart_sim");

            // WIP, does not support arrays yet
            //sim.TraceToVCD(PathTools.VCDOutputPath());

            var tl = sim.TopLevel;

            List<byte> txBytes = new List<byte>();

            sim.OnPostCommit += (m) =>
            {
                if (tl.UARTSim.State.UART_TX)
                {
                    txBytes.Add(tl.UARTSim.State.UART[0]);
                }
            };

            sim.RunToCompletion();

            var memDump = sim.MemoryDump();
            var cpuDump = sim.TopLevel.CPU.ToString();

            var str = Encoding.ASCII.GetString(txBytes.ToArray());
            Assert.AreEqual("Hello World\n", str);
        }
    }
}
