using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "Quokka")]
    public static class SequentialMath_SqrtController
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
                    UART.ReadFloat(115200, RXD, out float source);
                    var result = SequentialMath.Sqrt(source);
                    UART.WriteFloat(115200, result, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class SequentialMath_SqrtInitialApproximationController
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
                    UART.ReadFloat(115200, RXD, out float source);
                    var result = SequentialMath.InitialApproximation(source);
                    UART.WriteFloat(115200, result, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
    
}
