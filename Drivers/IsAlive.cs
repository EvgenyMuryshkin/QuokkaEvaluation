using FPGA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class IsAlive
    {
        public static void Blink(FPGA.OutputSignal<bool> LED)
        {
            bool internalAlive = false;
            FPGA.Config.Link(internalAlive, LED);

            Sequential aliveHandler = () =>
            {
                internalAlive = !internalAlive;
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), aliveHandler);
        }
    }
}
