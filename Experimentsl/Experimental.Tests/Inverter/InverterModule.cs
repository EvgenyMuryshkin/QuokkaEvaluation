using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class InverterModule : RTLCombinationalModule<InverterInputs>
    {
        public bool Output => !Inputs.Input;
    }
}
