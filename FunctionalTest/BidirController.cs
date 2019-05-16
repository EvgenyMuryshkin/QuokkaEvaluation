using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Runtime_BidirController
    {
        public static async Task Aggregator(
            FPGA.BidirSignal<bool> bidir1,
            FPGA.BidirSignal<bool> bidir2
            )
        {
            const bool trigger = true;
            Sequential handler = () =>
            {
                bidir1 = false;
                bidir2 = false;

                FPGA.Runtime.SetBidirAsOutput(bidir1);
                FPGA.Runtime.SetBidirAsInput(bidir2);

                bidir1 = true;
                bidir1 = false;
                bidir1 = bidir2;

                FPGA.Runtime.SetBidirAsInput(bidir1);
                FPGA.Runtime.SetBidirAsInput(bidir2);
            };
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
