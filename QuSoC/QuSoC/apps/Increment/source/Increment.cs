using System;
using System.Collections.Generic;
using System.Text;

namespace Increment
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            uint counter = 0;
            while (true)
            {
                counter++;
                SOC.Instance.Counter = counter;
            }
        }
    }
}
