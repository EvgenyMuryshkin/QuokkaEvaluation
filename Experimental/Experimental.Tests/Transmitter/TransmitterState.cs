using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class TransmitterState
    {
        public TransmitterFSM FSM = TransmitterFSM.Idle;
        public RTLBitArray Data = byte.MinValue;
        public byte Counter;
    }
}
