using System;
using System.Threading.Tasks;

namespace Drivers
{
    public class UART
    {
        private readonly uint Baud;

        public UART(uint baud)
        {
            Baud = baud;
        }

        // module has one non-registered input bit, and one registered output byte
        public static void Read(uint baud, FPGA.InputSignal<bool> RXD, out byte data)
        {
            FPGA.Const<ulong> delay = 1000000000 / baud;

            byte result = 0;

            // all combinational logic is expressed as delegates
            Func<bool> invertedRXD = () => !RXD;

            // wait for start bit
            FPGA.Runtime.WaitForAllConditions(invertedRXD);

            // wait for half bit time to allow some time shift errors
            FPGA.Runtime.Delay(delay / 2);

            // read 8 bits
            for (uint i = 0; i < 8; i++)
            {
                FPGA.Config.SetInclusiveRange(0, 8, i);
                FPGA.Runtime.Delay(delay);

                // this is assign of combinational expression
                // evaluated and assigned during single clock cycle 
                result = (byte)((result >> 1) | ((byte)RXD << 7));
            }

            // stop bit
            FPGA.Runtime.Delay(delay);

            // assign result and complete method call
            data = result;
        }

        public static void Write(uint baud, byte data, FPGA.Signal<bool> TXD)
        {
            // default TXD is high
            bool internalTXD = true;

            // hardlink from register to output signal, it has to hold its value
            FPGA.Config.Link(internalTXD, TXD);

            UART.RegisteredWrite(baud, data, out internalTXD);
        }

        public static void RegisteredWrite(uint baud, byte data, out bool TXD)
        {
            FPGA.Const<ulong> delay = 1000000000 / baud;

            byte stored = data;

            // start bit
            TXD = false;
            FPGA.Runtime.Delay(delay);

            // write data bits
            for (byte i = 0; i < 8; i++)
            {
                FPGA.Config.SetInclusiveRange(0, 8, i);
                TXD = (stored & 1) > 0;
                FPGA.Runtime.Delay(delay);
                stored = (byte)(stored >> 1);
            }

            // stop bit
            TXD = true;
            FPGA.Runtime.Delay(delay);
            FPGA.Runtime.Delay(delay);
        }

        public static void ReadUnsigned32(uint baud, FPGA.InputSignal<bool> RXD, ref uint data)
        {
            byte part = 0;
            for (byte i = 0; i < 4; i++)
            {
                FPGA.Config.SetInclusiveRange(0, 4, i);
                UART.Read(baud, RXD, out part);
                data = (uint)((data >> 8) | (part << 24));
            }
        }

        public static void ReadFloat(uint baud, FPGA.InputSignal<bool> RXD, out float data)
        {
            uint uns = 0;
            UART.ReadUnsigned32(baud, RXD, ref uns);
            FPGA.Runtime.Assign(FPGA.Expressions.Unchecked(uns, out data));
        }

        public static void WriteUnsigned32(uint baud, uint data, FPGA.Signal<bool> TXD)
        {
            // default TXD is high
            bool internalTXD = true;

            // hardlink from register to output signal, it has to hold its value
            FPGA.Config.Link(internalTXD, TXD);

            UART.RegisteredWriteUnsigned32(baud, data, out internalTXD);
        }

        public static void RegisteredWriteUnsigned32(uint baud, uint data, out bool TXD)
        {
            // cannot use data direclty as it is used by "ref" in hardware
            uint buff = data;
            Func<byte> lsb = () => (byte)buff;
            FPGA.Config.Default(out TXD, true);

            for (byte i = 0; i < 4; i++)
            {
                FPGA.Config.SetInclusiveRange(0, 4, i);
                UART.RegisteredWrite(baud, lsb(), out TXD);
                buff = buff >> 8;
            }
        }

        public static void WriteFloat(uint baud, float data, FPGA.Signal<bool> TXD)
        {
            bool internalTXD = true;

            // hardlink from register to output signal, it has to hold its value
            FPGA.Config.Link(internalTXD, TXD);

            UART.RegisteredWriteFloat(baud, data, out internalTXD);
        }

        public static void RegisteredWriteFloat(uint baud, float data, out bool TXD)
        {
            uint uns = 0;
            FPGA.Runtime.Assign(FPGA.Expressions.Unchecked(data, out uns));

            UART.RegisteredWriteUnsigned32(baud, uns, out TXD);
        }
    }
}
