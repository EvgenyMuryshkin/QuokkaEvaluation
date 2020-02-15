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
    public static class NeuralNet_Test1
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                byte[] buff = new byte[4];
                bool[] results = new bool[4];
                const int baud = 115200;

                for (int i = 0; i < buff.Length; i++)
                {
                    byte data = UART.Read(baud, RXD);
                    buff[i] = data;
                }

                sbyte value1, value2, value3, value4;
                FPGA.Signal<bool> result1 = false, result2 = false, result3 = false, result4 = false;

                value1 = (sbyte)buff[0];
                value2 = (sbyte)buff[1];
                value3 = (sbyte)buff[2];
                value4 = (sbyte)buff[3];

                FPGA.Config.NeuralNet<sbyte>("nn1.json");

                // simple interval wait for net to process data
                FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1));

                results[0] = result1;
                results[1] = result2;
                results[2] = result3;
                results[3] = result4;

                for (int i = 0; i < results.Length; i++)
                {
                    bool data = false;
                    data = results[i];
                    UART.Write(baud, (byte)(data ? 1 : 0), TXD);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
