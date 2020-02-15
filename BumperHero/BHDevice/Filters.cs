using FPGA;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHDevice
{
    public static class Filters
    {
        public static void Average(ushort[] buff, out ushort average)
        {
            uint sum = 0;
            for (int i = 0; i < buff.Length; i++)
            {
                ushort tmp = 0;
                tmp = buff[i];
                sum += tmp;
            }

            uint result = 0, remainder = 0;
            // TODO: length accessor in cast
            //uint denominator = (uint)buff.Length;
            SequentialMath.DivideUnsigned<uint>(sum, 10, out result, out remainder);
            average = (ushort)result;
        }
    }
}
