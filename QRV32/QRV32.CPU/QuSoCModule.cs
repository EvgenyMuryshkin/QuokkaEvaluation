using Quokka.RTL;
using System;

namespace QRV32.CPU
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
        
        public bool Blink { get; set; }
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
        public bool Blink => State.Blink;

        public void SetInstructions(uint[] instructions)
        {
            instructions.CopyTo(State.BlockRAM, 0);

            Setup();
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
        RTLBitArray wordAddress => internalMemAddress[31, 2];
        RTLBitArray byteAddress => new RTLBitArray(internalMemAddress[1, 0]) << 3;

        RTLBitArray internalMemReadData => State.MemReadData >> byteAddress;
        bool internalMemReady => State.MemReady;

        RTLBitArray mask =>
            CPU.MemWriteMode == 0
            ? (new RTLBitArray(byte.MaxValue) << byteAddress).Resized(32)
            : CPU.MemWriteMode == 1
                ? (new RTLBitArray(ushort.MaxValue) << byteAddress).Resized(32)
                : new RTLBitArray(uint.MaxValue);

        RTLBitArray blockRAMWriteData =>
            (State.MemReadData & !mask) | (CPU.MemWriteData & mask);

        RTLBitArray internalMemReadAddress => (CPU.MemRead || CPU.MemWrite)
            ? wordAddress
            : new RTLBitArray(0U);

        protected override void OnStage()
        {
            if (State.BlockRAMWE)
            {
                NextState.BlockRAM[wordAddress] = blockRAMWriteData;
            }

            NextState.MemReadData = State.BlockRAM[internalMemReadAddress];
            NextState.MemReady = CPU.MemRead;

            // TODO: 32768U
            // TODO: State.BlockRAM.Length

            NextState.BlockRAMWE = false;
            var blockRAMWrite = !State.BlockRAMWE && CPU.MemWrite && CPU.MemAddress < 32768;
            if (blockRAMWrite)
            {
                // write back to block ram on next cycle
                NextState.BlockRAMWE = true;
                NextState.MemReady = true;
            }
        }
    }
}
