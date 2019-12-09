using System;

namespace FPGA.Trigonometry
{
    public static class FPGATrigonometry
    {
        public static float Sin(float rad)
        {
            var result = FPGATrigonometryTools.Normalize(rad);

            bool negative = result > FPGATrigonometryConstants.PI;

            result = FPGATrigonometryTools.Q1Project(result);

            result = FPGATrigonometryTools.TaylorSin(result);

            if (negative)
                return -result;

            return result;
        }

        public static float Cos(float rad)
        {
            var result = FPGATrigonometryTools.Normalize(rad);

            bool negative = result > FPGATrigonometryConstants.HalfPI && result < FPGATrigonometryConstants.ThreeHalfPI;
            
            result = FPGATrigonometryTools.Q1Project(result);

            result = FPGATrigonometryTools.TaylorCos(result);

            if (negative)
                return -result;

            return result;
        }
    }
}
