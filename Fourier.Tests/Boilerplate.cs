using FPGA.Fourier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fourier.Tests
{
    public class Boilerplate
    {
        public uint Bits = 10;
        public uint ArrayLength => GeneratorTools.ArrayLength(Bits);
        public uint NumberOfSamples => ArrayLength;
        public IEnumerable<uint> Range => Enumerable.Range(0, (int)ArrayLength).Select(i => (uint)i);


        public ComplexFloat[] ZeroSignal => SignalTools.ZeroComplex(NumberOfSamples);

        public ComplexFloat[] TestSignal()
        {
            var freq30 = SignalTools.PeriodicComplex(30, ArrayLength, NumberOfSamples);
            var freq40 = SignalTools.PeriodicComplex(40, ArrayLength, NumberOfSamples);
            var freq50 = SignalTools.PeriodicComplex(50, ArrayLength, NumberOfSamples);

            var sourceSignal = SignalTools.Combine(freq30, freq40, freq50);

            return sourceSignal;
        }
    }
}
