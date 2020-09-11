using Quokka.RTL;
using System;

namespace RTL.Modules
{
    public class GetterModuleInputs
    {
        public byte Value;
    }

    public class BitArrayGetterModule : RTLCombinationalModule<GetterModuleInputs>
    {
        public RTLBitArray Getter
        {
            get
            {
                var result = new RTLBitArray(byte.MaxValue);

                if (Inputs.Value < 50)
                    result = (byte)42;
                else if (Inputs.Value < 100)
                    result = new RTLBitArray(Inputs.Value).Resized(8);

                return result;
            }
        }
    }

    public class NativeGetterModule : RTLCombinationalModule<GetterModuleInputs>
    {
        public byte Getter
        {
            get
            {
                var result = byte.MaxValue;

                if (Inputs.Value < 100)
                    result = Inputs.Value;

                return result;
            }
        }
    }
}
