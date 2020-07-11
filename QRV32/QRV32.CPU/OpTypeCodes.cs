namespace QRV32.CPU
{
    public enum OpTypeCodes : byte
    {
        OPIMM = 0x13,
        OP = 0x33,
        LUI = 0x37,
        AUIPC = 0x17,
        JAL = 0x6F,
        JALR = 0x67,
        B = 0x63,
        LOAD = 0x03,
        STORE = 0x23,
        SYSTEM = 0x73
    }

    public enum SysTypeCodes : byte
    {
        CALL = 0,
        BREAK = 1,
        TRAP = 2,
        IRQ = 5
    }

    public enum RetTypeCodes : byte
    {
        U = 0,
        S = 0x8,
        M = 0x18
    }

    public enum IRQTypeCodes : byte
    {
        WFI = 0x8
    }

    public enum SystemCodes : byte
    {
        E = 0,
        CSRRW = 1,
        CSRRS = 2,
        CSRRC = 3,
        CSRRWI = 5,
        CSRRSI = 6,
        CSRRCI = 7
    }

    public enum OPIMMCodes : byte
    {
        ADDI = 0,
        SLTI = 2,
        SLTIU = 3,
        XORI = 4,
        ORI = 6,
        ANDI = 7,
        SLLI = 1,
        SRLI_SRAI = 5        
    }

    public enum OPCodes : byte
    {
        ADD_SUB = 0,
        SLL = 1,
        SLT = 2,
        SLTU = 3,
        XOR = 4,
        SRL_SRA = 5,
        OR = 6,
        AND = 7
    }

    public enum BranchTypeCodes : byte
    {
        EQ = 0,
        NE = 1,
        LT = 4,
        GE = 5,
        LTU = 6,
        GEU = 7,
    }

    public enum LoadTypeCodes : byte
    {
        LB = 0,
        LH = 1,
        LW = 2,
        LBU = 4,
        LHU = 5
    }

    public enum StoreTypeCodes : byte
    {
        SB = 0,
        SH = 1,
        SW = 2,
    }

    public enum CSRCodes : ushort
    {
        // Machine Information Registers
        mvendorid   = 0xF11, // Vendor ID
        marchid     = 0xF12, // Architecture ID
        mimpid      = 0xF13, // Implementation ID
        mhartid     = 0xF14, // Hardware thread ID
        // Machine Trap Setup
        mstatus     = 0x300, // Machine status register
        misa        = 0x301, // ISA and extensions
        mie         = 0x304, // Machine interrupt-enable register
        mtvec       = 0x305, // Machine trap-handler base address
        // Machine Trap Handling
        mepc        = 0x341, // Machine exception program counter
        mcause      = 0x342, // Machine trap cause
        mip         = 0x344, // Machine interrupt pending
    }

    public enum CSRAddr : byte
    {
        // Machine Information Registers
        mvendorid,
        marchid,
        mimpid,
        mhartid,
        // Machine Trap Setup
        mstatus,
        misa,
        mie,
        mtvec,
        // Machine Trap Handling
        mepc,
        mcause,
        mip,
    }
}
