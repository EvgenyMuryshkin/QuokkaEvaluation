using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void OnSystem()
        {
            bool IsCSR = ID.SystemCode >= SystemCodes.CSRRW && ID.SystemCode <= SystemCodes.CSRRCI;

            if (ID.SystemCode == SystemCodes.E)
            {
                OnE();
            }
            else if (IsCSR)
            {
                OnCSR();
            }
            else
            {
                Halt(HaltCode.SystemCode);
            }
        }
    }
}
