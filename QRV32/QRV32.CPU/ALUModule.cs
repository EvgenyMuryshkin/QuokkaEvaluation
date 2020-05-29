using Quokka.RTL;

namespace QRV32.CPU
{
    public class ALUModuleInputs
    {
        public RTLBitArray Op1 = new RTLBitArray().Resized(32);
        public RTLBitArray Op2 = new RTLBitArray().Resized(32);

        public RTLBitArray SHAMT = new RTLBitArray().Resized(5);
    }

    public class ALUModule : RTLCombinationalModule<ALUModuleInputs>
    {
        protected RTLBitArray internalAdd => Inputs.Op1.Signed() + Inputs.Op2.Signed();
        protected RTLBitArray internalSub => Inputs.Op1.Signed().Resized(33) - Inputs.Op2.Signed().Resized(33);

        public RTLBitArray ADD => internalAdd[31, 0];
        public bool ADDOverflow => internalAdd[32];
        public RTLBitArray SUB => internalSub[31, 0];
        public bool SUBUnderflow => internalSub[32];

        public RTLBitArray resAND => Inputs.Op1 & Inputs.Op2;
        public RTLBitArray resOR => Inputs.Op1 | Inputs.Op2;
        public RTLBitArray resXOR => Inputs.Op1 ^ Inputs.Op2;

        public RTLBitArray SHLL => (Inputs.Op1 << Inputs.SHAMT)[31, 0];
        public RTLBitArray SHRL => (Inputs.Op1.Unsigned() >> Inputs.SHAMT)[31, 0];
        public RTLBitArray SHRA => (Inputs.Op1.Signed() >> Inputs.SHAMT)[31, 0];

    }
}
