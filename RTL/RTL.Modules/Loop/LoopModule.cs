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

        public byte Encode
        {
            get
            {
                byte result = 0;
                for (byte idx = 0; idx < Inputs.InData.Size; idx++)
                {
                    if (Inputs.InData[idx])
                        result = idx;
                }
                
                return result;
            }
        }

        public byte EncodeInv
        {
            get
            {
                var result = 0;
                for (var idx = Inputs.InData.Size; idx > 0; idx--)
                {
                    if (Inputs.InData[idx-1])
                        result = idx - 1;
                }
                
                return (byte)result;
            }
        }

        (byte, bool) Tuple
        {
            get
            {
                bool isValid = false;
                byte result = 0;
                for (byte idx = 0; idx < Inputs.InData.Size; idx++)
                {
                    isValid = isValid | Inputs.InData[idx];
                    if (Inputs.InData[idx])
                        result = idx;
                }
                
                return (result, isValid);
            }
        }
        public byte OutTupleAddress => Tuple.Item1;
        public bool OutTupleIsValid => Tuple.Item2;
    }
}
