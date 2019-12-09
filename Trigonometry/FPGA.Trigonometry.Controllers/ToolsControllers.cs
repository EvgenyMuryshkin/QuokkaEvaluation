using Drivers;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace FPGA.Trigonometry.Controllers
{
    [BoardConfig(Name = "Quokka")]
    public static class NormalizeController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
    )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while(true)
                {
                    float data = 0;
                    UART.ReadFloat(115200, RXD, out data);

                    data = FPGATrigonometryTools.Normalize(data);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class ProjectController
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

                    data = FPGATrigonometryTools.Q1Project(data);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class PowController
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

                    data = FPGATrigonometryTools.Pow(data, 5);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class TaylorSinController
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

                    data = FPGATrigonometryTools.TaylorSin(data);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class TaylorCosController
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

                    data = FPGATrigonometryTools.TaylorCos(data);

                    UART.WriteFloat(115200, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
