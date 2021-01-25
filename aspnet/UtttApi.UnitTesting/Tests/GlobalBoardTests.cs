using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class GlobalBoardTests
    {
        private readonly Move move;

        public GlobalBoard gb { get; set; }

        public GlobalBoardTests()
        {
            move = new Move() { Mark = MarkType.PLAYER1, LbIndex = 0, MarkIndex = 0 };

            gb = new GlobalBoard();
        }

        [Fact]
        public void GlobalBoard_Initializes9LocalBoards()
        {
            Assert.Equal(9, gb.LocalBoards.Length);
            foreach (var lb in gb.LocalBoards)
            {
                Assert.NotNull(lb);
            }
        }

        [Fact]
        public void GlobalBoard_ShouldCreateLocalBoards()
        {
            Assert.IsType<LocalBoard[]>(gb.LocalBoards);
            Assert.Equal(9, gb.LocalBoards.Length);
        }

        [Fact]
        public void IsValidMove_ReturnsFalse_WhenLbIsNotInFocus()
        {
            gb.LocalBoards[move.LbIndex].Focus = false;

            var result = gb.IsValidMove(move);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMove_ReturnsFalse_WhenMarkIsNotEmpty()
        {
            gb.LocalBoards[move.LbIndex].Board[move.MarkIndex] = MarkType.DRAW;

            var result = gb.IsValidMove(move);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMove_ReturnsTrue_WhenMoveIsValid()
        {
            var result = gb.IsValidMove(move);

            Assert.True(result);
        }

        [Fact]
        public void MakeMove_MarksLocalBoard()
        {
            gb.MakeMove(move);

            Assert.Equal(move.Mark, gb.LocalBoards[move.LbIndex].Board[move.MarkIndex]);
        }

        [Fact]
        public void MakeMove_MarksGlobalBoard_WhenLocalBoardHasTicTacToe()
        {
            for (int i = 0; i < 9; i++)
            {
                gb.LocalBoards[move.LbIndex].Board[i] = move.Mark;
            }

            gb.MakeMove(move);

            Assert.Equal(move.Mark, gb.Board[move.LbIndex]);
        }

        [Fact]
        public void MakeMove_MarksGlobalBoard_WhenLocalBoardDraws()
        {
            for (int i = 0; i < 9; i++)
            {
                gb.LocalBoards[move.LbIndex].Board[i] = MarkType.DRAW;
            }

            gb.MakeMove(move);

            Assert.Equal(MarkType.DRAW, gb.Board[move.LbIndex]);
        }

        [Fact]
        public void UpdateFocus_ShouldSetEverythingToFalse_WhenGameIsFinished()
        {
            gb.UpdateFocus(move, GameStatus.DRAW);

            foreach (var lb in gb.LocalBoards)
            {
                Assert.False(lb.Focus);
                Assert.False(lb.Playable);
            }
        }

        [Fact]
        public void UpdateFocus_ShouldSetOnlyNextLbInFocus_WhenNextLbIsPlayableAndGameIsInProgress()
        {
            var nextLb = gb.LocalBoards[move.MarkIndex];

            gb.UpdateFocus(move, GameStatus.IN_PROGRESS);

            foreach (var lb in gb.LocalBoards)
            {
                if (lb == nextLb)
                {
                    Assert.True(lb.Focus);
                }
                else
                {
                    Assert.False(lb.Focus);
                }
            }
        }

        [Fact]
        public void UpdateFocus_ShouldSetAllPlayableLbsInFocus_WhenNextLbIsNotPlayableAndGameIsInProgress()
        {
            var nextLb = gb.LocalBoards[move.MarkIndex];
            nextLb.Playable = false;

            gb.UpdateFocus(move, GameStatus.IN_PROGRESS);

            foreach (var lb in gb.LocalBoards)
            {
                Assert.Equal(lb.Playable, lb.Focus);
            }
        }
    }
}