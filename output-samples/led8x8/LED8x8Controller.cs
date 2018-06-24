using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    [BoardConfig(Name = "Quokka")]
    public static class LEDController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,
            FPGA.OutputSignal<bool> LED2,
            FPGA.OutputSignal<bool> LED3,
            FPGA.OutputSignal<bool> LED4
            )
        {
            IsAlive.Blink(LED1);
            IsAlive.Blink(LED2);
            IsAlive.Blink(LED3);
            IsAlive.Blink(LED4);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class JSONTxController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD

            )
        {
            IsAlive.Blink(LED1);

            SnakeDBG dbg = new SnakeDBG();
            Action handler = () =>
            {
                dbg.C1++;
                JSON.SerializeToUART(dbg, TXD);
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class UARTTxController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD

            )
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            IsAlive.Blink(LED1);

            Action handler = () =>
            {
                for (byte b = 0; b < 10; b++)
                {
                    UART.RegisteredWrite(115200, b, out internalTXD);
                }

                UART.RegisteredWrite(115200, 255, out internalTXD);
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class UARTTxMemController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD

            )
        {
            IsAlive.Blink(LED1);

            Action handler = () =>
            {
                byte[] buff = new byte[11];
                for (byte b = 0; b < 10; b++)
                {
                    buff[b] = b;
                }
                buff[10] = 255;

                for (byte i = 0; i < buff.Length; i++)
                {
                    byte data = 0; // TODO: initializer from memory not supported yet
                    data = buff[i];
                    UART.Write(115200, data, TXD);
                }
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class LED8x8Controller
    {
        public static async Task Aggregator(
            // banks 
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,

            // blinker
            FPGA.OutputSignal<bool> LED1,

            // WS2812
            FPGA.OutputSignal<bool> DOUT
            )
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

            IsAlive.Blink(LED1);

            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            byte state = 0;
            eCellType[] fieldMatrix = new eCellType[64];

            Action handler = () =>
            {
                state++;
                FieldMatrix.Reset(fieldMatrix);

                switch (state)
                {
                    case 1:
                        fieldMatrix[0] = eCellType.RedCross;
                        fieldMatrix[63] = eCellType.GreenCross;
                        break;
                    case 2:
                        FieldMatrix.DrawCross(fieldMatrix, eCellType.GreenCross);
                        break;
                    case 3:
                        FieldMatrix.DrawCross(fieldMatrix, eCellType.RedCross);
                        break;
                    case 4:
                        Position head = new Position(), tail = new Position();
                        FieldMatrix.Seed(fieldMatrix, ref head, ref tail);
                        break;
                    default:
                        state = 0;
                        break;
                }

                Graphics.DrawFieldMatrix(1, fieldMatrix, out internalDOUT);
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), handler);
        }
    }


    [BoardConfig(Name = "Test")]
    public static class FuncController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<byte> outRow,
            FPGA.OutputSignal<byte> outCol,
            FPGA.OutputSignal<byte> outOffset,
            FPGA.OutputSignal<bool> tick,
            FPGA.OutputSignal<bool> setColor
            )
        {
            Action handler = () =>
            {
                for (byte row = 0; row < 8; row++)
                {
                    for (byte col = 0; col < 8; col++)
                    {
                        tick = true;
                        byte offset = 0;
                        Lookups.RowColToOffset(row, col, ref offset);

                        FPGA.Config.Link(row, outRow);
                        FPGA.Config.Link(col, outCol);
                        FPGA.Config.Link(offset, outOffset);

                        if (row == col || (7 - row) == col)
                        {
                            setColor = true;
                        }
                    }
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Test")]
    [BoardConfig(Name = "Quokka")]
    public static class PeripheralsController
    {
        public static async Task Aggregator(
            // banks 
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,

            // blinker
            FPGA.OutputSignal<bool> LED1,
            FPGA.OutputSignal<bool> LED2,
            FPGA.OutputSignal<bool> LED3,
            FPGA.OutputSignal<bool> LED4,
            /*
            // keypad
            FPGA.OutputSignal<bool> K7,
            FPGA.OutputSignal<bool> K6,
            FPGA.OutputSignal<bool> K5,
            FPGA.OutputSignal<bool> K4,
            FPGA.InputSignal<bool> K3,
            FPGA.InputSignal<bool> K2,
            FPGA.InputSignal<bool> K1,
            FPGA.InputSignal<bool> K0,

            // WS2812
            FPGA.OutputSignal<bool> DOUT,

            // ADC
            FPGA.OutputSignal<bool> ADC1NCS,
            FPGA.OutputSignal<bool> ADC1SLCK,
            FPGA.OutputSignal<bool> ADC1DIN,
            FPGA.InputSignal<bool> ADC1DOUT
            */

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD

            )
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

            GameControlsState controlsState = new GameControlsState();

            IsAlive.Blink(LED1);

            IsAlive.Blink(LED2);
            IsAlive.Blink(LED3);
            IsAlive.Blink(LED4);

            /*
            Peripherals.GameControls(
                ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT,
                K7, K6, K5, K4, K3, K2, K1, K0,
                ref controlsState);

            LEDControl(controlsState.keyCode, DOUT);
            */

            byte data = 0;
            Action handler = () =>
            {
                UART.Write(115200, data, TXD);
                data++;
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), handler);
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