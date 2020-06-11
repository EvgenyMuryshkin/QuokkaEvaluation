using Quokka.Public.Config;
using Quokka.Public.Logging;
using Quokka.Public.Quartus;
using Quokka.Public.Tools;
using Quokka.Public.Transformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuSoC
{
    /// <summary>
    /// PostRun will add all generated files to quartus project
    /// </summary>
    public class PostRun : ILowLevelTransformationPostRun
    {
        private readonly ILogStream _logStream;
        private readonly VirtualFS _virtualFS;
        private readonly QuartusProjectTools _quartusTools;
        private readonly RuntimeConfiguration _runtimeConfiguration;

        public PostRun(
            ILogStream logStream, 
            VirtualFS virtualFS,
            QuartusProjectTools quartusTools,
            RuntimeConfiguration runtimeConfiguration)
        {
            _logStream = logStream;
            _virtualFS = virtualFS;
            _quartusTools = quartusTools;
            _runtimeConfiguration = runtimeConfiguration;
        }

        public void Run()
        {
            _logStream.WriteLine(DirectoryLogging.Summary, $"QuSoC translation completed");
            _logStream.WriteLine(DirectoryLogging.Summary, $"Source location: {_runtimeConfiguration.SourceLocation}");
            _logStream.WriteLine(DirectoryLogging.Summary, $"Config name: {_runtimeConfiguration.ConfigName}");

            var config = QuokkaConfigLoader.Load(Path.Combine(_runtimeConfiguration.SourceLocation, _runtimeConfiguration.ConfigName));
            var generatedFilesLocation = FileTools.ToAbsolutePath(Path.Combine(_runtimeConfiguration.SourceLocation, config.ProjectLocation));
            _logStream.WriteLine(DirectoryLogging.Summary, $"Generated files location: {generatedFilesLocation}");

            var hdlLocation = Path.Combine(Path.GetDirectoryName(_runtimeConfiguration.SourceLocation), "QuSoC.HDL");
            var qsfPath = Path.Combine(hdlLocation, "Verilog.qsf");
            _logStream.WriteLine(DirectoryLogging.Summary, $"Updating quartus files: {qsfPath}");

            if (File.Exists(qsfPath))
            {
                var generatedFiles = _virtualFS
                    .RecursiveFileNames
                    .Where(f => Path.GetFileNameWithoutExtension(f) != "Quokka")
                    .Select(f => Path.Combine(generatedFilesLocation, f))
                    .OrderBy(f => f)
                    .ToList();
                
                foreach (var fileName in generatedFiles)
                {
                    _logStream.WriteLine(DirectoryLogging.Summary, $"Generated file: {fileName}");
                }

                _quartusTools.RemoveGeneratedFiles(qsfPath);
                _quartusTools.AddFiles(qsfPath, generatedFiles);
            }
            else
            {
                _logStream.WriteLine(DirectoryLogging.Summary, $"Project not found");
            }

            _logStream.WriteLine(DirectoryLogging.Summary, $"======================================");
        }
    }
}
