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
}
