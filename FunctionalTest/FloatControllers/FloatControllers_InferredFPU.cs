using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class FloatControllers_InferredFPU
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int baud = 115200;
                byte command = 0;

                float op1, op2, res = 0;

                while(true)
                {
                    UART.ReadFloat(baud, RXD, out op1);
                    UART.ReadFloat(baud, RXD, out op2);
                    command = UART.Read(baud, RXD);

                    FloatControllersOps.TestHandler(op1, op2, command, out res);

                    UART.WriteFloat(baud, res, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
