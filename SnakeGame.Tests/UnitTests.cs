using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SnakeGame.Tests
{
    [TestClass]
    public class UnitTests
    {
        byte TestOffset(byte row, byte col)
        {
            byte offset = 0;
            Lookups.RowColToOffset(row, col, ref offset);
            return offset;
        }
        
        byte TestOffset(Position pos) => TestOffset(pos.row, pos.col);

        [TestMethod]
        public void SnakeGame_GameIteration_SimpleMove()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];
            field[0] = eCellType.SnakeHead;
            eDirectionType currentDirection = eDirectionType.None, nextDirection = eDirectionType.None;

            GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded);
            Assert.AreEqual(eCellType.SnakeHead, field[0], "Head should not move");

            nextDirection = eDirectionType.Right;
            GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out expanded);

            Assert.AreEqual(eCellType.None, field[0], "Tail did not move");
            Assert.AreEqual(eCellType.SnakeHead, field[1], "Head did not move");
        }

        [TestMethod]
        public void SnakeGame_GameIteration_StraightMove()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];

            FieldMatrix.Seed(field, head, tail);
            eDirectionType currentDirection = eDirectionType.None, nextDirection = eDirectionType.Right;

            GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded);

            Assert.AreEqual(3, field.Count(c => c != eCellType.None));

            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 3)], "Tail did not move to the right");
            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 4)], "Body did not move to the right");
            Assert.AreEqual(eCellType.SnakeHead, field[TestOffset(3, 5)], "Head did not move to the right");
        }

        [TestMethod]
        public void SnakeGame_GameIteration_TurnUp()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];

            FieldMatrix.Seed(field, head, tail);
            eDirectionType currentDirection = eDirectionType.None, nextDirection = eDirectionType.Up;

            GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded);

            Assert.AreEqual(3, field.Count(c => c != eCellType.None));

            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 3)], "Tail did not move to the right");
            Assert.AreEqual(eCellType.SnakeUp, field[TestOffset(3, 4)], "Body did not move to the right");
            Assert.AreEqual(eCellType.SnakeHead, field[TestOffset(2, 4)], "Head did not move up");
        }

        [TestMethod]
        public void SnakeGame_GameIteration_Expanded()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];

            FieldMatrix.Seed(field, head, tail);

            field[TestOffset(3, 5)] = eCellType.NextPart;
            eDirectionType currentDirection = eDirectionType.None, nextDirection = eDirectionType.Right;

            GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded);

            Assert.IsTrue(expanded, "Snake did not expand");

            Assert.AreEqual(4, field.Count(c => c != eCellType.None));

            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 2)], "Tail moved");
            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 3)], "Body moved");
            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 4)], "Body moved");
            Assert.AreEqual(eCellType.SnakeHead, field[TestOffset(3, 5)], "Head did not extend");
        }

        [TestMethod]
        public void SnakeGame_GameIteration_TurnDown()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];

            FieldMatrix.Seed(field, head, tail);
            eDirectionType currentDirection = eDirectionType.None, nextDirection = eDirectionType.Down;

            GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded);

            Assert.AreEqual(3, field.Count(c => c != eCellType.None));

            Assert.AreEqual(eCellType.SnakeRight, field[TestOffset(3, 3)], "Tail did not move to the right");
            Assert.AreEqual(eCellType.SnakeDown, field[TestOffset(3, 4)], "Body did not move to the right");
            Assert.AreEqual(eCellType.SnakeHead, field[TestOffset(4, 4)], "Head did not move down");
        }

        [TestMethod]
        public void SnakeGame_GameIteration_SnakeCrash()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];

            FieldMatrix.Seed(field, head, tail);
            eDirectionType currentDirection = eDirectionType.None, nextDirection = eDirectionType.Right;

            FieldMatrix.SetCellTypeByPosition(field, new Position() { row = head.row, col = (byte)(head.col + 1) }, eCellType.SnakeDown);

            Assert.ThrowsException<CrashedInSnakeException>(() => GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded), "Did not crash moving to the right");

            FieldMatrix.Seed(field, head, tail);

            FieldMatrix.SetCellTypeByPosition(field, new Position() { row = (byte)(head.row + 1), col = head.col }, eCellType.SnakeHead);
            nextDirection = eDirectionType.Down;

            Assert.ThrowsException<CrashedInSnakeException>(() => GameEngine.ApplyChange(field, head, tail, currentDirection, nextDirection, out bool expanded), "Did not crash moving down");
        }

        [DataTestMethod]
        [DataRow(0, 0, eDirectionType.Up)]
        [DataRow(0, 0, eDirectionType.Left)]
        [DataRow(7, 7, eDirectionType.Right)]
        [DataRow(7, 7, eDirectionType.Down)]
        public void SnakeGame_GameIteration_WallCrash(int row, int col, eDirectionType direction)
        {
            Position    head = new Position() { row = (byte)row, col = (byte)col }, 
                        tail = new Position() { row = (byte)row, col = (byte)col };

            eCellType[] field = new eCellType[64];

            byte offset = 0;
            Lookups.PositionToOffset(head, ref offset);
            field[offset] = eCellType.SnakeHead;

            Assert.ThrowsException<CrashedInWallException>(() => GameEngine.ApplyChange(field, head, tail, eDirectionType.None, direction, out bool expanded));
        }

        [TestMethod]
        public void SnakeGame_PlaceNextPiece()
        {
            Position head = new Position(), tail = new Position();
            eCellType[] field = new eCellType[64];

            FieldMatrix.Seed(field, head, tail);
            var rnd = new Random();

            while(field.Any(f => f == eCellType.None))
            {
                var currentEmpty = field.Count(f => f == eCellType.None);

                FieldMatrix.PlaceNextPiece(field, (byte)rnd.Next());

                var nextEmpty = field.Count(f => f == eCellType.None);

                Assert.AreEqual(currentEmpty - 1, nextEmpty, "Did not place next piece");
            }

            Assert.ThrowsException<GameCompletedException>(() => FieldMatrix.PlaceNextPiece(field, 10));
        }
    }
}
