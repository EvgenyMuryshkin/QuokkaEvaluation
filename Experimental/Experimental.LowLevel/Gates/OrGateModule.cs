using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class OrGateModule : RTLCombinationalModule<GateInputs>
    {
        public bool O => Inputs.I1 || Inputs.I2;
    }
}
