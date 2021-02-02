using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Exceptions;
using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class UtttObjectTests
    {
        private readonly Move move;
        private readonly UtttObject uttt;

        public UtttObjectTests()
        {
            move = new Move() { Mark = MarkType.PLAYER1, LbIndex = 0, MarkIndex = 0 };

            uttt = new UtttObject();
        }

        [Fact]
        public void UtttObject_InitializesCurrentPlayer_AsPlayer1()
        {
            Assert.Equal(MarkType.PLAYER1, uttt.CurrentPlayer);
        }

        [Theory]
        [InlineData(GameStatus.DRAW)]
        [InlineData(GameStatus.PLAYER1_WINS)]
        [InlineData(GameStatus.PLAYER2_WINS)]
        public void IsValidMove_ReturnsFalse_WhenGameIsNotInProgress(GameStatus status)
        {
            uttt.Status = status;
            string message;
            Assert.False(uttt.IsValidMove(move, out message));
            Assert.NotNull(message);
        }

        [Fact]
        public void IsValidMove_ReturnsFalse_WhenNotPlayersTurn()
        {
            uttt.CurrentPlayer = MarkType.PLAYER2;
            string message;
            Assert.False(uttt.IsValidMove(move, out message));
            Assert.NotNull(message);
        }

        [Fact]
        public void IsValidMove_ReturnsFalse_WhenNextLbIsNotInFocus()
        {
            uttt.GlobalBoard.LocalBoards[move.LbIndex].Focus = false; // target board is not in focus
            string message;
            Assert.False(uttt.IsValidMove(move, out message));
            Assert.NotNull(message);
        }

        [Fact]
        public void IsValidMove_ReturnsFalse_WhenMarkIsNotEmpty()
        {
            uttt.GlobalBoard.LocalBoards[move.LbIndex].Board[move.MarkIndex] = MarkType.DRAW;
            string message;
            Assert.False(uttt.IsValidMove(move, out message));
            Assert.NotNull(message);
        }

        [Fact]
        public void IsValidMove_ReturnsTrue_WhenValidMove()
        {
            Assert.True(uttt.IsValidMove(move, out _));
        }

        [Fact]
        public void SwitchPlayer_SwitchesPlayer() // wait that's what this method does???
        {
            uttt.SwitchCurrentPlayer();
            Assert.Equal(MarkType.PLAYER2, uttt.CurrentPlayer);

            uttt.SwitchCurrentPlayer();
            Assert.Equal(MarkType.PLAYER1, uttt.CurrentPlayer);
        }

        [Theory]
        [InlineData(MarkType.PLAYER1, GameStatus.PLAYER1_WINS)]
        [InlineData(MarkType.PLAYER2, GameStatus.PLAYER2_WINS)]
        public void UpdateGameStatus_SetsStatusToWinner_WhenGlobalBoardHasTicTacToe(MarkType player, GameStatus status)
        {
            for (int i = 0; i < 3; i++)
            {
                uttt.GlobalBoard.Board[i] = player;
            }

            uttt.UpdateGameStatus(player);

            Assert.Equal(status, uttt.Status);
        }

        [Fact]
        public void UpdateGameStatus_SetsStatusToDraw_WhenGlobalBoardIsFull()
        {
            for (int i = 0; i < 9; i++)
            {
                uttt.GlobalBoard.Board[i] = MarkType.DRAW;
            }

            uttt.UpdateGameStatus(MarkType.PLAYER1);

            Assert.Equal(GameStatus.DRAW, uttt.Status);
        }

        [Fact]
        public void MakeMove_ThrowsException_WhenInvalidMove()
        {
            move.Mark = MarkType.PLAYER2;
            var result = Assert.Throws<HttpResponseException>(() => uttt.MakeMove(move));
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void MakeMove_MakesMove_WhenValidMove()
        {
            uttt.MakeMove(move);
            Assert.Equal(move.Mark, uttt.GlobalBoard.LocalBoards[move.LbIndex].Board[move.MarkIndex]);
        }

        [Fact]
        public void MakeMove_UpdatesStatus_WhenGameFinishes()
        {
            for (int i = 1; i < 3; i++)
            {
                uttt.GlobalBoard.Board[i] = MarkType.PLAYER1;
                uttt.GlobalBoard.LocalBoards[move.LbIndex].Board[i] = MarkType.PLAYER1;
            }

            uttt.MakeMove(move);

            Assert.Equal(GameStatus.PLAYER1_WINS, uttt.Status);
        }

        [Fact]
        public void MakeMove_UpdatesFocus()
        {
            move.MarkIndex = 3;
            var nextLb = uttt.GlobalBoard.LocalBoards[move.MarkIndex];

            uttt.MakeMove(move);

            Assert.True(nextLb.Focus);
        }

        [Fact]
        public void MakeMove_SwitchesPlayer_WhenGameIsNotFinished()
        {
            uttt.MakeMove(move);

            Assert.NotEqual(move.Mark, uttt.CurrentPlayer);
        }

        [Fact]
        public void MakeMove_DoesNotSwitchPlayer_WhenGameIsFinished()
        {
            for (int i = 1; i < 3; i++)
            {
                uttt.GlobalBoard.Board[i] = MarkType.PLAYER1;
                uttt.GlobalBoard.LocalBoards[move.LbIndex].Board[i] = MarkType.PLAYER1;
            }

            uttt.MakeMove(move);

            Assert.Equal(move.Mark, uttt.CurrentPlayer);
        }
    }
}