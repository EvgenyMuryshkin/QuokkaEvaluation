using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.VCD;
using QuokkaTests.Experimental;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Experimental.Tests
{
    [TestClass]
    public class WaveformBuilderTests
    {
        [TestMethod]
        public void Test()
        {
            var vcd = new VCDBuilder(@"C:\tmp\1.vcd");
            vcd.Scopes.Add(new VCDScope()
            {
                Name = "TOP",
                Scopes = new List<VCDScope>()
                {
                    new VCDScope()
                    {
                        Name = "ChildScope1",
                        Variables = new List<VCDVariable>()
                        {
                            new VCDVariable()
                            {
                                Name = "Signal1",
                                Size = 1
                            }
                        }

                    }
                },

                Variables = new List<VCDVariable>()
                {
                    new VCDVariable()
                    {
                        Name = "Data",
                        Size = 8
                    }
                }
            });

            vcd.Init();
            vcd.Snapshot(0, new VCDSignalsSnapshot()
            {
                { "TOP_Data", "1" }
            });
        }
    }
}
