using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class FloatControllers_Cast_Return_Expression
    {
        public static float ReturnExpression(float value)
        {
            return value * 5;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const uint baud = 115200;

            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    float data = 0;
                    UART.ReadFloat(baud, RXD, out data);

                    var result = (long)ReturnExpression(data);
                    UART.WriteUnsigned64(baud, (ulong)result, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
