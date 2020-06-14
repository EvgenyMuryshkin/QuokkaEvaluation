using QRV32.CPU;
using Quokka.RTL;
using System;

namespace QuSoC
{
    public class QuSoCModuleInputs
    {
    }

    public class QuSoCModuleState
    {
        public uint[] BlockRAM = new uint[1024];
        public bool BlockRAMWE;

        public RTLBitArray MemReadData = 0U;
        public bool MemReady;

        public RTLBitArray Counter = new RTLBitArray(byte.MinValue);
        public byte[] UART = new byte[4];
        public bool UART_TX;
    }

    /*
    public class QuSoCBlinkerModule : QuSoCModule
    {
        public QuSoCBlinkerModule()
        {
        }

        protected override void OnSchedule(Func<QuSoCModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);
        }
    }
    */

    public class QuSoCModule : RTLSynchronousModule<QuSoCModuleInputs, QuSoCModuleState>
    {
        internal RISCVModule CPU = new RISCVModule();
        public byte Counter => (byte)State.Counter;
        public RTLBitArray CPUAddress => CPU.MemAddress;
        public bool CPUMemRead => CPU.MemRead;
        public bool CPUMemWrite => CPU.MemWrite;
        public RTLBitArray CPUMemReadData => internalMemReadData;
        public bool SOCMemReady => State.MemReady;
        public bool CPUHalted => CPU.IsHalted;
        public bool BlockRAMWE => State.BlockRAMWE;
        public byte DbgState => CPU.DbgState;
        public RTLBitArray DbgWBData => CPU.DbgWBData;
        public bool DbgWDDataReady => CPU.DbgWDDataReady;

        public QuSoCModule(uint[] instructions)
        {
            instructions.CopyTo(State.BlockRAM, 0);
        }

        protected override void OnSchedule(Func<QuSoCModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            CPU.Schedule(() => new RISCVModuleInputs()
            {
                BaseAddress = 0U,
                MemReadData = internalMemReadData,
                MemReady = internalMemReady
            });
        }

        RTLBitArray internalMemAddress => new RTLBitArray(CPU.MemAddress);
        RTLBitArray wordAddress => internalMemAddress >> 2;
        RTLBitArray byteAddress => new RTLBitArray(internalMemAddress[1, 0]) << 3;

        RTLBitArray uartReadData => new RTLBitArray(State.UART[uartAddress]).Resized(32);

        RTLBitArray internalMemReadData =>
            memSegment == 2 
            ? uartReadData
            :
            memSegment == 1
            ? State.Counter.Resized(32)
            : State.MemReadData >> byteAddress;

        bool internalMemReady => State.MemReady;

        RTLBitArray mask =>
            CPU.MemWriteMode == 0
            ? (new RTLBitArray(byte.MaxValue) << byteAddress).Resized(32)
            : CPU.MemWriteMode == 1
                ? (new RTLBitArray(ushort.MaxValue) << byteAddress).Resized(32)
                : new RTLBitArray(uint.MaxValue);

        RTLBitArray blockRAMWriteData =>
            (State.MemReadData & !mask) | (CPU.MemWriteData & mask);

        RTLBitArray memSegment => wordAddress[31, 10];
        RTLBitArray blockRamAddress => wordAddress[9, 0];

        RTLBitArray uartAddress => internalMemAddress[1, 0];

        public byte UARTWriteData => State.UART[0];

        protected override void OnStage()
        {
            if (State.BlockRAMWE)
            {
                NextState.BlockRAM[blockRamAddress] = blockRAMWriteData;
            }

            NextState.MemReadData = State.BlockRAM[blockRamAddress];
            NextState.MemReady = CPU.MemRead;

            // TODO: 32768U
            // TODO: State.BlockRAM.Length

            NextState.BlockRAMWE = false;
            NextState.UART_TX = false;

            if (CPU.MemWrite)
            {
                switch ((byte)memSegment)
                {
                    case 0:
                        if (!State.BlockRAMWE)
                        {
                            // write back to block ram on next cycle
                            NextState.BlockRAMWE = true;
                            NextState.MemReady = true;
                        }
                        break;
                    case 1:
                        NextState.Counter = CPU.MemWriteData[7, 0];
                        NextState.MemReady = true;
                        break;
                    case 2:
                        if (State.UART[2] != 0)
                        {
                            // TODO: implicit cast is not handled in rtl transform
                            NextState.UART[0] = (byte)CPU.MemWriteData;
                            NextState.UART[2] = 0;
                            NextState.UART_TX = true;
                            NextState.MemReady = true;
                        }
                        break;
                }
            }
        }
    }
}
