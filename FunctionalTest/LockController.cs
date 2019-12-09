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
    public static class Lock_ParallelAssign
    {
        public static void TestMethod(byte inData, out byte result)
        {
            byte r = 0;
            object l = new object();
            byte counter = 0;
            FPGA.Register<byte> data = 0;
            
            // TODO: const handlers count
            Func<bool> completed = () => counter == 2;

            Sequential handler = () =>
            {
                lock(l)
                {
                    r += data;
                    counter++;
                }
            };

            FPGA.Config.OnRegisterWritten(data, handler, 2);

            FPGA.Runtime.Assign(FPGA.Expressions.AssignRegister(inData, data));

            FPGA.Runtime.WaitForAllConditions(completed);

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
