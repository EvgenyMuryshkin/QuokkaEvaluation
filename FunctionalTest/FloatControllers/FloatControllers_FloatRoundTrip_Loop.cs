using Drivers;
using FPGA;
using FPGA.Attributes;
using FPGA.Optimizers; using Quokka.Schema.HLS;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class FloatControllers_FloatRoundTrip_Loop
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const uint baud = 115200;

            Sequential handler = () =>
            {
                FPGA.Optimizations.AddOptimizer<TestOptimizer>();

                float f1 = 0, f2 = 1.234f;

                bool internalTXD = true;

                // hardlink from register to output signal, it has to hold its value
                FPGA.Config.Link(internalTXD, TXD);

                while (true)
                {
                    UART.ReadFloat(baud, RXD, out f1);
                    UART.RegisteredWriteFloat(baud, f1, out internalTXD);
                    //UART.RegisteredWriteFloat(baud, f2, out internalTXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    public class TestOptimizer : DefaultOptimizer
    {
        public override bool? ShouldRecycle(Data data) => true;
    }
}
