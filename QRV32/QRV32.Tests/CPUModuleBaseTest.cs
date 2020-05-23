using Quokka.RISCV.Integration.Client;
using System;
using System.IO;
using System.Linq;

namespace QRV32.Tests
{
    public class CPUModuleBaseTest
    {
        protected virtual string ProjectLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.csproj").Any())
                return current;

            return ProjectLocation(Path.GetDirectoryName(current));
        }

        protected virtual string AsmFilesLocation => Path.Combine(ProjectLocation(), "asm");
        protected virtual uint[] FromAsmFile(string fileName)
        {
            var files = Directory.EnumerateFiles(AsmFilesLocation, $"{fileName}.*").ToList();

            if (files.Count == 0)
                throw new Exception($"No files found for '{fileName}' in {AsmFilesLocation}");

            if (files.Count > 1)
                throw new Exception($"Multiple files found for '{fileName}' in '{AsmFilesLocation}'");

            return FromAsmSource(File.ReadAllText(files[0]));
        }

        protected virtual uint[] FromAsmSource(string asm)
        {
            // making a API call to integration server.
            // default implemetation just call locally running server (in docker or WSL on Windows or local process on Linux)
            var instructions = RISCVIntegrationClient.Asm(new RISCVIntegrationEndpoint(), asm);
            return instructions.Result;
        }
    }
}
