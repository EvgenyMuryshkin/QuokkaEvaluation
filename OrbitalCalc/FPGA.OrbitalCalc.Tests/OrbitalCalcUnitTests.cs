using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPGA.OrbitalCalc.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void VInclination()
        {
            var R = 6371e+3f;
            var M = 5.972e+24f;
            var Inc = 0.4974188f;
            var nonOptimized = FPGAOrbitalCalc.DeltaVInclinationOrbit(M, R + 100e+3f, R + 1000e3f, Inc);
            var optimized = FPGAOrbitalCalc.DeltaVInclinationOrbitOptimized(M, R + 100e+3f, R + 1000e3f, Inc);

            Assert.AreEqual(nonOptimized, optimized);
        }
    }
}
