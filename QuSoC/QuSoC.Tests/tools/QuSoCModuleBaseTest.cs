﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        protected QuSoCModuleSimulator PowerUp<T>()
            where T : QuSoCModule, new()
        {
            var module = new T();

            var sim = new QuSoCModuleSimulator(module);

            // first cycle handles CPU reset state
            sim.ClockCycle();

            return sim;
        }
    }
}
