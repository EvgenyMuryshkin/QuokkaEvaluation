using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class FloatControllers_FloatRoundTrip
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const uint baud = 115200;

            var stream = new FPGA.SyncStream<float>();

            Sequential<float> streamHandler = (data) =>
            {
                UART.WriteFloat(baud, data, TXD);
            };
            FPGA.Config.OnStream(stream, streamHandler);

            Sequential handler = () =>
            {
                float f1 = 0, f2 = 1.234f;
                
                UART.ReadFloat(baud, RXD, out f1);
                stream.Write(f1);
                stream.Write(f2);
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
