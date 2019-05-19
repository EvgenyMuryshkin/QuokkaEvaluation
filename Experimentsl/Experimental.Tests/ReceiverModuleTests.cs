using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System.Collections.Generic;
using System.Linq;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class ReceiverModuleTests
    {
        [TestMethod]
        public void Receiving()
        {
            var module = new ReceiverModule();

            Assert.AreEqual(ReceiverFSM.Idle, module.State.FSM);

            foreach (var idx in Enumerable.Range(0, 8))
            {
                module.Cycle(new ReceiverInputs() { IsValid = true, Bit = (idx % 2 == 1 ? true : false) });
            }
            Assert.AreEqual(ReceiverFSM.Receiving, module.State.FSM);

            module.Cycle(new ReceiverInputs() { IsValid = false });
            Assert.AreEqual(ReceiverFSM.WaitingForAck, module.State.FSM);

            Assert.AreEqual((byte)0xAA, module.Data);

            module.Cycle(new ReceiverInputs() { Ack = true });
            Assert.AreEqual(ReceiverFSM.Idle, module.State.FSM);
        }
    }
}
