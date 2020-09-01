using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class CombinationalL2Override : CombinationalOverridesBase<OverridesBaseInputs>
    {
        protected override byte DecrementValue(byte value)
        {
            return (byte)(value - 3);
        }

        protected override byte IncrementValue(byte value)
        {
            return (byte)(value + 3);
        }
    }
}
