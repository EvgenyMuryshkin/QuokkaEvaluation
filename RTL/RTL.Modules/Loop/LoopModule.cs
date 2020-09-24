using Quokka.RTL;
using System;

namespace RTL.Modules
{
    public class LoopModuleInputs
    {
        public RTLBitArray InData { get; set; } = new RTLBitArray(byte.MinValue);
    }

    public class LoopModule : RTLCombinationalModule<LoopModuleInputs>
    {
        public bool OutOr
        {
            get
            {
                bool result = Inputs.InData[0];
                for (var idx = 1; idx < Inputs.InData.Size; idx++)
                {
                    result = result | Inputs.InData[idx];
                }
                return result;
            }
        }

        public bool OutAnd
        {
            get
            {
                bool result = true;
                for (var idx = 0; idx < Inputs.InData.Size; idx++)
                {
                    result = result & Inputs.InData[idx];
                }
                return result;
            }
        }

        public bool OutXor
        {
            get
            {
                bool result = Inputs.InData[0];
                for (var idx = 1; idx < Inputs.InData.Size; idx++)
                {
                    result = result ^ Inputs.InData[idx];
                }
                return result;
            }
        }
    }
}
