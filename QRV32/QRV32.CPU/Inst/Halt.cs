﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void Halt()
        {
            // TODO: add halt reasong as parameter and support in translator
            NextState.State = CPUState.Halt;

            // calls to Debugger and Trace are not translated into HDL.
            Trace.WriteLine($"CPU halted");
            Debugger.Break();
        }
    }
}
