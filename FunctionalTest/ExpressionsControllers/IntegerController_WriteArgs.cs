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
    public static class IntegerController_WriteArgs
    {
 
        public static long TestMethod(byte op, int op1, int op2)
        {
            switch (op)
            {
                case 0:
                    return op1++;
                case 1:
                    return ++op2;
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
                    byte op = UART.Read(baud, RXD);

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
