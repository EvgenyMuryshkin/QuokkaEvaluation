using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace QRV32.Tests.Compliance
{
    [TestClass]
    public class ComplianceRV32ITests : ComplianceTestsBase
    {
        [TestMethod]
        public void I_ADD_01() => RunTest("I-ADD-01");
        [TestMethod]
        public void I_ADDI_01() => RunTest("I-ADDI-01");
        [TestMethod]
        public void I_AND_01() => RunTest("I-AND-01");
        [TestMethod]
        public void I_ANDI_01() => RunTest("I-ANDI-01");
        [TestMethod]
        public void I_AUIPC_01() => RunTest("I-AUIPC-01");
        [TestMethod]
        public void I_BEQ_01() => RunTest("I-BEQ-01");
        [TestMethod]
        public void I_BGE_01() => RunTest("I-BGE-01");
        [TestMethod]
        public void I_BGEU_01() => RunTest("I-BGEU-01");
        [TestMethod]
        public void I_BLT_01() => RunTest("I-BLT-01");
        [TestMethod]
        public void I_BLTU_01() => RunTest("I-BLTU-01");
        [TestMethod]
        public void I_BNE_01() => RunTest("I-BNE-01");
        [TestMethod]
        public void I_DELAY_SLOTS_01() => RunTest("I-DELAY_SLOTS-01");
        [TestMethod]
        public void I_EBREAK_01()
        {
            var sim = RunTest("I-EBREAK-01", allowAllZeroAsserts: true);
            // x1 containes address of the memory with test results
            var expectedValues = new[]
            {
                3U,         // mcause - Breakpoint
                0x11111111U,// test data
                0U,         // zero from interrupt handler
                0U          // zero from trailing instruction
            };

            var testAddress = 64U;
            Assert.AreEqual(sim.TopLevel.Regs.State.x[1], (testAddress * 4) + 12);
            for (var i = 0; i < expectedValues.Length; i++)
            {
                var actualWordAddress = testAddress + i;
                var expected = expectedValues[i];
                var actual = sim.MemoryBlock[actualWordAddress];

                Assert.AreEqual(expected, actual, $"Failed for value at {i} (0x{actualWordAddress:X8})");
            }
        }

        [TestMethod]
        public void I_ECALL_01() => RunTest("I-ECALL-01");
        [TestMethod]
        public void I_ENDIANESS_01() => RunTest("I-ENDIANESS-01");
        [TestMethod]
        public void I_IO_01() => RunTest("I-IO-01");
        [TestMethod]
        public void I_JAL_01() => RunTest("I-JAL-01");
        [TestMethod]
        public void I_JALR_01() => RunTest("I-JALR-01");
        [TestMethod]
        public void I_LB_01() => RunTest("I-LB-01");
        [TestMethod]
        public void I_LBU_01() => RunTest("I-LBU-01");
        [TestMethod]
        public void I_LH_01() => RunTest("I-LH-01");
        [TestMethod]
        public void I_LHU_01() => RunTest("I-LHU-01");
        [TestMethod]
        public void I_LUI_01() => RunTest("I-LUI-01");
        [TestMethod]
        public void I_LW_01() => RunTest("I-LW-01");
        [TestMethod]
        public void I_MISALIGN_JMP_01() => RunTest("I-MISALIGN_JMP-01");
        [TestMethod]
        public void I_MISALIGN_LDST_01() => RunTest("I-MISALIGN_LDST-01");
        [TestMethod]
        public void I_NOP_01() => RunTest("I-NOP-01");
        [TestMethod]
        public void I_OR_01() => RunTest("I-OR-01");
        [TestMethod]
        public void I_ORI_01() => RunTest("I-ORI-01");
        [TestMethod]
        public void I_RF_size_01() => RunTest("I-RF_size-01");
        [TestMethod]
        public void I_RF_width_01() => RunTest("I-RF_width-01");
        [TestMethod]
        public void I_RF_x0_01() => RunTest("I-RF_x0-01", allowAllZeroAsserts: true);
        [TestMethod]
        public void I_SB_01() => RunTest("I-SB-01");
        [TestMethod]
        public void I_SH_01() => RunTest("I-SH-01");
        [TestMethod]
        public void I_SLL_01() => RunTest("I-SLL-01");
        [TestMethod]
        public void I_SLLI_01() => RunTest("I-SLLI-01");
        [TestMethod]
        public void I_SLT_01() => RunTest("I-SLT-01");
        [TestMethod]
        public void I_SLTI_01() => RunTest("I-SLTI-01");
        [TestMethod]
        public void I_SLTIU_01() => RunTest("I-SLTIU-01");
        [TestMethod]
        public void I_SLTU_01() => RunTest("I-SLTU-01");
        [TestMethod]
        public void I_SRA_01() => RunTest("I-SRA-01");
        [TestMethod]
        public void I_SRAI_01() => RunTest("I-SRAI-01");
        [TestMethod]
        public void I_SRL_01() => RunTest("I-SRL-01");
        [TestMethod]
        public void I_SRLI_01() => RunTest("I-SRLI-01");
        [TestMethod]
        public void I_SUB_01() => RunTest("I-SUB-01");
        [TestMethod]
        public void I_SW_01() => RunTest("I-SW-01");
        [TestMethod]
        public void I_XOR_01() => RunTest("I-XOR-01");
        [TestMethod]
        public void I_XORI_01() => RunTest("I-XORI-01");
    }
}
