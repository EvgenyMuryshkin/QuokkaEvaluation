using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class GateInputs
    {
        public bool I1 = false;
        public bool I2 = false;
    }
    public class AndGateModule : RTLCombinationalModule<GateInputs>
    {
        public bool O => Inputs.I1 && Inputs.I2;
    }
    
    public class OrGateModule : RTLCombinationalModule<GateInputs>
    {
        public bool O => Inputs.I1 || Inputs.I2;
    }

    public class XorGateModule : RTLCombinationalModule<GateInputs>
    {
        public bool O => Inputs.I1 ^ Inputs.I2;
    }
}
