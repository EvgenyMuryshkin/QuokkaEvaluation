using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fourier.Tests
{
    [TestClass]
    public class GeneratorToolsTests
    {
        [TestMethod]
        public void ZeroLength()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => GeneratorTools.ArrayLength(0));
        }

        [TestMethod]
        public void Length10bits()
        {
            Assert.AreEqual(1024, GeneratorTools.ArrayLength(10));
        }

        [TestMethod]
        public void Length16bits()
        {
            Assert.AreEqual(65536, GeneratorTools.ArrayLength(16));
        }

        [TestMethod]
        public void FloatLength10bits()
        {
            Assert.AreEqual(1024f, GeneratorTools.FloatArrayLength(10));
        }


        [TestMethod]
        public void Mask10bits()
        {
            Assert.AreEqual(1023, GeneratorTools.Mask(10));
        }

        [TestMethod]
        public void Mask16bits()
        {
            Assert.AreEqual(65535, GeneratorTools.Mask(16));
        }

        [TestMethod]
        public void CosArray8PointsForward()
        {
            var cos45 = (float)(Math.Sqrt(2) / 2);

            var arr = GeneratorTools.CosArray(8, Direction.Forward);

            var expected = new float[] { 1, cos45, 0, -cos45, -1, -cos45, 0, cos45 };
        }
    }
}
