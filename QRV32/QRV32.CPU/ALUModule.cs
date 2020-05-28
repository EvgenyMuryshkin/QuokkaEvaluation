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
        RTLBitArray Sum => Inputs.Op1.Signed() + Inputs.Op2.Signed();

        public RTLBitArray ADD => Sum[31, 0];
        public bool ADDOverflow => Sum[32];

        public RTLBitArray resAND => Inputs.Op1 & Inputs.Op2;
        public RTLBitArray resOR => Inputs.Op1 | Inputs.Op2;
        public RTLBitArray resXOR => Inputs.Op1 ^ Inputs.Op2;

        public RTLBitArray SHLL => (Inputs.Op1 << Inputs.SHAMT)[31, 0];
        public RTLBitArray SHRL => (Inputs.Op1.Unsigned() >> Inputs.SHAMT)[31, 0];
        public RTLBitArray SHRA => (Inputs.Op1.Signed() >> Inputs.SHAMT)[31, 0];

    }
}
