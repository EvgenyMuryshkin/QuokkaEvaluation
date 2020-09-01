using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class CombinationalGetValueOverride : CombinationalOverridesBase<OverridesBaseInputs>
    {
        public override byte OutValue
        {
            get
            {
                byte result = base.OutValue;

                if (Inputs.InOverride)
                    result = (byte)(Inputs.InValue + 3);

                return result;
            }
        }
    }
}
