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
    [BoardConfig(Name = "Test")]
    public static class MainController
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

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
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

            SnakeControl(
                controlsState,
                DOUT, TXD);
        }

        public static void SnakeControl(
            GameControlsState controlsState,
            FPGA.OutputSignal<bool> DOUT,
            FPGA.OutputSignal<bool> TXD)
        {
            bool internalDOUT = false;
            FPGA.Config.Link(internalDOUT, DOUT);

            FPGA.Register<int> randomValue = 0;
            RandomValueGenerator.MakeRandomValue(controlsState, randomValue);

            eCellType[] fieldMatrix = new eCellType[64];
            var head = new Position();
            var tail = new Position();
            eDirectionType currentDirection = eDirectionType.None;
            byte baseColor = 0x1;
            eGameMode gameMode = eGameMode.Setup;

            // drawing
            Sequential drawHandler = () =>
            {
                try
                {
                    switch (gameMode)
                    {
                        case eGameMode.Setup:
                            {
                                GameEngine.Setup(
                                    controlsState, 
                                    fieldMatrix, 
                                    ref head, 
                                    ref tail,
                                    ref currentDirection,
                                    randomValue,
                                    ref gameMode,
                                    ref baseColor);
                            }
                            break;
                        case eGameMode.Play:
                            {
                                GameEngine.GameIteration(
                                    controlsState,
                                    fieldMatrix,
                                    ref head, ref tail,
                                    ref currentDirection,
                                    randomValue,
                                    TXD);
                            }
                            break;
                        default:
                            if (controlsState.keyCode == Drivers.KeypadKeyCode.PWR)
                            {
                                gameMode = eGameMode.Setup;
                            }

                            break;
                    }
                }
                catch (GameCompletedException)
                {
                    FieldMatrix.DrawCross(fieldMatrix, eCellType.GreenCross);
                    gameMode = eGameMode.Completed;
                }
                catch (Exception)
                {
                    FieldMatrix.DrawCross(fieldMatrix, eCellType.RedCross);
                    gameMode = eGameMode.Failed;
                }

                Graphics.DrawFieldMatrix(baseColor, fieldMatrix, out internalDOUT);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(400), drawHandler);
        }
    }
}