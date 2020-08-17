namespace QRV32.CPU
{
    public enum SystemCodes : byte
    {
        E = 0,
        CSRRW = 1,
        CSRRS = 2,
        CSRRC = 3,
        Unsupported = 4,
        CSRRWI = 5,
        CSRRSI = 6,
        CSRRCI = 7
    }
}
