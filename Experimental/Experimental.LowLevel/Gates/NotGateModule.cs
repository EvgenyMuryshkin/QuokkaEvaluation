using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    // Example module

    /// <summary>
    /// Inputs declarations, names, types and sizes;
    /// </summary>
    public class NotGateInputs
    {
        public bool Input = false;
    }

    public class NotGateModule : RTLCombinationalModule<NotGateInputs>
    {
        public bool Output => !Inputs.Input;
    }
}
