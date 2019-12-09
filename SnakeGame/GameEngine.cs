using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public static class GameEngine
    {
        public static void Setup(
            GameControlsState controlsState,
            eCellType[] fieldMatrix,
            Position head,
            Position tail,
            ref eDirectionType currentDirection,
            int randomValue,
            ref eGameMode gameMode,
            ref byte baseColor)
        {
            // seed initial state
            FieldMatrix.Seed(fieldMatrix, head, tail);
            currentDirection = eDirectionType.None;

            switch (controlsState.keyCode)
            {
                case KeypadKeyCode.GO:
                    FieldMatrix.PlaceNextPiece(fieldMatrix, randomValue);
                    gameMode = eGameMode.Play;
                    break;
                case KeypadKeyCode.D2:
                    baseColor = (byte)(baseColor < 10 ? baseColor + 1 : baseColor);
                    break;
                case KeypadKeyCode.D8:
                    baseColor = (byte)(baseColor > 1 ? baseColor - 1 : baseColor);
                    break;
            }
        }   
        
        public static void GameIteration(
            GameControlsState controlsState,
            eCellType[] fieldMatrix,
            Position head,
            Position tail,
            ref eDirectionType currentDirection,
            int randomValue,
            FPGA.Signal<bool> TXD)
        {

            eDirectionType nextDirectionFromKeypad = eDirectionType.None;
            eDirectionType nextDirectionFromJoystick = eDirectionType.None;
            eDirectionType nextDirection = eDirectionType.None;

            Lookups.KeyCodeToDirection(
                controlsState.keyCode,
                ref nextDirectionFromKeypad);

            Lookups.JoystickPositionToDirection(
                controlsState.adcChannel1,
                controlsState.adcChannel2,
                ref nextDirectionFromJoystick);

            bool isReverse = false;
            Lookups.IsReverse(currentDirection, nextDirection, ref isReverse);

            // TODO: variable declaration from conditional expression;
            nextDirection = isReverse 
                            ? currentDirection 
                            : nextDirectionFromKeypad != eDirectionType.None
                                    ? nextDirectionFromKeypad
                                        : nextDirectionFromJoystick != eDirectionType.None
                                            ? nextDirectionFromJoystick
                                            : currentDirection;

            Diagnostics.ReportState(
                controlsState,
                nextDirectionFromKeypad,
                nextDirectionFromJoystick,
                nextDirection,
                TXD);

            bool expanded = false;
            ApplyChange(
                fieldMatrix,
                head, 
                tail,
                currentDirection, nextDirection,
                out expanded);

            if (expanded)
                FieldMatrix.PlaceNextPiece(fieldMatrix, randomValue);

            currentDirection = nextDirection;
        }

        public static void ApplyChange(
            eCellType[] fieldMatrix,
            Position head,
            Position tail,
            eDirectionType currentDirection,
            eDirectionType nextDirection,
            out bool expanded)
        {
            expanded = false;

            if (nextDirection == eDirectionType.None)
            {
                return;
            }

            eCellType nextDirectionCellType = eCellType.None;
            Lookups.DirectionTypeToCellType(nextDirection, ref nextDirectionCellType);

            Position nextHeadPosition = new Position();
            Lookups.ApplyDirection(head, nextHeadPosition, nextDirection);
            
            ThrowIfCrashed(fieldMatrix, nextHeadPosition);
            
            eCellType tailCellType, nextHeadCellType;
            FieldMatrix.GetCellTypeByPosition(fieldMatrix, tail, out tailCellType);
            FieldMatrix.GetCellTypeByPosition(fieldMatrix, nextHeadPosition, out nextHeadCellType);

            // move head
            FieldMatrix.SetCellTypeByPosition(fieldMatrix, head, nextDirectionCellType);
            FieldMatrix.SetCellTypeByPosition(fieldMatrix, nextHeadPosition, eCellType.SnakeHead);

            FPGA.Runtime.DeepCopy(head, nextHeadPosition);
            
            if (nextHeadCellType == eCellType.NextPart)
            {
                expanded = true;
                return;
            }

            // move tail
            eDirectionType tailDirection = eDirectionType.None;
            // get value at current tail

            Lookups.CellTypeToDirectionType(tailCellType, ref tailDirection);

            // clear current tail
            FieldMatrix.SetCellTypeByPosition(fieldMatrix, tail, eCellType.None);

            // move tail
            Lookups.ApplyDirection(tail, tail, tailDirection);
        }

        public static void ThrowIfCrashed(eCellType[] fieldMatrix, Position position)
        {
            bool isCrashed = position.row > 7 || position.col > 7;

            if (isCrashed)
            {
                throw new CrashedInWallException();
            }

            eCellType type;
            FieldMatrix.GetCellTypeByPosition(fieldMatrix, position, out type);

            var crashTypes = new FPGA.Collections.ReadOnlyDictionary<eCellType, bool>()
            {
                { eCellType.SnakeHead, true },
                { eCellType.SnakeDown, true },
                { eCellType.SnakeUp, true },
                { eCellType.SnakeLeft, true },
                { eCellType.SnakeRight, true }
            };

            isCrashed = crashTypes[type];
            if (isCrashed)
            {
                throw new CrashedInSnakeException();
            }
        }
    }
}
