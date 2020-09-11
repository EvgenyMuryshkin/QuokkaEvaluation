using Quokka.Public.Tools;
using Quokka.RISCV.CS2CPP.Tools;
using Quokka.RISCV.CS2CPP.Translator;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Generator.SOC;
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
                var limitToApps = new HashSet<string>() 
                {
                    //"MemBlock"
                };

                foreach (var appPath in apps)
                {
                    var appName = Path.GetFileName(appPath);
                    
                    if (appName == "template")
                        continue;

                    if (limitToApps.Any() && !limitToApps.Contains(appName))
                        continue;

                    var firmwareTools = new FirmwareTools(appPath);

                    if (firmwareTools.FirmwareFromAppFolder())
                    {
                        var firmwareData = File.ReadAllBytes(firmwareTools.FirmwareFile);
                        var firmwareinstructions = RISCVIntegrationClient.ToInstructions(firmwareData).ToArray();

                        var app = new QuSoCModule(firmwareinstructions);
                        yield return new RTLModuleConfig() { Instance = app, Name = appName };
                    }
                    else
                    {
                        var mainPath = Path.Combine(appPath, "main.S");
                        var mainSource = File.ReadAllText(mainPath);
                        var instructions = FromAsmSource(mainSource);
                        var blinker = new QuSoCModule(instructions);
                        yield return new RTLModuleConfig() { Instance = blinker, Name = appName };
                    }
                }
            }
        }
    }
}
