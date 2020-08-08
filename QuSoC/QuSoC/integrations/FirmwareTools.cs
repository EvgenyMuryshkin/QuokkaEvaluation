using Quokka.Public.Tools;
using Quokka.RISCV.CS2CPP.Tools;
using Quokka.RISCV.CS2CPP.Translator;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Generator.SOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuSoC
{
    public class FirmwareTools
    {
        private readonly string appPath;
        public FirmwareTools(string path)
        {
            appPath = path;
        }

        public string SourceFolder => Path.Combine(appPath, "source");
        public string FirmwareFolder => Path.Combine(appPath, "firmware");
        public string FirmwareFile => Path.Combine(FirmwareFolder, "firmware.bin");
        public string FirmwareAsmFile => Path.Combine(FirmwareFolder, "firmware.asm");
        public string MakefileFile => Path.Combine(FirmwareFolder, "makefile");
        public bool SourceExists => Directory.Exists(SourceFolder);

        public bool FirmwareFromAppFolder()
        {
            if (!SourceExists)
                return false;

            // delete old firmware before making new one
            if (File.Exists(FirmwareFile))
                File.Delete(FirmwareFile);

            var appName = Path.GetFileName(appPath);
            var sourceFolder = Path.Combine(appPath, "source");
            var firmwareFolder = Path.Combine(appPath, "firmware");

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

            ModifyMakefile();

            if (File.Exists(MakefileFile))
            {
                var context = RISCVIntegration
                    .DefaultContext(firmwareFolder)
                    .WithMakeTarget("bin");

                RISCVIntegrationClient.Make(context).Wait();
            }

            if (File.Exists(FirmwareFile))
            {
                var disassembler = new Disassembler();
                File.WriteAllText(FirmwareAsmFile, disassembler.Disassemble(Instructions()));
            }

            return File.Exists(FirmwareFile);
        }

        public uint[] Instructions()
        {
            if (!File.Exists(FirmwareFile))
                return new uint[0];

            var instructions = RISCVIntegrationClient
                .ToInstructions(File.ReadAllBytes(FirmwareFile))
                .ToArray();

            return instructions;

        }

        void ModifyMakefile()
        {
            if (!File.Exists(MakefileFile))
                return;

            var sourceFiles = FirmwareSourceFiles();
            var names = string.Join(" ", sourceFiles.Select(s => Path.GetFileName(s)));
            var makefileLines = File.ReadAllLines(MakefileFile);
            var modifiedLines =
                makefileLines
                .Select(l =>
                {
                    if (l.StartsWith("files"))
                        return $"files = {names}";

                    return l;
                });

            File.WriteAllLines(MakefileFile, modifiedLines);
        }

        List<string> FirmwareSourceFiles()
        {
            var sourceFiles = new List<string>()
            {
                ".s",
                ".c",
                ".cpp"
            };

            return Directory
                .EnumerateFiles(FirmwareFolder, "*.*", SearchOption.AllDirectories)
                .Where(f => sourceFiles.Contains(Path.GetExtension(f).ToLower()))
                .OrderBy(f => sourceFiles.IndexOf(Path.GetExtension(f).ToLower()))
                .Select(f => Path.GetFileName(f))
                .ToList();
        }
    }
}
