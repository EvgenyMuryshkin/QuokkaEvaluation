using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void OnLUI()
        {
            NextState.WBDataReady = true;
            NextState.WBData = ID.UTypeImm;
        }
    }
}
