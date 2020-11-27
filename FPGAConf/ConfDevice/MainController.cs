using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace ConfDevice
{
    [BoardConfig(Name = "Quokka")]
    public static class MainController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // IO banks for Quokka board, not needed for another boards
            FPGA.OutputSignal<bool> Bank1,

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD,

            // Servo
            FPGA.OutputSignal<bool> Servo,
            FPGA.OutputSignal<bool> ServoInf
            )
        {
            QuokkaBoard.OutputBank(Bank1);
            IsAlive.Blink(LED1);

            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            byte servoValue = 90;
            bool internalServo = false;
            FPGA.Config.Link(internalServo, Servo);
            MG996R.Continuous(servoValue, internalServo);

            byte servoInfValue = 0;
            bool internalInfServo = false;
            FPGA.Config.Link(internalInfServo, ServoInf);
            MG996R.Continuous(servoInfValue, internalInfServo);


            Sequential servoHandler = () =>
            {
                while (true)
                {
                    byte cmd = UART.Read(115200, RXD);

                    if (cmd == 1)
                    {
                        servoValue = 0;
                        FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(500));
                        servoValue = 90;
                    }
                }
            };
            FPGA.Config.OnStartup(servoHandler);

            Sequential servoInfHandler = () =>
            {
                while (true)
                {
                    servoInfValue = 0;
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1000));
                    servoInfValue = 90;
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1000));
                }
            };
            FPGA.Config.OnStartup(servoInfHandler);
        }
    }
}
