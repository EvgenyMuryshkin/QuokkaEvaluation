namespace QRV32.CPU
{
    public enum OpCodes : byte
    {
        OPIMM = 0x13,
        OP = 0x33,
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
}
