﻿using Experimental.Tests;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.Public.Tools;
using Quokka.RTL;
using Quokka.RTL.Simulator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RTL.Modules
{
    class NotGateFeedbackInputs
    {

    }

    class NotGateFeedbackModule : RTLCombinationalModule<NotGateFeedbackInputs>
    {
        NotGateModule Inverter = new NotGateModule();

        protected override void OnSchedule(Func<NotGateFeedbackInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            Inverter.Schedule(() => new NotGateInputs() { Input = Inverter.Output });
        }
    }

    [TestClass]
    public class ExampleTests
    {
        static string VCDOutputPath([CallerMemberName] string testName = "")
        {
            return Path.Combine(PathTools.ProjectPath, "SimResults", $"{testName}.vcd");
        }

        T Module<T>()
            where T : IRTLCombinationalModule, new()
        {
            var module = new T();
            module.Setup();

            return module;
        }

        [TestMethod]
        public void SignedCastModuleTest()
        {
            var sim = new CombinationalRTLSimulator<SignedCastModule>();
            var values = new short[] {
                short.MinValue,
                -257, -256, -255,
                -1, 0, 1,
                127, 128, 129,
                254, 255, 256,
                short.MaxValue,
                -32641,
                -128, -127, -126,
            };

            foreach (var value in values.Reverse())
            {
                sim.TopLevel.Cycle(new SignedCastModuleInputs() { ShortValue = value });
                Assert.AreEqual((byte)value, sim.TopLevel.ByteValue);
                Assert.AreEqual((sbyte)value, sim.TopLevel.SByteValue);
                Assert.AreEqual((ushort)value, sim.TopLevel.UShortValue);
                Assert.AreEqual((int)value, sim.TopLevel.IntValue);
                Assert.AreEqual((uint)value, sim.TopLevel.UIntValue);
            }
        }

        [TestMethod]
        public void UnsignedCastModuleTest()
        {
            var sim = new CombinationalRTLSimulator<UnsignedCastModule>();
            var values = new ushort[] {
                0,
                127, 128, 129,
                254, 255, 256,
                0x7FFE,
                0x7FFF,
                0x8000,
                0x8001,
                ushort.MaxValue
            };

            foreach (var value in values)
            {
                sim.TopLevel.Cycle(new UnsignedCastModuleInputs() { UShortValue = value });
                Assert.AreEqual((byte)value, sim.TopLevel.ByteValue);
                Assert.AreEqual((sbyte)value, sim.TopLevel.SByteValue);
                Assert.AreEqual((short)value, sim.TopLevel.ShortValue);
                Assert.AreEqual((int)value, sim.TopLevel.IntValue);
                Assert.AreEqual((uint)value, sim.TopLevel.UIntValue);
            }
        }

        [TestMethod]
        public void BitArrayModuleTest()
        {
            var sim = new CombinationalRTLSimulator<BitArrayModule>();
            sim.TopLevel.Cycle(new BitArrayInputs() { Value = 0xC2 });
            Assert.AreEqual(0xC2, (byte)sim.TopLevel.Direct);
            Assert.AreEqual(0xC, (byte)sim.TopLevel.High);
            Assert.AreEqual(0x2, (byte)sim.TopLevel.Low);
            Assert.AreEqual(0x43, (byte)sim.TopLevel.Reversed);
            Assert.AreEqual(0x3, (byte)sim.TopLevel.ReversedHigh);
            Assert.AreEqual(0x4, (byte)sim.TopLevel.ReversedLow);
            Assert.AreEqual(0x9, (byte)sim.TopLevel.Picks);
            Assert.AreEqual(0xD, (byte)sim.TopLevel.FromBits1);
            Assert.AreEqual(0x7, (byte)sim.TopLevel.FromBits2);
        }

        [TestMethod]
        public void CombinationalROMModuleTest()
        {
            var sim = new CombinationalRTLSimulator<CombinationalROMModule>();

            var buff = CombinationalROMModule.GetBuffer();

            for (var idx = 0; idx < buff.Length; idx++)
            {
                sim.TopLevel.Cycle(new CombinationalROMModuleInputs()
                {
                    ReadAddress1 = (byte)idx,
                    ReadAddress2 = (byte)(255 - idx)
                });

                Assert.AreEqual(buff[idx], sim.TopLevel.Value1);
                Assert.AreEqual(buff[255 - idx], sim.TopLevel.Value2);
            }
        }

        [TestMethod]
        public void SynchronousROMModuleTest()
        {
            var sim = new RTLSimulator<SynchronousROMModule>();

            var buff = SynchronousROMModule.GetBuffer();

            for (var idx = 0; idx < buff.Length; idx++)
            {
                sim.TopLevel.Cycle(new SynchronousROMModuleInputs()
                {
                    Addr1 = (byte)idx,
                    Addr2 = (byte)(255 - idx)
                });

                Assert.AreEqual(buff[idx], sim.TopLevel.Data1);
                Assert.AreEqual(buff[255 - idx], sim.TopLevel.Data2);
            }
        }

        [TestMethod]
        public void SDP_RF_RAMModuleTest()
        {
            var sim = new RTLSimulator<SDP_RF_RAMModule>();
            sim.TopLevel.Cycle(new SDP_RF_RAMModule_Inputs()
            {
                ReadAddress = 10,
                WE = true,
                WriteAddress = 10,
                WriteData = 10
            });
            Assert.AreEqual(0, sim.TopLevel.Data);
            sim.TopLevel.Cycle(new SDP_RF_RAMModule_Inputs()
            {
                ReadAddress = 10,
                WE = true,
                WriteAddress = 10,
                WriteData = 11
            });
            Assert.AreEqual(10, sim.TopLevel.Data);
        }

        [TestMethod]
        public void SDP_WF_RAMModuleTest()
        {
            var sim = new RTLSimulator<SDP_WF_RAMModule>();
            sim.TopLevel.Cycle(new SDP_WF_RAMModule_Inputs()
            {
                ReadAddress = 10,
                WE = true,
                WriteAddress = 10,
                WriteData = 10
            });
            Assert.AreEqual(10, sim.TopLevel.Data);
            sim.TopLevel.Cycle(new SDP_WF_RAMModule_Inputs()
            {
                ReadAddress = 10,
                WE = true,
                WriteAddress = 10,
                WriteData = 11
            });
            Assert.AreEqual(11, sim.TopLevel.Data);
        }

        [TestMethod]
        public void LogicRAMModuleTest()
        {
            var sim = new RTLSimulator<LogicRAMModule>();

            sim.TopLevel.Cycle(new LogicRAMModuleInputs() { Value = 0xFF });
            Assert.AreEqual(0x3F, sim.TopLevel.Avg);
            sim.TopLevel.Cycle(new LogicRAMModuleInputs() { Value = 0xFF });
            Assert.AreEqual(0x7F, sim.TopLevel.Avg);
            sim.TopLevel.Cycle(new LogicRAMModuleInputs() { Value = 0xFF });
            Assert.AreEqual(0xBF, sim.TopLevel.Avg);
            sim.TopLevel.Cycle(new LogicRAMModuleInputs() { Value = 0xFF });
            Assert.AreEqual(0xFF, sim.TopLevel.Avg);
        }

        [TestMethod]
        public void LogicRAMIndexingModuleTest()
        {
            var sim = new RTLSimulator<LogicRAMIndexingModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new LogicRAMIndexingModuleInputs() { WE = true, WriteAddr = 0, WriteData = 0x66 });
            tl.Cycle(new LogicRAMIndexingModuleInputs() { WE = true, WriteAddr = 1, WriteData = 0xAA });
            tl.Cycle(new LogicRAMIndexingModuleInputs() { WE = true, WriteAddr = 2, WriteData = 0x55 });
            tl.Cycle(new LogicRAMIndexingModuleInputs() { WE = true, WriteAddr = 3, WriteData = 0xFF });

            sim.TopLevel.Cycle(new LogicRAMIndexingModuleInputs() { ReadAddr = 2, OpData = 0xF0 });
            Assert.AreEqual(false, tl.CmpMemLhs);
            Assert.AreEqual(true, tl.CmpMemRhs);
            Assert.AreEqual(0xF5, tl.LogicMemLhs);
            Assert.AreEqual(0x50, tl.LogicMemRhs);
            Assert.AreEqual(0x65, tl.MathMemLhs);
            Assert.AreEqual(0x45, tl.MathMemRhs);
            Assert.AreEqual(0xFF, tl.MemLhsRhs);

            sim.TopLevel.Cycle(new LogicRAMIndexingModuleInputs() { ReadAddr = 0, OpData = 0x50 });
            Assert.AreEqual(true, tl.CmpMemLhs);
            Assert.AreEqual(false, tl.CmpMemRhs);
            Assert.AreEqual(0x76, tl.LogicMemLhs);
            Assert.AreEqual(0x40, tl.LogicMemRhs);
            Assert.AreEqual(0x16, tl.MathMemLhs);
            Assert.AreEqual(0xB6, tl.MathMemRhs);
            Assert.AreEqual(0x10, tl.MemLhsRhs);
        }


        [TestMethod]
        public void CompositionModuleTest()
        {
            var bytesToProcess = 256;
            var receivedData = new List<byte>();

            var sim = new RTLSimulator<CompositionModule>();
            //sim.TraceToVCD(PathTools.VCDOutputPath());

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

        [TestMethod]
        public void EmitterModuleTest()
        {
            var module = Module<EmitterModule>();

            var expected = Enumerable.Range(0, 256).Select(idx => (byte)idx).ToList();
            List<byte> actual = new List<byte>();

            foreach (var idx in Enumerable.Range(0, 256))
            {
                actual.Add(module.Data);
                module.Cycle(new EmitterInputs()
                {
                    IsEnabled = true,
                    Ack = true
                });
            }

            var missing = expected.Zip(actual, (e, a) => new { e, a }).Where(v => v.e != v.a).ToList();
            Assert.AreEqual(0, missing.Count, missing.Select(v => $"Exp: {v.e}, Act: {v.a}").ToCSV());
        }

        [TestMethod]
        public void FullAdderModuleTest()
        {
            var values = new[]
            {
                new { A = 0, B = 0, CIn = 0, O = 0, COut = 0 },
                new { A = 1, B = 0, CIn = 0, O = 1, COut = 0 },
                new { A = 0, B = 1, CIn = 0, O = 1, COut = 0 },
                new { A = 1, B = 1, CIn = 0, O = 0, COut = 1 },
                new { A = 0, B = 0, CIn = 1, O = 1, COut = 0 },
                new { A = 1, B = 0, CIn = 1, O = 0, COut = 1 },
                new { A = 0, B = 1, CIn = 1, O = 0, COut = 1 },
                new { A = 1, B = 1, CIn = 1, O = 1, COut = 1 },
            };

            Func<int, bool> toBool = (v) => v == 0 ? false : true;

            foreach (var set in values)
            {
                var sim = new RTLSimulator<FullAdderModule>();
                sim.IsRunning = (cb) => cb.Clock == 0;
                sim.TopLevel.Schedule(() => new FullAdderInputs() { A = toBool(set.A), B = toBool(set.B), CIn = toBool(set.CIn) });
                sim.Run();
                Assert.AreEqual(toBool(set.O), sim.TopLevel.O);
                Assert.AreEqual(toBool(set.COut), sim.TopLevel.COut);
            }
        }

        [TestMethod]
        public void CounterModuleTest()
        {
            var sim = new RTLSimulator<CounterModule>();
            sim.IsRunning = (cb) => cb.Clock < 100;
            sim.TraceToVCD(VCDOutputPath());
            sim.TopLevel.Schedule(() => new CounterInputs() { Enabled = true });

            Assert.AreEqual(0, sim.TopLevel.Value);
            sim.Run();
            Assert.AreEqual(100, sim.TopLevel.Value);
        }

        [TestMethod]
        public void NotGateFeedbackModuleTest()
        {
            var sim = new RTLSimulator<NotGateFeedbackModule>();
            sim.TraceToVCD(VCDOutputPath());

            Assert.ThrowsException<MaxStageIterationReachedException>(() =>
            {
                sim.Run();
            });
        }

        [TestMethod]
        public void NotGateModuleTest()
        {
            var sim = new RTLSimulator<NotGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            sim.TraceToVCD(VCDOutputPath());
            sim.TopLevel.Schedule(() => new NotGateInputs() { Input = true }); ;

            Assert.AreEqual(true, sim.TopLevel.Output);
            sim.Run();
            Assert.AreEqual(false, sim.TopLevel.Output);
        }

        [TestMethod]
        public void AndGateModuleTest()
        {
            var sim = new RTLSimulator<AndGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = true, I2 = false });
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = true, I2 = true });
            Assert.AreEqual(true, sim.TopLevel.O);
        }

        [TestMethod]
        public void OrGateModuleTest()
        {
            var sim = new RTLSimulator<OrGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = false, I2 = false });
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = true, I2 = false });
            Assert.AreEqual(true, sim.TopLevel.O);
        }

        [TestMethod]
        public void XorGateModuleTest()
        {
            var sim = new RTLSimulator<XorGateModule>();
            sim.IsRunning = (cb) => cb.Clock == 0;
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = false, I2 = false });
            Assert.AreEqual(false, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = true, I2 = false });
            Assert.AreEqual(true, sim.TopLevel.O);

            sim.TopLevel.Cycle(new GateInputs() { I1 = true, I2 = true });
            Assert.AreEqual(false, sim.TopLevel.O);
        }

        [TestMethod]
        public void ReceiverModuleTest()
        {
            var module = Module<ReceiverModule>();

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

        [TestMethod]
        public void TransmitterModule_IdleTest()
        {
            var module = Module<TransmitterModule>();
            module.Cycle(new TransmitterInputs());
            Assert.AreEqual(TransmitterFSM.Idle, module.State.FSM);
        }

        [TestMethod]
        public void TransmitterModuleTest()
        {
            var module = Module<TransmitterModule>();
            RTLBitArray sourceData = (byte)0xAA;

            Assert.IsTrue(module.IsReady);

            module.Schedule(() => new TransmitterInputs() { Trigger = true, Data = sourceData });
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

        [TestMethod]
        public void ShifterTests()
        {
            var shlData = new RTLBitArray((byte)0x81);
            var shaData = shlData.Signed();

            var shifter = Module<ShifterModule>();
            foreach (var shiftBy in Enumerable.Range(0, 8))
            {
                var sb = new RTLBitArray(shiftBy).Unsigned().Resized(3);
                shifter.Cycle(new ShifterInputs() 
                { 
                    Value = shlData, 
                    ShiftBy = sb
                });

                Assert.AreEqual(shlData >> shiftBy, shifter.SHRL);
                Assert.AreEqual(shaData >> shiftBy, shifter.SHRA);
                Assert.AreEqual(shlData << shiftBy, shifter.SHLL);
            }
        }

        [TestMethod]
        public void SignalsMuxTest()
        {
            var mux = Module<SignalsMuxModule>();
            foreach (var idx in Enumerable.Range(0,4))
            {
                mux.Cycle(new SignalsMuxModuleInputs()
                {
                    Addr = idx,
                    Sig0 = 10,
                    Sig1 = 20,
                    Sig2 = 30,
                    Sig3 = 40
                });
                Assert.AreEqual(10 * (idx + 1), mux.Value);
            }
        }

        [TestMethod]
        public void TuplesModuleTest()
        {
            var t = Module<TuplesModule>();
            Action<bool, bool, bool, bool> iteration = (v1, v2, s, d) =>
            {
                t.Cycle(new TuplesModuleInputs() { Value1 = v1, Value2 = v2 });
                Assert.AreEqual(s, t.Same, $"Same failed for {v1}, {v2}");
                Assert.AreEqual(d, t.Diff, $"Diff failed for {v1}, {v2}");
            };
            iteration(false, false, true, false);
            iteration(false, true, false, true);
            iteration(true, false, false, true);
            iteration(true, true, true, false);
        }
    }
}

