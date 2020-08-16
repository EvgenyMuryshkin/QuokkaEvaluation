using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void EStage()
        {
            NextState.State = CPUState.WB;
        }
    }
}
