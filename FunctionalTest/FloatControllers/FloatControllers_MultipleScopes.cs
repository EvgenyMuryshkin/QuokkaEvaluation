using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
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
}
