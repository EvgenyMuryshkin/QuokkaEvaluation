using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QRV32.Compliance
{
    [TestClass]
    public class RV32Zifencei : ComplianceTestsBase
    {
        public RV32Zifencei() : base("rv32Zifencei", "rv32i") { }
        public override ComplianceCPUSimilator RunAndAssert(string testName)
        {
            Assert.Inconclusive($"Test was disabled: {testName}");
            return null;
        }

        [TestMethod]
        public void I_FENCE_I_01() => RunAndAssert("I-FENCE.I-01");
    }
}
