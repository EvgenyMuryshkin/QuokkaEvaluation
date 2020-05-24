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

        bool PCWE => State.State == CPUState.Reset || State.State == CPUState.WB;
        uint PCOffset => State.State == CPUState.Reset ? Inputs.BaseAddress : 4;
        bool PCOverwrite => State.State == CPUState.Reset;

        RTLBitArray ALUOp1 => Regs.RS1;
        RTLBitArray ALUOp2 => ID.ITypeImm;

        bool RegsRead => State.State == CPUState.ID;
        bool RegsWE => State.State == CPUState.WB && State.WBDataReady;

        RTLBitArray CMPLhs => Regs.RS1;
        RTLBitArray CMPRhs => ID.ITypeImm;

        OpCodes OpCode => (OpCodes)(byte)ID.OpCode;
        OPIMMCodes OPIMMCode => (OPIMMCodes)(byte)ID.Funct3;

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
                Op2 = ALUOp2 
            });

            CMP.Schedule(() => new CompareModuleInputs() 
            { 
                Lhs = CMPLhs, 
                Rhs = CMPRhs 
            });
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
                    NextState.State = CPUState.MEM;
                    NextState.WBDataReady = false;

                    switch (OpCode)
                    {
                        case OpCodes.OPIMM:
                            OnOPIMM();
                            break;
                        default:
                            NextState.State = CPUState.Halt;
                            break;
                    }

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
