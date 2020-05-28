using Quokka.RTL;

namespace QRV32.CPU
{
    public class ALUModuleInputs
    {
        public RTLBitArray Op1 = new RTLBitArray().Resized(32);
        public RTLBitArray Op2 = new RTLBitArray().Resized(32);
    }

    public class ALUModule : RTLCombinationalModule<ALUModuleInputs>
    {
        RTLBitArray Sum => Inputs.Op1.Signed() + Inputs.Op2.Signed();

        public RTLBitArray ADD => Sum[31, 0];
        public bool ADDOverflow => Sum[32];

        public RTLBitArray AND => Inputs.Op1 & Inputs.Op2;
        public RTLBitArray OR => Inputs.Op1 | Inputs.Op2;
        public RTLBitArray XOR => Inputs.Op1 ^ Inputs.Op2;
    }
}
