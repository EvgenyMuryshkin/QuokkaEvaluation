using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_Decl_ConstCond_Inc
    {
        public static void TestMethod(out byte result)
        {
            byte r = 0;
            for (byte i = 0; i < 10; i++)
            {
                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_Decl_VarCond_Inc
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;
            for (byte i = 0; i < max; i++)
            {
                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_VarCond_Inc
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0, i = 5; ;
            for (; i < max; i++)
            {
                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_Init_VarCond_Inc
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0, i = 5; ;
            for (i = 0; i < max; i++)
            {
                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_Init_VarCond_MultiExprInc
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0, i = 5, j = 0; 
            for (i = 0; i < max; i += 2, j += 3)
            {
                r = (byte)(r + i + j);
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_Redeclaration
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;
            for (byte i = 0; i < max; ++i)
            {
                r += i;
            }

            for (byte i = 0; i < max; ++i)
            {
                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_BreakContinue
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;
            for (byte i = 0; i < max; ++i)
            {
                if (i == 8)
                    break;

                if( i == 4 )
                {
                    continue;
                }

                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_ForLoop_NestedBreakContinue
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;
            for (byte i = 0; i < max; ++i)
            {
                if (i == 8)
                    break;

                if (i == 4)
                {
                    continue;
                }

                for (byte j = (byte)(i + 1); j < max; j++)
                {
                    if (j > 7)
                        break;

                    if (j == 5)
                        continue;

                    r += j;
                }

                r += i;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_WhileLoop_VarCond
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;

            while(r < max)
            {
                r++;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_WhileLoop_BreakContinue
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;

            while (r < max)
            {
                r++;

                if (r == 4)
                    continue;

                if (r == 8)
                    break;
            }

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Loop_DoLoop_VarCond
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;

            do
            {
                r++;
            }
            while (r < max);

            result = r;
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);

                byte result = 0;
                TestMethod(data, out result);

                UART.Write(115200, result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
    /*
    [BoardConfig(Name = "NEB")]
    public static class Loop_WhileLoop_PrefixOp
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;
            FPGA.Config.GenBreak();

            while(--max > 0 )
            {
                r++;
            }

            result = r; 
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Action handler = () =>
            {
                byte data = 0;
                UART.Read(RXD, out data);

                byte result = 0;
                Controllers.Loop_WhileLoop_PrefixOp.TestMethod(data, out result);

                UART.Write(result, TXD);
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }*/
}
