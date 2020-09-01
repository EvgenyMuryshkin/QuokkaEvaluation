using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class CombinationalAutoValueOverride : CombinationalOverridesBase<OverridesBaseInputs>
    {
        public override byte OutValue => (byte)(Inputs.InOverride ? Inputs.InValue + 2 : base.OutValue);
    }
}
