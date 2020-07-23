using Quokka.RTL;

namespace QRV32.CPU
{
    public class CompareModuleInputs
    {
        public RTLBitArray Lhs = new RTLBitArray().Resized(32);
        public RTLBitArray Rhs = new RTLBitArray().Resized(32);
    }

    public class CompareModule : RTLCombinationalModule<CompareModuleInputs>
    {
        bool internalEQ => Inputs.Lhs == Inputs.Rhs;
        public bool EQ => internalEQ;
        public bool NE => !internalEQ;
        public bool GTU => Inputs.Lhs.Unsigned() > Inputs.Rhs.Unsigned();
        public bool LTU => Inputs.Lhs.Unsigned() < Inputs.Rhs.Unsigned();

        public bool GTS => Inputs.Lhs.Signed() > Inputs.Rhs.Signed();
        public bool LTS => Inputs.Lhs.Signed() < Inputs.Rhs.Signed();
    }
}
