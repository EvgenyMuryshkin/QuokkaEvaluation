using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class CombinationalL3Override : CombinationalOverridesBase<OverridesBaseInputs>
    {
        protected override byte InternalOffset => Inputs.InOverride ? (byte)4 : base.InternalOffset;
    }
}
