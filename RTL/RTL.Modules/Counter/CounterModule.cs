using Quokka.RTL;

namespace RTL.Modules
{
    public class CounterInputs
    {
        public bool Enabled { get; set; }
    }

    public class CounterState 
    {
        public byte Value { get; set; }
    }

    public class CounterModule : RTLSynchronousModule<CounterInputs, CounterState>
    {
        public byte Value => State.Value;

        byte NextValue => (byte)(Inputs.Enabled ? State.Value + 1 : State.Value);
        
        protected override void OnStage()
        {
            NextState.Value = NextValue;
        }
    }
}
