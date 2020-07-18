using Quokka.Public.Tools;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.Engine;
using Quokka.Schema.HLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace QuSoC
{
    public class QuokkaAssembly : IQuokkaAssembly
    {
        private readonly RuntimeConfiguration _runtimeConfiguration;

        public QuokkaAssembly(RuntimeConfiguration runtimeConfiguration)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public string SolutionLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.sln").Any())
                return current;

            return SolutionLocation(Path.GetDirectoryName(current));
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

        public IEnumerable<RTLModuleConfig> RTLModules
        {
            get
            {
                var apps = Directory.EnumerateDirectories(Path.Combine(_runtimeConfiguration.SourceLocation, "apps"), "*.*" );

                foreach (var appName in apps)
                {
                    var mainPath = Path.Combine(appName, "main.S");
                    var mainSource = File.ReadAllText(mainPath);
                    var instructions = FromAsmSource(mainSource);
                    var blinker = new QuSoCModule(instructions);
                    yield return new RTLModuleConfig() { Instance = blinker, Name = Path.GetFileName(appName) };
                }
            }
        }
    }
}
