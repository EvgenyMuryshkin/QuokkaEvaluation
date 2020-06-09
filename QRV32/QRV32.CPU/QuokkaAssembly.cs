using Quokka.Public.Tools;
using Quokka.Schema.HLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QRV32.CPU
{
    public class QuokkaAssembly : IQuokkaAssembly
    {
        public string SolutionLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.sln").Any())
                return current;

            return SolutionLocation(Path.GetDirectoryName(current));
        }

        public IEnumerable<RTLModuleConfig> RTLModules
        {
            get
            {
                var payload = File.ReadAllText(Path.Combine(SolutionLocation(), "QRV32", "images", "blinker_inf.json"));
                var instructions = QuokkaJson.DeserializeArray<uint>(payload).ToArray();
                var blinker = new QuSoCModule(instructions);

                yield return new RTLModuleConfig() { Instance = blinker };
            }
        }
    }
}
