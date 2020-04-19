using Quokka.RTL;

namespace QuokkaTests.Experimental
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

        protected override void OnStage()
        {
            NextState.Value = (byte)(Inputs.Enabled ? State.Value + 1 : State.Value);
        }
    }
}
