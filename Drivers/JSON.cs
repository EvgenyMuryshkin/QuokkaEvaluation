using FPGA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class JSON
    {
        public static void DeserializeFromUART<T>(
            ref T obj, 
            FPGA.InputSignal<bool> RXD, 
            FPGA.Signal<bool> deserialized) where T : new()
        {
            byte data = 0;
            FPGA.Config.JSONDeserializer(obj, data, deserialized);

            const bool trigger = true;
            Sequential uartHandler = () =>
            {
                while(true)
                {
                    UART.Read(115200, RXD, out data);
                }
            };

            FPGA.Config.OnSignal(trigger, uartHandler);
        }

        public static void SerializeToUART<T>(ref T data, FPGA.Signal<bool> TXD)
        {
            FPGA.Signal<bool> hasMoreData, triggerDequeue, dataDequeued;
            FPGA.Signal<byte> currentByte;

            FPGA.Config.JSONSerializer(
                data,
                out hasMoreData,
                out triggerDequeue,
                out dataDequeued,
                out currentByte);

            while (hasMoreData)
            {
                triggerDequeue = true;
                FPGA.Runtime.WaitForAllConditions(dataDequeued);
                UART.Write(115200, currentByte, TXD);
            }
        }
    }
}
