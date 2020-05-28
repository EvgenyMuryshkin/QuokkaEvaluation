using Quokka.RTL;

namespace RTL.Modules
{
    public class ShifterInputs
    {
        public RTLBitArray Value = new RTLBitArray().Resized(8);
        public RTLBitArray ShiftBy = new RTLBitArray().Resized(3);
    }

    public class ShifterModule : RTLCombinationalModule<ShifterInputs>
    {
        public RTLBitArray SHLL => Inputs.Value << Inputs.ShiftBy;
        public RTLBitArray SHRL => Inputs.Value.Unsigned() >> Inputs.ShiftBy;
        public RTLBitArray SHRA => Inputs.Value.Signed() >> Inputs.ShiftBy;
    }
}
