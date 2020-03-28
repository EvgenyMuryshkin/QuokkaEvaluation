using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class CounterInputs
    {
        public bool InReset { get; set; }
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
            NextState.Value = Inputs.InReset ? byte.MinValue : (byte)(State.Value + 1);
        }
    }
}
