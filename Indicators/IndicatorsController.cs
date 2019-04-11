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
                controlsState);

            LEDControl(controlsState);

            IndicatorsControl(
                controlsState,
                DOUT, 
                Servo);
        }

        public static void LEDControl(IndicatorsControlsState controlState)
        {
            FPGA.Config.Default(out controlState.dim, 1);
            FPGA.Config.Default(out controlState.flashSpeedInTicks, 500);

            Action uiControlsHandler = () =>
            {
                Func<bool> noKey = () => controlState.keyCode == 0;

                switch (controlState.keyCode)
                {
                    case KeypadKeyCode.D2:
                        if (controlState.dim < 10)
                            controlState.dim++;
                        break;
                    case KeypadKeyCode.D8:
                        if (controlState.dim > 1)
                            controlState.dim--;
                        break;
                    case KeypadKeyCode.D1:
                        if (controlState.flashSpeedInTicks > 100)
                            controlState.flashSpeedInTicks -= 100;
                        break;
                    case KeypadKeyCode.D7:
                        if (controlState.flashSpeedInTicks < 2000)
                            controlState.flashSpeedInTicks += 100;
                        break;
                }

                FPGA.Runtime.WaitForAllConditions(noKey);
            };
            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(50), uiControlsHandler);
        }

        public static void IndicatorsControl(
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

            byte[] buffData = new byte[8];
            uint indicatorColor = 0;
            object buffLock = new object();

            Action ledHandler = () =>
            {
                // resetting flashIndicatorTimeStamp to 0 will trigger isIndicatorActive on first run
                uint flashIndicatorTimeStamp = 0;
                bool isIndicatorActive = true;

                eIndicatorType lastIndicator = eIndicatorType.None;
                uint lastIndicatorTimeStamp = 0;

                uint autoIndicatorTimeStamp = 0;

                Action updateIndicator = () =>
                {
                    lastIndicator = indicator;
                    lastIndicatorTimeStamp = indicatorKeyEventTimeStamp;
                };

                while (true)
                {
                    uint currentTimeStamp = controlState.msTickCounter;

                    lock (indicatorLock)
                    {
                        if (lastIndicator != indicator)
                        {
                            // indicator was changed
                            if (autoIndicatorTimeStamp != 0)
                            {
                                // running on auto indicator
                                if (indicator == eIndicatorType.None)
                                {
                                    // no key pressed
                                    if (
                                        // was break signal
                                        (lastIndicator == eIndicatorType.Break) || 
                                        // timed out and wait for not active
                                        (!isIndicatorActive && 
                                        currentTimeStamp - autoIndicatorTimeStamp > 3000))
                                    {
                                        // turn off auto indicator after 3 seconds and when it is off to avoid quick blinks
                                        autoIndicatorTimeStamp = 0;
                                        FPGA.Config.GenBreak();
                                        updateIndicator();
                                    }

                                    // do nothing if running on auto, and nothing changed
                                }
                                else
                                {
                                    // different indicator
                                    autoIndicatorTimeStamp = 0;

                                    updateIndicator();

                                    flashIndicatorTimeStamp = 0;
                                    isIndicatorActive = false;
                                    indicatorColor = 0;
                                }
                            }
                            else
                            {
                                if (indicator == eIndicatorType.None && (currentTimeStamp - lastIndicatorTimeStamp) < 500)
                                {
                                    // turn on auto indicator if key was pressed less then 500 ms
                                    autoIndicatorTimeStamp = currentTimeStamp;
                                }
                                else
                                {
                                    updateIndicator();

                                    flashIndicatorTimeStamp = 0;
                                    isIndicatorActive = false;
                                    indicatorColor = 0;
                                }
                            }
                        }
                    }

                    // blinking logic
                    if (currentTimeStamp - flashIndicatorTimeStamp > controlState.flashSpeedInTicks)
                    {
                        flashIndicatorTimeStamp = currentTimeStamp;
                        isIndicatorActive = !isIndicatorActive;
                    }

                    switch (lastIndicator)
                    {
                        case eIndicatorType.Left:
                            Graphics.LeftIndicator(buffData, controlState.dim, out indicatorColor);
                            break;
                        case eIndicatorType.Right:
                            Graphics.RightIndicator(buffData, controlState.dim, out indicatorColor);
                            break;
                        case eIndicatorType.Break:
                            Graphics.BreakIndicator(buffData, controlState.dim, out indicatorColor);
                            isIndicatorActive = true;
                            break;
                        default:
                            isIndicatorActive = false;
                            break;
                    }

                    Graphics.DrawIndicator(buffData, indicatorColor, isIndicatorActive, out internalDOUT);
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

                    if (nextIndicator != indicator)
                    {
                        lock (indicatorLock)
                        {
                            indicator = nextIndicator;
                            indicatorKeyEventTimeStamp = keyEventTimeStamp;
                        }
                    }
                }
            };

            FPGA.Config.OnStartup(keypadHandler);
        }
    }
}