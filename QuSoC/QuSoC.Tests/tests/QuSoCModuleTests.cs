using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            var id = new InstructionDecoderModule();
            id.Setup();

            var files = new[] { "blinker_sim", "blinker_inf" };
            foreach (var file in files)
            {
                var instructions = Inst.FromAsmFile("blinker_inf");
                var lines = instructions.Select((i, idx) =>
                {
                    id.Cycle(new InstructionDecoderInputs() { Instruction = i });
                    return $"{i.ToString("X8")} // {(idx << 2).ToString("X2")} {id.OpTypeCode}";
                });

                var sln = Inst.SolutionLocation();
                File.WriteAllLines(Path.Combine(sln, "QuSoC", "images", $"{file}.txt"), lines);
            }
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

            Assert.AreEqual(20U, (uint)tl.State.Counter);
        }


        [TestMethod]
        public void UARTSim()
        {
            var sim = PowerUp("uart_sim");

            // WIP, does not support arrays yet
            //sim.TraceToVCD(PathTools.VCDOutputPath());

            var tl = sim.TopLevel;

            List<byte> txBytes = new List<byte>();

            var txCounter = 0;
            sim.OnPostCommit += (m) =>
            {
                // simulate long uart transmission
                if (tl.State.UART_TX)
                {
                    txCounter = 100;
                    txBytes.Add(tl.State.UART[0]);
                }

                if (txCounter == 0)
                {
                    tl.State.UART[2] = 2;
                }
                else
                {
                    txCounter--;
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
