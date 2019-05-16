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
    public static class Memory_ReadWrite
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential mainHandler = () =>
            {
                byte data = 0;
                byte[] buff = new byte[1000];

                for( int i = 0; i < 1000; i++ )
                {
                    UART.Read(115200, RXD, out data);
                    buff[i] = data;
                }

                byte sum = 0;
                for (int i = 0; i < 1000; i++)
                {
                    data = buff[i];
                    sum += data;
                }

                UART.Write(115200, sum, TXD);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);
        }
    }

    public struct TestDTO
    {
        public byte byteValue;
        public bool boolValue;
        public short shortValue;
    }

    [BoardConfig(Name = "NEB")]
    public static class Memory_ReadWriteDTO
    {
        public static void Handler(byte seed, out int sum)
        {
            Controllers.TestDTO[] buff = new Controllers.TestDTO[100];

            for (int i = 0; i < buff.Length; i++)
            {
                Controllers.TestDTO memWrite = new Controllers.TestDTO();
                memWrite.byteValue = (byte)i;
                memWrite.boolValue = FPGA.Config.Bit(i, 0);
                memWrite.shortValue = (short)(seed + i);

                buff[i] = memWrite;
            }

            sum = 0;
            for (int i = 0; i < buff.Length; i++)
            {
                Controllers.TestDTO memRead = new Controllers.TestDTO();
                memRead = buff[i];
                sum += memRead.byteValue + (memRead.boolValue ? 5 : 10) + memRead.shortValue;
            }
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential mainHandler = () =>
            {
                byte seed = 0;
                UART.Read(115200, RXD, out seed);

                int sum = 0;
                Handler(seed, out sum);

                UART.Write(115200, (byte)sum, TXD);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Memory_ConstLength
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Sequential mainHandler = () =>
            {
                const uint buffLength = 1000;
                byte data = 0;
                byte[] buff = new byte[buffLength];

                for (int i = 0; i < buff.Length; i++)
                {
                    UART.Read(115200, RXD, out data);
                    buff[i] = data;
                }

                byte sum = 0;
                for (int i = 0; i < buffLength; i++)
                {
                    data = buff[i];
                    sum += data;
                }

                UART.Write(115200, sum, TXD);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Memory_DefaultInit
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            Sequential mainHandler = () =>
            {
                byte data = 0;
                byte[] buff = new byte[] { 1, 2, 3, 4, 5 };

                UART.Read(115200, RXD, out data);

                for (int i = 0; i < buff.Length; i++)
                {
                    byte existing = 0;
                    existing = buff[i];
                    UART.RegisteredWrite(115200, existing, out internalTXD);
                }

            };

            FPGA.Config.OnStartup(mainHandler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Memory_InlineInit
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            Sequential mainHandler = () =>
            {
                byte data = 0;
                byte[] buff = new byte[] { 0, 1, 2, 3, 4 };

                for (int i = 0; i < buff.Length; i++)
                {
                    UART.Read(115200, RXD, out data);
                    UART.RegisteredWrite(115200, data, out internalTXD);
                    byte existing = 0;
                    existing = buff[i];
                    UART.RegisteredWrite(115200, existing, out internalTXD);

                    buff[i] = (byte)(data + existing);
                }

                for (int i = 0; i < buff.Length; i++)
                {
                    data = buff[i];
                    UART.RegisteredWrite(115200, data, out internalTXD);
                }

                byte sum = 0;
                for (int i = 0; i < buff.Length; i++)
                {
                    data = buff[i];
                    sum += data;
                }

                UART.RegisteredWrite(115200, sum, out internalTXD);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);
        }
    }

    [BoardConfig(Name = "NEB")]
    public static class Memory_Reinit
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            Sequential mainHandler = () =>
            {
                byte data = 0;
                byte[] buff = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };

                UART.Read(115200, RXD, out data);

                for (byte i = 0; i < buff.Length; i++)
                {
                    byte tmp = 0;
                    tmp = buff[i];
                    tmp += data;
                    buff[i] = tmp;

                    UART.RegisteredWrite(115200, tmp, out internalTXD);
                }

                for (byte j = 0; j < buff.Length; j++)
                {
                    buff[j] = j;
                };
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, mainHandler);
        }
    }
}
