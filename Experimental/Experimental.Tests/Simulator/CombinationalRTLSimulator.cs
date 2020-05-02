namespace Quokka.RTL
{
    public class CombinationalRTLSimulator<TModule> : RTLSimulator<TModule>
        where TModule : IRTLCombinationalModule, new()
    {
        public CombinationalRTLSimulator()
        {
            IsRunning = (cb) => cb.Clock == 0;

            TopLevel.Scheduled += (s, a) =>
            {
                Run();
            };
        }
    }
}
