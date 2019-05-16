using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatControllers
{
    public static class FloatControllersOps
    {
        public static void TestHandler(float op1, float op2, byte command, out float res)
        {
            switch (command)
            {
                case 0: res = op1 + op2; break;
                case 1: res = op1 - op2; break;
                case 2: res = op1 * op2; break;
                case 3: res = op1 / op2; break;
                case 4: res = (op1 + op2) * 1.5f; break;
                case 5: res = (op1 - op2) * -50.6f; break;
                case 6:
                    res = op1;
                    res += op2;
                    break;
                case 7:
                    res = op1;
                    res -= op2;
                    break;
                case 8:
                    res = op1;
                    res *= op2 + 1.0f;
                    break;
                default: res = 1.2f * 345.7f; break;
            }
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_MultipleScopes
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            const int baud = 115200;

            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            FPGA.Signal<bool> handler0 = false, handler1 = false;
            float res0 = 0, res1 = 0;
            FPGA.Config.Suppress("W0003", handler0, handler1, res0, res1);

            Sequential handler = () =>
            {
                uint idx = FPGA.Config.InstanceId();

                FPU.FPUScope();

                float op1, op2, res = 0;

                UART.ReadFloat(baud, RXD, out op1);
                UART.ReadFloat(baud, RXD, out op2);

                FloatControllersOps.TestHandler(op1, op2, 0, out res);

                // TODO: this is constant expression, should remove branch comletely
                if (idx == 0 )
                {
                    res0 = res;
                    handler0 = true;
                }
                else
                {
                    res1 = res;
                    handler1 = true;
                }
            };

            FPGA.Config.OnStartup(handler, 2);

            // expect to be completed at exactly the same time as they should not share FPU
            Func<bool> trigger = () => handler0 && handler1;

            Sequential onTigger = () =>
            {
                UART.RegisteredWriteFloat(baud, res0, out internalTXD);
                UART.RegisteredWriteFloat(baud, res1, out internalTXD);
            };

            FPGA.Config.OnSignal(trigger, onTigger);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_InferredFPU
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                FPU.FPUScope();

                const int baud = 115200;
                byte command = 0;

                float op1, op2, res = 0;

                while(true)
                {
                    UART.ReadFloat(baud, RXD, out op1);
                    UART.ReadFloat(baud, RXD, out op2);
                    UART.Read(baud, RXD, out command);

                    FloatControllersOps.TestHandler(op1, op2, command, out res);

                    UART.WriteFloat(baud, res, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_InferredFPU_Sim
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<byte> OutWithDefault,
            FPGA.OutputSignal<byte> OutOperation,
            FPGA.OutputSignal<bool> OutTrigger,
            FPGA.OutputSignal<float> OutResult,
            FPGA.OutputSignal<bool> OutCompleted)
        {
            byte value = 100;
            FPGA.Config.Link(value, OutWithDefault);

            Sequential handler = () =>
            {
                FPU.FPUScope();

                float op1 = 1.23f, op2 = 10f, res = 0;
                FPGA.Config.Link(res, OutResult);

                for(byte op = 0; op < 10; op++)
                {
                    FPGA.Config.Link(op, OutOperation);
                    OutTrigger = true;
                    FloatControllersOps.TestHandler(op1, op2, op, out res);
                    OutCompleted = true;
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_Sim
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<float> OutResult,
            FPGA.OutputSignal<bool> Completed
            )
        {
            FPGA.Config.Suppress("W0007", OutResult);
            float f1 = 0, f2 = 0;
            FPGA.Signal<float> result = 0;
            byte fpuOp = 0;
            FPGA.Signal<bool> fpuTrigger = false, fpuCompleted = false;
            FPGA.Config.Entity<IFPU>().Op(fpuTrigger, fpuCompleted, f1, f2, fpuOp, result);

            FPGA.Config.Link(result, OutResult);

            Sequential handler = () =>
            {
                f1 = 20;
                f2 = 10;
                fpuTrigger = true;
                FPGA.Runtime.WaitForAllConditions(fpuCompleted);
                Completed = true;
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_FloatOp
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            float f1 = 0, f2 = 0;
            FPGA.Signal<float> result = 0;
            byte fpuOp = 0;
            FPGA.Signal<bool> fpuTrigger = false, fpuCompleted = false;
            FPGA.Config.Entity<IFPU>().Op(fpuTrigger, fpuCompleted, f1, f2, fpuOp, result);

            Sequential handler = () =>
            {
                const int baud = 115200;
                
                UART.ReadFloat(baud, RXD, out f1);
                UART.ReadFloat(baud, RXD, out f2);
                UART.Read(baud, RXD, out fpuOp);

                fpuTrigger = true;

                FPGA.Runtime.WaitForAllConditions(fpuCompleted);

                UART.WriteFloat(baud, result, TXD);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_FloatRoundTrip
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const uint baud = 115200;

            var stream = new FPGA.SyncStream<float>();

            Sequential<float> streamHandler = (data) =>
            {
                UART.WriteFloat(baud, data, TXD);
            };
            FPGA.Config.OnStream(stream, streamHandler);

            Sequential handler = () =>
            {
                float f1 = 0, f2 = 1.234f;
                
                UART.ReadFloat(baud, RXD, out f1);
                stream.Write(f1);
                stream.Write(f2);
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
