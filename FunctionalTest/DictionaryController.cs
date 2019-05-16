using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Runtime_DictionaryController
    {
        public static void Lookpup(byte key, out byte value)
        {
            FPGA.Collections.ReadOnlyDictionary<byte, byte> items = new FPGA.Collections.ReadOnlyDictionary<byte, byte>()
            {
                { 0, 1 },
                { 1, 2 },
                { 2, 4 },
                { 3, 10 },
                { 4, 15 }
            };

            value = items[key];
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const bool trigger = true;

            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                Lookpup(data, out result);

                UART.Write(115200, result, TXD);
            };
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
