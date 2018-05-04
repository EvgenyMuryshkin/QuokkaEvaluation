using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Triggers_ConstTrueTrigger
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const bool trigger = true;
            Action handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                UART.Write(115200, data, TXD);
            };

            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Triggers_ConstFalseTrigger
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const bool trigger = false;
            // this should never be triggered
            Action handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                UART.Write(115200, data, TXD);
            };

            FPGA.Config.OnSignal(trigger, handler);
        }
    }


    [BoardConfig(Name = "NEB")]
    public static class Triggers_Timer
    {
        public static async Task Aggregator(FPGA.OutputSignal<bool> LED)
        {
            bool state = false;
            FPGA.Config.Link(state, LED);

            Action blinkerHandler = () =>
            {
                state = !state;
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), blinkerHandler);
        }
    }


    [BoardConfig(Name = "NEB")]
    public static class Triggers_Timer_Multiple
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            FPGA.SyncStream<byte> tdxStream = new FPGA.SyncStream<byte>();
            Action<byte> txdHandler = (value) =>
            {
                UART.Write(115200, value, TXD);
            };
            FPGA.Config.OnStream(tdxStream, txdHandler);

            byte data = 0;
            Action mainHandler = () =>
            {
                UART.Read(115200, RXD, out data);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);

            Action handler1 = () =>
            {
                if (data == 0)
                    return;

                tdxStream.Write(1);
            };

            Action handler2 = () =>
            {
                if (data == 0)
                    return;

                tdxStream.Write(2);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(100), handler1, handler2);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Triggers_Startup
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            byte data = 0;
            Action mainHandler = () =>
            {
                const uint baud = 115200;
                UART.Read(baud, RXD, out data);
                UART.Write(baud, data, TXD);
            };

            FPGA.Config.OnStartup(mainHandler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Triggers_Signal
    {
        public static void Handler(FPGA.OutputSignal<bool> TXD)
        {
            TXD = true;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            Action mainHandler = () =>
            {
                Handler(TXD);
            };

            FPGA.Config.OnSignal(RXD, mainHandler);
        }
    }

    // TODO: signal and register written basic tests
}
