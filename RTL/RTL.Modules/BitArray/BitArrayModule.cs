using Quokka.RTL;
using System;

namespace RTL.Modules
{
    public class BitArrayInputs
    {
        public byte Value;
    }

    public class BitArrayModule : RTLCombinationalModule<BitArrayInputs>
    {
        RTLBitArray Bits => Inputs.Value;
        public RTLBitArray Direct => Bits;
        public RTLBitArray High => Bits[7, 4];
        public RTLBitArray Low => Bits[3, 0];
        public RTLBitArray Reversed => Bits[0, 7];
        public RTLBitArray ReversedHigh => Bits[4, 7];
        public RTLBitArray ReversedLow => Bits[0, 3];
        public RTLBitArray Picks => new RTLBitArray(Bits[6,5], Bits[0,1]);
        public RTLBitArray FromBits1 => new RTLBitArray(true, true, false, true);
        public RTLBitArray FromBits2 => new RTLBitArray(false, true, true, true);

    }
}
