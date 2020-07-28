using Quokka.RTL;

namespace RTL.Modules
{
    public class AndGateModule : RTLCombinationalModule<GateInputs>, ILogicGate
    {
        public bool O => Inputs.I1 && Inputs.I2;
    }
}
