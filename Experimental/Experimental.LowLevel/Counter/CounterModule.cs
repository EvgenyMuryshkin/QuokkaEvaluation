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
        protected override void OnStage()
        {
            NextState.Value = (byte)(State.Value + 1);
        }
    }
}