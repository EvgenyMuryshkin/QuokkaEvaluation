using Drivers;
using FPGA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indicators
{
    static class Peripherals
    {
        public static void IndicatorsControls(
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
            Sequential keypadHandler = () =>
            {
                Keypad4x4.ReadASCIICode(K7, K6, K5, K4, K3, K2, K1, K0, out controlsState.keyCode);
            };
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(20), keypadHandler);

            Sequential tickHandler = () =>
            {
                controlsState.counterMs++;
            };
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(1), tickHandler);
        }
    }
}
