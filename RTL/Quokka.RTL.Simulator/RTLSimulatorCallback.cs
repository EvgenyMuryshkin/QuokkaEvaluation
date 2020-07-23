namespace Quokka.RTL.Simulator
{
    public class RTLSimulatorCallback<TModule>
    {
        public TModule TopLevel;
        public int Clock;
        public int StageIteration;
    }
}
