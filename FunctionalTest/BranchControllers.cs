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
    public static class Branch_Switch
    {
        public static void TestMethod(byte max, out byte result)
        {
            switch(max)
            {
                case 0:
                    result = 1;
                    break;
                case 1:
                    result = 2;
                    break;
                case 4:
                    result = 10;
                    break;
                default:
                    result = 100;
                    break;
            }
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

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Branch_IfElse
    {
        public static void TestMethod(byte max, out byte result)
        {
            if( max == 0 )
            {
                result = 2;
            }
            else if ( max == 1)
            {
                result = 4;
            }
            else if ( max == 4 )
            {
                result = 120;
            }
            else
            {
                result = 150;
            }
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

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Branch_Switch_IfElse_Break
    {
        public static void TestMethod(byte max, out byte result)
        {
            switch (max)
            {
                case 0:
                    result = 10;
                    break;
                case 1:
                    result = 20;
                    break;
                case 4:
                    result = 100;
                    break;
                default:
                    result = 104;
                    if (max > 150)
                        break;

                    result = 200;
                    break;
            }
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

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Branch_Switch_For_While_IfElse_Break
    {
        public static void TestMethod(byte max, out byte result)
        {
            byte r = 0;
            
            for(byte i = 0; i < max; i++ )
            {
                switch (i)
                {
                    case 2:
                        continue;
                    case 3:
                        i++;
                        break;
                    default:
                        if( i == 4 )
                        {
                            i = 5;
                        }
                        else if (i == 7)
                        {
                            i = 7;
                        }

                        break;
                }

                if (i == 7)
                    continue;

                r += i;

                if (i == 8)
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
}
