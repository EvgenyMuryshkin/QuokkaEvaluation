using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indicators
{
    static class Peripherals
    {
        public static void IndicatorsControls(
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
            IndicatorsControlsState controlsState)
        {
            KeypadKeyCode internalCode = 0;
            FPGA.Config.Link(internalCode, out controlsState.keyCode);

            ushort adcChannel1Value = 32767, adcChannel2Value = 32767;
            uint msTickCounter = 0;

            FPGA.Config.Link(adcChannel1Value, controlsState.adcChannel1);
            FPGA.Config.Link(adcChannel2Value, controlsState.adcChannel2);
            FPGA.Config.Link(msTickCounter, controlsState.counterMs);

            Action keypadHandler = () =>
            {
                Keypad4x4.ReadASCIICode(K7, K6, K5, K4, K3, K2, K1, K0, out internalCode);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(20), keypadHandler);

            Action joystickHandler = () =>
            {
                ADC102S021.Read(out adcChannel1Value, out adcChannel2Value, ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, joystickHandler);

            Action tickHandler = () =>
            {
                msTickCounter++;
            };
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(1), tickHandler);
        }
    }
}
