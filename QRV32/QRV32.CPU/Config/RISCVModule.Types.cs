using Quokka.RTL;
using System;

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
        public HaltCode HaltCode;
        public uint Instruction;

        public bool WBDataReady;
        public uint WBData;

        public RTLBitArray PC = new RTLBitArray(uint.MinValue);

        public RTLBitArray PCOffset = new RTLBitArray(uint.MinValue);

        public uint[] CSR = CSRInit();

        static uint[] CSRInit()
        {
            var result = new uint[]
            {
                // Machine Information Registers
                0,         // mvendorid:   0 - open-source
                0xFA57DB9, // marchid:     quokka signature code
                0x01010101,// mimpid:      1.1.1.1
                0,         // mhartid:     0, all code runs in default hart
                // Machine Trap Setup
                0,         // mstatus:     0, TLDR, will sort out later
                0x40000100,// misa:        MXL: 32 bit, ISA: I
                0,         // mie          No interrupts enabled at startup
                0,         // mtvec
                // Machine Trap Handling
                0,         // mscratch
                0,         // mepc
                0,         // mcause
                0,         // mtval
                0,         // mip
            };

            if (Enum.GetValues(typeof(CSRCodes)).Length != result.Length)
            {
                throw new Exception($"CSR init lenght does not match supported codes");
            }

            return result;
        }
    }
}
