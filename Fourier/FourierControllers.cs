using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FPGA.Fourier.Controllers
{
    public static class RTX
    {
        public static void ReadData(uint baud, FPGA.InputSignal<bool> RXD, ComplexFloat[] data)
        {
            uint tmp32 = 0;
            for (uint i = 0; i < data.Length; i++)
            {
                FPGA.Config.SetInclusiveRange(0, data.Length, i);

                ComplexFloat tmp = new ComplexFloat();

                for (byte idx = 0; idx < 2; idx++)
                {
                    FPGA.Config.SetInclusiveRange(0, 2, idx);

                    UART.ReadUnsigned32(baud, RXD, ref tmp32);
                    switch (idx)
                    {
                        case 0:
                            FPGA.Runtime.Assign(FPGA.Expressions.Unchecked(tmp32, out tmp.Re));
                            break;
                        case 1:
                            FPGA.Runtime.Assign(FPGA.Expressions.Unchecked(tmp32, out tmp.Im));
                            break;
                    }
                }
                data[i] = tmp;
            }
        }

        public static void WriteData(uint baud, FPGA.OutputSignal<bool> TXD, ComplexFloat[] data, uint duration)
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            FPGA.SyncStream<uint> streamWriter = new FPGA.SyncStream<uint>();
            Sequential<uint> streamHandler = (d) =>
            {
                UART.RegisteredWriteUnsigned32(baud, d, out internalTXD);
            };

            FPGA.Config.OnStream(streamWriter, streamHandler);

            ComplexFloat tmp = new ComplexFloat();
            uint uns = 0;

            for (uint i = 0; i < data.Length; i++)
            {
                FPGA.Config.SetInclusiveRange(0, data.Length, i);

                tmp = data[i];

                for (byte idx = 0; idx < 2; idx++)
                {
                    FPGA.Config.SetInclusiveRange(0, 2, idx);

                    switch (idx)
                    {
                        case 0:
                            FPGA.Runtime.Assign(FPGA.Expressions.Unchecked(tmp.Re, out uns));
                            break;
                        case 1:
                            FPGA.Runtime.Assign(FPGA.Expressions.Unchecked(tmp.Im, out uns));
                            break;
                    }

                    streamWriter.Write(uns);
                }
            }

            streamWriter.Write(duration);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class FPUTimingController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            const int baud = 115200;

            uint clockCounter = 0;
            Diag.ClockCounter(clockCounter);

            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            float op1, op2, res = 0;

            Sequential handler = () =>
            {
                FPU.FPUScope();

                while (true)
                {
                    uint start = 0, end = 0;
                    res = 0;

                    var testOp = UART.Read(baud, RXD);
                    UART.ReadFloat(baud, RXD, out op1);
                    UART.ReadFloat(baud, RXD, out op2);

                    switch ((FPUTimingType)testOp)
                    {
                        case FPUTimingType.Add:
                            start = clockCounter;
                            res = op1 + op2;
                            end = clockCounter;
                           break;
                        case FPUTimingType.Sub:
                            start = clockCounter;
                            res = op1 - op2;
                            end = clockCounter;
                            break;
                        case FPUTimingType.Mul:
                            start = clockCounter;
                            res = op1 * op2;
                            end = clockCounter;
                            break;
                        case FPUTimingType.Div:
                            start = clockCounter;
                            res = op1 / op2;
                            end = clockCounter;
                            break;
                        default:
                            start = clockCounter;
                            end = clockCounter;
                            break;
                    }
                    UART.RegisteredWriteFloat(baud, res, out internalTXD);
                    UART.RegisteredWriteUnsigned32(baud, end - start, out internalTXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class LoopbackController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> LED1,
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int width = 10;
                const int baud = 115200;

                ComplexFloat[] data = new ComplexFloat[GeneratorTools.ArrayLength(width)];

                while (true)
                {
                    RTX.ReadData(baud, RXD, data);

                    RTX.WriteData(baud, TXD, data, 0);
                }
            };

            FPGA.Config.OnStartup(handler);

            Drivers.IsAlive.Blink(LED1);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class BitReverseController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> LED1,
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int width = 10;
                const int baud = 115200;

                while (true)
                {
                    byte data = UART.Read(baud, RXD);

                    uint length = GeneratorTools.ArrayLength(width);
                    for(uint i = 0; i < length; i++)
                    {
                        uint reversed = FPGA.Runtime.Reverse(i, width);
                        UART.WriteUnsigned32(baud, reversed, TXD);
                    }
                }
            };

            FPGA.Config.OnStartup(handler);

            Drivers.IsAlive.Blink(LED1);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class OffsetController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> LED1,
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int width = 10;
                const int baud = 115200;

                ComplexFloat[] data = new ComplexFloat[GeneratorTools.ArrayLength(width)];

                while (true)
                {
                    RTX.ReadData(baud, RXD, data);

                    for(int idx = 0; idx < data.Length; idx++)
                    {
                        ComplexFloat tmp = new ComplexFloat();
                        tmp = data[idx];

                        tmp.Re = 1024f;
                        tmp.Im = tmp.Im + 10f;

                        data[idx] = tmp;
                    }

                    RTX.WriteData(baud, TXD, data, 0);
                }
            };

            FPGA.Config.OnStartup(handler);

            Drivers.IsAlive.Blink(LED1);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class CopyAndNormalizeController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> LED1,
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int width = 10;
                const int baud = 115200;

                ComplexFloat[] data = new ComplexFloat[GeneratorTools.ArrayLength(width)];
                ComplexFloat tmp = new ComplexFloat();

                while (true)
                {
                    RTX.ReadData(baud, RXD, data);

                    FTTools.CopyAndNormalize(width, data, data, Direction.Forward, ref tmp);

                    RTX.WriteData(baud, TXD, data, 0);
                }
            };

            FPGA.Config.OnStartup(handler);

            Drivers.IsAlive.Blink(LED1);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class DFTController
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> LED1,
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int width = 10;
                const int baud = 115200;

                ComplexFloat[] data = new ComplexFloat[GeneratorTools.ArrayLength(width)];

                while(true)
                {
                    RTX.ReadData(baud, RXD, data);

                    DFT.Transform(width, data, Direction.Forward);

                    RTX.WriteData(baud, TXD, data, 0);
                }
            };

            FPGA.Config.OnStartup(handler);

            Drivers.IsAlive.Blink(LED1);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class FFTController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            uint clockCounter = 0;
            Diag.ClockCounter(clockCounter);

            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                const int width = 10;
                const int baud = 115200;
                                
                ComplexFloat[] source = new ComplexFloat[GeneratorTools.ArrayLength(width)];
                ComplexFloat[] target = new ComplexFloat[GeneratorTools.ArrayLength(width)];
                FPGA.Config.NoSync(source);

                while (true)
                {
                    RTX.ReadData(baud, RXD, source);

                    uint start = clockCounter;
                    FFT.Transform(width, source, target, Direction.Forward);
                    uint end = clockCounter;

                    RTX.WriteData(baud, TXD, target, end - start);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
