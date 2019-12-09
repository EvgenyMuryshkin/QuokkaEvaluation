using Drivers;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace FPGA.Trigonometry.Controllers
{
    [BoardConfig(Name = "Quokka")]
    public static class CosController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
    )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    float data = 0;
                    UART.ReadFloat(115200, RXD, out data);

                    data = FPGATrigonometry.Cos(data);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class SinController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
    )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    float data = 0;
                    UART.ReadFloat(115200, RXD, out data);

                    data = FPGATrigonometry.Sin(data);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
