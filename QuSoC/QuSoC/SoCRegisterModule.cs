using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuSoC
{
    public class SoCRegisterModuleInputs : SoCComponentModuleInputs
    {
    }

    public class SoCRegisterModuleState
    {
        public uint Value { get; set; }
    }

    public class SoCRegisterModule : SoCComponentModule<SoCRegisterModuleInputs, SoCRegisterModuleState>
    {
        bool internalIsActive => Inputs.Common.Address == Inputs.DeviceAddress;

        public override uint ReadValue => State.Value;
        public override bool IsReady => true;
        public override bool IsActive => internalIsActive;
        protected override void OnStage()
        {
            if (Inputs.Common.WE && internalIsActive)
            {
                NextState.Value = Inputs.Common.WriteValue;
            }
        }
    }
}
