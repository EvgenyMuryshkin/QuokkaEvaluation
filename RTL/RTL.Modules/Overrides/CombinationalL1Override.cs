using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class CombinationalL1Override : CombinationalOverridesBase<OverridesBaseInputs>
    {
        protected override byte DecrementInput()
        {
            return (byte)(Inputs.InValue - 2);
        }

        protected override byte IncrementInput()
        {
            return (byte)(Inputs.InValue + 2);
        }
    }
}
