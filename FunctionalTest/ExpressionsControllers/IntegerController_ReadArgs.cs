using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTest.ExpressionsControllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "Quokka")]
    public static class IntegerController_ReadArgs
    {
        public static long TestMethod(byte op, int op1, int op2)
        {
            switch (op)
            {
                case 0:
                    return op1;
                case 1:
                    return op2;
                case 2:
                    return op1 + op2;
                case 3:
                    return op1 - op2;
                case 4:
                    return op1 * op2;
                case 5:
                    return op1 / op2;
                case 6:
                    return (op1 + op2) * 10;
                default:
                    return 42;
            }

        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            const int baud = 115200;
            Sequential handler = () =>
            {
                while(true)
                {
                    byte op = 0;
                    UART.Read(baud, RXD, out op);

                    uint op1 = 0, op2 = 0;
                    UART.ReadUnsigned32(baud, RXD, ref op1);
                    UART.ReadUnsigned32(baud, RXD, ref op2);

                    var result = TestMethod(op, (int)op1, (int)op2);

                    UART.WriteUnsigned64(baud, (ulong)result, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
