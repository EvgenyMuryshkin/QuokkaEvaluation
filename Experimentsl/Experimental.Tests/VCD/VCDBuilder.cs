using System;
using System.Collections.Generic;

namespace Experimental.Tests
{
    public class VCDBuilder
    {
        public DateTime Date = DateTime.Now;
        public string Version = "Quokka VCD Generator";
        public string Comments = "";
        public string Timescale = "1s";
        public List<VCDScope> Scopes = new List<VCDScope>();

        public override string ToString()
        {
            var vcd = new VCDStreamBuilder();
            vcd.Date(Date);
            vcd.Version(Version);
            vcd.Timescale(Timescale);

            foreach (var s in Scopes)
            {
                vcd.Scope(s);
            }

            vcd.EndDefinitions();

            vcd.SetTime(0);
            vcd.SetTime(100);

            return vcd.ToString();
        }
    }
}
