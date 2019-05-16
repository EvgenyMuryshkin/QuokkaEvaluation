using FPGA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class MG996R
    {
		public static void Write(byte value, out bool dout)
		{
			const byte maxValue = 180;
			byte current = value < maxValue ? (byte)value : maxValue;

			dout = true;
			FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(544));
			for (ushort i = 0; i < 1945; i++)
			{
				// wait for 10 microseconds for each degree
				FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(10));
				if (i == current) 
				{
					dout = false;	
				}
			}
		}

		public static void Continuous(FPGA.Register<byte> value, FPGA.Signal<bool> DOUT)
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, out DOUT);

            Sequential worker = () =>
            {
				while(true)
				{
					Write(value, out internalDOUT);
				}
			};
            FPGA.Config.OnStartup(worker);
        }
    }
}
