using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class AndGateModule : RTLCombinationalModule<GateInputs>
    {
        public bool O => Inputs.I1 && Inputs.I2;
    }
}
