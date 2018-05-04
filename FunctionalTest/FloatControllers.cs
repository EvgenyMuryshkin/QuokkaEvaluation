using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatControllers
{
    [BoardConfig(Name = "NEB")]
    public static class FloatControllers_MultipleScopes
    {
        public static async Task Aggregator()
        {
            Action handler = () =>
            {
                var RXD = new FPGA.InputSignal<bool>();
                var TXD = new FPGA.OutputSignal<bool>();

                FPU.FPUScope();

                const int baud = 115200;
                byte command = 0;

                float op1, op2, res = 0;

                UART.ReadFloat(baud, RXD, out op1);
                UART.ReadFloat(baud, RXD, out op2);
                UART.Read(baud, RXD, out command);

                switch (command)
                {
                    case 0: res = op1 + op2; break;
                    case 1: res = op1 - op2; break;
                    case 2: res = op1 * op2; break;
                    case 3: res = op1 / op2; break;
                    case 4: res = (op1 + op2) * 1.5f; break;
                    default: res = 1.2f * 345.7f; break;
                }

                UART.WriteFloat(baud, res, TXD);
            };

            FPGA.Config.OnStartup(handler, 2);
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
            Action handler = () =>
            {
                FPU.FPUScope();

                const int baud = 115200;
                byte command = 0;

                float op1, op2, res = 0;

                UART.ReadFloat(baud, RXD, out op1);
                UART.ReadFloat(baud, RXD, out op2);
                UART.Read(baud, RXD, out command);

                switch(command)
                {
                    case 0: res = op1 + op2; break;
                    case 1: res = op1 - op2; break;
                    case 2: res = op1 * op2; break;
                    case 3: res = op1 / op2; break;
                    case 4: res = (op1 + op2) * 1.5f; break;
                    default: res = 1.2f * 345.7f; break;
                }

                UART.WriteFloat(baud, res, TXD);
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

            Action handler = () =>
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

            Action<float> streamHandler = (data) =>
            {
                UART.WriteFloat(baud, data, TXD);
            };
            FPGA.Config.OnStream(stream, streamHandler);

            Action handler = () =>
            {
                float f1 = 0, f2 = 1.234f;
                
                UART.ReadFloat(baud, RXD, out f1);
                stream.Write(f1);
                stream.Write(f2);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
