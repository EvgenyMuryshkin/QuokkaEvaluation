using Quokka.RTL;

namespace QRV32.CPU
{
    public partial class RISCVModule : RTLSynchronousModule<RISCVModuleInputs, CPUModuleState>
    {
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
