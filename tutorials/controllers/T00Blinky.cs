using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace Tutorials
{
    [BoardConfig(Name = "Quokka")]
    public static class T00BlinkyController
    {
        public static async Task Aggregator(OutputSignal<bool> LED1)
        {
            bool internalAlive = false;
            Config.Link(internalAlive, LED1);

            Sequential aliveHandler = () =>
            {
                internalAlive = !internalAlive;
            };

            Config.OnTimer(TimeSpan.FromSeconds(1), aliveHandler);
        }
    }
}
