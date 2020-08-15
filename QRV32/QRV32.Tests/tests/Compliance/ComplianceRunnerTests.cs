using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Compliance.RV32I;

namespace QRV32.Compliance.Runner.Tests
{
    [TestClass]
    public class ComplianceRunnerTests : ComplianceTestsBase
    {
        [TestMethod]
        public void I_ShouldPass() => RunAndAssert("I-ShouldPass");

        [TestMethod]
        public void I_ShouldFail()
        {
            Assert.ThrowsException<AssertFailedException>(() => RunAndAssert("I-ShouldFail"));
        }
    }
}
