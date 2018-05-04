using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    [BoardConfig(Name = "Test")]
    [BoardConfig(Name = "Quokka")]
    public static class PeripheralsController
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
            FPGA.InputSignal<bool> ADC1DOUT

            )
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

            GameControlsState controlsState = new GameControlsState();

            IsAlive.Blink(LED1);

            Peripherals.GameControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

            LEDControl(controlsState.keyCode, DOUT);
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
            FPGA.OutputSignal<bool> DOUT)
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            Func<bool> hasCode = () => keyCode != 0;

            int position = 0, direction = 0;
            FPGA.Config.Suppress("W0003", position);

            uint intensity = 6;

            bool autoPilotEnabled = false;

            uint[] buff = new uint[5];

            Action autoPilotHandler = () =>
            {
                if (!autoPilotEnabled)
                    return;

                HandlePosition(position, direction, 0, buff.Length, out position);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(300), autoPilotHandler);

            Action codeHandler = () =>
            {
                // there is race condition possible - code can go down while handler is executed
                
                switch (keyCode)
                {
                    case KeypadKeyCode.GO:
                        autoPilotEnabled = true;
                        break;
                    case KeypadKeyCode.STOP:
                        autoPilotEnabled = false;
                        break;
                    case KeypadKeyCode.D4:
                        direction = -1;

                        if (!autoPilotEnabled)
                            HandlePosition(position, direction, 0, buff.Length, out position);
                        break;
                    case KeypadKeyCode.D5:
                        direction = 0;
                        break;
                    case KeypadKeyCode.D6:
                        direction = 1;

                        if (!autoPilotEnabled)
                            HandlePosition(position, direction, 0, buff.Length, out position);

                        break;
                    case KeypadKeyCode.D2:
                        intensity = intensity == 10 ? 10 : intensity + 1;
                        break;
                    case KeypadKeyCode.D8:
                        intensity = intensity == 0 ? 0 : intensity - 1;
                        break;
                }

                FPGA.Runtime.WaitForAllConditions(!hasCode());
            };

            FPGA.Config.OnSignal(hasCode(), codeHandler);

            Action ledHandler = () =>
            {
                for (byte i = 0; i < buff.Length; i++)
                {
                    uint data = 0;
                    if (i == position)
                    {
                        data = (intensity * 25);
                    }

                    buff[i] = data;
                }
                
                Graphics.Draw(buff, out internalDOUT);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, ledHandler);


            Action onStartup = () =>
            {
                var outerRow = new FPGA.OutputSignal<byte>();
                var outerCol = new FPGA.OutputSignal<byte>();
                var dto = new Position();
                FPGA.Config.Link(dto.row, outerRow);
                FPGA.Config.Link(dto.col, outerCol);
                DTOTest(dto);
            };
            FPGA.Config.OnStartup(onStartup);
        }

        public static void DTOTest(Position pos)
        {
            var innerRow = new FPGA.OutputSignal<byte>();
            var innerCol = new FPGA.OutputSignal<byte>();

            pos.row++;
            pos.col++;

            FPGA.Config.Link(pos.row, innerRow);
            FPGA.Config.Link(pos.col, innerCol);
        }
    }
}