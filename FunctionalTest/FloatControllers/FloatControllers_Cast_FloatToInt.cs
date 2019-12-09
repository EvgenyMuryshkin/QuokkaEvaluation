using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class FloatControllers_Cast_FloatToInt
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const uint baud = 115200;

            Sequential handler = () =>
            {
                FPU.FPUCastNoSync();

                while (true)
                {
                    float data = 0;
                    UART.ReadFloat(baud, RXD, out data);

                    var floatCast = (long)data;

                    UART.WriteUnsigned64(baud, (ulong)floatCast, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
