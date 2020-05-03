using System.Linq;
using Quokka.RTL;

namespace RTL.Modules
{
    public class CombinationalROMModuleInputs
    {
        public byte ReadAddress1;
        public byte ReadAddress2;

    }

    public class CombinationalROMModule : RTLCombinationalModule<CombinationalROMModuleInputs>
    {
        public static byte[] GetBuffer() 
        {
            return Enumerable.Range(0, 256).Select(b => (byte)(32 + b * 192.0 / 256.0)).ToArray();
        }

        byte[] buff = GetBuffer();

        public byte Value1 => buff[Inputs.ReadAddress1];
        public byte Value2 => buff[Inputs.ReadAddress2];
    }
}
