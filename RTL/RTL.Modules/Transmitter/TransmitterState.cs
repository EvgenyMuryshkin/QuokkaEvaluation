using Quokka.RTL;

namespace RTL.Modules
{
    public class TransmitterState
    {
        public TransmitterFSM FSM = TransmitterFSM.Idle;
        public RTLBitArray Data = byte.MinValue;
        public byte Counter;
    }
}
