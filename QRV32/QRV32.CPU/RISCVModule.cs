using Autofac.Core.Lifetime;
using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace QRV32.CPU
{
    public partial class RISCVModule : RTLSynchronousModule<RISCVModuleInputs, CPUModuleState>
    {
        public byte DbgState => (byte)State.State;
        public RTLBitArray DbgWBData => ID.UTypeImm;
        public bool DbgWDDataReady => RegsWE;

        internal bool IsLoadOp => ID.OpTypeCode == OpTypeCodes.LOAD;
        internal bool IsStoreOp => ID.OpTypeCode == OpTypeCodes.STORE;

        public bool MemRead => State.State == CPUState.IF || (State.State == CPUState.MEM && IsLoadOp);
        public bool MemWrite => State.State == CPUState.MEM && IsStoreOp;
        uint internalMemAddress
        {
            get
            {
                uint address = 0;
                if (State.State == CPUState.IF)
                {
                    address = State.PC;
                }
                else if (IsLoadOp)
                {
                    address = Regs.RS1 + ID.ITypeImm;
                }
                else if (IsStoreOp)
                {
                    address = Regs.RS1 + ID.STypeImm;
                }

                return address;
            }
        }
        public uint MemAddress => internalMemAddress;

        public bool IsHalted => State.State == CPUState.Halt;

        public RTLBitArray MemWriteData => Regs.RS2;
        public RTLBitArray MemWriteMode => ID.Funct3;

        RTLBitArray ALUOp1 => Regs.RS1;
        RTLBitArray ALUOp2 => ID.OpTypeCode == OpTypeCodes.OPIMM ? ID.ITypeImm : Regs.RS2;

        RTLBitArray ALUSHAMT => ID.OpTypeCode == OpTypeCodes.OPIMM ? ID.SHAMT : Regs.RS2[4, 0];
        bool RegsRead => State.State == CPUState.ID;
        bool RegsWE => State.State == CPUState.WB && State.WBDataReady;

        RTLBitArray CMPLhs => Regs.RS1;
        RTLBitArray CMPRhs => ID.OpTypeCode == OpTypeCodes.OPIMM ? ID.ITypeImm : Regs.RS2;

        RTLBitArray CSRAddress => new RTLBitArray((byte)CSRLookup())[3, 0];

        void Halt()
        {
            // TODO: add halt reasong as parameter and support in translator
            NextState.State = CPUState.Halt;

            // calls to Debugger and Trace are not translated into HDL.
            Trace.WriteLine($"CPU halted");
            Debugger.Break();
        }

        CSRAddr CSRLookup()
        {
            CSRAddr address = CSRAddr.mvendorid;

            switch (ID.CSRAddress)
            {
                case CSRCodes.mvendorid: address = CSRAddr.mvendorid;   break;
                case CSRCodes.marchid:   address = CSRAddr.marchid;     break;
                case CSRCodes.mimpid:    address = CSRAddr.mimpid;      break;
                case CSRCodes.mhartid:   address = CSRAddr.mhartid;     break;
                case CSRCodes.mstatus:   address = CSRAddr.mstatus;     break;
                case CSRCodes.misa:      address = CSRAddr.misa;        break;
                case CSRCodes.mie:       address = CSRAddr.mie;         break;
                case CSRCodes.mtvec:     address = CSRAddr.mtvec;       break;
                case CSRCodes.mepc:      address = CSRAddr.mepc;        break;
                case CSRCodes.mcause:    address = CSRAddr.mcause;      break;
                case CSRCodes.mtval:     address = CSRAddr.mtval;       break;
                case CSRCodes.mip:       address = CSRAddr.mip;         break;
            }

            return address;
        }
        
        void OnOPIMM()
        {
            NextState.WBDataReady = true;
            switch(ID.OPIMMCode)
            {
                case OPIMMCodes.ADDI:
                    NextState.WBData = ALU.ADD;
                    break;
                case OPIMMCodes.SLTI:
                    NextState.WBData = CMP.LTS ? 1U : 0U;
                    break;
                case OPIMMCodes.SLTIU:
                    NextState.WBData = CMP.LTU ? 1U : 0U;
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
            switch (ID.OPCode)
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
                    NextState.WBData = CMP.LTS ? 1U : 0U;
                    break;
                case OPCodes.SLTU:
                    NextState.WBData = CMP.LTU ? 1U : 0U;
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

        RTLBitArray BranchOffset => ID.BTypeImm;
        void OnBranch()
        {
            /*
            TODO:
            var cmpRes = new RTLBitArray(CMP.EQ, CMP.NE...)
            if (cmpRes[BranchTypeCode])
                NextState.PCOffset = PC.PC + ID.BTypeImm;
            */
            switch (ID.BranchTypeCode)
            {
                case BranchTypeCodes.EQ:
                    if (CMP.EQ)
                        NextState.PCOffset = BranchOffset;
                    break;
                case BranchTypeCodes.NE:
                    if (CMP.NE)
                        NextState.PCOffset = BranchOffset;
                    break;
                case BranchTypeCodes.GE:
                    if (CMP.GTS || CMP.EQ)
                        NextState.PCOffset = BranchOffset;
                    break;
                case BranchTypeCodes.GEU:
                    if (CMP.GTU || CMP.EQ)
                        NextState.PCOffset = BranchOffset;
                    break;
                case BranchTypeCodes.LT:
                    if (CMP.LTS)
                        NextState.PCOffset = BranchOffset;
                    break;
                case BranchTypeCodes.LTU:
                    if (CMP.LTU)
                        NextState.PCOffset = BranchOffset;
                    break;
                default:
                    Halt();
                    break;
            }
        }

        bool IsCSR => ID.SystemCode >= SystemCodes.CSRRW && ID.SystemCode <= SystemCodes.CSRRCI;
        RTLBitArray CSRI => ID.RS1.Unsigned().Resized(32);
        bool CSRWE => ID.RS1 != 0 && CSRAddress != 0;

        bool HasMTVEC => State.CSR[(byte)CSRAddr.mtvec] != 0;

        void OnSystem()
        {
            if (ID.SystemCode == SystemCodes.E)
            {
                switch(ID.SysTypeCode)
                {
                    case SysTypeCodes.CALL:
                        if (HasMTVEC)
                        {
                            // go into trap handler
                            NextState.pendingMCause = MCAUSE.MECall;
                            NextState.State = CPUState.WB;
                        }
                        else
                        {
                            NextState.State = CPUState.E;
                        }
                        break;
                    case SysTypeCodes.BREAK:
                        if (HasMTVEC)
                        {
                            // go into trap handler
                            NextState.pendingMCause = MCAUSE.Breakpoint;
                            NextState.State = CPUState.WB;
                        }
                        else
                        {
                            NextState.State = CPUState.E;
                        }
                        break;
                    case SysTypeCodes.TRAP:
                        switch(ID.RetTypeCode)
                        {
                            case RetTypeCodes.MRET:
                                // handled in WB
                                break;
                            default:
                                Halt();
                                break;
                        }
                        break;
                    case SysTypeCodes.IRQ:
                        switch (ID.IRQTypeCode)
                        {
                            case IRQTypeCodes.WFI:
                                NextState.State = CPUState.WB;
                                break;
                            default:
                                Halt();
                                break;
                        }
                        break;
                    default:
                        Halt();
                        break;
                }
            }
            else if (IsCSR)
            {
                NextState.State = CPUState.WB;
                NextState.WBData = State.CSR[CSRAddress];
                NextState.WBDataReady = ID.RD != 0;

                if (CSRWE)
                {
                    switch (ID.SystemCode)
                    {
                        case SystemCodes.CSRRW:
                            NextState.CSR[CSRAddress] = Regs.RS1;
                            break;
                        case SystemCodes.CSRRWI:
                            NextState.CSR[CSRAddress] = CSRI;
                            break;
                        case SystemCodes.CSRRS:
                            NextState.CSR[CSRAddress] = State.CSR[CSRAddress] | Regs.RS1;
                            break;
                        case SystemCodes.CSRRSI:
                            NextState.CSR[CSRAddress] = State.CSR[CSRAddress] | CSRI;
                            break;
                        case SystemCodes.CSRRC:
                            NextState.CSR[CSRAddress] = State.CSR[CSRAddress] & !Regs.RS1;
                            break;
                        case SystemCodes.CSRRCI:
                            NextState.CSR[CSRAddress] = State.CSR[CSRAddress] & !CSRI;
                            break;
                        default:
                            Halt();
                            break;
                    }
                }
            }
            else
            {
                Halt();
            }
        }
        bool halfMisaliged => (new RTLBitArray(internalMemAddress))[0] != false;
        bool wordMisaliged => (new RTLBitArray(internalMemAddress))[1, 0] != 0;

        bool MemAddressMisaligned
        {
            get
            {
                bool result = false;
                switch (ID.LoadTypeCode)
                {
                    case LoadTypeCodes.LW:
                        result = wordMisaliged;
                        break;
                    case LoadTypeCodes.LH:
                        result = halfMisaliged;
                        break;
                    case LoadTypeCodes.LHU:
                        result = halfMisaliged;
                        break;
                }

                return result;
            }
        }

        void CheckMemAddressMisalign()
        {
            if (MemAddressMisaligned)
            {
                NextState.State = CPUState.IF;

                // address misalign caused trap, store current address
                DisableInterrupts();
                NextState.CSR[(byte)CSRAddr.mepc] = State.PC;
                NextState.CSR[(byte)CSRAddr.mtval] = internalMemAddress;
                NextState.PC = State.CSR[(byte)CSRAddr.mtvec];

                switch (ID.OpTypeCode)
                {
                    case OpTypeCodes.LOAD:
                        NextState.CSR[(byte)CSRAddr.mcause] = (uint)MCAUSE.LoadAddrMisalign;
                        break;
                    case OpTypeCodes.STORE:
                        NextState.CSR[(byte)CSRAddr.mcause] = (uint)MCAUSE.StoreAddrMisalign;
                        break;
                }
            }
        }

        RTLBitArray InstructionOffset => new RTLBitArray(4).Unsigned();
        RTLBitArray NextSequentialPC => State.PC + InstructionOffset;
        RTLBitArray internalNextPC => ID.OpTypeCode == OpTypeCodes.JALR ? State.PCOffset : State.PC + State.PCOffset;
        public bool PCMisaligned => new RTLBitArray(internalNextPC[1, 0]) != 0;

        RTLBitArray MSTATUS => State.CSR[(byte)CSRAddr.mstatus];
        bool MIE => MSTATUS[3];

        bool MRET => ID.SystemCode == SystemCodes.E && ID.SysTypeCode == SysTypeCodes.TRAP && ID.RetTypeCode == RetTypeCodes.MRET;

        protected override void OnStage()
        {
            switch (State.State)
            {
                case CPUState.Reset:
                    ResetStage();
                    break;
                case CPUState.IF:
                    InstructionFetchStage();
                    break;
                case CPUState.ID:
                    InstructionDecodeStage();
                    break;
                case CPUState.EX:
                    ExecuteStage();
                    break;
                case CPUState.MEM:
                    MemStage();
                    break;
                case CPUState.WB:
                    WriteBackStage();
                    break;
                case CPUState.E:
                    EStage();
                    break;
            }
        }
    }
}
