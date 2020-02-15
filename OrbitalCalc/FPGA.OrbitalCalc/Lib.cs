using FPGA;
using FPGA.Trigonometry;
using System;

namespace FPGA.OrbitalCalc
{
    public static class FPGAOrbitalCalc
    {
        public static float VEsc(float mass, float radius)
        {
            return SequentialMath.Sqrt(FPGAOrbitalCalcConstants.TwoG * mass / radius);
        }

        public static float VOrbit(float mass, float radius)
        {
            return SequentialMath.Sqrt(FPGAOrbitalCalcConstants.G * mass / radius);
        }

        public static float VOrbitSquared(float mass, float radius)
        {
            return FPGAOrbitalCalcConstants.G * mass / radius;
        }

        public static float LOrbit(float radius)
        {
            return FPGATrigonometryConstants.TwoPI * radius;
        }

        public static float TOrbit(float mass, float radius)
        {
            var vOrbit = VOrbit(mass, radius);
            var lOrbit = LOrbit(radius);

            return lOrbit / vOrbit;
        }

        static float DeltaVTerm1(float mass, float radius)
        {
            return SequentialMath.Sqrt(FPGAOrbitalCalcConstants.G * mass / radius);
        }

        static float DeltaVTerm2(float innerRadius, float outerRadius)
        {
            return SequentialMath.Sqrt(2 * outerRadius / (innerRadius + outerRadius));
        }

        public static float DeltaVInnerOrbit(float mass, float innerRadius, float outerRadius)
        {
            var term1 = DeltaVTerm1(mass, innerRadius);
            var term2 = DeltaVTerm2(innerRadius, outerRadius) - 1;
            return term1 * term2;
        }
        public static float DeltaVOuterOrbit(float mass, float innerRadius, float outerRadius)
        {
            var term1 = DeltaVTerm1(mass, outerRadius);
            // to match logic of term2 calculations, swap arguments
            var term2 = 1 - DeltaVTerm2(outerRadius, innerRadius);

            return term1 * term2;
        }
        public static float DeltaVInclinationOrbit(float mass, float innerRadius, float outerRadius, float inclination)
        {
            var vApogee = SequentialMath.Sqrt(2 * FPGAOrbitalCalcConstants.G * mass * (innerRadius / outerRadius) * (1 / (innerRadius + outerRadius)));
            var vOrbital = VOrbit(mass, outerRadius);
            var cosInc = FPGATrigonometry.Cos(inclination);
            var deltaV = SequentialMath.Sqrt(vApogee * vApogee + vOrbital * vOrbital - 2 * vApogee * vOrbital * cosInc);

            return deltaV;
        }

        public static float DeltaVInclinationOrbitOptimized(float mass, float innerRadius, float outerRadius, float inclination)
        {
            var vApogeeSquared = 2 * FPGAOrbitalCalcConstants.G * mass * (innerRadius / outerRadius) * (1 / (innerRadius + outerRadius));
            var vOrbitalSquared = VOrbitSquared(mass, outerRadius);
            var cosInc = FPGATrigonometry.Cos(inclination);

            var vCos = SequentialMath.Sqrt(vApogeeSquared * vOrbitalSquared * cosInc * cosInc);
            var deltaV = SequentialMath.Sqrt(vApogeeSquared + vOrbitalSquared -2 * vCos);

            return deltaV;
        }
    }
}
