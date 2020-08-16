using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void InstructionDecodeStage()
        {
            if (Regs.Ready)
            {
                NextState.State = CPUState.EX;
            }
        }
    }
}
