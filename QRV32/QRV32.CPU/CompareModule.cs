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
        public bool NEQ => !internalEQ;
        public bool UGT => Inputs.Lhs.Unsigned() > Inputs.Rhs.Unsigned();
        public bool ULT => Inputs.Lhs.Unsigned() < Inputs.Rhs.Unsigned();

        public bool SGT => Inputs.Lhs.Signed() > Inputs.Rhs.Signed();
        public bool SLT => Inputs.Lhs.Signed() < Inputs.Rhs.Signed();
    }
}
