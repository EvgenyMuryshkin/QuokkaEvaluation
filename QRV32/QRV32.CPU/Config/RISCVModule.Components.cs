using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        internal InstructionDecoderModule ID = new InstructionDecoderModule();
        internal IRegistersModule Regs = null;
        internal ALUModule ALU = new ALUModule();
        internal CompareModule CMP = new CompareModule();

        RTLBitArray ALUOp1 => Regs.RS1;
        RTLBitArray ALUOp2 => ID.OpTypeCode == OpTypeCodes.OPIMM ? ID.ITypeImm : Regs.RS2;

        RTLBitArray ALUSHAMT => ID.OpTypeCode == OpTypeCodes.OPIMM ? ID.SHAMT : Regs.RS2[4, 0];
        bool RegsRead => State.State == CPUState.ID && !Regs.Ready;
        bool RegsWE => State.State == CPUState.WB && State.WBDataReady;

        RTLBitArray CMPLhs => Regs.RS1;
        RTLBitArray CMPRhs => ID.OpTypeCode == OpTypeCodes.OPIMM ? ID.ITypeImm : Regs.RS2;

        public RISCVModule()
        {
            var ramRegs = true;
            if (ramRegs)
            {
                Regs = new RegistersRAMModule();
            }
            else
            {
                Regs = new RegistersBlockModule();
            }
        }

        protected override void OnSchedule(Func<RISCVModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            ID.Schedule(() => new InstructionDecoderInputs()
            {
                Instruction = State.Instruction
            });

            Regs.Schedule(() => new RegistersModuleInput()
            {
                RS1Addr = ID.RS1,
                RS2Addr = ID.RS2,
                RD = ID.RD,
                Read = RegsRead,
                WriteData = State.WBData,
                WE = RegsWE
            });

            ALU.Schedule(() => new ALUModuleInputs()
            {
                Op1 = ALUOp1,
                Op2 = ALUOp2,
                SHAMT = ALUSHAMT
            });

            CMP.Schedule(() => new CompareModuleInputs()
            {
                Lhs = CMPLhs,
                Rhs = CMPRhs
            });
        }
    }
}
