using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class FullAdderTests
    {
        [TestMethod]
        public void Test()
        {
            var values = new[]
            {
                new { A = 0, B = 0, CIn = 0, O = 0, COut = 0 },
                new { A = 1, B = 0, CIn = 0, O = 1, COut = 0 },
                new { A = 0, B = 1, CIn = 0, O = 1, COut = 0 },
                new { A = 1, B = 1, CIn = 0, O = 0, COut = 1 },
                new { A = 0, B = 0, CIn = 1, O = 1, COut = 0 },
                new { A = 1, B = 0, CIn = 1, O = 0, COut = 1 },
                new { A = 0, B = 1, CIn = 1, O = 0, COut = 1 },
                new { A = 1, B = 1, CIn = 1, O = 1, COut = 1 },
            };

            Func<int, bool> toBool = (v) => v == 0 ? false : true;

            foreach (var set in values)
            {
                var sim = new RTLSimulator<FullAdderModule>();
                sim.IsRunning = (cb) => cb.Clock == 0;
                sim.TopLevel.Schedule(() => new FullAdderInputs() { A = toBool(set.A), B = toBool(set.B), CIn = toBool(set.CIn) });
                sim.Run();
                Assert.AreEqual(toBool(set.O), sim.TopLevel.O);
                Assert.AreEqual(toBool(set.COut), sim.TopLevel.COut);
            }
        }
    }
}

