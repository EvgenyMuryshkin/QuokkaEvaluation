using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace Tutorials
{
    [BoardConfig(Name = "Quokka")]
    public static class T01UARTController
    {
        public static async Task Aggregator(
            OutputSignal<bool> LED1,
            OutputSignal<bool> TXD)
        {
            IsAlive.Blink(LED1);

            Sequential handler = () =>
            {
                byte[] data = TutorialsDataSource.HelloWorldBytes();

                while (true)
                {
                    for (byte i = 0; i < data.Length; i++)
                    {
                        byte b = data[i];
                        UART.Write(115200, b, TXD);
                    }
                    Runtime.Delay(TimeSpan.FromSeconds(1));
                }
            };

            Config.OnStartup(handler);
        }
    }
}
