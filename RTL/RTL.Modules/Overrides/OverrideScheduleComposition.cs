using Quokka.RTL;
using System;

namespace RTL.Modules.Overrides
{
    public class OverrideScheduleComposition : OverrideInputsComposition
    {
        protected override void OnSchedule(Func<OverridesCompositionInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            NoOverride.Schedule(() => Inputs);
            AutoOverride.Schedule(() => Inputs);
        }
    }
}
