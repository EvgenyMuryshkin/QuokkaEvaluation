using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    [BoardConfig(Name = "Quokka")]
    public static class LEDController
	{
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // keypad
            FPGA.OutputSignal<bool> K7,
            FPGA.OutputSignal<bool> K6,
            FPGA.OutputSignal<bool> K5,
            FPGA.OutputSignal<bool> K4,
            FPGA.InputSignal<bool> K3,
            FPGA.InputSignal<bool> K2,
            FPGA.InputSignal<bool> K1,
            FPGA.InputSignal<bool> K0,

            // banks 
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,

            // WS2812
            FPGA.OutputSignal<bool> DOUT,

            // ADC
            FPGA.OutputSignal<bool> ADC1NCS,
            FPGA.OutputSignal<bool> ADC1SLCK,
            FPGA.OutputSignal<bool> ADC1DIN,
            FPGA.InputSignal<bool> ADC1DOUT,

            // SERVO
            FPGA.OutputSignal<bool> Servo,
			
			// UART
			FPGA.InputSignal<bool> RXD
			//FPGA.OutputSignal<bool> TXD
			)
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

            GameControlsState controlsState = new GameControlsState();

            IsAlive.Blink(LED1);

            Peripherals.GameControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

            LEDControl(controlsState.keyCode, DOUT, Servo, RXD);
        }

        public static void LEDControl(
            KeypadKeyCode keyCode, 
            FPGA.OutputSignal<bool> DOUT,
            FPGA.OutputSignal<bool> Servo,
			FPGA.InputSignal<bool> RXD
            )
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            byte servoValue = 0;
            MG996R.Continuous(servoValue, Servo);

            uint[] buff = new uint[64];

            Action ledHandler = () =>
            {
				uint data = 0;
				while(true)
				{
					for (int i = 0; i < buff.Length; i++) 
					{
						UART.ReadUnsigned32(115200, RXD, out data);
						buff[i] = data;
					}
					Graphics.Draw(buff, out internalDOUT);
				}
            };

            FPGA.Config.OnStartup(ledHandler);
        }
    }
}