using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System.Collections.Generic;
using System.Linq;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class EmitterModuleTests
    {
        [TestMethod]
        public void Emitting()
        {
            var module = new EmitterModule();

            var expected = Enumerable.Range(0, 256).Select(idx => (byte)idx).ToList();
            List<byte> actual = new List<byte>();

            foreach (var idx in Enumerable.Range(0, 256))
            {
                Assert.AreEqual(EmitterFSM.Emitting, module.State.FSM);

                module.Cycle(new EmitterInputs() {
                    IsEnabled = true
                });
                Assert.AreEqual(EmitterFSM.WaitingForAck, module.State.FSM);

                actual.Add(module.Data);

                module.Cycle(new EmitterInputs() { Ack = true });
            }

            var missing = expected.Zip(actual, (e, a) => new { e, a }).Where(v => v.e != v.a).ToList();
            Assert.AreEqual(0, missing.Count, missing.Select(v => $"E: {v.e}, A: {v.a}").ToCSV());

        }
    }
}
