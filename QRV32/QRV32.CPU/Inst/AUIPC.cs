using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void OnAUIPC()
        {
            NextState.WBDataReady = true;
            NextState.WBData = State.PC + ID.UTypeImm;
        }
    }
}
