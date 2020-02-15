using FPGA.Attributes;
using System;
using System.Linq;

namespace FPGA.Fourier
{
    /// <summary>
    /// Non-synthesizable, evaluated at translation time
    /// </summary>
    [OnTranslation]
    public static class GeneratorTools
    {
        public static float[] CosArray(uint arrayLength, Direction direction)
        {
            return Enumerable
                .Range(0, (int)arrayLength)
                .Select(v => (float)Math.Cos(-(int)direction * 2.0 * Math.PI * v / arrayLength))
                .ToArray();
        }

        public static uint ArrayLength(uint bits)
        {
            if (bits == 0)
                throw new ArgumentOutOfRangeException(nameof(bits), bits, "should be positive");

            return (uint)(1 << (int)bits);
        }

        public static uint Mask(uint bits)
        {
            return (uint)((1 << (int)bits) - 1);
        }

        public static float FloatArrayLength(uint bits)
        {
            return ArrayLength(bits);
        }
    }
}
