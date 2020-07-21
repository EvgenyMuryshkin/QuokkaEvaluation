using System.IO;

namespace QuSoC.Tests
{
    public class QuSoCModuleBaseTest
    {
        protected InstructionsProvider Inst = new InstructionsProvider();

        protected QuSoCModuleSimulator PowerUp(string source)
        {
            var instructions = Inst.FromAsmFile(source);
            return PowerUp(instructions);
        }

        protected QuSoCModuleSimulator PowerUp(uint[] instructions)
        {
            var sim = new QuSoCModuleSimulator(instructions);

            // first cycle handles CPU reset state
            sim.ClockCycle();

            return sim;
        }
    }
}
