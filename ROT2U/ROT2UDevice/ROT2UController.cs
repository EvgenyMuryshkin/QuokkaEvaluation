using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    [BoardConfig(Name = "Quokka")]
    public static class ROT2UController
	{
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // banks 
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,
			FPGA.OutputSignal<bool> Bank3,

			// WS2812
			FPGA.OutputSignal<bool> DOUT,
			
			// UART
			FPGA.InputSignal<bool> RXD,
			FPGA.OutputSignal<bool> TXD

			// SERVO IO is generated inside handlers
			)
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);
			QuokkaBoard.OutputBank(Bank3);

            IsAlive.Blink(LED1);

            DeviceControl(DOUT, RXD, TXD);
        }

        public static void DeviceControl(
            FPGA.OutputSignal<bool> DOUT,
			FPGA.InputSignal<bool> RXD,
			FPGA.OutputSignal<bool> TXD
            )
        {
			bool internalTXD = true;
			FPGA.Config.Link(internalTXD, TXD);
			object commsLock = new object();

            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

			const uint baud = 115200;

			FPGA.SyncStream<uint> heartBeat = new FPGA.SyncStream<uint>();

			const int servosCount = 5;
			byte[] servosBuff = new byte[servosCount];

            Sequential servosHandler = () =>
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
            Sequential<uint> servosDataHandler = (iteration) =>
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
            Sequential<uint> ledHandler = (iteration) =>
			{
				uint data = 0;
				uint[] buff = new uint[64];

				lock(commsLock)
				{
					UART.RegisteredWrite(baud, 1, out internalTXD);

					for (int i = 0; i < buff.Length; i++)
					{
						UART.ReadUnsigned32(baud, RXD, ref data);
						buff[i] = data;
					}
				}

				WS2812B.SyncWrite(buff, 0, buff.Length, out internalDOUT);
			};
			FPGA.Config.OnStream(heartBeat, ledHandler);

	
			// main application driver 
			uint beat = 0;
            Sequential heartBeatHandler = () =>
			{
				heartBeat.Write(beat);
				beat++;
			};

			FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(100), heartBeatHandler);
        }
    }
}