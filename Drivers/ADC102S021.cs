using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class ADC102S021
    {
        public static void Read(
            out ushort IN1Value,
            out ushort IN2Value,
            FPGA.OutputSignal<bool> NCS,
            FPGA.OutputSignal<bool> SCLK,
            FPGA.OutputSignal<bool> DIN,
            FPGA.InputSignal<bool> DOUT
            )
        {
            bool internalDIN = false, internalNCS = true, internalSCLK = true;

            FPGA.Config.Link(internalDIN, DIN);
            FPGA.Config.Link(internalNCS, NCS);
            FPGA.Config.Link(internalSCLK, SCLK);

            ushort[] buff = new ushort[2];
            for(byte channel = 0; channel < 2; channel++ )
            {
                byte controlRegister = (byte)((channel & 1) << 3);
                ushort current = 0;

                Func<bool> controlMSB = () => FPGA.Config.HighBit(controlRegister);

                internalNCS = false;
                FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(1));

                for (byte i = 0; i < 16; i++)
                {
                    internalSCLK = false;
                    if (i < 8)
                    {
                        internalDIN = controlMSB();
                        controlRegister = (byte)(controlRegister << 1);
                    }

                    FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(1));

                    if (i > 3)
                    {
                        current = (ushort)(current << 1 | DOUT);
                    }

                    internalSCLK = true;
                    FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(1));
                }

                internalSCLK = true;
                internalNCS = true;

                buff[channel] = (ushort)(current << 4);
            }

            IN1Value = buff[0];
            IN2Value = buff[1];
        }
    }
}
