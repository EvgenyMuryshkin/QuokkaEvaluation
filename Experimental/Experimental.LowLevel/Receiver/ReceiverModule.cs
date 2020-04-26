using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class ReceiverModule : RTLSynchronousModule<ReceiverInputs, ReceiverState>
    {
        // public data points
        public bool HasData => State.FSM  == ReceiverFSM.WaitingForAck;
        public byte Data => State.Data;
        byte PartialData => (byte)(Inputs.Bit ? 0x80 : 0);

        protected override void OnStage()
        {
            switch(State.FSM)
            {
                case ReceiverFSM.Idle:
                    if (Inputs.IsValid)
                    {
                        NextState.Data = PartialData;
                        NextState.FSM = ReceiverFSM.Receiving;
                    }
                    break;
                case ReceiverFSM.Receiving:
                    if (Inputs.IsValid)
                    {
                        NextState.Data = (byte)((State.Data >> 1) | PartialData);
                    }
                    else
                    {
                        NextState.FSM = ReceiverFSM.WaitingForAck;
                    }
                    break;
                case ReceiverFSM.WaitingForAck:
                    if (Inputs.Ack)
                    {
                        NextState.FSM = ReceiverFSM.Idle;
                        NextState.Data = 0;
                    }
                    break;
            }
        }
    }
}
