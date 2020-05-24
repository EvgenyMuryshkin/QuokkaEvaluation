namespace QRV32.Tests
{
    public class CPUModuleBaseTest
    {
        protected InstructionsProvider Inst = new InstructionsProvider();

        protected CPUSimulator PowerUp()
        {
            var sim = new CPUSimulator();
            // first cycle handles CPU reset state
            sim.ClockCycle();
            return sim;
        }
    }
}
