using System;
using System.Collections.Generic;
using System.IO;

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
            using (var sw = new StringWriter())
            {
                var vcdStream = new VCDStreamBuilder(sw);
                vcdStream.Date(Date);
                vcdStream.Version(Version);
                vcdStream.Timescale(Timescale);

                foreach (var s in Scopes)
                {
                    vcdStream.Scope(s);
                }

                vcdStream.EndDefinitions();

                vcdStream.SetTime(0);

                return sw.ToString();
            }
        }

        public void Save(string path)
        {
            File.WriteAllText(path, ToString());
        }
    }
}
