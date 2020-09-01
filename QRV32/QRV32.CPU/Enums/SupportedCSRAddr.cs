namespace QRV32.CPU
{
    public enum SupportedCSRAddr : byte
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
        mscratch,
        mepc,
        mcause,
        mtval,
        mip,
    }
}
