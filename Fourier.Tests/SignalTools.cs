using FPGA.Fourier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fourier.Tests
{
    class SignalTools
    {
        public static ComplexFloat[] ZeroComplex(uint numSamles)
        {
            return Enumerable.Range(0, (int)numSamles).Select(s => new ComplexFloat()).ToArray();
        }

        public static ComplexFloat[] PeriodicComplex(float freq, uint samplingRate, uint numSamles)
        {
            float step = (float)(2 * Math.PI * freq / samplingRate);

            return Enumerable.Range(0, (int)numSamles).Select(s => new ComplexFloat()
            {
                Re = (float)Math.Cos(step * s),
                Im = (float)Math.Sin(step * s),
            }).ToArray();
        }

        public static ComplexFloat[] Combine(params ComplexFloat[][] data)
        {
            switch (data.Length)
            {
                case 0:
                    return new ComplexFloat[0];
                case 1:
                    return data[0];
                default:
                    return data[0]
                            .Zip(
                                Combine(data.Skip(1).ToArray()),
                                (l, r) => new ComplexFloat() { Re = l.Re + r.Re, Im = l.Im + r.Im })
                            .ToArray();
            }
        }
    }
}
