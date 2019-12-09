using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class FloatControllers_Cast_IntToFloat
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
                    ulong data = 0;
                    UART.ReadUnsigned64(baud, RXD, ref data);
                    
                    var floatCast = (float)(long)data;
                    UART.WriteFloat(baud, floatCast, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
