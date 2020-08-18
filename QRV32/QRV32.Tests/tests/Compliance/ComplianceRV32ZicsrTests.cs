using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QRV32.Compliance
{
    [TestClass]
    public class RV32Zicsr : ComplianceTestsBase
    {
        public RV32Zicsr() : base("rv32Zicsr") { }

        [TestMethod]
        public void I_CSRRC_01() => RunAndAssert("I-CSRRC-01");
        [TestMethod]
        public void I_CSRRCI_01() => RunAndAssert("I-CSRRCI-01");
        [TestMethod]
        public void I_CSRRS_01() => RunAndAssert("I-CSRRS-01");
        [TestMethod]
        public void I_CSRRSI_01() => RunAndAssert("I-CSRRSI-01");
        [TestMethod]
        public void I_CSRRW_01() => RunAndAssert("I-CSRRW-01");
        [TestMethod]
        public void I_CSRRWI_01() => RunAndAssert("I-CSRRWI-01");
    }
}
