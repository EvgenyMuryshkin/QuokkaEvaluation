using System.IO;

namespace QRV32.Tests
{
    public class CPUInstuctionsProvider : InstructionsProvider
    {
        public override string AsmFilesLocation => Path.Combine(base.AsmFilesLocation, "riscv");
    }

    public class CPUModuleBaseTest
    {
        protected CPUInstuctionsProvider Inst = new CPUInstuctionsProvider();

        protected CPUSimulator PowerUp()
        {
            var sim = new CPUSimulator();
            // first cycle handles CPU reset state
            sim.ClockCycle();
            return sim;
        }
    }
}
