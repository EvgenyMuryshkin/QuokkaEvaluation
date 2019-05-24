using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quokka.VCD
{
    public class VCDBuilder
    {
        public DateTime Date = DateTime.Now;
        public string Version = "Quokka VCD Generator";
        public string Comments = "";
        public string Timescale = "1s";
        public string FileName;

        public VCDBuilder(string fileName)
        {
            FileName = fileName;
        }

        public void Init(VCDSignalsSnapshot snapshot)
        {
            using (var sw = new StringWriter())
            {
                var vcdStream = new VCDStreamBuilder(sw);
                vcdStream.Date(Date);
                vcdStream.Version(Version);
                vcdStream.Timescale(Timescale);

                vcdStream.Scope(snapshot);

                vcdStream.EndDefinitions();

                vcdStream.SetTime(0);
                vcdStream.Snapshot(snapshot);

                File.WriteAllText(FileName, sw.ToString());
            }
        }

        public void Snapshot(int time, VCDSignalsSnapshot signals)
        {
            using (var sw = new StringWriter())
            {
                var vcdStream = new VCDStreamBuilder(sw);
                vcdStream.SetTime(time);
                vcdStream.Snapshot(signals);
                File.AppendAllText(FileName, sw.ToString());
            }
        }
    }
}
