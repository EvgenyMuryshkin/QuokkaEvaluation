using System;

namespace FPGA.Trigonometry
{
    public static class FPGATrigonometryConstants
    {
        public const float TwoPI = (float)(2 * Math.PI);
        public const float PI = (float)Math.PI;
        public const float HalfPI = (float)(Math.PI / 2);
        public const float ThreeHalfPI = (float)(3 * Math.PI / 2);

        public static float[] Factorials = FPGATrigonometryTranslation.FactorialArray();
    }
}
