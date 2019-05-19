using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var vcd = new VCDBuilder();
            vcd.Scopes.Add(new VCDScope()
            {
                Name = "TOP Module 1",
                Scopes = new List<VCDScope>()
                {
                    new VCDScope()
                    {
                        Name = "Child Scope 1",
                        Variables = new List<VCDVariable>()
                        {
                            new VCDVariable()
                            {
                                Name = "Signal 1",
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

            File.WriteAllText(@"C:\tmp\1.vcd", vcd.ToString());
        }
    }
}
