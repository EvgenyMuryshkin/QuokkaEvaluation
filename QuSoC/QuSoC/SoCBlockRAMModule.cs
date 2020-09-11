using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuSoC
{
    public class SoCBlockRAMModuleInputs : SoCComponentModuleInputs
    {
        public RTLBitArray MemAccessMode = new RTLBitArray().Resized(2);
    }

    public class SoCBlockRAMModuleState
    {
        public uint[] BlockRAM = new uint[1024];
        public uint ReadValue;
        public bool Ready;
        public bool ReadBeforeWrite;
    }

    public class SoCBlockRAMModule : SoCComponentModule<SoCBlockRAMModuleInputs, SoCBlockRAMModuleState>
    {
        private readonly uint addressSpan;
        public SoCBlockRAMModule(uint size)
        {
            addressSpan = size * 4;
            State.BlockRAM = new uint[size];
        }

        RTLBitArray internalAddress => new RTLBitArray(Inputs.Common.Address);
        RTLBitArray internalWordAddress => internalAddress[11, 2];
        RTLBitArray internalByteAddress => internalAddress[1, 0] << 3;
        bool internalIsActive => (Inputs.Common.RE || Inputs.Common.WE) && Inputs.Common.Address >= Inputs.DeviceAddress && Inputs.Common.Address < (Inputs.DeviceAddress + addressSpan);

        public override uint ReadValue => (State.ReadValue >> internalByteAddress) & memAccessMask;
        public override bool IsReady => State.Ready;
        public override bool IsActive => internalIsActive;

        RTLBitArray internalWriteValueBits => Inputs.Common.WriteValue;
        RTLBitArray writeMask => (memAccessMask << internalByteAddress).Resized(32);
        RTLBitArray internalWriteData =>
            Inputs.MemAccessMode == 2
            ? internalWriteValueBits
            : (State.ReadValue & !writeMask) | ((internalWriteValueBits << internalByteAddress) & writeMask);

        bool readBeforeWrite => Inputs.MemAccessMode != 2;

        bool internalWE => !readBeforeWrite || State.ReadBeforeWrite;

        RTLBitArray memAccessMask
        {
            get
            {
                var mask = new RTLBitArray(uint.MaxValue);

                if (Inputs.MemAccessMode == 0)
                    mask = new RTLBitArray(byte.MaxValue).Resized(32);
                else if (Inputs.MemAccessMode == 1)
                    mask = new RTLBitArray(ushort.MaxValue).Resized(32);

                return mask;
            }
        }

        protected override void OnStage()
        {
            NextState.Ready = internalIsActive && (internalWE || Inputs.Common.RE);
            NextState.ReadBeforeWrite = readBeforeWrite && !State.ReadBeforeWrite;

            if (Inputs.Common.WE && internalIsActive && internalWE)
            {
                NextState.BlockRAM[internalWordAddress] = internalWriteData;
            }

            NextState.ReadValue = State.BlockRAM[internalWordAddress];
        }
    }
}
