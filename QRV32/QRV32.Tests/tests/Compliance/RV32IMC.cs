using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QRV32.Compliance
{
    //[TestClass]
    public class RV32IMC : ComplianceTestsBase
    {
        public RV32IMC() : base("rv32imc", "rv32imc") { }
        public override ComplianceCPUSimilator RunAndAssert(string testName)
        {
            Assert.Inconclusive($"Test was disabled: {testName}");
            return null;
        }

        [TestMethod]
        public void C_ADD() => RunAndAssert("C-ADD");
        [TestMethod]
        public void C_ADDI() => RunAndAssert("C-ADDI");
        [TestMethod]
        public void C_ADDI16SP() => RunAndAssert("C-ADDI16SP");
        [TestMethod]
        public void C_ADDI4SPN() => RunAndAssert("C-ADDI4SPN");
        [TestMethod]
        public void C_AND() => RunAndAssert("C-AND");
        [TestMethod]
        public void C_ANDI() => RunAndAssert("C-ANDI");
        [TestMethod]
        public void C_BEQZ() => RunAndAssert("C-BEQZ");
        [TestMethod]
        public void C_BNEZ() => RunAndAssert("C-BNEZ");
        [TestMethod]
        public void C_J() => RunAndAssert("C-J");
        [TestMethod]
        public void C_JAL() => RunAndAssert("C-JAL");
        [TestMethod]
        public void C_JALR() => RunAndAssert("C-JALR");
        [TestMethod]
        public void C_JR() => RunAndAssert("C-JR");
        [TestMethod]
        public void C_LI() => RunAndAssert("C-LI");
        [TestMethod]
        public void C_LUI() => RunAndAssert("C-LUI");
        [TestMethod]
        public void C_LW() => RunAndAssert("C-LW");
        [TestMethod]
        public void C_LWSP() => RunAndAssert("C-LWSP");
        [TestMethod]
        public void C_MV() => RunAndAssert("C-MV");
        [TestMethod]
        public void C_OR() => RunAndAssert("C-OR");
        [TestMethod]
        public void C_SLLI() => RunAndAssert("C-SLLI");
        [TestMethod]
        public void C_SRAI() => RunAndAssert("C-SRAI");
        [TestMethod]
        public void C_SRLI() => RunAndAssert("C-SRLI");
        [TestMethod]
        public void C_SUB() => RunAndAssert("C-SUB");
        [TestMethod]
        public void C_SW() => RunAndAssert("C-SW");
        [TestMethod]
        public void C_SWSP() => RunAndAssert("C-SWSP");
        [TestMethod]
        public void C_XOR() => RunAndAssert("C-XOR");
    }
}
