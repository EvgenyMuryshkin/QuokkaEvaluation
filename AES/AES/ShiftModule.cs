using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace AES
{
    public class ShiftModuleInputs
    {
        public RTLBitArray Value = new RTLBitArray().Resized(128);
    }

    public class ShiftModule : RTLCombinationalModule<ShiftModuleInputs>
    {
        public RTLBitArray Result
        {
            get
            {
                var row3 = Inputs.Value[127, 96];
                var row2 = Inputs.Value[95, 64];
                var row1 = Inputs.Value[63, 32];
                var row0 = Inputs.Value[31, 0];

                return new RTLBitArray(
                    new RTLBitArray(row3[7, 0], row3[31, 8]),
                    new RTLBitArray(row2[7, 0], row2[31, 8]),
                    new RTLBitArray(row1[7, 0], row1[31, 8]),
                    new RTLBitArray(row0[7, 0], row0[31, 8])
                    );
            }
        }
    }
}
