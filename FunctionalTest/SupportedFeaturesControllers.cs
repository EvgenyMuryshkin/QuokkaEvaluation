using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class SFC_FunctionCallController
    {
        static byte Increment(byte input)
        {
            return (byte)(input + 1);
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                while(true)
                {
                    byte data = UART.Read(115200, RXD);
                    switch (data)
                    {
                        case 0:
                            data = Increment(data);
                            break;
                        case 1:
                            var result = Increment(data);
                            data = result;
                            break;
                    }
                    UART.Write(115200, data, TXD);
                }
            };
            FPGA.Config.OnStartup(handler);
        }
    }
}
