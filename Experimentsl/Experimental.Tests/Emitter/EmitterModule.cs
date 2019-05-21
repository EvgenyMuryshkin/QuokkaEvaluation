namespace QuokkaTests.Experimental
{
    public class EmitterModule : SynchronizedRTLModule<EmitterState, EmitterInputs>
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
