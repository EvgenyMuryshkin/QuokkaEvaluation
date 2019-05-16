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
    public static class JSON_Serializer
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            byte data = 0;

            Sequential processingHandler = () =>
            {
                DTOs.RoundTrip response = new DTOs.RoundTrip();
                response.b = data;
                Drivers.JSON.SerializeToUART<DTOs.RoundTrip>(response, TXD);
            };

            FPGA.Config.OnRegisterWritten(data, processingHandler);

            Sequential deserializeHandler = () =>
            {
                UART.Read(115200, RXD, out data);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, deserializeHandler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class JSON_Deserializer
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
                UART.Write(115200, request.b, TXD);
            };

            FPGA.Config.OnSignal(deserialized, processingHandler);
        }
    }
}
