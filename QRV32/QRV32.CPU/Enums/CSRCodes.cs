namespace QRV32.CPU
{
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
        mscratch    = 0x340, // Machine scratch register
        mepc        = 0x341, // Machine exception program counter
        mcause      = 0x342, // Machine trap cause
        mtval       = 0x343, // Machine bad address or instruction
        mip         = 0x344, // Machine interrupt pending
    }
}
