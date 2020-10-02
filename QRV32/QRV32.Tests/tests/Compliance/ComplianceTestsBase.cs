using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using QRV32.Tests;
using Quokka.Public.Tools;
using Quokka.RISCV.Integration.Client;
using System;
using System.IO;
using System.Linq;

namespace QRV32.Compliance
{
    public class ComplianceTestsBase
    {
        string _isa, _arch;
        public ComplianceTestsBase(string isa, string arch)
        {
            _isa = isa;
            _arch = arch;
        }

        string ComplianceTestsPath => Path.Combine(TestPathTools.ProjectLocation(), "compliance");
        string ISATestsPath => Path.Combine(ComplianceTestsPath, _isa);
        string SourcesLocation => Path.Combine(ISATestsPath, "src");
        string TestLocation => Path.Combine(ComplianceTestsPath, "test");
        string ReferencesLocation => Path.Combine(ISATestsPath, "references");
        string FirmwareFile => Path.Combine(TestLocation, "firmware.bin");
        string FirmwareMap => Path.Combine(TestLocation, "firmware.map");
        string FirmwareAsmFile => Path.Combine(TestLocation, "firmware.asm");

        void CleanUp()
        {
            // cleanup code from test
            var sourceFiles = Directory.EnumerateFiles(TestLocation, "*.S").ToList();
            sourceFiles.ForEach(f => File.Delete(f));
        }

        void Build(string testName)
        {
            // prepare source code for test
            File.Copy(Path.Combine(SourcesLocation, $"{testName}.S"), Path.Combine(TestLocation, $"{testName}.S"));

            var Makefile = Path.Combine(TestLocation, "Makefile");
            var makeLines = File.ReadAllLines(Makefile);
            makeLines[0] = $"files = {testName}.S";
            makeLines[1] = $"arch = {_arch}";
            FileTools.WriteAllLines(Makefile, makeLines);

            // make test instructions
            var context = RISCVIntegration
                .DefaultContext(TestLocation)
                .WithMakeTarget("bin");

            RISCVIntegrationClient.Make(context).Wait();
        }

        uint[] Instructions()
        {
            var instructions = RISCVIntegrationClient.ToInstructions(File.ReadAllBytes(FirmwareFile)).ToArray();
            return instructions;
        }

        uint[] ReferenceOutput(string testName)
        {
            var lines = File
                .ReadAllLines(Path.Combine(ReferencesLocation, $"{testName}.reference_output"))
                .Where(l => !string.IsNullOrWhiteSpace(l));

            return lines.Select(l => Convert.ToUInt32(l, 16)).ToArray();
        }

        void Disassemble()
        {
            var disassembler = new Disassembler();
            FileTools.WriteAllText(FirmwareAsmFile, disassembler.Disassemble(Instructions()));
        }

        int DataMarkerAddress()
        {
            return Instructions().ToList().IndexOf(0x87654321);
        }

        int DataSectionAddress()
        {
            var mapFile = File.ReadAllLines(FirmwareMap);
            var sectionLine = mapFile.First(l => l.Contains("begin_signature")).Trim();
            var address = sectionLine.Split(" ")[0];
            return Convert.ToInt32(address.Substring(2), 16);
        }

        ComplianceCPUSimilator Run()
        {
            var dataMarkerAddress = DataMarkerAddress();
            var sim = new ComplianceCPUSimilator(dataMarkerAddress);
            sim.ClockCycle();
            sim.RunAll(Instructions());

            return sim;
        }

        void AssertReferenceOutput(string testName, ComplianceCPUSimilator sim)
        {
            var dataSectionAddress = DataSectionAddress() >> 2;
            var referenceOutput = ReferenceOutput(testName);
            AssertMemory(sim, dataSectionAddress, referenceOutput);
        }

        public ComplianceCPUSimilator Run(string testName)
        {
            CleanUp();
            Build(testName);
            Disassemble();
            return Run();
        }

        public virtual ComplianceCPUSimilator RunAndAssert(string testName)
        {
            var sim = Run(testName);
            AssertReferenceOutput(testName, sim);
            return sim;
        }

        protected void AssertMemory(ComplianceCPUSimilator sim, int testAddress, uint[] expectedValues)
        {
            for (var i = 0; i < expectedValues.Length; i++)
            {
                var actualWordAddress = testAddress + i;
                var expected = expectedValues[i];
                var actual = sim.MemoryBlock[actualWordAddress];

                if (expected != actual)
                {
                    throw new AssertFailedException($"Expected:<0x{expected:X8}>. Actual:<0x{actual:X8}>. Failed for value at {i} (0x{(actualWordAddress * 4):X8})");
                }
            }
        }
    }
}
