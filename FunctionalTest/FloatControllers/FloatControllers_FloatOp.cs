using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class FloatControllers_FloatOp
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            float f1 = 0, f2 = 0;
            FPGA.Signal<float> result = 0;
            byte fpuOp = 0;
            FPGA.Signal<bool> fpuTrigger = false, fpuCompleted = false;
            FPGA.Config.Entity<IFPU>().Op(fpuTrigger, fpuCompleted, f1, f2, fpuOp, result);

            Sequential handler = () =>
            {
                const int baud = 115200;
                
                UART.ReadFloat(baud, RXD, out f1);
                UART.ReadFloat(baud, RXD, out f2);
                UART.Read(baud, RXD, out fpuOp);

                fpuTrigger = true;

                FPGA.Runtime.WaitForAllConditions(fpuCompleted);

                UART.WriteFloat(baud, result, TXD);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
