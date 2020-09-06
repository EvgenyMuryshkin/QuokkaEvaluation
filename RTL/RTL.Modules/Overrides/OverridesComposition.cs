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
        protected CombinationalNoOverride NoOverride = new CombinationalNoOverride();
        protected CombinationalAutoValueOverride AutoOverride = new CombinationalAutoValueOverride();
        protected CombinationalGetValueOverride GetOverride = new CombinationalGetValueOverride();
        protected CombinationalL1Override L1Override = new CombinationalL1Override();
        protected CombinationalL2Override L2Override = new CombinationalL2Override();
        protected CombinationalL3Override L3Override = new CombinationalL3Override();

        public byte NoOverrideValue => NoOverride.OutValue;
        public byte AutoOverrideValue => AutoOverride.OutValue;
        public byte L1Value => L1Override.OutValue;
        public byte L2Value => L2Override.OutValue;
        public byte L3Value => L3Override.OutValue;
        public byte GetValue => GetOverride.OutValue;

        protected virtual OverridesCompositionInputs ModulesInputs => Inputs;
        protected override void OnSchedule(Func<OverridesCompositionInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            NoOverride.Schedule(() => ModulesInputs);
            AutoOverride.Schedule(() => ModulesInputs);
            L1Override.Schedule(() => ModulesInputs);
            L2Override.Schedule(() => ModulesInputs);
            L3Override.Schedule(() => ModulesInputs);
            GetOverride.Schedule(() => ModulesInputs);
        }
    }
}
