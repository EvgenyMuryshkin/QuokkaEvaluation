using System;

namespace Fourier
{
    public static class FFT
    {
        public static void Transform(
            uint bits, 
            ComplexFloat[] source,
            ComplexFloat[] target,
            Direction direction)
        {
            FPGA.Const<uint> length = GeneratorTools.ArrayLength(bits);
            float[] cosMap = GeneratorTools.CosArray(length, direction);
            FPGA.Config.NoSync(cosMap, target);

            var eK = new ComplexFloat();
            var oK = new ComplexFloat();
            var rotated = new ComplexFloat();
            var tmp = new ComplexFloat();

            for (uint i = 0; i < source.Length; i++)
            {
                uint j = FPGA.Runtime.Reverse(i, bits);
                tmp = source[i];
                target[j] = tmp;
            }

            uint m = 1;
            uint groupSize = length;

            for (uint i = 0; i < bits; i++)
            {
                FPGA.Config.SetInclusiveRange(0, bits, i);

                groupSize = groupSize >> 1;

                for (uint group = 0; group < m; group++)
                {
                    FPGA.Config.SetInclusiveRange(0, bits, i);

                    uint arg = groupSize * group;
                    for (uint idx = group; idx < length; idx += m * 2)
                    {
                        FPGA.Config.SetInclusiveRange(0, bits, i);

                        eK = target[idx];
                        oK = target[idx + m];

                        FTTools.Rotate(cosMap, bits, oK, ref rotated, arg);

                        tmp.Re = eK.Re + rotated.Re;
                        tmp.Im = eK.Im + rotated.Im;

                        target[idx] = tmp;

                        tmp.Re = eK.Re - rotated.Re;
                        tmp.Im = eK.Im - rotated.Im;

                        target[idx + m] = tmp;
                    }
                }
                m = m << 1;
            }

            FTTools.CopyAndNormalize(bits, target, target, direction, ref tmp);
        }
    }
}
