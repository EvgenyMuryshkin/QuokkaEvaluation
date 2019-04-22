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
            FPGA.Config.Default(out controlState.flashSpeedMs, 500);

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
                        if (controlState.flashSpeedMs > 100)
                            controlState.flashSpeedMs -= 100;
                        break;
                    case KeypadKeyCode.D7:
                        if (controlState.flashSpeedMs < 2000)
                            controlState.flashSpeedMs += 100;
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

            Action ledHandler = () =>
            {
                while (true)
                {
                    lock (indicatorLock)
                    {
                        IndicatorsEngine.Process(controlState);
                    }
                }
            };
            FPGA.Config.OnStartup(ledHandler);

            Action drawHandler = () =>
            {
                byte[] buffData = new byte[8];
                uint indicatorColor = 0;

                while (true)
                {
                    switch (controlState.lastIndicator)
                    {
                        case eIndicatorType.Left:
                            Graphics.LeftIndicator(buffData, controlState.dim, out indicatorColor);
                            break;
                        case eIndicatorType.Right:
                            Graphics.RightIndicator(buffData, controlState.dim, out indicatorColor);
                            break;
                        case eIndicatorType.Break:
                            Graphics.BreakIndicator(buffData, controlState.dim, out indicatorColor);
                            break;
                    }

                    Graphics.DrawIndicator(buffData, indicatorColor, controlState.isIndicatorActive, out internalDOUT);
                }
            };
            FPGA.Config.OnStartup(drawHandler);

            Action keypadHandler = () =>
            {
                while(true)
                {
                    uint keyEventTimeStamp = controlState.counterMs;
                    eIndicatorType nextIndicator = controlState.nextIndicator;

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

                    if (nextIndicator != controlState.nextIndicator)
                    {
                        lock (indicatorLock)
                        {
                            controlState.nextIndicator = nextIndicator;
                            controlState.nextIndicatorKeyEventTimeStamp = keyEventTimeStamp;
                        }
                    }
                }
            };

            FPGA.Config.OnStartup(keypadHandler);
        }
    }
}