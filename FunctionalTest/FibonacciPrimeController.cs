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
    public static class Math_FibonacciPrime
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential handler = () =>
            {
                while (true)
                {
                    byte start = 0;
                    UART.Read(115200, RXD, out start);

                    for(uint counter = 0; counter < 100; counter++)
                    {
                        ulong fib = 0;
                        SequentialMath.Calculators.Fibonacci(counter, out fib);

                        if (fib > uint.MaxValue)
                            break;

                        bool isPrime = false;
                        SequentialMath.Calculators.IsPrime((uint)fib, out isPrime);

                        if (isPrime)
                        {
                            for (byte i = 0; i < 4; i++)
                            {
                                byte data = (byte)fib;
                                UART.Write(115200, data, TXD);
                                fib = fib >> 8;
                            }
                        }
                    }
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
