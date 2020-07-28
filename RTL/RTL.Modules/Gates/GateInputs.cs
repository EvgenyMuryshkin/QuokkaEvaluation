using Quokka.RTL;

namespace RTL.Modules
{
    public class GateInputs
    {
        public bool I1 = false;
        public bool I2 = false;
    }

    public interface ILogicGate : IRTLCombinationalModule<GateInputs>
    {
        public bool O { get; }
    }
}
