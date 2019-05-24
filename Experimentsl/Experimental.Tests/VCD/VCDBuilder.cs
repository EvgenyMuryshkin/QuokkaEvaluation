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
        public List<VCDScope> Scopes = new List<VCDScope>();
        public string FileName;

        public VCDBuilder(string fileName)
        {
            FileName = fileName;
        }

        public void Init()
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

                File.WriteAllText(FileName, sw.ToString());
            }
        }

        public void Snapshot(int time, VCDSignalsSnapshot signals)
        {
            using (var sw = new StringWriter())
            {
                var vcdStream = new VCDStreamBuilder(sw);
                vcdStream.SetTime(time);
                foreach (var pair in signals)
                {
                    string value = null;

                    switch (pair.Value)
                    {
                        case bool b:
                            value = b ? "1" : "0";
                            break;
                        case string s:
                            value = s;
                            break;
                        default:
                            //value = rawValue.ToString();
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        vcdStream.SetValue(pair.Key, value);
                    }
                }
                File.AppendAllText(FileName, sw.ToString());
            }
        }
    }
}
