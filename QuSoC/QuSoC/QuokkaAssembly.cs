using Quokka.Public.Tools;
using Quokka.Schema.HLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuSoC
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
                var lines = File.ReadAllLines(Path.Combine(SolutionLocation(), "QRV32", "images", "blinker_inf.txt"));
                var instructions = lines.Select(l => l.Split(' ')[0]).Select(l => Convert.ToUInt32(l, 16)).ToArray();
                var blinker = new QuSoCModule(instructions);

                yield return new RTLModuleConfig() { Instance = blinker };
            }
        }
    }
}
