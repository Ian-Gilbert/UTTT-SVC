using System.Collections;
using System.Collections.Generic;
using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class TicTacToeBoardTests
    {
        private readonly Move move;
        private readonly ITicTacToeBoard ticTacToeBoard;

        public static IEnumerable<object[]> threeInARows =>
                new List<object[]>()
                {
                // horizontal
                new object[] {new int[] {0, 1, 2}},
                new object[] {new int[] {3, 4, 5}},
                new object[] {new int[] {6, 7, 8}},

                // vertical
                new object[] {new int[] {0, 3, 6}},
                new object[] {new int[] {1, 4, 7}},
                new object[] {new int[] {2, 5, 8}},

                // diagonal
                new object[] {new int[] {0, 4, 8}},
                new object[] {new int[] {2, 4, 6}},
                };

        public static IEnumerable<object[]> IndexRange
        {
            get
            {
                var list = new List<object[]>();
                for (int i = 0; i < 9; i++)
                {
                    list.Add(new object[] { i });
                }
                return list;
            }
        }

        public TicTacToeBoardTests()
        {
            move = new Move() { Mark = MarkType.PLAYER1, LbIndex = 0, MarkIndex = 0 };
            ticTacToeBoard = new TicTacToeBoard();
        }

        [Fact]
        public void TicTacToeBoard_Initializes9EmptyMarks()
        {
            Assert.Equal(9, ticTacToeBoard.Board.Length);
            foreach (var mark in ticTacToeBoard.Board)
            {
                Assert.Equal(MarkType.EMPTY, mark);
            }
        }

        [Fact]
        public void HasTicTacToe_ReturnsFalse_WhenThereIsNoThreeInARow()
        {
            var result = ticTacToeBoard.HasTicTacToe(move.Mark);
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(threeInARows))]
        public void HasTicTacToe_ReturnsTrue_WhenThereIsAThreeInARow(int[] marks)
        {
            foreach (var mark in marks)
            {
                ticTacToeBoard.Board[mark] = move.Mark;
            }

            var result = ticTacToeBoard.HasTicTacToe(move.Mark);

            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(IndexRange))]
        public void IsFull_ReturnsFalse_WhenAnyMarkIsEmpty(int index)
        {
            for (int i = 0; i < 9; i++)
            {
                ticTacToeBoard.Board[i] = MarkType.PLAYER1;
            }
            ticTacToeBoard.Board[index] = MarkType.EMPTY;

            var result = ticTacToeBoard.IsFull();

            Assert.False(result);
        }

        [Fact]
        public void IsFull_ReturnsTrue_WhenBoardIsFull()
        {
            for (int i = 0; i < 9; i++)
            {
                ticTacToeBoard.Board[i] = MarkType.PLAYER1;
            }

            var result = ticTacToeBoard.IsFull();

            Assert.True(result);
        }

        [Fact]
        public void MarkBoard_MarksBoard() // wait, that's what the method does???
        {
            ticTacToeBoard.MarkBoard(move);

            Assert.Equal(move.Mark, ticTacToeBoard.Board[move.MarkIndex]);
        }
    }
}