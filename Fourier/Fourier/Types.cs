using System;

namespace FPGA.Fourier
{
    public struct ComplexFloat
    {
        public float Re;
        public float Im;

        public override string ToString()
        {
            return $"{Re} {(Im >= 0 ? "+" : "-")} {Math.Abs(Im)}i";
        }
    }

    public enum Direction
    {
        Forward = 1,
        Backward = -1
    }
}
