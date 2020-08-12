using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.RISCV.Integration.Client;
using System.IO;
using System.Linq;

namespace QRV32.Tests.Compliance
{
    public class ComplianceTestsBase
    {
        public ComplianceCPUSimilator RunTest(string testName, bool allowAllZeroAsserts = false)
        {
            var sourcesLocation = Path.Combine(PathTools.ProjectLocation(), "compliance", "source");
            var testLocation = Path.Combine(PathTools.ProjectLocation(), "compliance", "test");

            // cleanup code from test
            var sourceFiles = Directory.EnumerateFiles(testLocation, "*.S").ToList();
            sourceFiles.ForEach(f => File.Delete(f));

            // prepare source code for test
            File.Copy(Path.Combine(sourcesLocation, $"{testName}.S"), Path.Combine(testLocation, $"{testName}.S"));

            var Makefile = Path.Combine(testLocation, "Makefile");
            var makeLines = File.ReadAllLines(Makefile);
            makeLines[0] = $"files = {testName}.S";
            File.WriteAllLines(Makefile, makeLines);

            // make test instructions
            var context = RISCVIntegration
                .DefaultContext(testLocation)
                .WithMakeTarget("bin");

            RISCVIntegrationClient.Make(context).Wait();

            var disassembler = new Disassembler();
            var FirmwareFile = Path.Combine(testLocation, "firmware.bin");
            var FirmwareAsmFile = Path.Combine(testLocation, "firmware.asm");
            var instructions = RISCVIntegrationClient.ToInstructions(File.ReadAllBytes(FirmwareFile)).ToArray();

            File.WriteAllText(FirmwareAsmFile, disassembler.Disassemble(instructions));

            var dataMarkerAddress = instructions.ToList().IndexOf(0x87654321);

            var sim = new ComplianceCPUSimilator(dataMarkerAddress);
            sim.ClockCycle();
            sim.RunAll(instructions);

            Assert.IsTrue(sim.Asserts > 0, "No asserts were performed during simulation");

            if (!allowAllZeroAsserts)
                Assert.IsTrue(sim.HasNonZeroValues, "No non-zero values were asserted");

            return sim;
        }
    }
}
