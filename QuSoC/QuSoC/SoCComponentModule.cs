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
    }
}
