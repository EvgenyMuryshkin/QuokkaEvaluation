namespace Fourier
{
    public static class DFT
    {
        public static void Transform(
            uint bits, 
            ComplexFloat[] source,
            ComplexFloat[] target,
            Direction direction)
        {
            FPGA.Const<uint> n = GeneratorTools.ArrayLength(bits);
            FPGA.Const<float> nFloat = GeneratorTools.FloatArrayLength(bits);

            uint mask = GeneratorTools.Mask(bits);

            float[] cosMap = GeneratorTools.CosArray(n, direction);

            var tmp = new ComplexFloat();

            // for each destination element
            for (uint i = 0; i < n; i++)
            {
                tmp.Re = 0.0f;
                tmp.Im = 0.0f;

                // sum source elements
                for (uint j = 0; j < n; j++)
                {
                    ComplexFloat data = new ComplexFloat();
                    data = source[j];

                    FTTools.RotateAndAdd(cosMap, bits, data, ref tmp, i * j);
                }

                target[i] = tmp;
            }
            
            FTTools.CopyAndNormalize(bits, target, target, direction, ref tmp);
        }
    }
}
