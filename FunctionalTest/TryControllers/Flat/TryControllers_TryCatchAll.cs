using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class TryControllers_TryCatchAll
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            TryControllers_Blocks.Bootstrap(
                RXD,
                TXD,
                Controllers.TryControllers_Blocks.TryCatchAll);
        }
    }
}
