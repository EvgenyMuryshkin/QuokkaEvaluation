using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quokka.VCD
{
    public class VCDBuilder
    {
        public DateTime Date = DateTime.Now;
        public string Version = "Quokka VCD Generator";
        public string Comments = "";
        public string Timescale = "1s";
        public string FileName;
        public bool TrackChanges = true;
        Dictionary<string, VCDVariable> lastValues = new Dictionary<string, VCDVariable>();

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

                if (TrackChanges)
                {
                    lastValues = VCDTools.FlatternHierarchy(snapshot).ToDictionary(v => v.Name);
                }

                File.WriteAllText(FileName, sw.ToString());
            }
        }

        public void Snapshot(int time, VCDSignalsSnapshot signals)
        {
            var flatterned = VCDTools.FlatternHierarchy(signals);

            var modified = !TrackChanges
                ? flatterned
                : flatterned
                    .Where(v => !lastValues.ContainsKey(v.Name) || !v.Value.Equals(lastValues[v.Name].Value))
                    .ToList();

            if (TrackChanges)
            {
                modified.ForEach(m => lastValues[m.Name] = m);
            }

            using (var sw = new StringWriter())
            {
                var vcdStream = new VCDStreamBuilder(sw);
                vcdStream.SetTime(time);
                vcdStream.Snapshot(modified);
                File.AppendAllText(FileName, sw.ToString());
            }
        }
    }
}
