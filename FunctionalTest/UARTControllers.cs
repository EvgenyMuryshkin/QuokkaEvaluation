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

    [BoardConfig(Name = "NEB")]
    public static class UART_RoundTripController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            DTOs.RoundTrip request = new DTOs.RoundTrip();
            FPGA.Signal<bool> deserialized = new FPGA.Signal<bool>();
            Drivers.JSON.DeserializeFromUART<DTOs.RoundTrip>(request, RXD, deserialized);

            Sequential processingHandler = () =>
            {
                Drivers.JSON.SerializeToUART<DTOs.RoundTrip>(request, TXD);
            };

            FPGA.Config.OnSignal(deserialized, processingHandler);
        }
    }
}
