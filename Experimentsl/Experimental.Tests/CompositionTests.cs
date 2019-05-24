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
            var topLevel = new CompositionModule();
            topLevel.Schedule(() => new CompositionInputs() { IsEnabled = true });

            var topLevelSnapshot = new VCDSignalsSnapshot("TOP");

            var controlScope = topLevelSnapshot.Scope("Control");
            var clockSignal = controlScope.Variable("Clock", true);

            topLevel.PopulateSnapshot(topLevelSnapshot);


            var vcdBuilder = new VCDBuilder(@"c:\tmp\combined.vcd");
            vcdBuilder.Init(topLevelSnapshot);

            var receivedData = new List<byte>();
            var clock = 0;
            var stageIteration = 0;

            var bytesToProcess = 256;
            var maxCycles = 100000;
            var maxStageIterations = 1000;

            while (receivedData.Count < bytesToProcess && clock < maxCycles)
            {
                var currentTime = clock * 2 * maxStageIterations;
                clockSignal.Value = true;

                stageIteration = 0;
                do
                {
                    currentTime++;

                    var modified = topLevel.Stage(stageIteration);

                    topLevel.PopulateSnapshot(topLevelSnapshot);
                    vcdBuilder.Snapshot(currentTime, topLevelSnapshot);

                    // no modules were modified during stage iteration, all converged
                    if (!modified)
                        break;
                }
                while (++stageIteration < maxStageIterations);

                if (stageIteration >= maxStageIterations)
                    throw new MaxStageIterationReachedException();

                if (topLevel.HasData)
                {
                    receivedData.Add(topLevel.Data);
                }

                currentTime = clock * 2 * maxStageIterations + maxStageIterations;

                clockSignal.Value = false;
                topLevel.PopulateSnapshot(topLevelSnapshot);
                vcdBuilder.Snapshot(currentTime, topLevelSnapshot);

                topLevel.Commit();
                clock++;
            }

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
