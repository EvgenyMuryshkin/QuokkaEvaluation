using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Runtime_ResetSequenceController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            object testLock = new object();
            byte data = 0;
            Sequential dataHandler = () => 
            {
                UART.Write(115200, data, TXD);
            };
            FPGA.Config.OnRegisterWritten(data, dataHandler);

            Sequential mainHandler1 = () =>
            {
                lock(testLock)
                {
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(5));
                    data = 48;
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(20));
                    data = 49;
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(5));
                }
            };

            Sequential mainHandler2 = () =>
            {
                lock (testLock)
                {
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(5));
                    data = 50;
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(20));
                    data = 51;
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(5));
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler1);
            FPGA.Config.OnSignal(trigger, mainHandler2);

            Sequential resetHandler1 = () =>
            {
                FPGA.Runtime.ResetSequence(mainHandler1);
            };

            Sequential resetHandler2 = () =>
            {
                FPGA.Runtime.ResetSequence(mainHandler2);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(10), resetHandler1);
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(19), resetHandler2);
        }
    }
}
