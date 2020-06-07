using QRV32.Tests;
using System.IO;

namespace QuSoC.Tests
{
    public class QuSoCInstuctionsProvider : InstructionsProvider
    {
        public override string AsmFilesLocation => Path.Combine(base.AsmFilesLocation, "qusoc");
    }

    public class QuSoCModuleBaseTest
    {
        protected QuSoCInstuctionsProvider Inst = new QuSoCInstuctionsProvider();

        protected QuSoCModuleSimulator PowerUp(string source)
        {
            var instructions = Inst.FromAsmFile(source);

            var sim = new QuSoCModuleSimulator(instructions);

            // first cycle handles CPU reset state
            sim.ClockCycle();

            return sim;
        }
    }
}
