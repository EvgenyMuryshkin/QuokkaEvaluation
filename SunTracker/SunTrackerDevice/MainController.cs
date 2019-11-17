using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace SunTrackerDevice
{
    [BoardConfig(Name = "Quokka")]
    public static class MainController
    {
        public static async Task Aggregator(
            // blinker
            FPGA.OutputSignal<bool> LED1,

            // IO banks for Quokka board, not needed for another boards
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,
            FPGA.OutputSignal<bool> Bank5,

            // ADC
            FPGA.OutputSignal<bool> ADC1NCS,
            FPGA.OutputSignal<bool> ADC1SLCK,
            FPGA.OutputSignal<bool> ADC1DIN,
            FPGA.InputSignal<bool> ADC1DOUT,

            // UART
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);
            QuokkaBoard.OutputBank(Bank5);
            IsAlive.Blink(LED1);

            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            const int servosCount = 4;
            byte[] servosData = new byte[servosCount] 
            { 
                90, // main panel starts lookup up
                90, // main panel starts lookup up
                50, // sensor panel starts at beginning of measurement cycle
                50, // sensor panel starts at beginning of measurement cycle
            };
            object servosLock = new object();
            bool autoScan = true;

            Sequential servoHandler = () =>
            {
                uint instanceId = FPGA.Config.InstanceId();
                var servoOutputPin = new FPGA.OutputSignal<bool>();
                byte value = 0;
                bool servoOutput = false;
                byte requestValue = 0;

                FPGA.Config.Link(servoOutput, servoOutputPin);

                while (true)
                {
                    requestValue = servosData[instanceId];

                    if (FPGA.Config.InstanceId() <= 1)
                    {
                        // large panel moves with smoothing
                        if (requestValue != value)
                        {
                            if (requestValue < value)
                            {
                                value--;
                            }
                            else
                            {
                                value++;
                            }
                        }
                    }
                    else
                    {
                        value = requestValue;
                    }

                    MG996R.Write(value, out servoOutput);
                }
            };

            FPGA.Config.OnStartup(servoHandler, servosCount);

            Sequential autoScanHandler = () =>
            {
                while (true)
                {
                    if (!autoScan)
                        continue;

                    byte s2Delta = 5;
                    sbyte s3Delta = 5;
                    ushort maxChannel1Value = 0;
                    byte s2Max = 0, s3Max = 0;

                    // reset sensor panel to center
                    for (byte i = 2; i < servosCount; i++)
                    {
                        servosData[i] = 90;
                    }
                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(50));

                    byte s3 = 50;

                    for (byte s2 = 50; s2 <= 130; s2 += s2Delta)
                    {
                        servosData[2] = s2;
                        while (s3 >= 50 && s3 <= 130)
                        {
                            servosData[3] = s3;

                            // TODO: barrier sync all servo handlers
                            FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(50));

                            ushort adcChannel1Value = 0, adcChannel2Value = 0;
                            ADC102S021.Read(out adcChannel1Value, out adcChannel2Value, ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT);

                            if (adcChannel1Value > maxChannel1Value)
                            {
                                s2Max = s2;
                                s3Max = s3;
                                maxChannel1Value = adcChannel1Value;

                                byte[] buff = new byte[3];
                                byte tmp;
                                buff[0] = 255;
                                buff[1] = s2Max;
                                buff[2] = s3Max;

                                for (byte i = 0; i < buff.Length; i++)
                                {
                                    tmp = buff[i];
                                    UART.Write(115200, tmp, internalTXD);
                                }
                            }

                            s3 = (byte)(s3 + s3Delta);
                        }
                        s3Delta = (sbyte)(-s3Delta);
                        s3 = (byte)(s3 +  s3Delta); // compensate for overshoot
                    }

                    servosData[1] = (byte)(180 - s2Max); // should be mirrored around 90 as servo is backwards
                    servosData[0] = s3Max; // same value

                    FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1000));
                }
            };

            FPGA.Config.OnStartup(autoScanHandler);
            
            SetServos request = new SetServos();
            FPGA.Signal<bool> requestDeserialized = new FPGA.Signal<bool>();
            JSON.DeserializeFromUART<SetServos>(request, RXD, requestDeserialized);

            Sequential onRequest = () =>
            {
                autoScan = request.autoRun == 0 ? false : true;
                servosData[0] = request.s0;
                servosData[1] = request.s1;
                servosData[2] = request.s2;
                servosData[3] = request.s3;
            };

            FPGA.Config.OnSignal(requestDeserialized, onRequest);
        }
    }
}
