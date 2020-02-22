using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class TransmitterModule : RTLSynchronousModule<TransmitterInputs, TransmitterState>
    {
        // public data points
        public bool Bit => State.Data[0];
        public bool IsReady => State.FSM  == TransmitterFSM.Idle;
        public bool IsTransmitting => State.FSM == TransmitterFSM.Transmitting;
        public bool IsTransmissionStarted => State.FSM == TransmitterFSM.Idle && NextState.FSM == TransmitterFSM.Transmitting;
        protected override void OnStage()
        {
            switch(State.FSM)
            {
                case TransmitterFSM.Idle:
                    if (Inputs.Trigger)
                    {
                        NextState.Counter = 0;
                        NextState.Data = Inputs.Data;
                        NextState.FSM = TransmitterFSM.Transmitting;
                    }
                    break;
                case TransmitterFSM.Transmitting:
                    if (State.Counter == 7)
                    {
                        NextState.FSM = TransmitterFSM.WaitingForAck;
                    }
                    else
                    {
                        NextState.Counter = (byte)(State.Counter + 1);
                    }
                    NextState.Data = State.Data >> 1;
                    break;
                case TransmitterFSM.WaitingForAck:
                    if (Inputs.Ack)
                        NextState.FSM = TransmitterFSM.Idle;
                    break;
            }
        }
    }
}
