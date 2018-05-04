using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public static class RandomValueGenerator
    {
        public static void MakeRandomValue(
            GameControlsState controlsState,
            FPGA.Register<int> randomValue)
        {
            FPGA.Register<byte> tickCounter = 0;
            Func<byte> nextTickCounter = () => (byte)(tickCounter + 1);
            Func<bool> tickCounterWE = () => controlsState.keyCode != Drivers.KeypadKeyCode.None;

            FPGA.Config.RegisterOverride(tickCounter, nextTickCounter, tickCounterWE);

            FPGA.Register<bool> randomCounterWE = true;

            Func<int> nextRandomCounter = () => tickCounter + randomValue + controlsState.adcChannel1 + controlsState.adcChannel2;
            FPGA.Config.RegisterOverride(randomValue, nextRandomCounter, randomCounterWE);
        }
    }
}
