using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void ExecuteStage()
        {
            NextState.State = CPUState.WB;
            NextState.WBDataReady = false;
            NextState.PCOffset = InstructionOffset;

            switch (ID.OpTypeCode)
            {
                case OpTypeCodes.OPIMM:
                    OnOPIMM();
                    break;
                case OpTypeCodes.OP:
                    OnOP();
                    break;
                case OpTypeCodes.B:
                    OnBranch();
                    break;
                case OpTypeCodes.LUI:
                    OnLUI();
                    break;
                case OpTypeCodes.AUIPC:
                    OnAUIPC();
                    break;
                case OpTypeCodes.JAL:
                    OnJAL();
                    break;
                case OpTypeCodes.JALR:
                    OnJALR();
                    break;
                case OpTypeCodes.LOAD:
                    OnLoadStore();
                    break;
                case OpTypeCodes.STORE:
                    OnLoadStore();
                    break;
                case OpTypeCodes.SYSTEM:
                    OnSystem();
                    break;
                default:
                    Halt(HaltCode.OpTypeCode);
                    break;
            }
        }
    }
}
