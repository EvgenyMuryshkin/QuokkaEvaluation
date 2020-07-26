using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuSoC
{
    public class SoCRegisterModuleInputs
    {
        public bool WE { get; set; }
        public uint WriteValue { get; set; }
    }

    public class SoCRegisterModuleState
    {
        public uint Value { get; set; }
    }

    public class SoCRegisterModule : RTLSynchronousModule<SoCRegisterModuleInputs, SoCRegisterModuleState>
    {
        public uint Value => State.Value;

        protected override void OnStage()
        {
            if (Inputs.WE)
            {
                NextState.Value = Inputs.WriteValue;
            }
        }
    }
}
