using Quokka.RTL;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        RTLBitArray NextSequentialPC => State.PC + InstructionOffset;

        void OnJAL()
        {
            NextState.WBDataReady = true;
            NextState.WBData = NextSequentialPC;
            NextState.PCOffset = ID.JTypeImm;
        }

        void OnJALR()
        {
            NextState.WBDataReady = true;
            NextState.WBData = NextSequentialPC;
            NextState.PCOffset = new RTLBitArray(new RTLBitArray(Regs.RS1 + ID.ITypeImm)[31, 1], false);
        }
    }
}
