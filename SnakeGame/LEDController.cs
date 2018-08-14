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
			FPGA.OutputSignal<bool> Bank3,

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
			FPGA.InputSignal<bool> RXD,
			FPGA.OutputSignal<bool> TXD
			)
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);
			QuokkaBoard.OutputBank(Bank3);

            GameControlsState controlsState = new GameControlsState();

            IsAlive.Blink(LED1);

            Peripherals.GameControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

            LEDControl(controlsState.keyCode, DOUT, Servo, RXD, TXD);
        }

        public static void LEDControl(
            KeypadKeyCode keyCode, 
            FPGA.OutputSignal<bool> DOUT,
            FPGA.OutputSignal<bool> Servo,
			FPGA.InputSignal<bool> RXD,
			FPGA.OutputSignal<bool> TXD
            )
        {
			bool internalTXD = true;
			FPGA.Config.Link(internalTXD, TXD);
			object commsLock = new object();

            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            byte servoValue = 0;
			bool internalServo = false;
			FPGA.Config.Link(internalServo, Servo);
			//MG996R.Continuous(servoValue, Servo);

			Action testHandler = () =>
			{
				servoValue = servoValue == 0 ? (byte)180 : (byte)0;
				MG996R.Write(servoValue, out internalServo);
			};
			FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), testHandler);

			const uint baud = 115200;

			FPGA.SyncStream<uint> heartBeat = new FPGA.SyncStream<uint>();

			const int servosCount = 5;
			byte[] servosBuff = new byte[servosCount];

			Action servosHandler = () =>
			{
				FPGA.OutputSignal<bool> ServoItem = new FPGA.OutputSignal<bool>();
				bool servoOut = false;
				FPGA.Config.Link(servoOut, ServoItem);

				while (true)
				{
					uint idx = FPGA.Config.InstanceId();
					byte servoData = 0;
					servoData = servosBuff[idx];

					MG996R.Write(servoData, out servoOut);
				}
			};
			FPGA.Config.OnStartup(servosHandler, servosCount);


			// SERVOs driver
			Action<uint> servosDataHandler = (iteration) =>
			{
				byte data = 0;

				lock (commsLock)
				{
					UART.RegisteredWrite(baud, 2, out internalTXD);

					for (int i = 0; i < servosCount; i++)
					{
						UART.Read(baud, RXD, out data);
						servosBuff[i] = data;
					}
				}
			};
			FPGA.Config.OnStream(heartBeat, servosDataHandler);

			// LED driver
			Action<uint> ledHandler = (iteration) =>
			{
				uint data = 0;
				uint[] buff = new uint[64];

				lock(commsLock)
				{
					UART.RegisteredWrite(baud, 1, out internalTXD);

					for (int i = 0; i < buff.Length; i++)
					{
						UART.ReadUnsigned32(baud, RXD, out data);
						buff[i] = data;
					}
				}

				Graphics.Draw(buff, out internalDOUT);
			};
			FPGA.Config.OnStream(heartBeat, ledHandler);

	
			// main application driver 
			uint beat = 0;
			Action heartBeatHandler = () =>
			{
				heartBeat.Write(beat);
				beat++;
			};

			FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(100), heartBeatHandler);
        }
    }
}