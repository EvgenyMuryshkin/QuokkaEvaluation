namespace QRV32.CPU
{
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
        mtval,
        // Machine Trap Handling
        mepc,
        mcause,
        mip,
    }
}
