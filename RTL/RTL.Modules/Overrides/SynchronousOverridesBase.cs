using Quokka.RTL;

namespace RTL.Modules.Overrides
{
    public class SynchronousOverridesBaseState
    {
        public byte Value;
    }
    public abstract class SynchronousOverridesBase<TInputs, TState> : RTLSynchronousModule<TInputs, TState>, IOverridesOutput
        where TInputs : OverridesBaseInputs, new()
        where TState : SynchronousOverridesBaseState, new()
    {
        public virtual byte OutValue => State.Value;

        protected override void OnStage()
        {
            if (Inputs.InOverride)
                NextState.Value = (byte)(State.Value - 1);
            else
                NextState.Value = (byte)(State.Value + 1);
        }
    }
}
