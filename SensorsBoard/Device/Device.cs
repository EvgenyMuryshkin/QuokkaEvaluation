using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace SensorsBoard
{
    [BoardConfig(Name = "Quokka")]
    public static class Device
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> TXD,
            FPGA.InputSignal<bool> RXD
            )
        {
            const uint baud = 115200;
            const uint tasksCount = 10;
            ushort[] buff = new ushort[tasksCount];
            FPGA.Signal<bool> tasksTrigger = false;
            object buffLock = new object();
            uint completedTasks = 0;
            Func<bool> tasksCompleted = () => completedTasks == tasksCount;
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            Sequential onStartup = () =>
            {
                while(true)
                {
                    byte cmd = UART.Read(baud, RXD);
                    lock(buffLock)
                    {
                        completedTasks = 0;
                        tasksTrigger = true;
                    }

                    FPGA.Runtime.WaitForAllConditions(tasksCompleted);

                    for (uint idx = 0; idx < buff.Length; idx++)
                    {
                        ushort data = 0;
                        data = buff[idx];
                        UART.RegisteredWrite(baud, (byte)data, out internalTXD);
                        UART.RegisteredWrite(baud, (byte)(data >> 8), out internalTXD);
                    }
                }
            };

            FPGA.Config.OnStartup(onStartup);

            Sequential taskHandler = () =>
            {
                uint taskIndex = FPGA.Config.InstanceId();

                FPGA.OutputSignal<bool> ADC1NCS = new FPGA.OutputSignal<bool>();
                FPGA.OutputSignal<bool> ADC1SLCK = new FPGA.OutputSignal<bool>();
                FPGA.OutputSignal<bool> ADC1DIN = new FPGA.OutputSignal<bool>();
                FPGA.InputSignal<bool> ADC1DOUT = new FPGA.InputSignal<bool>();

                ushort adcChannel1Value = 0, adcChannel2Value = 0;
                ADC102S021.Read(out adcChannel1Value, out adcChannel2Value, ADC1NCS, ADC1SLCK, ADC1DIN, ADC1DOUT);

                lock(buffLock)
                {
                    // TODO: inline expression in memory accessor
                    uint offset1 = taskIndex * 2, offset2 = offset1 + 1;
                    buff[offset1] = adcChannel1Value;
                    buff[offset2] = adcChannel2Value;
                    completedTasks++;
                }
            };

            FPGA.Config.OnSignal(tasksTrigger, taskHandler, tasksCount);
        }
    }
}
