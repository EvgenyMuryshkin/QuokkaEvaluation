using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules.Overrides
{
    public class OverridesCompositionInputs : OverridesBaseInputs
    {
    }

    public class OverridesComposition : RTLCombinationalModule<OverridesCompositionInputs>
    {
        CombinationalNoOverride NoOverride = new CombinationalNoOverride();
        CombinationalAutoValueOverride AutoOverride = new CombinationalAutoValueOverride();
        CombinationalGetValueOverride GetOverride = new CombinationalGetValueOverride();
        CombinationalL1Override L1Override = new CombinationalL1Override();
        CombinationalL2Override L2Override = new CombinationalL2Override();
        CombinationalL3Override L3Override = new CombinationalL3Override();

        public byte NoOverrideValue => NoOverride.OutValue;
        public byte AutoOverrideValue => AutoOverride.OutValue;
        public byte L1Value => L1Override.OutValue;
        public byte L2Value => L2Override.OutValue;
        public byte L3Value => L3Override.OutValue;
        public byte GetValue => GetOverride.OutValue;

        protected override void OnSchedule(Func<OverridesCompositionInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            NoOverride.Schedule(() => Inputs);
            AutoOverride.Schedule(() => Inputs);
            L1Override.Schedule(() => Inputs);
            L2Override.Schedule(() => Inputs);
            L3Override.Schedule(() => Inputs);
            GetOverride.Schedule(() => Inputs);
        }
    }
}
