namespace FPGA.Fourier
{
    public static class DFT
    {
        public static void Transform(uint bits, ComplexFloat[] data, Direction direction)
        {
            FPGA.Const<uint> n = GeneratorTools.ArrayLength(bits);
            FPGA.Const<float> nFloat = GeneratorTools.FloatArrayLength(bits);

            uint mask = GeneratorTools.Mask(bits);

            ComplexFloat[] transformed = new ComplexFloat[data.Length];
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
                    ComplexFloat source = new ComplexFloat();
                    source = data[j];

                    FTTools.RotateAndAdd(cosMap, bits, ref source, ref tmp, i * j);
                }

                transformed[i] = tmp;
            }
            
            FTTools.CopyAndNormalize(bits, transformed, data, direction, ref tmp);
        }
    }
}
