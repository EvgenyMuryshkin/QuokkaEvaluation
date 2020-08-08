using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.Integration.Client;
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
        string AppPath(string app) 
            => Path.Combine(
                Inst.SolutionLocation(), 
                "QuSoC", 
                "QuSoC", 
                "apps", app);

        public QuSoCModuleSimulator FromApp(string appName)
        {
            var firmwareTools = new FirmwareTools(AppPath(appName));
            Assert.IsTrue(firmwareTools.FirmwareFromAppFolder());

            var instructions = RISCVIntegrationClient
                .ToInstructions(File.ReadAllBytes(firmwareTools.FirmwareFile))
                .ToArray();
            var sim = PowerUp(instructions);
            return sim;
        }

        [TestMethod]
        public void RecursionTest()
        {
            var sim = FromApp("Recursion");
            sim.RunToCompletion();

            Recursion.Firmware.EntryPoint();
            Assert.AreEqual(Recursion.SOC.Instance.Counter, sim.TopLevel.CSCounter);
        }

        [TestMethod]
        public void ArraysTest()
        {
            var sim = FromApp("Arrays");
            sim.RunToCompletion();

            Arrays.Firmware.EntryPoint();
            Assert.AreEqual(Arrays.SOC.Instance.Counter, sim.TopLevel.CSCounter);
        }
        [TestMethod]
        public void ArraysDisasm()
        {
            var firmwareTools = new FirmwareTools(AppPath("Arrays"));
            var disassembler = new Disassembler();
            var t = disassembler.Disassemble(new[] { 0xFEF40783 });

            File.WriteAllText(firmwareTools.FirmwareAsmFile, disassembler.Disassemble(firmwareTools.Instructions()));
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

        [TestMethod]
        public void CSCounterTest()
        {
            var sim = FromApp("Counter");
            sim.RunToCompletion(() => sim.TopLevel.CSCounter < 10);

            Assert.AreEqual(10U, sim.TopLevel.CSCounter);
        }
    }
}
