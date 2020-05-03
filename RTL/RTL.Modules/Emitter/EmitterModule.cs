using Quokka.RTL;

namespace RTL.Modules
{
    public class EmitterInputs
    {
        public bool IsEnabled = false;
        public bool Ack = false;
    }

    public class EmitterState
    {
        public byte Data = byte.MinValue;
    }

    public class EmitterModule : RTLSynchronousModule<EmitterInputs, EmitterState>
    {
        public byte Data => State.Data;
        public bool HasData => Inputs.IsEnabled;
        
        protected override void OnStage()
        {
            if (Inputs.IsEnabled && Inputs.Ack)
                NextState.Data = (byte)(State.Data + 1);
        }
    }
}
