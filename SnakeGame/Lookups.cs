using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public static class Lookups
    {
        [Inlined]
        public static void PositionToOffset(
            Position position,
            ref byte offset)
        {
            RowColToOffset(position.row, position.col, ref offset);
        }

        [Inlined]
        public static void RowColToOffset(
            byte row,
            byte col,
            ref byte offset
            )
        {
            offset = (byte)(row * 8 + col);
        }

        public static void JoystickPositionToDirection(
            int xAxis,
            int yAxis,
            ref eDirectionType direction)
        {
            if (xAxis < 20000)
            {
                direction = eDirectionType.Up;
            }
            else if (xAxis > 40000)
            {
                direction = eDirectionType.Down;
            }
            else if (yAxis < 20000)
            {
                direction = eDirectionType.Right;
            }
            else if (yAxis > 40000)
            {
                direction = eDirectionType.Left;
            }
            else
            {
                direction = eDirectionType.None;
            }
        }

        public static void KeyCodeToDirection(
            KeypadKeyCode keyCode, 
            ref eDirectionType direction)
        {
            var lookup = new FPGA.Collections.ReadOnlyDictionary<KeypadKeyCode, eDirectionType>()
            {
                { KeypadKeyCode.D2, eDirectionType.Up },
                { KeypadKeyCode.D8, eDirectionType.Down },
                { KeypadKeyCode.D4, eDirectionType.Left },
                { KeypadKeyCode.D6, eDirectionType.Right }
            };

            direction = lookup[keyCode];
        }

        public static void CellTypeToDirectionType(
            eCellType cellType, 
            ref eDirectionType directionType)
        {
            var lookup = new FPGA.Collections.ReadOnlyDictionary<eCellType, eDirectionType>()
            {
                { eCellType.SnakeUp, eDirectionType.Up },
                { eCellType.SnakeDown, eDirectionType.Down },
                { eCellType.SnakeLeft, eDirectionType.Left },
                { eCellType.SnakeRight, eDirectionType.Right }
            };

            directionType = lookup[cellType];
        }

        public static void DirectionTypeToCellType(
            eDirectionType directionType,
            ref eCellType cellType
            )
        {
            var lookup = new FPGA.Collections.ReadOnlyDictionary<eDirectionType, eCellType>()
            {
                { eDirectionType.Up, eCellType.SnakeUp },
                { eDirectionType.Down, eCellType.SnakeDown },
                { eDirectionType.Left, eCellType.SnakeLeft },
                { eDirectionType.Right, eCellType.SnakeRight }
            };

            cellType = lookup[directionType];
        }

        public static void IsReverse(
            eDirectionType currentDirection,
            eDirectionType nextDirection,
            ref bool shouldReverse)
        {
            shouldReverse =
                currentDirection == eDirectionType.Up && nextDirection == eDirectionType.Down ||
                currentDirection == eDirectionType.Down && nextDirection == eDirectionType.Up ||
                currentDirection == eDirectionType.Left && nextDirection == eDirectionType.Right ||
                currentDirection == eDirectionType.Right && nextDirection == eDirectionType.Left;
        }

        public static void ApplyDirection(
            Position current, 
            ref Position next, 
            eDirectionType direction)
        {
            next = current;

            switch (direction)
            {
                case eDirectionType.Up:
                    next.row--;
                    break;
                case eDirectionType.Down:
                    next.row++;
                    break;
                case eDirectionType.Left:
                    next.col--;
                    break;
                case eDirectionType.Right:
                    next.col++;
                    break;
            }
        }

    }
}
