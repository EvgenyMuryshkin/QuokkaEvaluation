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
    public static class DemoController
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

            Peripherals.GameControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                controlsState);

            LEDControl(controlsState.keyCode, DOUT, Servo);
        }

        public static void HandlePosition(
            int current, 
            int delta, 
            int min,
            int max,
            out int newPosition)
        {
            int nextPosition = current + delta;

            if (nextPosition >= max)
            {
                nextPosition = 0;
            }
            else if (nextPosition < min)
            {
                nextPosition = max - 1;
            }

            newPosition = nextPosition;
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

            uint[] buff = new uint[64];
            byte baseColor = 5;
            Sequential ledHandler = () =>
            {
                uint color = 0;

                switch(keyCode)
                {
                    case KeypadKeyCode.STOP:
                        color = (uint)(baseColor << 16);
                        break;
                    case KeypadKeyCode.GO:
                        color = (uint)((baseColor << 8) | (baseColor << 16));
                        break;
                    case KeypadKeyCode.LOCK:
                        color = (uint)(baseColor << 8);
                        break;
                    case KeypadKeyCode.D2:
                        baseColor++;
                        break;
                    case KeypadKeyCode.D8:
                        baseColor--;
                        break;
                    // servo control
                    case KeypadKeyCode.ENT:
                        servoValue = 0;
                        break;
                    case KeypadKeyCode.D0:
                        servoValue = 60;
                        break;
                    case KeypadKeyCode.ESC:
                        servoValue = 120;
                        break;
                    case KeypadKeyCode.PWR:
                        servoValue = 180;
                        break;
                }

                for (byte i = 0; i < buff.Length; i++)
                {
                    buff[i] = color;
                }
                
                Graphics.Draw(buff, out internalDOUT);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, ledHandler);
        }
    }
}