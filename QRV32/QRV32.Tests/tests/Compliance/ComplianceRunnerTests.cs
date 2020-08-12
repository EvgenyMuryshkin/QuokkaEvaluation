using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QRV32.Tests.Compliance
{
    [TestClass]
    public class ComplianceRunnerTests : ComplianceTestsBase
    {
        [TestMethod]
        public void _01_ShouldPass() => RunTest("I-ShouldPass");

        [TestMethod]
        public void _02_ShouldFail()
        {
            Assert.ThrowsException<AssertFailedException>(() => RunTest("I-ShouldFail"));
        }
    }
}
