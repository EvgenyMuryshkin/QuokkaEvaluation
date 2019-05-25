using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using Quokka.VCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    class Empty
    {

    }

    class InverterFeedback : RTLCombinationalModule<Empty>
    {
        InverterModule Inverter = new InverterModule();

        public override void Schedule(Func<Empty> inputsFactory)
        {
            base.Schedule(inputsFactory);

            Inverter.Schedule(() => new InverterInputs() { Input = Inverter.Output });
        }
    }

    [TestClass]
    public class InverterTests
    {
        [TestMethod]
        public void InverterFeedback()
        {
            var sim = new RTLSynchronousSimulator<InverterFeedback>();
            sim.Trace(PathTools.VCDOutputPath());

            Assert.ThrowsException<MaxStageIterationReachedException>(() =>
            {
                sim.Run();
            });
        }
    }
}
