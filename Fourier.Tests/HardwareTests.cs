using FPGA.Fourier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuokkaIntegrationTests;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace Fourier.Tests
{
    class COMPort : QuokkaPort
    {
        public COMPort() : base("COM3", 115200)
        {

        }

        protected override void OnBeforeOpen()
        {
            base.OnBeforeOpen();
            mPort.ReadBufferSize = 20000;
        }

        public void Send(ComplexFloat[] data)
        {
            foreach (var item in data)
            {
                WriteFloat(item.Re);
                WriteFloat(item.Im);
            }
        }

        public void Receive(ComplexFloat[] data, out uint duration)
        {
            for (var idx = 0; idx < data.Length; idx++)
            {
                var item = data[idx];
                item.Re = ReadFloat(DefaultTimeout);
                item.Im = ReadFloat(DefaultTimeout);
                data[idx] = item;
            }

            duration = ReadUInt32(DefaultTimeout);
        }
    }

    [TestClass]
    public class HardwareTests
    {
        Boilerplate _bp = new Boilerplate();

        [TestMethod]
        public void LoopbackController()
        {
            using (var port = new COMPort())
            {
                var sourceSignal = _bp.TestSignal();
                port.Send(sourceSignal);

                var receiverSignal = _bp.ZeroSignal;
                port.Receive(receiverSignal, out uint duration);

                Validation.AssertSpectres(sourceSignal, receiverSignal, true, true);
            }
        }

        [TestMethod]
        public void OffsetController()
        {
            using (var port = new COMPort())
            {
                var sourceSignal = _bp.TestSignal();
                port.Send(sourceSignal);

                //offset logic
                for (int idx = 0; idx < sourceSignal.Length; idx++)
                {
                    ComplexFloat tmp = new ComplexFloat();
                    tmp = sourceSignal[idx];

                    tmp.Re = 1024f;
                    tmp.Im = tmp.Im + 10f;

                    sourceSignal[idx] = tmp;
                }

                var receiverSignal = _bp.ZeroSignal;
                port.Receive(receiverSignal, out uint duration);

                Validation.AssertSpectres(sourceSignal, receiverSignal, true, true);
            }
        }

        [TestMethod]
        public void CopyAndNormalizeController()
        {
            using (var port = new COMPort())
            {
                var sourceSignal = _bp.TestSignal();
                port.Send(sourceSignal);
                ComplexFloat tmp = new ComplexFloat();
                FTTools.CopyAndNormalize(_bp.Bits, sourceSignal, sourceSignal, Direction.Forward, ref tmp);

                var receiverSignal = _bp.ZeroSignal;
                port.Receive(receiverSignal, out uint duration);

                Validation.AssertSpectres(sourceSignal, receiverSignal, true, true);
            }
        }

        [TestMethod]
        public void FPUTimingController()
        {
            using (var port = new COMPort())
            {
                var ops = new[]
                {
                    FPUTimingType.None,
                    FPUTimingType.Add,
                    FPUTimingType.Sub,
                    FPUTimingType.Mul,
                    FPUTimingType.Div
                };

                var durations = new Dictionary<FPUTimingType, uint>();
                var results = new Dictionary<FPUTimingType, float>();
                var op1 = 100000000f;
                var op2 = 10f;

                foreach (var op in ops)
                {
                    port.Write(new byte[] { (byte)op });
                    port.WriteFloat(op1);
                    port.WriteFloat(op2);

                    var opResult = port.ReadFloat(port.DefaultTimeout);
                    var duration = port.ReadUInt32(port.DefaultTimeout);
                    durations[op] = duration;
                    results[op] = opResult;
                }

                /// current FPU overhead is 11 clocks 8-{
                const int overhead = 11;
                var opDurations = durations
                    .Select(p => new { p.Key, Value = Math.Max(overhead, p.Value - durations[FPUTimingType.None]) - overhead })
                    .ToDictionary(p => p.Key, p => p.Value);

            }
        }

        [TestMethod]
        public void DFTController()
        {
            using (var port = new COMPort())
            {
                var sourceSignal = _bp.TestSignal();
                port.Send(sourceSignal);
                DFT.Transform(_bp.Bits, sourceSignal, Direction.Forward);

                port.WaitForData(TimeSpan.FromMinutes(1));

                var receiverSignal = _bp.ZeroSignal;
                port.Receive(receiverSignal, out uint duration);

                Validation.AssertSpectres(sourceSignal, receiverSignal, true, true);
            }
        }

        [TestMethod]
        public void FFTController()
        {
            using (var port = new COMPort())
            {
                var sourceSignal = _bp.TestSignal();
                var target = _bp.ZeroSignal;

                port.Send(sourceSignal);
                FFT.Transform(_bp.Bits, sourceSignal, target, Direction.Forward);

                port.WaitForData(TimeSpan.FromMinutes(1));

                var receivedSignal = _bp.ZeroSignal;
                port.Receive(receivedSignal, out uint duration);

                Validation.AssertSpectres(target, receivedSignal, true, true);
            }
        }

        [TestMethod]
        public void BitReverseController()
        {
            using (var port = new COMPort())
            {
                port.Write(0);
                foreach (var i in _bp.Range)
                {
                    var reversed = port.ReadUInt32(port.DefaultTimeout);
                    Assert.AreEqual(FPGA.Runtime.Reverse(i, _bp.Bits), reversed, $"Failed for {i}");
                }
            }
        }
    }
}
