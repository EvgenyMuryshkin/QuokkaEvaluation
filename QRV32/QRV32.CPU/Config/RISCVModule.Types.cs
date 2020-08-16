using Quokka.RTL;

namespace QRV32.CPU
{
    public class RISCVModuleInputs
    {
        public RTLBitArray BaseAddress = new RTLBitArray(uint.MinValue);
        public RTLBitArray MemReadData = new RTLBitArray(uint.MinValue);
        public bool MemReady;
        public bool ExtIRQ;
    }

    public class CPUModuleState
    {
        public CPUState State;
        public uint Instruction;

        public bool WBDataReady;
        public uint WBData;

        public RTLBitArray PC = new RTLBitArray(uint.MinValue);

        public RTLBitArray PCOffset = new RTLBitArray(uint.MinValue);

        public MCAUSE pendingMCause;

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
                0U,         // mtval
                // Machine Trap Handling
                0U,         // mscratch
                0U,         // mepc
                0U,         // mcause
                0U,         // mip
            };
        }
    }
}
