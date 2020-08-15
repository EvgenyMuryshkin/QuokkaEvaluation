using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace QRV32.Compliance.RV32I
{
    [TestClass]
    public class ComplianceRV32ITests : ComplianceTestsBase
    {
        [TestMethod]
        public void I_ADD_01() => RunAndAssert("I-ADD-01");
        [TestMethod]
        public void I_ADDI_01() => RunAndAssert("I-ADDI-01");
        [TestMethod]
        public void I_AND_01() => RunAndAssert("I-AND-01");
        [TestMethod]
        public void I_ANDI_01() => RunAndAssert("I-ANDI-01");
        [TestMethod]
        public void I_AUIPC_01() => RunAndAssert("I-AUIPC-01");
        [TestMethod]
        public void I_BEQ_01() => RunAndAssert("I-BEQ-01");
        [TestMethod]
        public void I_BGE_01() => RunAndAssert("I-BGE-01");
        [TestMethod]
        public void I_BGEU_01() => RunAndAssert("I-BGEU-01");
        [TestMethod]
        public void I_BLT_01() => RunAndAssert("I-BLT-01");
        [TestMethod]
        public void I_BLTU_01() => RunAndAssert("I-BLTU-01");
        [TestMethod]
        public void I_BNE_01() => RunAndAssert("I-BNE-01");
        [TestMethod]
        public void I_DELAY_SLOTS_01() => RunAndAssert("I-DELAY_SLOTS-01");
        [TestMethod]
        public void I_EBREAK_01() => RunAndAssert("I-EBREAK-01");
        [TestMethod]
        public void I_ECALL_01() => RunAndAssert("I-ECALL-01");
        [TestMethod]
        public void I_ENDIANESS_01() => RunAndAssert("I-ENDIANESS-01");
        [TestMethod]
        public void I_IO_01() => RunAndAssert("I-IO-01");
        [TestMethod]
        public void I_JAL_01() => RunAndAssert("I-JAL-01");
        [TestMethod]
        public void I_JALR_01() => RunAndAssert("I-JALR-01");
        [TestMethod]
        public void I_LB_01() => RunAndAssert("I-LB-01");
        [TestMethod]
        public void I_LBU_01() => RunAndAssert("I-LBU-01");
        [TestMethod]
        public void I_LH_01() => RunAndAssert("I-LH-01");
        [TestMethod]
        public void I_LHU_01() => RunAndAssert("I-LHU-01");
        [TestMethod]
        public void I_LUI_01() => RunAndAssert("I-LUI-01");
        [TestMethod]
        public void I_LW_01() => RunAndAssert("I-LW-01");
        [TestMethod]
        public void I_MISALIGN_JMP_01()
        {
            Run("I-MISALIGN_JMP-01");

            // test_A1_res_exc
            var test_A1_res_exc = new[]
            {
                (uint)MCAUSE.MECall,// mcause - Breakpoint
                0x11111111U,            // test data
                0U,                     // zero from interrupt handler
                0U                      // zero from trailing instruction
            };

        }

        [TestMethod]
        public void I_MISALIGN_LDST_01() => RunAndAssert("I-MISALIGN_LDST-01");
        [TestMethod]
        public void I_NOP_01() => RunAndAssert("I-NOP-01");
        [TestMethod]
        public void I_OR_01() => RunAndAssert("I-OR-01");
        [TestMethod]
        public void I_ORI_01() => RunAndAssert("I-ORI-01");
        [TestMethod]
        public void I_RF_size_01() => RunAndAssert("I-RF_size-01");
        [TestMethod]
        public void I_RF_width_01() => RunAndAssert("I-RF_width-01");
        [TestMethod]
        public void I_RF_x0_01() => RunAndAssert("I-RF_x0-01");
        [TestMethod]
        public void I_SB_01() => RunAndAssert("I-SB-01");
        [TestMethod]
        public void I_SH_01() => RunAndAssert("I-SH-01");
        [TestMethod]
        public void I_SLL_01() => RunAndAssert("I-SLL-01");
        [TestMethod]
        public void I_SLLI_01() => RunAndAssert("I-SLLI-01");
        [TestMethod]
        public void I_SLT_01() => RunAndAssert("I-SLT-01");
        [TestMethod]
        public void I_SLTI_01() => RunAndAssert("I-SLTI-01");
        [TestMethod]
        public void I_SLTIU_01() => RunAndAssert("I-SLTIU-01");
        [TestMethod]
        public void I_SLTU_01() => RunAndAssert("I-SLTU-01");
        [TestMethod]
        public void I_SRA_01() => RunAndAssert("I-SRA-01");
        [TestMethod]
        public void I_SRAI_01() => RunAndAssert("I-SRAI-01");
        [TestMethod]
        public void I_SRL_01() => RunAndAssert("I-SRL-01");
        [TestMethod]
        public void I_SRLI_01() => RunAndAssert("I-SRLI-01");
        [TestMethod]
        public void I_SUB_01() => RunAndAssert("I-SUB-01");
        [TestMethod]
        public void I_SW_01() => RunAndAssert("I-SW-01");
        [TestMethod]
        public void I_XOR_01() => RunAndAssert("I-XOR-01");
        [TestMethod]
        public void I_XORI_01() => RunAndAssert("I-XORI-01");
    }
}
