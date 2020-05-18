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
        RTLBitArray Sum => Inputs.Op1 + Inputs.Op2;

        public RTLBitArray ADD => Sum.Resized(32);
        public bool ADDOverflow => Sum[32];
    }
}
