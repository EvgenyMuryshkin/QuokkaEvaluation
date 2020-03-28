using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public enum EmitterFSM
    {
        Emitting,
        WaitingForAck,
    }

    public class EmitterInputs
    {
        public bool IsEnabled = false;
        public bool Ack = false;
    }

    public class EmitterState
    {
        public EmitterFSM FSM = EmitterFSM.Emitting;
        public byte Data = byte.MinValue;
    }

    public class EmitterModule : RTLSynchronousModule<EmitterInputs, EmitterState>
    {
        public byte Data => State.Data;
        public bool HasData => State.FSM == EmitterFSM.Emitting;
        
        protected override void OnStage()
        {
            switch(State.FSM)
            {
                case EmitterFSM.Emitting:
                    if (Inputs.IsEnabled)
                        NextState.FSM = EmitterFSM.WaitingForAck;
                    break;
                case EmitterFSM.WaitingForAck:
                    if (Inputs.Ack)
                    {
                        NextState.FSM = EmitterFSM.Emitting;
                        NextState.Data = (byte)(State.Data + 1);
                    }
                    break;
            }
        }
    }
}
