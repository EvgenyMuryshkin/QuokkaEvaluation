using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace Tutorials
{
    [BoardConfig(Name = "Quokka")]
    public static class T01SharedUARTController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> LED1,
            FPGA.OutputSignal<bool> TXD)
        {
            IsAlive.Blink(LED1);

            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);
            object txdLock = new object();

            Sequential handler = () =>
            {
                Const<uint> instanceId = FPGA.Config.InstanceId();
                byte[] data = TutorialsDataSource.HelloWorldBytesFromHandler(instanceId);

                while (true)
                {
                    lock (txdLock)
                    {
                        for (byte i = 0; i < data.Length; i++)
                        {
                            byte b = data[i];
                            UART.RegisteredWrite(115200, b, out internalTXD);
                        }
                    }

                    Runtime.Delay(TimeSpan.FromSeconds(1));
                }
            };

            Config.OnStartup(handler, 3);
        }
    }
}
