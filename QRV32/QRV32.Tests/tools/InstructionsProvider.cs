using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.Engine;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace QRV32.Tests
{
    public class InstructionsProvider
    {
        public virtual string ProjectLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.csproj").Any())
                return current;

            return ProjectLocation(Path.GetDirectoryName(current));
        }

        public virtual string SolutionLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.sln").Any())
                return current;

            return SolutionLocation(Path.GetDirectoryName(current));
        }

        public virtual string AsmFilesLocation => Path.Combine(ProjectLocation(), "asm");
        public virtual uint[] FromAsmFile(string fileName)
        {
            var files = Directory.EnumerateFiles(AsmFilesLocation, $"{fileName}.*", SearchOption.AllDirectories).ToList();

            if (files.Count == 0)
                throw new Exception($"No files found for '{fileName}' in {AsmFilesLocation}");

            if (files.Count > 1)
                throw new Exception($"Multiple files found for '{fileName}' in '{AsmFilesLocation}'");

            return FromAsmSource(File.ReadAllText(files[0]));
        }

        public virtual uint[] FromAsmSource(string asmSource)
        {
            // making a API call to integration server.

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // on Windows, integration server is required to run in Docker or WSL.
                // Installation steps are same for WSL and for docker.
                // https://github.com/EvgenyMuryshkin/Quokka.RISCV.Docker/blob/master/Dockerfile

                var instructions = RISCVIntegrationClient.Asm(new RISCVIntegrationEndpoint(), asmSource);
                return instructions.Result;
            }
            else
            {
                // on Linux, just make local call to RISCV toolchain
                return RISCVIntegrationClient.ToInstructions(Toolchain.Asm(asmSource)).ToArray();
            }
        }
    }
}
