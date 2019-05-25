using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System.Linq;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class TransmiterModuleTests
    {
        [TestMethod]
        public void Idle()
        {
            var module = new TransmitterModule();
            module.Cycle(new TransmitterInputs());
            Assert.AreEqual(TransmitterFSM.Idle, module.State.FSM);
        }

        [TestMethod]
        public void Transmit()
        {
            var module = new TransmitterModule();
            RTLBitArray sourceData = (byte)0xAA;

            Assert.IsTrue(module.IsReady);

            module.Schedule(() => new TransmitterInputs() { Trigger = true, Data = sourceData } );
            module.Stage(0);

            // property depends on state change
            Assert.IsTrue(module.IsTransmissionStarted);

            module.Commit();
            Assert.IsTrue(module.IsTransmitting);
            Assert.IsFalse(module.IsTransmissionStarted);

            var result = new RTLBitArray(byte.MinValue);
            foreach (var idx in Enumerable.Range(0, 8))
            {
                Assert.AreEqual(TransmitterFSM.Transmitting, module.State.FSM);
                result[idx] = module.Bit;
                module.Cycle(new TransmitterInputs());
            }

            Assert.AreEqual(TransmitterFSM.WaitingForAck, module.State.FSM);

            // retrigger should be ignored
            module.Cycle(new TransmitterInputs() { Trigger = true });
            Assert.AreEqual(TransmitterFSM.WaitingForAck, module.State.FSM);

            module.Cycle(new TransmitterInputs() { Ack = true });
            Assert.AreEqual(TransmitterFSM.Idle, module.State.FSM);
            Assert.IsTrue(module.IsReady);

            Assert.AreEqual((byte)sourceData, (byte)result);
        }
    }
}
