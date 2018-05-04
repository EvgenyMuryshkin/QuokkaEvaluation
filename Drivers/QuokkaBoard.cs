using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class QuokkaBoard
    {
        public static void InputBank(FPGA.OutputSignal<bool> bank)
        {
            FPGA.Config.Default(out bank, false);
            FPGA.Config.Suppress("W0002", bank);
        }

        public static void OutputBank(FPGA.OutputSignal<bool> bank)
        {
            FPGA.Config.Default(out bank, true);
            FPGA.Config.Suppress("W0002", bank);
        }
    }
}
