using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class CompositionTests
    {
        VCDScope MakeScope(string name, ICombinationalRTLModule module)
        {
            var scope = new VCDScope()
            {
                Name = name,
                Scopes = new List<VCDScope>()
                {
                    new VCDScope()
                    {
                        Name = "Inputs",
                        Variables = module.InputProps.Select(p => new VCDVariable()
                        {
                            Name = p.Name,
                            Size = 1,
                        }).ToList()
                    },
                    new VCDScope()
                    {
                        Name = "Outputs",
                        Variables = module.OutputProps.Select(p => new VCDVariable()
                        {
                            Name = p.Name,
                            Size = 1,
                        }).ToList()
                    }
                }
            };

            switch (module)
            {
                case IRTLModule sync:
                    scope.Scopes.Add(new VCDScope()
                    {
                        Name = "State",
                        Variables = sync.StateProps.Select(p => new VCDVariable()
                        {
                            Name = p.Name,
                            Size = 1,
                        }).ToList()
                    });
                    break;
            }

            scope.Scopes.AddRange(module.ModuleProps.Select(child => MakeScope(child.Name, (ICombinationalRTLModule)child.GetValue(module))));

            return scope;
        }

        VCDScope ControlScope()
        {
            return new VCDScope()
            {
                Name = "Control",
                Variables = new List<VCDVariable>()
                {
                    new VCDVariable()
                    {
                        Name = "Clock",
                        Size = 1,
                    }
                }
            };
            
        }
        [TestMethod]
        public void Combined()
        {
            var topLevel = new CompositionModule();
            topLevel.Schedule(() => new CompositionInputs() { IsEnabled = true });

            var topLevelScope = MakeScope("TOP", topLevel);
            var vcdBuilder = new VCDBuilder(@"c:\tmp\combined.vcd")
            {
                Scopes = new List<VCDScope>()
                {
                    topLevelScope,
                    ControlScope()
                }
            };
            vcdBuilder.Init();

            var receivedData = new List<byte>();
            var clock = 0;
            var stageIteration = 0;

            var bytesToProcess = 256;
            var maxCycles = 100000;
            var maxStageIterations = 1000;

            var controlClockName = "Control_Clock";
            var signalsSnapshot = new Dictionary<string, object>()
                {
                    { controlClockName, true }
                };

            vcdBuilder.Snapshot(0, signalsSnapshot);

            while (receivedData.Count < bytesToProcess && clock < maxCycles)
            {
                var currentTime = clock * 2 * maxStageIterations;
                signalsSnapshot[controlClockName] = true;

                stageIteration = 0;
                do
                {
                    currentTime++;

                    var modified = topLevel.Stage(stageIteration);

                    topLevel.PopulateSnapshot(topLevelScope.Name, signalsSnapshot);
                    vcdBuilder.Snapshot(currentTime, signalsSnapshot);

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

                signalsSnapshot[controlClockName] = false;
                topLevel.PopulateSnapshot(topLevelScope.Name, signalsSnapshot);
                vcdBuilder.Snapshot(currentTime, signalsSnapshot);

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
