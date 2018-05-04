using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    static class Graphics
    {
        public static void DrawFieldMatrix(
            uint baseColor,
            eCellType[] matrix, 
            out bool DOUT)
        {
            uint[] buff = new uint[matrix.Length];
            uint color = 0;

            for (byte idx = 0; idx < buff.Length; idx++)
            {
                eCellType value = eCellType.None;
                value = matrix[idx];

                FPGA.Collections.ReadOnlyDictionary<eCellType, uint> mapColors = new FPGA.Collections.ReadOnlyDictionary<eCellType, uint>()
                    {
                        {  eCellType.SnakeUp, baseColor },
                        {  eCellType.SnakeDown, baseColor },
                        {  eCellType.SnakeLeft, baseColor },
                        {  eCellType.SnakeRight, baseColor },
                        {  eCellType.SnakeHead, baseColor * 2 },
                        {  eCellType.NextPart, baseColor << 8 },
                        {  eCellType.RedCross, baseColor << 16 },
                        {  eCellType.GreenCross, baseColor << 8 },
                    };

                color = mapColors[value];
                buff[idx] = color;
            }

            Draw(buff, out DOUT);
        }

        public static void Draw(uint[] buff, out bool DOUT)
        {
            WS2812B.SyncWrite(buff, 0, buff.Length, out DOUT);
        }
    }
}
