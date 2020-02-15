using FPGA.Fourier;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fourier.Tests
{
    /// <summary>
    /// Sourced from https://github.com/andrewkirillov/AForge.NET/blob/master/Sources/Math/FourierTransform.cs
    /// </summary>
    class Reference
    {
        public static void DFT(ComplexFloat[] data, Direction direction)
        {
            int n = data.Length;
            double arg, cos, sin;
            ComplexFloat[] dst = new ComplexFloat[n];

            // for each destination element
            for (int i = 0; i < n; i++)
            {
                dst[i] = new ComplexFloat();

                arg = -(int)direction * 2.0 * System.Math.PI * (double)i / (double)n;

                // sum source elements
                for (int j = 0; j < n; j++)
                {
                    cos = Math.Cos(j * arg);
                    sin = Math.Sin(j * arg);

                    dst[i].Re += (float)(data[j].Re * cos - data[j].Im * sin);
                    dst[i].Im += (float)(data[j].Re * sin + data[j].Im * cos);
                }
            }

            // copy elements
            if (direction == Direction.Forward)
            {
                // devide also for forward transform
                for (int i = 0; i < n; i++)
                {
                    data[i].Re = dst[i].Re / n;
                    data[i].Im = dst[i].Im / n;
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    data[i].Re = dst[i].Re;
                    data[i].Im = dst[i].Im;
                }
            }
        }
    }
}
