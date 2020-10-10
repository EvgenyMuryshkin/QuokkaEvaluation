using Quokka.Core.Builders.LowLevel;
using Quokka.Public.Tools;
using Quokka.RISCV.CS2CPP.Tools;
using Quokka.RISCV.CS2CPP.Translator;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Generator.SOC;
using Quokka.RTL;
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
        private readonly RTLModulesDiscovery _rtlModulesDiscovery;

        public QuokkaAssembly(RuntimeConfiguration runtimeConfiguration, RTLModulesDiscovery rtlModulesDiscovery)
        {
            _runtimeConfiguration = runtimeConfiguration;
            _rtlModulesDiscovery = rtlModulesDiscovery;
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
                var apps = new HashSet<string>()
                {
                    //nameof(MemBlock),
                    //nameof(Counter)
                };

                if (apps.Any())
                {
                    foreach (var app in apps)
                    {
                        var module = new QuSoCModule(FirmwareTools.FromApp(app));
                        yield return new RTLModuleConfig() { Instance = module, Name = app };
                    }
                }
                else
                {
                    // add default creatable modules, declared in this assembly only
                    foreach (var moduleType in _rtlModulesDiscovery.ModuleTypes.Where(t => typeof(QuSoCModule).IsAssignableFrom(t)))
                    {
                        var instance = Activator.CreateInstance(moduleType) as IRTLCombinationalModule;
                        yield return new RTLModuleConfig() { Instance = instance, Name = instance.ModuleName };
                    }
                }
            }
        }
    }
}
