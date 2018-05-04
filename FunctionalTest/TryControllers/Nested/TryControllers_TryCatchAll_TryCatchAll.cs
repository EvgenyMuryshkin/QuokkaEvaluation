using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class TryControllers_TryCatchAll_TryCatchAll
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            TryControllers_Blocks.Bootstrap(
                RXD,
                TXD,
                Controllers.TryControllers_Blocks.TryCatchAll_TryCatchAll);
        }
    }
}
