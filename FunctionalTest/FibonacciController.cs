using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Math_Fibonacci
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                while (true)
                {
                    byte start = 0;
                    UART.Read(115200, RXD, out start);

                    ulong result = 0;
                    SequentialMath.Calculators.Fibonacci(start, out result);

                    for(byte i = 0; i < 8; i++ )
                    {
                        byte data = (byte)result;
                        UART.Write(115200, data, TXD);
                        result = result >> 8;
                    }
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
