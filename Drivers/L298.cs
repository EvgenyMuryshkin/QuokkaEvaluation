using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class L298
    {
        public static void SingleMotorDriver(
            sbyte value, 
            FPGA.OutputSignal<bool> pin1,
            FPGA.OutputSignal<bool> pin2,
            FPGA.OutputSignal<bool> enabled)
        {
            bool internalPin1 = false, internalPin2 = false, internalEnabled = false;
            FPGA.Config.Link(internalPin1, out pin1);
            FPGA.Config.Link(internalPin2, out pin2);
            FPGA.Config.Link(internalEnabled, out enabled);

            Action pwm = () =>
            {
                while (true)
                {
                    if ( value == 0 )
                    {
                        internalEnabled = false;
                        internalPin1 = false;
                        internalPin2 = false;
                    }
                    else
                    {
                        byte steps = 0;
                        if (value > 0)
                        {
                            internalPin1 = false;
                            internalPin2 = true;
                            steps = (byte)value;
                        }
                        else
                        {
                            steps = (byte)(0 - value);
                            internalPin2 = false;
                            internalPin1 = true;
                        }

                        byte counter = 0;
                        while (counter < 128)
                        {
                            if (counter <= steps)
                            {
                                internalEnabled = true;
                            }
                            else
                            {
                                internalEnabled = false;
                            }

                            FPGA.Runtime.Delay(TimeSpanEx.FromMicroseconds(100));
                            counter++;
                        }
                    }
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, pwm);
        }
    }
}
