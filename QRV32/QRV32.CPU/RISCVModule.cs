using Autofac.Core.Lifetime;
using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

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
        E,
        Halt
    }

    public class RISCVModuleInputs
    {
        public RTLBitArray BaseAddress = new RTLBitArray(uint.MinValue);
        public RTLBitArray MemReadData = new RTLBitArray(uint.MinValue);
        public bool MemReady;
        public bool IRQ;
    }

    public class CPUModuleState
    {
        public CPUState State;
        public uint Instruction;

        public bool WBDataReady;
        public uint WBData;

        public RTLBitArray PC = new RTLBitArray(uint.MinValue);

        public RTLBitArray PCOffset = new RTLBitArray(uint.MinValue);

        public uint[] CSR = CSRInit();

        static uint[] CSRInit()
        {
            return new uint[]
            {
                // Machine Information Registers
                0U,         // mvendorid:   0 - open-source
                0xFA57DB9,  // marchid:     quokka signature code
                0x01010101U,// mimpid:      1.1.1.1
                0U,         // mhartid:     0, all code runs in default hart
                // Machine Trap Setup
                0U,         // mstatus:     0, TLDR, will sort out later
                0x40000100U,// misa:        MXL: 32 bit, ISA: I
                0U,         // mie          No interrupts enabled at startup
                0U,         // mtvec
                // Machine Trap Handling
                0U,         // mscratch
                0U,         // mepc
                0U,         // mcause
                0U,         // mip
            };
        }
    }

    public class RISCVModule : RTLSynchronousModule<RISCVModuleInputs, CPUModuleState>
    {
        internal InstructionDecoderModule ID = new InstructionDecoderModule();
        internal IRegistersModule Regs = null;
        internal ALUModule ALU = new ALUModule();
        internal CompareModule CMP = new CompareModule();

        public byte DbgState => (byte)State.State;
        public RTLBitArray DbgWBData => ID.UTypeImm;
        public bool DbgWDDataReady => RegsWE;

        internal bool IsLoadOp => ID.OpTypeCode == OpTypeCodes.LOAD;
        internal bool IsStoreOp => ID.OpTypeCode == OpTypeCodes.STORE;

        public bool MemRead => State.State == CPUState.IF || (State.State == CPUState.MEM && IsLoadOp);
        public bool MemWrite => State.State == CPUState.MEM && IsStoreOp;
        public uint MemAddress
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

        void Halt()
        {
            NextState.State = CPUState.Halt;
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

        void OnSystem()
        {
            if (ID.SystemCode == SystemCodes.E)
            {
                switch(ID.SysTypeCode)
                {
                    case SysTypeCodes.CALL:
                        NextState.State = CPUState.E;
                        break;
                    case SysTypeCodes.BREAK:
                        NextState.State = CPUState.E;
                        break;
                    case SysTypeCodes.TRAP:
                        switch(ID.RetTypeCode)
                        {
                            case RetTypeCodes.M:
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

        void ExecuteStage()
        {
            NextState.State = CPUState.WB;
            NextState.WBDataReady = false;
            NextState.PCOffset = InstructionOffset;

            switch (ID.OpTypeCode)
            {
                case OpTypeCodes.OPIMM:
                    OnOPIMM();
                    break;
                case OpTypeCodes.OP:
                    OnOP();
                    break;
                case OpTypeCodes.B:
                    OnBranch();
                    break;
                case OpTypeCodes.LUI:
                    NextState.WBDataReady = true;
                    NextState.WBData = ID.UTypeImm;
                    break;
                case OpTypeCodes.AUIPC:
                    NextState.WBDataReady = true;
                    NextState.WBData = State.PC + ID.UTypeImm;
                    break;
                case OpTypeCodes.JAL:
                    NextState.WBDataReady = true;
                    NextState.WBData = NextSequentialPC;
                    NextState.PCOffset = ID.JTypeImm;
                    break;
                case OpTypeCodes.JALR:
                    NextState.WBDataReady = true;
                    NextState.WBData = NextSequentialPC;
                    NextState.PCOffset = new RTLBitArray(new RTLBitArray(Regs.RS1 + ID.ITypeImm)[31, 1], false);
                    break;
                case OpTypeCodes.LOAD:
                    NextState.State = CPUState.MEM;
                    break;
                case OpTypeCodes.STORE:
                    NextState.State = CPUState.MEM;
                    break;
                case OpTypeCodes.SYSTEM:
                    OnSystem();
                    break;
                default:
                    Halt();
                    break;
            }
        }

        RTLBitArray LWData => Inputs.MemReadData;
        RTLBitArray LHData => Inputs.MemReadData[15, 0].Signed().Resized(32);
        RTLBitArray LHUData => Inputs.MemReadData[15, 0].Unsigned().Resized(32);
        RTLBitArray LBData => Inputs.MemReadData[7, 0].Signed().Resized(32);
        RTLBitArray LBUData => Inputs.MemReadData[7, 0].Unsigned().Resized(32);

        // TODO: inlined RTLBitArray operations
        void MemStage()
        {
            if (Inputs.MemReady)
            {
                NextState.State = CPUState.WB;
                if (IsLoadOp)
                {
                    NextState.WBDataReady = true;

                    switch (ID.LoadTypeCode)
                    {
                        case LoadTypeCodes.LW:
                            NextState.WBData = LWData;
                            break;
                        case LoadTypeCodes.LH:
                            NextState.WBData = LHData;
                            break;
                        case LoadTypeCodes.LHU:
                            NextState.WBData = LHUData;
                            break;
                        case LoadTypeCodes.LB:
                            NextState.WBData = LBData;
                            break;
                        case LoadTypeCodes.LBU:
                            NextState.WBData = LBUData;
                            break;
                    }
                }
            }
        }

        RTLBitArray InstructionOffset => new RTLBitArray(4).Unsigned();
        RTLBitArray NextSequentialPC => State.PC + InstructionOffset;
        RTLBitArray internalNextPC => ID.OpTypeCode == OpTypeCodes.JALR ? State.PCOffset : State.PC + State.PCOffset;
        public bool PCMisaligned => new RTLBitArray(internalNextPC[1, 0]) != 0;

        RTLBitArray MSTATUS => State.CSR[(byte)CSRAddr.mstatus];
        bool MIE => MSTATUS[3];

        void DisableInterrupts()
        {
            NextState.CSR[(byte)CSRAddr.mstatus] = State.CSR[(byte)CSRAddr.mstatus] & 0xFFFFFFF7;
        }

        void EnableInterrupts()
        {
            NextState.CSR[(byte)CSRAddr.mstatus] = State.CSR[(byte)CSRAddr.mstatus] | 0x8;
        }

        void WriteBackStage()
        {
            if (PCMisaligned)
            {
                Halt();
            }
            else
            {
                NextState.State = CPUState.IF;

                if (MIE && Inputs.IRQ)
                {
                    NextState.CSR[(byte)CSRAddr.mepc] = internalNextPC;
                    DisableInterrupts();
                    NextState.PC = State.CSR[(byte)CSRAddr.mtvec];
                }
                else
                {
                    NextState.PC = internalNextPC;
                }
            }
        }

        void InstructionFetchStage()
        {
            if (Inputs.MemReady)
            {
                NextState.State = CPUState.ID;
                NextState.Instruction = Inputs.MemReadData;
            }
        }

        void InstructionDecodeStage()
        {
            if (Regs.Ready)
            {
                NextState.State = CPUState.EX;
            }
        }

        void EStage()
        {
            NextState.State = CPUState.WB;
        }

        protected override void OnStage()
        {
            switch (State.State)
            {
                case CPUState.Reset:
                    NextState.State = CPUState.IF;
                    NextState.PC = Inputs.BaseAddress;
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

        public override string ToString()
        {
            var dump = new StringBuilder();

            dump.AppendLine($"RISC-V Dump:");
            dump.AppendLine($"PC: 0x{State.PC}");
            dump.AppendLine($"State: {State.State}");
            dump.AppendLine($"Instruction: 0x{State.Instruction}");
            dump.AppendLine($"OpCode: {ID.OpTypeCode}");
            dump.AppendLine($"=== REGS ===");
            Regs
                .State
                .x
                .Select((r, idx) => $"X[{idx.ToString("D2")}]: 0x{(r.ToString("X8"))}")
                .ForEach(l => dump.AppendLine(l));

            return dump.ToString();
        }
    }
}
