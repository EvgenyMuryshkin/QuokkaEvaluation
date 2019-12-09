using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class FloatControllers_Compare
    {
        public static byte Compare(float op1, float op2)
        {
            var greater = op1 > op2;
            var greaterOrEqual = op1 >= op2;
            var less = op1 < op2;
            var lessOrEqual = op1 <= op2;
            var equal = op1 == op2;
            var notEqual = op1 != op2;

            byte result = (byte)(
                ((greater ? 1 : 0) << 5) | 
                ((greaterOrEqual ? 1 : 0) << 4) |
                ((less ? 1 : 0) << 3) |
                ((lessOrEqual ? 1 : 0) << 2) |
                ((equal ? 1 : 0) << 1) |
                (notEqual ? 1 : 0)
                );

            return result;
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
                    float op1 = 0, op2 = 0;
                    UART.ReadFloat(baud, RXD, out op1);
                    UART.ReadFloat(baud, RXD, out op2);

                    var result = Compare(op1, op2);

                    UART.Write(baud, result, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
