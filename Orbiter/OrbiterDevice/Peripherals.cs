using Drivers;
using OrbiterDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrbiterDevice
{
    static class Peripherals
    {
        public static void Controls(
            // ADC
            FPGA.OutputSignal<bool> ADC1NCS,
            FPGA.OutputSignal<bool> ADC1SLCK,
            FPGA.OutputSignal<bool> ADC1DIN,
            FPGA.InputSignal<bool> ADC1DOUT,

            // keypad
            FPGA.OutputSignal<bool> K7,
            FPGA.OutputSignal<bool> K6,
            FPGA.OutputSignal<bool> K5,
            FPGA.OutputSignal<bool> K4,
            FPGA.InputSignal<bool> K3,
            FPGA.InputSignal<bool> K2,
            FPGA.InputSignal<bool> K1,
            FPGA.InputSignal<bool> K0,
            ref KeysDTO controlsState)
        {
            KeypadKeyCode internalCode = 0;
            FPGA.Config.Link(internalCode, out controlsState.KeyCode);

            ushort adcChannel1Value = 32767, adcChannel2Value = 32767;

            FPGA.Config.Link(adcChannel1Value, controlsState.X);
            FPGA.Config.Link(adcChannel2Value, controlsState.Y);

            Action keypadHandler = () =>
            {
                Keypad4x4.ReadASCIICode(K7, K6, K5, K4, K3, K2, K1, K0, out internalCode);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(20), keypadHandler);

            Action joystickHandler = () =>
            {
				while(true)
				{
					ADC102S021.Read(out adcChannel1Value, out adcChannel2Value, ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT);
				}
			};

            FPGA.Config.OnStartup(joystickHandler);
        }
    }
}
