using Drivers;
using FPGA;
using FPGA.Attributes;
using OrbiterDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrbiterDevice
{
    [BoardConfig(Name = "Test")]
    [BoardConfig(Name = "Quokka")]
    public static class OrbiterController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,
			FPGA.OutputSignal<bool> LED2,

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

            // ADC
            FPGA.OutputSignal<bool> ADC1NCS,
            FPGA.OutputSignal<bool> ADC1SLCK,
            FPGA.OutputSignal<bool> ADC1DIN,
            FPGA.InputSignal<bool> ADC1DOUT,

			// UART
			FPGA.InputSignal<bool> RXD,
			FPGA.OutputSignal<bool> TXD
			)
		{
			IsAlive.Blink(LED1);

			QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

			KeysDTO controlsState = new KeysDTO();

            Peripherals.Controls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

			ConfigureReporting(controlsState, TXD, LED2);
		}

		public static void ConfigureReporting(
			KeysDTO controlsState,
			FPGA.Signal<bool> TXD,
			FPGA.Signal<bool> LED)
		{
            Sequential reportHandler = () =>
			{
				FPGA.Config.Suppress("W0002", LED);
				/*
				bool internalTXD = true;
				Func<bool> inv = () => !internalTXD;
				
				FPGA.Config.Link(internalTXD, TXD);
				FPGA.Config.Link(inv, LED);
				*/
				while (true)
				{
					//UART.RegisteredWrite(115200, 64, out internalTXD);
					FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(20));

					JSON.SerializeToUART(ref controlsState, TXD);
				}
			};

			FPGA.Config.OnStartup(reportHandler);
		}
    }
}