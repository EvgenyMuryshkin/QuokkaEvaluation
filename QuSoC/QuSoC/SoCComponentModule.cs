using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuSoC
{
    public struct SoCComponentModuleCommon
    {
        public uint Address;
        public uint WriteValue;
        public bool WE;
        public bool RE;
    }

    public class SoCComponentModuleInputs
    {
        public SoCComponentModuleCommon Common;
        public uint DeviceAddress;
    }

    public abstract class SoCComponentModule<TInputs, TState> : RTLSynchronousModule<TInputs, TState>
        where TInputs : SoCComponentModuleInputs, new()
        where TState : new()
    {
        public abstract bool IsActive { get; }
        public abstract bool IsReady { get; }
        public abstract uint ReadValue { get; }

        private readonly uint addressSpan;
        public SoCComponentModule(uint addressSpan)
        {
            this.addressSpan = addressSpan;
        }

        protected virtual bool addressMatch => Inputs.Common.Address >= Inputs.DeviceAddress && Inputs.Common.Address < (Inputs.DeviceAddress + addressSpan);
        protected RTLBitArray internalAddressBits => new RTLBitArray(Inputs.Common.Address);
        protected RTLBitArray internalByteAddress => internalAddressBits[1, 0] << 3;
    }
}
