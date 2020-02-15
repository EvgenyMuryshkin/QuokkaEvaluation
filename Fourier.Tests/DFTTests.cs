using FPGA.Fourier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Fourier.Tests
{
    [TestClass]
    public class DFTTests
    {
        Boilerplate _bp = new Boilerplate();

        [TestMethod]
        public void ForwardReferenceDFT()
        {
            var sourceSignal = _bp.TestSignal();

            var referenceDFT = sourceSignal.ToArray();
            Reference.DFT(referenceDFT, Direction.Forward);

            var expected = _bp.ZeroSignal;
            expected[30].Re = 1;
            expected[40].Re = 1;
            expected[50].Re = 1;

            Validation.AssertSpectres(expected, referenceDFT, true, false);
        }

        [TestMethod]
        public void ForwardQuokkaDFT()
        {
            var sourceSignal = _bp.TestSignal();

            var referenceDFT = sourceSignal.ToArray();
            Reference.DFT(referenceDFT, Direction.Forward);

            var quokkaDFT = sourceSignal.ToArray();
            FPGA.Fourier.DFT.Transform(_bp.Bits, quokkaDFT, Direction.Forward);

            Validation.AssertSpectres(referenceDFT, quokkaDFT, true, false);
        }
    }
}
