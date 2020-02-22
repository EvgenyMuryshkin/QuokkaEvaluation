using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using Quokka.VCD;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class CompositionTests
    {
        [TestMethod]
        public void Combined()
        {
            var bytesToProcess = 256;
            var receivedData = new List<byte>();

            var sim = new RTLSynchronousSimulator<CompositionModule>();
            sim.Trace(PathTools.VCDOutputPath());

            sim.IsRunning = (simulatorCallback) => receivedData.Count < bytesToProcess;

            sim.OnPostStage = (topLevel) =>
            {
                if (topLevel.HasData)
                {
                    receivedData.Add(topLevel.Data);
                }
            };

            sim.Run();

            Assert.AreEqual(bytesToProcess, receivedData.Count);
            var missing = Enumerable
                .Range(0, bytesToProcess)
                .Zip(receivedData, (e, a) => new { e, a })
                .Where(p => p.e != p.a)
                .Select(p => $"E: {p.e}, A: {p.a}")
                .ToList();

            Assert.AreEqual(0, missing.Count, missing.ToCSV());
        }
    }
}
