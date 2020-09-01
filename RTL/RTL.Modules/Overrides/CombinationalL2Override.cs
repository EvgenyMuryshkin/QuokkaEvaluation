using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class CombinationalL2Override : CombinationalOverridesBase<OverridesBaseInputs>
    {
        protected override byte DecrementValue(byte value)
        {
            var result = base.DecrementValue(value);
            if (value < 0xF0)
                result = (byte)(value - 3);

            return result;
        }

        protected override byte IncrementValue(byte value)
        {
            var result = (byte)(value + 3);
            if (value > 0xF0)
                result = base.IncrementValue(value);

            return result;
        }
    }
}
