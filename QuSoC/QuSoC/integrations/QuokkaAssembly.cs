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

                foreach (var appPath in apps)
                {
                    var appName = Path.GetFileName(appPath);
                    var sourceFolder = Path.Combine(appPath, "source");
                    var firmwareFolder = Path.Combine(appPath, "firmware");

                    if (Directory.Exists(sourceFolder))
                    {
                        var csFiles = Directory.EnumerateFiles(sourceFolder);
                        var csFilesContent = csFiles
                            .Select(path => new FSTextFile() { Name = path, Content = File.ReadAllText(path) })
                            .ToList();

                        if (csFilesContent.Any())
                        {
                            foreach (var cs in csFilesContent)
                            {
                                Console.WriteLine($"Found CS source: {cs.Name}");
                            }

                            // translate source files
                            var tx = new CSharp2CPPTranslator();
                            var source = new FSSnapshot();
                            source.Files.AddRange(csFilesContent);
                            tx.Run(source);
                            var firmwareSource = tx.Result;

                            // create soc resource records
                            var socGenerator = new SOCGenerator();
                            var socRecordsBuilder = new SOCRecordsBuilder();
                            var socRecords = socRecordsBuilder.ToSOCRecords(0x800, tx.SOCResources);
                            firmwareSource.Add(socGenerator.SOCImport(socRecords));

                            FileTools.CreateDirectoryRecursive(firmwareFolder);
                            var m = new FSManager(firmwareFolder);
                            m.SaveSnapshot(firmwareSource);
                        }

                        var makefile = Path.Combine(firmwareFolder, "makefile");
                        if (File.Exists(makefile))
                        {
                            var context = RISCVIntegration
                                .DefaultContext(firmwareFolder)
                                .WithMakeTarget("bin");

                            RISCVIntegrationClient.Make(context).Wait();
                        }

                        var firmwareFile = Path.Combine(firmwareFolder, "firmware.bin");
                        if (File.Exists(firmwareFile))
                        {
                            var firmwareData = File.ReadAllBytes(firmwareFile);
                            var firmwareinstructions = RISCVIntegrationClient.ToInstructions(firmwareData).ToArray();

                            var app = new QuSoCModule(firmwareinstructions);
                            yield return new RTLModuleConfig() { Instance = app, Name = appName };
                        }
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
