using System;

namespace FPGA.Trigonometry
{
    public static class FPGATrigonometryTools
    {
        public static float Normalize(float rad)
        {
            var result = rad;

            var rotations = (int)(rad / FPGATrigonometryConstants.TwoPI);
            if (rotations != 0)
            {
                result -= rotations * FPGATrigonometryConstants.TwoPI;
            }

            // rounding errors
            while (result > FPGATrigonometryConstants.TwoPI)
            {
                result -= FPGATrigonometryConstants.TwoPI;
            }

            while (result < 0)
            {
                result += FPGATrigonometryConstants.TwoPI;
            }

            return result;
        }

        public static float Q1Project(float rad)
        {
            float result = rad;

            if (result > FPGATrigonometryConstants.PI)
                result -= FPGATrigonometryConstants.PI;

            if (result > FPGATrigonometryConstants.HalfPI)
                result = FPGATrigonometryConstants.PI - result;

            return result;
        }

        public static float Pow(float rad, int power)
        {
            float result = 1;
            for (int i = 0; i < power; i++)
            {
                result *= rad;
            }

            return result;
        }

        public static float TaylorSin(float normalizedRad)
        {
            float result = 0;

            // calculate for 1,3,5,7,9
            for (int i = 0; i != 5; i++)
            {
                Func<byte> idx = () => (byte)((i << 1) + 1);
                float power = Pow(normalizedRad, idx());
                float factorial = FPGATrigonometryConstants.Factorials[idx()];
                float div = power / factorial;

                if ((i & 1) == 0)
                {
                    result += div;
                }
                else
                {
                    result -= div;
                }
            }

            return result;
        }

        public static float TaylorCos(float normalizedRad)
        {
            float result = 0;

            // calculate for 0, 2, 4, 6, 8
            for (byte i = 0; i < 5; i++)
            {
                Func<byte> idx = () => (byte)(i << 1);
                float power = Pow(normalizedRad, idx());
                float factorial = FPGATrigonometryConstants.Factorials[idx()];
                float div = power / factorial;

                if ((i & 1) == 0)
                {
                    result += div;
                }
                else
                {
                    result -= div;
                }
            }

            return result;
        }
    }
}
