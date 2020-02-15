using FPGA.Fourier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Fourier.Tests
{
    [TestClass]
    public class FFTTests
    {
        Boilerplate _bp = new Boilerplate();

        [TestMethod]
        public void ForwardQuokkaFFT()
        {
            var sourceSignal = _bp.TestSignal();

            var referenceDFT = sourceSignal.ToArray();
            Reference.DFT(referenceDFT, Direction.Forward);

            ComplexFloat[] quokkaFFT = _bp.ZeroSignal;
            FFT.Transform(_bp.Bits, sourceSignal, quokkaFFT, Direction.Forward);

            Validation.AssertSpectres(referenceDFT, quokkaFFT, true, false);
        }
    }
}
