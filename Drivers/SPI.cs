/*
 * Copyright: Evgeny Muryshkin evmuryshkin@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class SPI
    {
        public static void Write<T>(
            T data,
            FPGA.OutputSignal<bool> NCS,
            FPGA.OutputSignal<bool> SCK,
            FPGA.OutputSignal<bool> SDI
            ) where T : struct
        {
            T buff = data;
            FPGA.Config.Default(out NCS, true);
            bool internalNCS = true, internalCSK = false, internalSDI = false;

            FPGA.Config.Link(internalNCS, NCS);
            FPGA.Config.Link(internalCSK, SCK);
            FPGA.Config.Link(internalSDI, SDI);

            internalNCS = false;
            FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1));
            byte sizeOfData = FPGA.Config.SizeOf(data);
            for (int c = 0; c < sizeOfData; c++)
            {
                internalSDI = FPGA.Config.HighBit(buff);
                FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1));
                internalCSK = true;
                FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1));
                internalCSK = false;
                buff = FPGA.Config.LShift(buff, 1);
            }

            FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1));
            internalNCS = true;
        }
    }
}
