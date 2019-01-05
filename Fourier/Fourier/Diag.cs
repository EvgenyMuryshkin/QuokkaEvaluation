using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fourier
{
    public enum FPUTimingType : byte
    {
        None,
        Add,
        Sub,
        Mul,
        Div
    }

    public static class Diag
    {
        [Inlined]
        public static void ClockCounter(FPGA.Register<uint> value)
        {
            Func<uint> nextClockCounter = () => value + 1;
            Func<bool> clockCounterWE = () => true;

            FPGA.Config.RegisterOverride(value, nextClockCounter, clockCounterWE);
        }
    }
}
