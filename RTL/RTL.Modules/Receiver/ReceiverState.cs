using Quokka.RTL;

namespace RTL.Modules
{
    public class ReceiverState
    {
        public ReceiverFSM FSM = ReceiverFSM.Idle;
        public byte Data = byte.MinValue;
    }
}
