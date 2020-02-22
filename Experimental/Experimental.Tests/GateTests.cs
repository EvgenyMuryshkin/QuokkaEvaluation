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
    public class GateTests
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

        [TestMethod]
        public void InverterTest()
        {
            var sim = new RTLSynchronousSimulator<InverterModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            sim.Trace(PathTools.VCDOutputPath());
            sim.TopLevel.Schedule(() => new InverterInputs() { Input = true }); ;

            Assert.AreEqual(true, sim.TopLevel.Output);
            sim.Run();
            Assert.AreEqual(false, sim.TopLevel.Output);
        }

        [TestMethod]
        public void AndGateTest()
        {
            var sim = new RTLSynchronousSimulator<AndGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = true, I2 = false });
            sim.Run();
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = true, I2 = true });
            sim.Run();
            Assert.AreEqual(true, sim.TopLevel.O);
        }

        [TestMethod]
        public void OrGateTest()
        {
            var sim = new RTLSynchronousSimulator<OrGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = false, I2 = false });
            sim.Run();
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = true, I2 = false });
            sim.Run();
            Assert.AreEqual(true, sim.TopLevel.O);
        }

        [TestMethod]
        public void XorGateTest()
        {
            var sim = new RTLSynchronousSimulator<XorGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = false, I2 = false });
            sim.Run();
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = true, I2 = false });
            sim.Run();
            Assert.AreEqual(true, sim.TopLevel.O);

            sim.TopLevel.Schedule(() => new GateInputs() { I1 = true, I2 = true });
            sim.Run();
            Assert.AreEqual(false, sim.TopLevel.O);

        }
    }
}
