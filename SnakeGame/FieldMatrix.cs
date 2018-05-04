using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public static class FieldMatrix
    {
        public static void GetCellTypeByPosition(
            eCellType[] fieldMatrix, 
            Position position, 
            out eCellType cellType)
        {
            byte offset = 0;
            Lookups.PositionToOffset(position, ref offset);
            cellType = fieldMatrix[offset];
        }

        public static void SetCellTypeByPosition(
            eCellType[] fieldMatrix, 
            Position position, 
            eCellType cellType)
        {
            byte offset = 0;
            Lookups.PositionToOffset(position, ref offset);
            fieldMatrix[offset] = cellType;
        }

        public static void Reset(eCellType[] fieldMatrix)
        {
            for (byte row = 0; row < 8; row++)
            {
                for (byte col = 0; col < 8; col++)
                {
                    byte offset = 0;
                    Lookups.RowColToOffset(row, col, ref offset);

                    fieldMatrix[offset] = 0;
                }
            }
        }

        public static void Seed(
            eCellType[] fieldMatrix,
            ref Position head,
            ref Position tail)
        {
            Reset(fieldMatrix);

            tail.row = 3;
            tail.col = 2;
            head = tail;

            const byte seedSnakeLength = 3;
            for (byte i = 0; i < seedSnakeLength - 1; i++)
            {
                SetCellTypeByPosition(fieldMatrix, head, eCellType.SnakeRight);
                Lookups.ApplyDirection(head, ref head, eDirectionType.Right);
            }

            SetCellTypeByPosition(fieldMatrix, head, eCellType.SnakeHead);
        }

        public static void PlaceNextPiece(eCellType[] fieldMatrix, int randomValue)
        {
            byte indexOfEmptyCell = (byte)(randomValue & 63);
            bool foundSpot = false;
            do
            {
                for (byte row = 0; row < 8; row++)
                {
                    for (byte col = 0; col < 8; col++)
                    {
                        byte offset = 0;
                        Lookups.RowColToOffset(row, col, ref offset);
                        eCellType cellType;
                        cellType = fieldMatrix[offset];

                        if (cellType == eCellType.None)
                        {
                            foundSpot = true;
                            if (indexOfEmptyCell == 0)
                            {
                                fieldMatrix[offset] = eCellType.NextPart;
                                return;
                            }
                            else
                            {
                                indexOfEmptyCell--;
                            }
                        }
                    }
                }
            }
            while (foundSpot);

            throw new GameCompletedException();
        }

        public static void DrawCross(eCellType[] fieldMatrix, eCellType color)
        {
            for (byte row = 0; row < 8; row++)
            {
                for (byte col = 0; col < 8; col++)
                {
                    byte offset = 0;
                    Lookups.RowColToOffset(row, col, ref offset);

                    eCellType value = eCellType.None;

                    if (row == col || (7 - row) == col)
                    {
                        value = color;
                    }

                    fieldMatrix[offset] = value;
                }
            }
        }
    }
}
