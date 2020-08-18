using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Compliance;

namespace QRV32.Compliance
{
    [TestClass]
    public class Runner : ComplianceTestsBase
    {
        public Runner() : base("runner") { }

        [TestMethod]
        public void I_ShouldPass() => RunAndAssert("I-ShouldPass");

        [TestMethod]
        public void I_ShouldFail()
        {
            Assert.ThrowsException<AssertFailedException>(() => RunAndAssert("I-ShouldFail"));
        }
    }
}
