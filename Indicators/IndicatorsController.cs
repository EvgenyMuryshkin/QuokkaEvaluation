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

            IndicatorsControlsState controlsState = new IndicatorsControlsState();

            IsAlive.Blink(LED1);

            Peripherals.IndicatorsControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

            LEDControl(
                controlsState,
                DOUT, 
                Servo);
        }

        public static void LEDControl(
            IndicatorsControlsState controlState,
            FPGA.OutputSignal<bool> DOUT,
            FPGA.OutputSignal<bool> Servo
            )
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            byte servoValue = 0;
            MG996R.Continuous(servoValue, Servo);

            object indicatorLock = new object();
            eIndicatorType indicator = eIndicatorType.None;
            uint indicatorKeyEventTimeStamp = 0;

            uint dim = 1;
            uint flashSpeedInTicks = 500;
            Action uiControlsHandler = () =>
            {
                Func<bool> noKey = () => controlState.keyCode == 0;

                switch (controlState.keyCode)
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

            Action ledHandler = () =>
            {
                byte[] data = new byte[8];
                uint indicatorColor = 0;
                bool isIndicatorActive = true;

                uint flashIndicatorSpeed = 0;
                uint flashIndicatorTimeStamp = 0;

                eIndicatorType lastIndicator = eIndicatorType.None;
                uint lastIndicatorTimeStamp = 0;

                uint autoIndicatorTimeStamp = 0;

                while (true)
                {
                    lock(indicatorLock)
                    {
                        if (lastIndicator != indicator)
                        {
                            if (autoIndicatorTimeStamp != 0)
                            {
                                if (indicator == eIndicatorType.None)
                                {
                                    if (controlState.msTickCounter - autoIndicatorTimeStamp > 3000)
                                    {
                                        // turn off auto indicator after 3 seconds
                                        autoIndicatorTimeStamp = 0;
                                    }
                                }
                                else
                                {
                                    // different indicator
                                    lastIndicator = indicator;
                                    autoIndicatorTimeStamp = controlState.msTickCounter;
                                    lastIndicatorTimeStamp = controlState.msTickCounter;

                                    flashIndicatorTimeStamp = 0;
                                    isIndicatorActive = false;
                                    indicatorColor = 0;
                                }
                            }
                            else
                            {
                                // turn on auto indicator if key was pressed less then 500 ms
                                if (indicator == eIndicatorType.None && (controlState.msTickCounter - lastIndicatorTimeStamp) < 500)
                                {
                                    autoIndicatorTimeStamp = controlState.msTickCounter;
                                    lastIndicatorTimeStamp = controlState.msTickCounter;
                                }
                                else
                                {
                                    lastIndicator = indicator;
                                    lastIndicatorTimeStamp = controlState.msTickCounter;

                                    flashIndicatorTimeStamp = 0;
                                    isIndicatorActive = false;
                                    indicatorColor = 0;
                                }
                            }
                        }
                    }

                    switch (lastIndicator)
                    {
                        case eIndicatorType.Left:
                            Graphics.LeftIndicator(data, dim, out indicatorColor);
                            flashIndicatorSpeed = flashSpeedInTicks;
                            break;
                        case eIndicatorType.Right:
                            Graphics.RightIndicator(data, dim, out indicatorColor);
                            flashIndicatorSpeed = flashSpeedInTicks;
                            break;
                        case eIndicatorType.Break:
                            Graphics.BreakIndicator(data, dim, out indicatorColor);
                            // override indicator settings for break signal
                            flashIndicatorSpeed = 0;
                            isIndicatorActive = true;
                            break;
                        default:
                            // reset state when nothing is selected, so first event will be displayed immediately
                            Graphics.ClearIndicator(data, dim, out indicatorColor);
                            break;
                    }

                    if (flashIndicatorSpeed != 0 && (controlState.msTickCounter - flashIndicatorTimeStamp) > flashIndicatorSpeed)
                    {
                        flashIndicatorTimeStamp = controlState.msTickCounter;
                        isIndicatorActive = !isIndicatorActive;
                    }

                    if (!isIndicatorActive)
                        indicatorColor = 0;

                    Graphics.DrawIndicator(data, indicatorColor, out internalDOUT);
                }
            };
            FPGA.Config.OnStartup(ledHandler);

            Action keypadHandler = () =>
            {
                while(true)
                {
                    uint keyEventTimeStamp = controlState.msTickCounter;
                    eIndicatorType nextIndicator = indicator;

                    switch (controlState.keyCode)
                    {
                        case KeypadKeyCode.D6:
                            nextIndicator = eIndicatorType.Right;
                            break;
                        case KeypadKeyCode.D4:
                            nextIndicator = eIndicatorType.Left;
                            break;
                        case KeypadKeyCode.STOP:
                            nextIndicator = eIndicatorType.Break;
                            break;
                        default:
                            nextIndicator = eIndicatorType.None;
                            break;
                    }

                    lock (indicatorLock)
                    {
                        indicator = nextIndicator;
                        indicatorKeyEventTimeStamp = keyEventTimeStamp;
                    }
                }
            };

            FPGA.Config.OnStartup(keypadHandler);
        }
    }
}