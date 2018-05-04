using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SequentialMath
{
    public static class Calculators
    {
        public static void Fibonacci(uint n, out ulong result)
        {
            if(n == 0 )
            {
                result = 0;
                return;
            }

            if( n == 1 )
            {
                result = 1;
                return;
            }

            ulong n1 = 0;
            ulong n2 = 1;

            for (uint i = 2; i < n; i++)
            {
                ulong nextN2 = n1 + n2;
                n1 = n2;
                n2 = nextN2;
            }

            result = n1 + n2;
        }

        public static void IsPrime(uint value, out bool result)
        {
            // run some prechecks to skip known cases
            if (value < 2)
            {
                result = false;
                return;
            }

            if (value == 2)
            {
                result = true;
                return;
            }

            // check if value is even and skip it
            if ((value & 1) == 0)
            {
                result = false;
                return;
            }

            // remaining values here are 3,5,7
            if (value <= 7)
            {
                result = true;
                return;
            }

            // start checking from 3
            uint currentValue = 3;
            object currentValueLock = new object();

            ushort completedWorkers = 0;
            object completedWorkersLock = new object();

            Func<bool> completed = () => completedWorkers == 2;
            FPGA.Signal<bool> trigger = new FPGA.Signal<bool>();


            // run bruteforce check
            bool foundDivider = false;
            FPGA.Config.Suppress("W0003", foundDivider);

            Action worker = () =>
            {
                while(!foundDivider)
                {
                    uint workerValue = 0;
                    lock (currentValueLock)
                    {
                        workerValue = currentValue;
                        currentValue += 2;
                    }

                    Func<ulong> squared = () => workerValue * workerValue;

                    if( squared() == value )
                    {
                        foundDivider = true;
                        break;
                    }

                    if (squared() > value)
                    {
                        break;
                    }

                    uint divResult = 0, divRemainder = 0;
                    SequentialMath.Divider.Unsigned<uint>(value, workerValue, out divResult, out divRemainder);

                    if(divRemainder == 0)
                    {
                        foundDivider = true;
                        break;
                    }
                }

                lock (completedWorkersLock)
                {
                    completedWorkers++;
                }
            };

            FPGA.Config.OnSignal(trigger, worker, 2);

            trigger = true;
            FPGA.Runtime.WaitForAllConditions(completed);

            result = !foundDivider;
        }
    }

    public static class Divider
    {
        public static void Unsigned<T>(T inNumerator, T inDenominator, out T outResult, out T outRemainder) where T : struct
        {
            byte size = FPGA.Config.SizeOf(inNumerator);
            T num = inNumerator, den = inDenominator, res = default(T), rem = default(T);

            //FPGA.Config.Link(res, out outResult);
            //FPGA.Config.Link(rem, out outRemainder);

            Func<bool> needSubtract = () => FPGA.Config.Compare(rem, FPGA.CompareType.GreaterOrEqual, den);
            Func<T> shiftRemainder = () => FPGA.Config.LShift(rem, 1, num);
            Func<T> nextNumerator = () => FPGA.Config.LShift(num, 1);
            Func<T> nextResult = () => FPGA.Config.LShift(res, 1, needSubtract());
            Func<T> valueToSubtract = () => FPGA.Config.Math(FPGA.MathType.Multiply, den, needSubtract());
            Func<T> nextRemainder = () => FPGA.Config.Math(FPGA.MathType.Subtract, rem, valueToSubtract());

            for (byte i = 0; i < size; i++)
            {
                rem = shiftRemainder();

                FPGA.Runtime.Assign(
                    FPGA.Expressions.Assign(() => nextNumerator(), (v) => num = v),
                    FPGA.Expressions.Assign(() => nextResult(), (v) => res = v),
                    FPGA.Expressions.Assign(() => nextRemainder(), (v) => rem = v)
                    );
            }

            outResult = res;
            outRemainder = rem;
        }
    }
}
