using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class EmitterState
    {
        public EmitterFSM FSM = EmitterFSM.Emitting;
        public byte Data = byte.MinValue;
    }
}
