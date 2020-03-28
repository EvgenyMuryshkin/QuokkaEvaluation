namespace Quokka.RTL
{
    public class RTLSimulatorContext
    {
        public int MaxClockCycles { get; set; } = 100000;
        public int MaxStageIterations { get; set; } = 1000;

        public int CurrentTime { get; set; }

        public int Clock { get; set; }
        public int Iteration { get; set; }
    }
}
