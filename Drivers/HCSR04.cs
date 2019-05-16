using FPGA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class HCSR04
    {
        public static void Measure(FPGA.InputSignal<bool> Echo, FPGA.OutputSignal<bool> Trigger, out ushort Distance)
        {
            FPGA.Config.Default(out Trigger, false);
            FPGA.Config.Default(out Distance, 0);

            // sound does 1 cm return trip in that time
            const uint measurePeriod = 58822;
            object guard = new object();
            ushort counter = 0;
            bool internalTrigger = false;
            FPGA.Config.Link(internalTrigger, Trigger);

            Sequential measureHandler = () =>
            {
                if (Echo)
                {
                    lock(guard)
                    {
                        counter = (ushort)(counter + 1);
                    }
                }
            };

            FPGA.Config.OnTimer(measurePeriod, measureHandler);

            lock(guard)
            {
                counter = 0;
            }

            internalTrigger = true;
            // keep triger signal for 20ms
            FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(20));
            internalTrigger = false;

            // wait for 100ms to complete measurements
            // this is not really good design, need to wait for echo to become high
            // and then wait for echo to become low
            FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(100));

            Distance = counter;
        }
    }
}
