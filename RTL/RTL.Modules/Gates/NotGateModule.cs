using Quokka.RTL;

namespace RTL.Modules
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
