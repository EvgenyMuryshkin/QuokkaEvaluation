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
    public static class Streams_SyncStream_Single
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            var stream = new FPGA.SyncStream<byte>();

            Sequential<byte> streamHandler = (value) =>
            {
                UART.Write(115200, value, TXD);
            };

            FPGA.Config.OnStream(stream, streamHandler);

            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);
                for(ushort i = 0; i < data; i++)
                {
                    stream.Write((byte)i);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Streams_SyncStream_Multiple
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            var stream = new FPGA.SyncStream<byte>();
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);
            object txdLock = new object();

            Sequential<byte> streamHandler = (value) =>
            {
                lock(txdLock)
                {
                    UART.RegisteredWrite(115200, value, out internalTXD);
                }
            };

            FPGA.Config.OnStream(stream, streamHandler, 2);

            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);
                for (ushort i = 0; i < data; i++)
                {
                    stream.Write((byte)i);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Streams_SyncStream_Chained
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);
            object txdLock = new object();

            byte counter = 0;
            object counterLock = new object();

            var transmitterStream = new FPGA.SyncStream<byte>();
            Sequential<byte> transmitterStreamHandler = (value) =>
            {
                lock (txdLock)
                {
                    UART.RegisteredWrite(115200, value, out internalTXD);
                }
            };
            FPGA.Config.OnStream(transmitterStream, transmitterStreamHandler, 2);

            var receiverStream = new FPGA.SyncStream<byte>();
            Sequential<byte> receiverStreamHandler = (value) =>
            {
                Func<uint> instanceId = () => FPGA.Config.InstanceId();

                byte increment = 0;
                lock(counterLock)
                {
                    increment = counter;
                    counter++;
                }
                transmitterStream.Write((byte)(value + increment + instanceId()));
            };
            FPGA.Config.OnStream(receiverStream, receiverStreamHandler, 3);

            Sequential handler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);
                for (ushort i = 0; i < data; i++)
                {
                    receiverStream.Write((byte)i);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
