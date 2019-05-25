using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class ReceiverState
    {
        public ReceiverFSM FSM = ReceiverFSM.Idle;
        public byte Data = byte.MinValue;
    }
}
