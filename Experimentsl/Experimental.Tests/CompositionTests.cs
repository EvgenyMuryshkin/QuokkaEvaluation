using Experimental.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    public class TopLevel
    {
        public EmitterModule Emitter = new EmitterModule();
        public TransmitterModule Transmitter = new TransmitterModule();
        public ReceiverModule Receiver = new ReceiverModule();

        public List<IRTLModule> AllModules => new List<IRTLModule>() {
            Emitter,
            Transmitter,
            Receiver
        };

        public TopLevel()
        {
            Emitter.Schedule(() => new EmitterInputs()
            {
                Ack = Transmitter.IsReady
            });

            Transmitter.Schedule(() => new TransmitterInputs()
            {
                Data = Emitter.Data,
                Trigger = Emitter.HasData,
                Ack = Receiver.HasData
            });

            Receiver.Schedule(() => new ReceiverInputs()
            {
                Bit = Transmitter.Bit,
                IsValid = Transmitter.IsTransmitting,
                Ack = true
            });
        }
    }

    [TestClass]
    public class CompositionTests
    {
        VCDScope MakeScope(string name, IRTLModule module)
        {
            return new VCDScope()
            {
                Name = name,
                Scopes = new List<VCDScope>()
                {
                    new VCDScope()
                    {
                        Name = "State",
                        Variables = module.StateProps.Select(p => new VCDVariable()
                        {
                            Name = p.Name,
                            Size = 1,
                        }).ToList()
                    },
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
        }

        VCDScope MakeScope(TopLevel topLevel)
        {
            return new VCDScope()
            {
                Name = "TopLevel",
                Scopes = RTLModuleHelper
                    .ModuleProperties(topLevel.GetType())
                    .Select(p => MakeScope(p.Name, (IRTLModule)p.GetValue(topLevel))).ToList()
            };
            
        }

        [TestMethod]
        public void Combined()
        {
            var topLevel = new TopLevel();

            var topLevelScope = MakeScope(topLevel);
            var vcdBuilder = new VCDBuilder()
            {
                Scopes = new List<VCDScope>() { topLevelScope }
            };
            vcdBuilder.Save(@"c:\tmp\combined.vcd");

            var receivedData = new List<byte>();
            var clock = 0;
            var stageIteration = 0;

            var bytesToProcess = 256;
            var maxCycles = 1000000;
            var maxStageIterations = 1000;

            while (receivedData.Count < bytesToProcess && clock < maxCycles)
            {
                stageIteration = 0;
                while (stageIteration++ < maxStageIterations)
                {
                    var modified = topLevel
                        .AllModules
                        .Aggregate(false, (prev, module) => prev || module.Stage(stageIteration));

                    // no modules were modified during stage iteration, all converged
                    //if (!modified)
                        break;
                }

                if (stageIteration >= maxStageIterations)
                    throw new MaxStageIterationReachedException();

                if (topLevel.Receiver.HasData)
                {
                    receivedData.Add(topLevel.Receiver.Data);
                }

                topLevel.AllModules.ForEach(m => m.Commit());
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
