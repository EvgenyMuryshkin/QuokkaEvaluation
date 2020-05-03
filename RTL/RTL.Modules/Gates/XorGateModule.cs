using Quokka.RTL;

namespace RTL.Modules
{

    public class XorGateModule : RTLCombinationalModule<GateInputs>
    {
        public bool O => Inputs.I1 ^ Inputs.I2;
    }
}
