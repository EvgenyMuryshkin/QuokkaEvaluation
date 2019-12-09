using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class FloatControllers_Cast_Inline
    {
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

                    float[] buff = new float[2];

                    for (sbyte idx = -10; idx <= 10; idx++)
                    {
                        buff[0] = data * idx;
                        buff[1] = idx * data;

                        for (var i = 0; i < buff.Length; i++)
                        {
                            var tmp = buff[i];
                            UART.WriteFloat(baud, tmp, TXD);
                        }
                    }
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
