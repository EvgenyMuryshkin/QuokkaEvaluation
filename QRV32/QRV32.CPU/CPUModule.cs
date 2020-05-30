using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public enum CPUState : byte
    {
        Reset,
        IF,
        ID,
        EX,
        MEM,
        WB,
        Halt
    }

    public class CPUModuleInputs
    {
        public uint BaseAddress;
        public uint MemReadValue;
        public bool MemReady;
    }

    public class CPUModuleState
    {
        public CPUState State;
        public uint Instruction;

        public bool WBDataReady;
        public uint WBData;
    }

    public class CPUModule : RTLSynchronousModule<CPUModuleInputs, CPUModuleState>
    {
        internal InstructionDecoderModule ID = new InstructionDecoderModule();
        internal PCModule PC = new PCModule();
        internal RegistersBlockModule Regs = new RegistersBlockModule();
        internal ALUModule ALU = new ALUModule();
        internal CompareModule CMP = new CompareModule();
        public bool MemRead => State.State == CPUState.IF;
        public uint MemAddress => PC.PC;

        bool IsJump => OpCode == OpTypeCodes.JAL || OpCode == OpTypeCodes.JALR;
        RTLBitArray ResetAddress => Inputs.BaseAddress;
        RTLBitArray InstructionOffset => new RTLBitArray(4).Unsigned();
        RTLBitArray JumpOffset => OpCode == OpTypeCodes.JAL ? ID.JTypeImm : new RTLBitArray(new RTLBitArray(Regs.RS1 + ID.ITypeImm)[31, 1], false);

        bool PCWE => State.State == CPUState.Reset || State.State == CPUState.WB;
        RTLBitArray PCInstOffset => IsJump ? JumpOffset : InstructionOffset;
        RTLBitArray PCOffset => State.State == CPUState.Reset ? ResetAddress : PCInstOffset;
        bool PCOverwrite => State.State == CPUState.Reset;

        RTLBitArray ALUOp1 => Regs.RS1;
        RTLBitArray ALUOp2 => OpCode == OpTypeCodes.OPIMM ? ID.ITypeImm : Regs.RS2;

        RTLBitArray ALUSHAMT => OpCode == OpTypeCodes.OPIMM ? ID.SHAMT : Regs.RS2[4, 0];
        bool RegsRead => State.State == CPUState.ID;
        bool RegsWE => State.State == CPUState.WB && State.WBDataReady;

        RTLBitArray CMPLhs => Regs.RS1;
        RTLBitArray CMPRhs => OpCode == OpTypeCodes.OPIMM ? ID.ITypeImm : Regs.RS2;

        OpTypeCodes OpCode => (OpTypeCodes)(byte)ID.OpCode;
        OPIMMCodes OPIMMCode => (OPIMMCodes)(byte)ID.Funct3;
        OPCodes OPCode => (OPCodes)(byte)ID.Funct3;

        protected override void OnSchedule(Func<CPUModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            ID.Schedule(() => new InstructionDecoderInputs() 
            { 
                Instruction = State.Instruction 
            });

            PC.Schedule(() => new PCModuleInputs() 
            { 
                WE = PCWE, 
                Offset = PCOffset, 
                Overwrite = PCOverwrite 
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

        void Halt()
        {
            NextState.State = CPUState.Halt;
        }

        void OnOPIMM()
        {
            NextState.WBDataReady = true;
            switch(OPIMMCode)
            {
                case OPIMMCodes.ADDI:
                    NextState.WBData = ALU.ADD;
                    break;
                case OPIMMCodes.SLTI:
                    NextState.WBData = CMP.SLT ? 1U : 0U;
                    break;
                case OPIMMCodes.SLTIU:
                    NextState.WBData = CMP.ULT ? 1U : 0U;
                    break;
                case OPIMMCodes.ANDI:
                    NextState.WBData = ALU.resAND;
                    break;
                case OPIMMCodes.ORI:
                    NextState.WBData = ALU.resOR;
                    break;
                case OPIMMCodes.XORI:
                    NextState.WBData = ALU.resXOR;
                    break;
                case OPIMMCodes.SLLI:
                    NextState.WBData = ALU.SHLL;
                    break;
                case OPIMMCodes.SRLI_SRAI:
                    if (ID.SHARITH)
                    {
                        NextState.WBData = ALU.SHRA;
                    }
                    else
                    {
                        NextState.WBData = ALU.SHRL;
                    }
                    break;
                default:
                    Halt();
                    break;
            }
        }

        void OnOP()
        {
            NextState.WBDataReady = true;
            switch (OPCode)
            {
                case OPCodes.ADD_SUB:
                    if (ID.SUB)
                    {
                        NextState.WBData = ALU.SUB;
                    }
                    else
                    {
                        NextState.WBData = ALU.ADD;
                    }
                    break;
                case OPCodes.SLT:
                    NextState.WBData = CMP.SLT ? 1U : 0U;
                    break;
                case OPCodes.SLTU:
                    NextState.WBData = CMP.ULT ? 1U : 0U;
                    break;
                case OPCodes.AND:
                    NextState.WBData = ALU.resAND;
                    break;
                case OPCodes.OR:
                    NextState.WBData = ALU.resOR;
                    break;
                case OPCodes.XOR:
                    NextState.WBData = ALU.resXOR;
                    break;
                case OPCodes.SLL:
                    NextState.WBData = ALU.SHLL;
                    break;
                case OPCodes.SRL_SRA:
                    if (ID.SHARITH)
                    {
                        NextState.WBData = ALU.SHRA;
                    }
                    else
                    {
                        NextState.WBData = ALU.SHRL;
                    }
                    break;
                default:
                    Halt();
                    break;
            }
        }

        void ExecuteStage()
        {
            NextState.State = CPUState.MEM;
            NextState.WBDataReady = false;

            switch (OpCode)
            {
                case OpTypeCodes.OPIMM:
                    OnOPIMM();
                    break;
                case OpTypeCodes.OP:
                    OnOP();
                    break;
                case OpTypeCodes.LUI:
                    NextState.WBDataReady = true;
                    NextState.WBData = ID.UTypeImm;
                    break;
                case OpTypeCodes.AUIPC:
                    NextState.WBDataReady = true;
                    NextState.WBData = PC.PC + ID.UTypeImm;
                    break;
                case OpTypeCodes.JAL:
                    NextState.WBDataReady = true;
                    NextState.WBData = PC.PC + InstructionOffset;
                    break;
                case OpTypeCodes.JALR:
                    NextState.WBDataReady = true;
                    NextState.WBData = PC.PC + InstructionOffset;
                    break;
                default:
                    Halt();
                    break;
            }
        }

        protected override void OnStage()
        {
            switch (State.State)
            {
                case CPUState.Reset:
                    NextState.State = CPUState.IF;
                    break;
                case CPUState.IF:
                    if (Inputs.MemReady)
                    {
                        NextState.State = CPUState.ID;
                        NextState.Instruction = Inputs.MemReadValue;
                    }
                    break;
                case CPUState.ID:
                    if (Regs.Ready)
                    {
                        NextState.State = CPUState.EX;
                    }
                    break;
                case CPUState.EX:
                    ExecuteStage();
                    break;
                case CPUState.MEM:
                    NextState.State = CPUState.WB;
                    break;
                case CPUState.WB:
                    NextState.State = CPUState.IF;
                    break;
            }
        }
    }
}
