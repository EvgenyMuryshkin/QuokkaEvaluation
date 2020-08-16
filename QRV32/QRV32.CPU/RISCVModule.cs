using Quokka.RTL;

namespace QRV32.CPU
{
    public partial class RISCVModule : RTLSynchronousModule<RISCVModuleInputs, CPUModuleState>
    {
        public bool IsHalted => State.State == CPUState.Halt;

        public RTLBitArray MemWriteData => Regs.RS2;
        public RTLBitArray MemWriteMode => ID.Funct3;

        bool HasMTVEC => State.CSR[(byte)CSRAddr.mtvec] != 0;

        RTLBitArray InstructionOffset => new RTLBitArray(4).Unsigned();

        protected override void OnStage()
        {
            switch (State.State)
            {
                case CPUState.Reset:
                    ResetStage();
                    break;
                case CPUState.IF:
                    InstructionFetchStage();
                    break;
                case CPUState.ID:
                    InstructionDecodeStage();
                    break;
                case CPUState.EX:
                    ExecuteStage();
                    break;
                case CPUState.MEM:
                    MemStage();
                    break;
                case CPUState.WB:
                    WriteBackStage();
                    break;
                case CPUState.E:
                    EStage();
                    break;
            }
        }
    }
}
