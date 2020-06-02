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
        E = 0x73
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
}
