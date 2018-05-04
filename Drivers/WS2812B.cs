using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class WS2812B
    {
        public static void SyncWrite(
            uint[] rgb, 
            uint offset,
            int length,
            out bool DOUT)
        {
            FPGA.Config.Default(out DOUT, false);
            uint pixel = 0;

            for (uint addr = 0; addr < length; addr++)
            {
                Func<uint> actualAddr = () => addr + offset;
                pixel = rgb[actualAddr()];

                uint ordered = (uint)(
                    ((byte)(pixel >> 8) << 16) | // green
                    ((byte)(pixel >> 16) << 8) | // red
                    (byte)(pixel)
                    );

                Func<bool> bitValue = () => FPGA.Config.Bit(ordered, 23);

                for (byte bit = 0; bit < 24; bit++)
                {
                    if ( bitValue() )
                    {
                        DOUT = true;
                        FPGA.Runtime.Delay(TimeSpanEx.FromNanoseconds(700));
                        DOUT = false;
                        FPGA.Runtime.Delay(TimeSpanEx.FromNanoseconds(600));
                    }
                    else
                    {
                        DOUT = true;
                        FPGA.Runtime.Delay(TimeSpanEx.FromNanoseconds(350));
                        DOUT = false;
                        FPGA.Runtime.Delay(TimeSpanEx.FromNanoseconds(800));
                    }

                    ordered = ordered << 1;
                }
            }

            FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(50));
        }
    }
}
