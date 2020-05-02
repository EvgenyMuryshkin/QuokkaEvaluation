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
                actual.Add(module.Data);
                module.Cycle(new EmitterInputs() {
                    IsEnabled = true,
                    Ack = true
                });
            }

            var missing = expected.Zip(actual, (e, a) => new { e, a }).Where(v => v.e != v.a).ToList();
            Assert.AreEqual(0, missing.Count, missing.Select(v => $"Exp: {v.e}, Act: {v.a}").ToCSV());
        }
    }
}
