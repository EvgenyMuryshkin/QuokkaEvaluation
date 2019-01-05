using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fourier.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void FloatAreEqual()
        {
            Validation.AssertFloatEqual(1f, 1f + Validation.Epsilon * 0.9f);
        }

        [TestMethod]
        public void FloatAreNotEqual()
        {
            Assert.ThrowsException<AssertFailedException>(() => Validation.AssertFloatEqual(1f, 1f + Validation.Epsilon * 1.1f));
        }
    }
}
