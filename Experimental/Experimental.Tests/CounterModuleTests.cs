using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class CounterModuleTests
    {
        [TestMethod]
        public void CounterTest()
        {
            var sim = new RTLSimulator<CounterModule>();
            sim.IsRunning = (cb) => cb.Clock < 100;
            sim.Trace(PathTools.VCDOutputPath());
            sim.TopLevel.Schedule(() => new CounterInputs() { InReset = false });

            Assert.AreEqual(0, sim.TopLevel.Value);
            sim.Run();
            Assert.AreEqual(100, sim.TopLevel.Value);
        }
    }
}
