using Drivers;
using FPGA;
using FPGA.Attributes;using FPGA.Optimizers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class UART_MaxSpeed
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            byte data = 0;
            FPGA.Signal<bool> completed = false;

            Sequential dataHandler = () => 
            {
                UART.Write(115200, data, TXD);
                completed = true;
            };
            FPGA.Config.OnRegisterWritten(data, dataHandler);

            Sequential mainHandler = () =>
            {
                data = 48;
                FPGA.Runtime.WaitForAllConditions(completed);
                data = 49;
                FPGA.Runtime.WaitForAllConditions(completed);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class UART_RoundTripController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            DTOs.RoundTrip request = new DTOs.RoundTrip();
            FPGA.Signal<bool> deserialized = new FPGA.Signal<bool>();
            Drivers.JSON.DeserializeFromUART<DTOs.RoundTrip>(ref request, RXD, deserialized);

            Sequential processingHandler = () =>
            {
                Drivers.JSON.SerializeToUART<DTOs.RoundTrip>(ref request, TXD);
            };

            FPGA.Config.OnSignal(deserialized, processingHandler);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class UART_ByteRoundTripController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                const uint baud = 115200;

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();
                    byte data = UART.Read(115200, RXD);
                    UART.Write(baud, data, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
