using System;
using System.Linq;

namespace Fourier
{
    public static class FTTools
    {
        public static void RotateAndAdd(
            float[] cosMap,
            uint bits,
            ref ComplexFloat source,
            ref ComplexFloat target,
            uint arg)
        {
            FPGA.Const<uint> mask = GeneratorTools.Mask(bits);

            if (bits == 0)
            {
                target.Re = source.Re;
                target.Im = source.Im;
            }
            else
            {
                float cos = 0.0f, sin = 0.0f;

                uint cosIdx = arg & mask;
                cos = cosMap[cosIdx];

                if (bits > 1)
                {
                    uint sinIdx = (uint)(cosIdx + (cosMap.Length >> 2)) & mask;
                    sin = cosMap[sinIdx];
                }

                target.Re += source.Re * cos - source.Im * sin;
                target.Im += source.Re * sin + source.Im * cos;
            }
        }

        public static void Rotate(
            float[] cosMap, 
            uint bits, 
            ref ComplexFloat source, 
            ref ComplexFloat target, 
            uint arg)
        {
            FPGA.Const<uint> mask = GeneratorTools.Mask(bits);

            if (bits == 0)
            {
                target.Re = source.Re;
                target.Im = source.Im;
            }
            else
            {
                float cos = 0.0f, sin = 0.0f;

                Func<uint> cosIdx = () => arg & mask;
                Func<uint> sinIdx = () => (uint)(cosIdx() + (cosMap.Length >> 2)) & mask;

                cos = cosMap[cosIdx()];

                if (bits > 1)
                {
                    sin = cosMap[sinIdx()];
                }

                target.Re = source.Re * cos - source.Im * sin;
                target.Im = source.Re * sin + source.Im * cos;
            }
        }

        public static void CopyAndNormalize(
            uint bits,
            ComplexFloat[] source,
            ComplexFloat[] target,
            Direction direction,
            ref ComplexFloat tmpBuff)
        {
            FPGA.Const<float> nFloat = GeneratorTools.FloatArrayLength(bits);

            for (uint i = 0; i < source.Length; i++)
            {
                FPGA.Config.SetInclusiveRange(0, source.Length, i);

                tmpBuff = source[i];
                
                if (direction == Direction.Forward)
                {
                    tmpBuff.Re = tmpBuff.Re / nFloat;
                    tmpBuff.Im = tmpBuff.Im / nFloat;
                }
                
                target[i] = tmpBuff;
            }
        }
    }
}
