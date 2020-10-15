using Quokka.Core.Builders.LowLevel;
using Quokka.HLS.TypeLibrary;
using Quokka.Public.Logging;
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace QuSoC
{
    public class QuokkaAssembly : IQuokkaAssembly
    {
        private readonly ILogStream _logStream;
        private readonly RuntimeConfiguration _runtimeConfiguration;
        private readonly RTLModulesDiscovery _rtlModulesDiscovery;
        private readonly ComponentsLibrary _componentsLibrary;

        public QuokkaAssembly(
            ILogStream logStream,
            RuntimeConfiguration runtimeConfiguration, 
            RTLModulesDiscovery rtlModulesDiscovery,
            ComponentsLibrary componentsLibrary)
        {
            _logStream = logStream;
            _runtimeConfiguration = runtimeConfiguration;
            _rtlModulesDiscovery = rtlModulesDiscovery;
            _componentsLibrary = componentsLibrary;
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

        bool WriteFileIfChanged(string path, string content)
        {
            if (File.Exists(path) && File.ReadAllText(path) == content)
                return false;

            FileTools.CreateDirectoryRecursive(Path.GetDirectoryName(path));
            FileTools.WriteAllText(path, content);

            return true;
        }

        public bool CanTranslate()
        {
            var socTypes = _componentsLibrary.ProjectAssembly.ExportedTypes.Where(t => t.Name == "SOC").ToList();
            var qusocModules = _componentsLibrary.ProjectAssembly.ExportedTypes.Where(t => typeof(QuSoCModule).IsAssignableFrom(t)).ToList();

            // create partial declaration for each app base of SoC configuration
            var wasModified = false;
            var appsPath = Path.Combine(PathTools.ProjectPath, "apps");
            var apps = Directory.EnumerateDirectories(appsPath);
            foreach (var appPath in apps)
            {
                var app = Path.GetFileName(appPath);
                var socType = socTypes.SingleOrDefault(t => t.Namespace == app);
                var qusoc = qusocModules.SingleOrDefault(t => t.Name == app);

                if (socType != null && qusoc != null)
                {
                    var socTypeInstance = Activator.CreateInstance(socType);
                    var deviceAddress = 0x80000000;
                    Func<string> nextDeviceAddress = () =>
                    {
                        var address = deviceAddress;
                        deviceAddress += 0x100000;
                        return address.ToString("X8");
                    };

                    List<string> moduleDeclarations = new List<string>();
                    List<string> moduleInitializers = new List<string>();
                    List<string> moduleSchedule = new List<string>();
                    List<string> moduleNames = new List<string>();
                    foreach (var member in socType.GetMembers(BindingFlags.Public | BindingFlags.Instance))
                    {
                        switch (member)
                        {
                            case PropertyInfo pi:
                            case FieldInfo fi:
                                var value = member.GetValue(socTypeInstance);
                                var moduleName = $"{member.Name}Module";
                                moduleNames.Add(moduleName);
                                var memberType = member.GetMemberType();
                                if (memberType.IsArray)
                                {
                                    var arraySize = (value as IEnumerable).OfType<object>().Count();
                                    var elementType = memberType.GetElementType();
                                    if (elementType == typeof(uint))
                                    {
                                        moduleDeclarations.Add($"SoCBlockRAMModule {moduleName}");
                                        moduleInitializers.Add($"{moduleName} = new SoCBlockRAMModule({arraySize});");
                                        moduleSchedule.Add($"{moduleName}.Schedule(() => new SoCBlockRAMModuleInputs() {{ Common = ModuleCommon, DeviceAddress = 0x{nextDeviceAddress()} }});");
                                    }
                                    else
                                    {
                                        throw new Exception($"ElementType is not supported: {member}");
                                    }
                                }
                                else
                                {
                                    if (memberType == typeof(uint))
                                    {
                                        moduleDeclarations.Add($"SoCRegisterModule {moduleName}");
                                        moduleInitializers.Add($"{moduleName} = new SoCRegisterModule();");
                                        moduleSchedule.Add($"{moduleName}.Schedule(() => new SoCRegisterModuleInputs() {{ Common = ModuleCommon, DeviceAddress = 0x{nextDeviceAddress()} }});");
                                    }
                                    else
                                    {
                                        throw new Exception($"MemberType is not supported: {member}");
                                    }
                                }
                                break;
                        }
                    }

                    _logStream.WriteLine(DirectoryLogging.Summary, $"Found SoC Type: {socType}");
                    var content = new StringBuilder();
                    content.AppendLine($"// partial declaration for {app} SoC. Generated by {GetType().Name}");
                    content.AppendLine($"using QuSoC;");
                    content.AppendLine($"namespace {socType.Namespace}");
                    content.AppendLine("{");
                    content.AppendLine($"\tpublic partial class {qusoc.Name}");
                    content.AppendLine("\t{");
                    foreach (var decl in moduleDeclarations)
                    {
                        content.AppendLine($"\t\tinternal {decl};");
                    }
                    content.AppendLine($"\t\tprotected override ISoCComponentModule[] GeneratedModules => new ISoCComponentModule[] {{ {moduleNames.ToCSV()} }};");

                    content.AppendLine($"\t\tprotected override void CreateGeneratedModules()");
                    content.AppendLine("\t\t{");
                    foreach (var init in moduleInitializers)
                    {
                        content.AppendLine($"\t\t\t{init}");
                    }
                    content.AppendLine("\t\t} // CreateGeneratedModules");

                    content.AppendLine($"\t\tprotected override void ScheduleGeneratedModules()");
                    content.AppendLine("\t\t{");
                    foreach (var schedule in moduleSchedule)
                    {
                        content.AppendLine($"\t\t\t{schedule}");
                    }
                    content.AppendLine("\t\t} // ScheduleGeneratedModules");

                    content.AppendLine("\t}");
                    content.AppendLine("}");

                    var socFileName = Path.Combine(appPath, "soc", $"{app}.generated.cs");
                    wasModified |= WriteFileIfChanged(socFileName, content.ToString());
                }
            }

            return !wasModified;
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
