using Quokka.RTL;

namespace RTL.Modules.Overrides
{
    public class OverrideInputsComposition : OverridesComposition
    {
        RTLBitArray InvertedInput => !(new RTLBitArray(Inputs.InValue));

        public OverridesCompositionInputs RawInputs => Inputs;
        public OverridesCompositionInputs OverrideInputs => ModulesInputs;

        protected override OverridesCompositionInputs ModulesInputs
            => new OverridesCompositionInputs() {
                InOverride = !Inputs.InOverride,
                InValue = InvertedInput
            };
    }
}
