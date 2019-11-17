using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VRCamera
{
    [BoardConfig(Name = "Quokka")]
    [BoardConfig(Name = "Test")]
    public static class MainController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // banks 
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,
            FPGA.OutputSignal<bool> Bank5,

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);
            QuokkaBoard.OutputBank(Bank5);
            IsAlive.Blink(LED1);

            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            const int servosCount = 3;
            byte[] servosData = new byte[servosCount];

            Sequential servoHandler = () =>
            {
                uint instanceId = FPGA.Config.InstanceId();
                var servoOutputPin = new FPGA.OutputSignal<bool>();
                byte value = 0;
                bool servoOutput = false;
                byte requestValue = 0;

                FPGA.Config.Link(servoOutput, servoOutputPin);

                while (true)
                {
                    requestValue = servosData[instanceId];

                    if (requestValue != value)
                    {
                        if (requestValue < value)
                        {
                            value--;
                        }
                        else
                        {
                            value++;
                        }
                    }

                    MG996R.Write(value, out servoOutput);
                }
            };

            FPGA.Config.OnStartup(servoHandler, servosCount);

            Sequential readHandler = () =>
            {
                byte data = 0;
                byte counter = 0;
                while (true)
                {
                    UART.Read(115200, RXD, out data);
                    if (data == 255 || counter == 255)
                    {
                        // begin of packet or overflow
                        counter = 0;
                    }
                    else
                    {
                        if (counter < servosCount)
                        {
                            servosData[counter] = data;
                        }

                        counter++;
                    }
                }
            };

            FPGA.Config.OnStartup(readHandler);
        }
    }
}