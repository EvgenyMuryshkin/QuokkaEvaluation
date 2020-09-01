using Quokka.RTL;

namespace RTL.Modules.Overrides
{
    public class OverridesBaseInputs
    {
        public bool InOverride;
        public byte InValue;
    }

    public abstract class CombinationalOverridesBase<TInputs> : RTLCombinationalModule<TInputs>, IOverridesOutput
        where TInputs : OverridesBaseInputs, new()
    {
        public virtual byte OutValue => Inputs.InOverride ? IncrementInput() : DecrementInput();

        protected virtual byte InternalOffset => 1;

        protected virtual byte DecrementInput()
        {
            return DecrementValue(Inputs.InValue);
        }

        protected virtual byte IncrementInput()
        {
            return IncrementValue(Inputs.InValue);
        }

        protected virtual byte DecrementValue(byte value)
        {
            return (byte)(value - InternalOffset);
        }

        protected virtual byte IncrementValue(byte value)
        {
            return (byte)(value + InternalOffset);
        }
    }
}

