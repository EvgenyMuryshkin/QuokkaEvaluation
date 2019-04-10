using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indicators
{
    static class Graphics
    {
        public static void Copy(byte[] source, byte[] target)
        {
            for (byte i = 0, tmp = 0; i < target.Length; i++)
            {
                tmp = source[i];
                target[i] = tmp;
            }
        }

        public static void LeftIndicator(byte[] target, uint dim, out uint color)
        {
            byte[] data = new byte[]
            {
                0x03, // bottom row
                0x07,
                0x0E,
                0x9C,
                0xB8,
                0xF0,
                0xE0,
                0xF8, // top row
            };

            Copy(data, target);
            color = dim;
        }

        public static void RightIndicator(byte[] target, uint dim, out uint color)
        {
            byte[] data = new byte[]
            {
                0x1F, // bottom row
                0x07,
                0x0F,
                0x1D,
                0x39,
                0x70,
                0xE0,
                0xC0, // top row
            };

            Copy(data, target);
            color = dim << 8;
        }

        public static void ClearIndicator(byte[] target, uint dim, out uint color)
        {
            for(byte i = 0; i < target.Length; i++)
            {
                target[i] = 0;
            }

            color = 0;
        }


        public static void BreakIndicator(byte[] target, uint dim, out uint color)
        {
            byte[] data = new byte[]
            {
                0x18, // bottom row
                0x18,
                0x18,
                0xFF,
                0xFF,
                0x18,
                0x18,
                0x18, // top row
            };

            Copy(data, target);
            color = dim << 16;
        }

        public static void DrawIndicator(byte[] data, uint color, out bool DOUT)
        {
            uint[] buff = new uint[data.Length * 8];
            byte value = 0;
            int idx = 0;
            for (byte i = 0; i < data.Length; i++)
            {
                value = data[i];

                byte bit = 1;
                for (byte b = 0; b < 8; b++)
                {
                    if ((bit & value) == 0)
                    {
                        buff[idx] = 0;
                    }
                    else
                    {
                        buff[idx] = color;
                    }

                    bit = (byte)(bit << 1);
                    idx++;
                }
            }

            Draw(buff, out DOUT);
        }

        public static void Draw(uint[] buff, out bool DOUT)
        {
            WS2812B.SyncWrite(buff, 0, buff.Length, out DOUT);
        }
    }
}
