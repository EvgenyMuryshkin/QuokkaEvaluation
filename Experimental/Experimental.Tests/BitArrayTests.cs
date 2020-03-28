using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class BitArrayTests
    {
        [TestMethod]
        public void Test()
        {
            var sim = new RTLSimulator<BitArrayModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            sim.TopLevel.Schedule(() => new BitArrayInputs() { Value = 0xC2 });
            sim.Run();
            Assert.AreEqual(0xC2, (byte)sim.TopLevel.Direct);
            Assert.AreEqual(0xC,  (byte)sim.TopLevel.High);
            Assert.AreEqual(0x2,  (byte)sim.TopLevel.Low);
            Assert.AreEqual(0x43, (byte)sim.TopLevel.Reversed);
            Assert.AreEqual(0x3,  (byte)sim.TopLevel.ReversedHigh);
            Assert.AreEqual(0x4,  (byte)sim.TopLevel.ReversedLow);
            Assert.AreEqual(0xA,  (byte)sim.TopLevel.Picks);
        }
    }
}

