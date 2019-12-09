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
    public static class IntegerController_PassArgs
    {
        public static long MethodIdentity(int op)
        {
            return op;
        }

        public static long MethodFirstIdentity(int op1, int op2)
        {
            return op1;
        }

        public static long MethodSecondIdentity(int op1, int op2)
        {
            return op2;
        }

        public static long MethodPostInc(int op)
        {
            op++;
            return op;
        }

        public static long MethodPreDec(int op)
        {
            --op;
            return op;
        }

        public static long MethodSum(int sum1, int sum2)
        {
            return sum1 + sum2;
        }

        public static long TestMethod(byte op, int in1, int in2)
        {
            switch (op)
            {
                case 0:
                    return MethodIdentity(in1);
                case 1:
                    return MethodIdentity(in2);
                case 2:
                    return MethodPostInc(in1);
                case 3:
                    return MethodPreDec(in2);
                case 4:
                    return MethodIdentity(in1++);
                case 5:
                    return MethodIdentity(++in2);
                case 6:
                    return MethodSum(++in1, ++in2);
                case 7:
                    return MethodSum(in1++, in2++);
                case 8:
                    return MethodSum(++in1, ++in1);
                case 9:
                    return MethodSum(in1++, in1++);
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
