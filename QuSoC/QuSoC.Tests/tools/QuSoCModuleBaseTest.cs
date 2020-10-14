using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.Core.Tools;
using Quokka.Public.Tools;
using Quokka.RISCV.Integration.Client;
using System.IO;
using System.Linq;

namespace QuSoC.Tests
{
    public class QuSoCModuleBaseTest
    {
        protected AsmInstructionsProvider Inst = new AsmInstructionsProvider();

        protected string AppPath(string app)
            => Path.Combine(
                Inst.SolutionLocation,
                "QuSoC",
                "QuSoC",
                "apps", app);

        protected QuSoCModuleSimulator FromApp(string appName)
        {
            var firmwareTools = new FirmwareTools(AppPath(appName));
            Assert.IsTrue(firmwareTools.FirmwareFromAppFolder());

            var instructions = RISCVIntegrationClient
                .ToInstructions(File.ReadAllBytes(firmwareTools.FirmwareFile))
                .ToArray();
            var sim = PowerUp(instructions);
            return sim;
        }

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

        protected QuSoCModuleSimulator<T> PowerUp<T>()
            where T : QuSoCModule, new()
        {
            using (new CurrentDirectory(Path.Combine(PathTools.SolutionPath, "QuSoC", "QuSoC")))
            {
                var module = new T();

                var sim = new QuSoCModuleSimulator<T>(module);

                // first cycle handles CPU reset state
                sim.ClockCycle();

                return sim;
            }
        }
    }
}
