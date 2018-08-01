using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class MG996R
    {
        public static void Continuous(FPGA.Register<byte> value, FPGA.Signal<bool> DOUT)
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, out DOUT);

            Action worker = () =>
            {
                const byte maxValue = 180;
                byte current = value < maxValue ? (byte)value : maxValue;

                internalDOUT = true;
                FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(544));
                for (byte i = 0; i < current; i++)
                {
                    // wait for 10 microseconds for each degree
                    FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(10));
                }
                internalDOUT = false;
            };
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(50), worker);
        }
    }
}
