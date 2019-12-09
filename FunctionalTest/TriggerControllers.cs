using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Counter
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<byte> Tens,
            FPGA.OutputSignal<byte> Ones,
            FPGA.OutputSignal<byte> Tenths
            )
        {
            byte internalTens = 0, internalOnes = 0, internalTenths = 0;
            FPGA.Config.Link(internalTens, Tens);
            FPGA.Config.Link(internalOnes, Ones);
            FPGA.Config.Link(internalTenths, Tenths);

            Func<byte> nextTenths = () => (byte)(
                internalTenths == 9 
                    ? 0 
                    : internalTenths + 1);

            Func<byte> nextOnes = () => (byte)(
                internalTenths == 9 
                    ?   internalOnes == 9 
                            ? 0 
                            : internalOnes + 1
                    :   internalOnes);

            Func<byte> nextTens = () => (byte)(
                internalTenths == 9
                    ? internalOnes == 9
                        ? internalTens == 5
                            ? 0
                            : internalTens + 1
                        : internalTens
                    : internalTens);

            Sequential handler = () =>
            {
                while (true)
                {
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(() => nextTenths(), (v) => internalTenths = v),
                        FPGA.Expressions.Assign(() => nextOnes(), (v) => internalOnes = v),
                        FPGA.Expressions.Assign(() => nextTens(), (v) => internalTens = v)
                        );

                    /*
                    if (internalTenths == 9)
                    {
                        internalTenths = 0;
                        if (internalOnes == 9)
                        {
                            internalOnes = 0;
                            if (internalTens == 5 )
                            {
                                internalTens = 0;
                            }
                            else
                            {
                                internalTens++;
                            }
                        }
                        else
                        {
                            internalOnes++;
                        }
                    }
                    else
                    {
                        internalTenths++;
                    }
                    */
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Defaults_DefaultRegisterValue
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            Sequential handler = () =>
            {
                byte prev = 100, next = 0;
                while (true)
                {
                    UART.Read(115200, RXD, out next);

                    UART.RegisteredWrite(115200, prev, out internalTXD);
                    UART.RegisteredWrite(115200, next, out internalTXD);

                    prev = next;
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Triggers_ConstTrueTrigger
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const bool trigger = true;
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                UART.Write(115200, data, TXD);
            };

            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Triggers_ConstFalseTrigger
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const bool trigger = false;
            // this should never be triggered
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                UART.Write(115200, data, TXD);
            };

            FPGA.Config.OnSignal(trigger, handler);
        }
    }


    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Triggers_Timer
    {
        public static async Task Aggregator(FPGA.OutputSignal<bool> LED)
        {
            bool state = false;
            FPGA.Config.Link(state, LED);

            Sequential blinkerHandler = () =>
            {
                state = !state;
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), blinkerHandler);
        }
    }


    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Triggers_Timer_Multiple
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            FPGA.SyncStream<byte> tdxStream = new FPGA.SyncStream<byte>();
            Sequential<byte> txdHandler = (value) =>
            {
                UART.Write(115200, value, TXD);
            };
            FPGA.Config.OnStream(tdxStream, txdHandler);

            byte data = 0;
            Sequential mainHandler = () =>
            {
                UART.Read(115200, RXD, out data);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);

            Sequential handler1 = () =>
            {
                if (data == 0)
                    return;

                tdxStream.Write(1);
            };

            Sequential handler2 = () =>
            {
                if (data == 0)
                    return;

                tdxStream.Write(2);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(100), handler1, handler2);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Triggers_Startup
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            byte data = 0;
            Sequential mainHandler = () =>
            {
                const uint baud = 115200;
                UART.Read(baud, RXD, out data);
                UART.Write(baud, data, TXD);
            };

            FPGA.Config.OnStartup(mainHandler);
        }
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
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
            Sequential mainHandler = () =>
            {
                Handler(TXD);
            };

            FPGA.Config.OnSignal(RXD, mainHandler);
        }
    }

    // TODO: signal and register written basic tests
}
