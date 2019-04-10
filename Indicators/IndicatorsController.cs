using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Indicators
{
    [BoardConfig(Name = "Quokka")]
    public static class IndicatorsController
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
            FPGA.OutputSignal<bool> Servo
            )
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

            GameControlsState controlsState = new GameControlsState();

            IsAlive.Blink(LED1);

            Peripherals.IndicatorsControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

            LEDControl(controlsState.keyCode, DOUT, Servo);
        }

        public static void LEDControl(
            KeypadKeyCode keyCode, 
            FPGA.OutputSignal<bool> DOUT,
            FPGA.OutputSignal<bool> Servo
            )
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            byte servoValue = 0;
            MG996R.Continuous(servoValue, Servo);

            eIndicatorType indicator = eIndicatorType.None;

            object tickCounterLock = new object();
            uint msTickCounter = 0;
            uint dim = 1;
            uint flashSpeedInTicks = 1000;

            Action tickHandler = () =>
            {
                lock(tickCounterLock)
                {
                    msTickCounter++;
                }
            };
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(1), tickHandler);

            Action ledHandler = () =>
            {
                byte[] data = new byte[8];
                uint color = 0;
                bool isActive = true;
                uint indicatorSpeed = 0;

                while (true)
                {
                    switch (indicator)
                    {
                        case eIndicatorType.Left:
                            Graphics.LeftIndicator(data, dim, out color);
                            indicatorSpeed = flashSpeedInTicks;
                            break;
                        case eIndicatorType.Right:
                            Graphics.RightIndicator(data, dim, out color);
                            indicatorSpeed = flashSpeedInTicks;
                            break;
                        case eIndicatorType.Break:
                            Graphics.BreakIndicator(data, dim, out color);
                            // override indicator settings for break signal
                            indicatorSpeed = 0;
                            isActive = true;
                            break;
                        default:
                            Graphics.ClearIndicator(data, dim, out color);
                            break;
                    }

                    lock (tickCounterLock)
                    {
                        if (indicatorSpeed != 0 && msTickCounter > flashSpeedInTicks)
                        {
                            msTickCounter = 0;
                            isActive = !isActive;
                        }
                    }

                    if (!isActive)
                        color = 0;

                    Graphics.DrawIndicator(data, color, out internalDOUT);
                }
            };
            FPGA.Config.OnStartup(ledHandler);

            Action uiControlsHandler = () =>
            {
                Func<bool> noKey = () => keyCode == 0;

                switch (keyCode)
                {
                    case KeypadKeyCode.D2:
                        if (dim < 10)
                            dim++;
                        break;
                    case KeypadKeyCode.D8:
                        if (dim > 1)
                            dim--;
                        break;
                    case KeypadKeyCode.D1:
                        if (flashSpeedInTicks > 100)
                            flashSpeedInTicks -= 100;
                        break;
                    case KeypadKeyCode.D7:
                        if (flashSpeedInTicks < 2000)
                            flashSpeedInTicks += 100;
                        break;
                }

                FPGA.Runtime.WaitForAllConditions(noKey);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(50), uiControlsHandler);

            Action keypadHandler = () =>
            {
                while(true)
                {
                    switch (keyCode)
                    {
                        case KeypadKeyCode.D6:
                            indicator = eIndicatorType.Right;
                            break;
                        case KeypadKeyCode.D4:
                            indicator = eIndicatorType.Left;
                            break;
                        case KeypadKeyCode.STOP:
                            indicator = eIndicatorType.Break;
                            break;
                        case KeypadKeyCode.PWR:
                            indicator = eIndicatorType.None;
                            break;
                    }
                }
            };

            FPGA.Config.OnStartup(keypadHandler);
        }
    }
}