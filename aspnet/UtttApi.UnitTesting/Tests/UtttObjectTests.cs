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

        [Fact]
        public void CheckValidMove_ThrowsException_WhenGameIsNotInProgress()
        {
            uttt.Status = GameStatus.DRAW;
            var resultDraw = Assert.Throws<HttpResponseException>(() => uttt.CheckValidMove(move));
            Assert.Equal(400, resultDraw.StatusCode);

            uttt.Status = GameStatus.PLAYER1_WINS;
            var resultPlayer1 = Assert.Throws<HttpResponseException>(() => uttt.CheckValidMove(move));
            Assert.Equal(400, resultPlayer1.StatusCode);

            uttt.Status = GameStatus.PLAYER2_WINS;
            var resultPlayer2 = Assert.Throws<HttpResponseException>(() => uttt.CheckValidMove(move));
            Assert.Equal(400, resultPlayer2.StatusCode);
        }

        [Fact]
        public void CheckValidMove_ThrowsException_WhenNotPlayersTurn()
        {
            uttt.CurrentPlayer = MarkType.PLAYER2;
            var result = Assert.Throws<HttpResponseException>(() => uttt.CheckValidMove(move));
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void CheckValidMove_ThrowsException_WhenInvalidMove()
        {
            uttt.Board.LocalBoards[move.LbIndex].Focus = false; // target board is not in focus

            var result2 = Assert.Throws<HttpResponseException>(() => uttt.CheckValidMove(move));
            Assert.Equal(400, result2.StatusCode);
        }

        [Fact]
        public void SwitchPlayer_SwitchesPlayer() // wait that's what this method does???
        {
            uttt.SwitchCurrentPlayer();
            Assert.Equal(MarkType.PLAYER2, uttt.CurrentPlayer);

            uttt.SwitchCurrentPlayer();
            Assert.Equal(MarkType.PLAYER1, uttt.CurrentPlayer);
        }

        [Fact]
        public void UpdateGameStatus_SetsStatusToPlayer1Wins_WhenPlayer1HasTicTacToeOnGlobalBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                uttt.Board.Board[i] = MarkType.PLAYER1;
            }

            uttt.UpdateGameStatus();

            Assert.Equal(GameStatus.PLAYER1_WINS, uttt.Status);
        }

        [Fact]
        public void UpdateGameStatus_SetsStatusToPlayer2Wins_WhenPlayer2HasTicTacToeOnGlobalBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                uttt.Board.Board[i] = MarkType.PLAYER2;
            }

            uttt.UpdateGameStatus();

            Assert.Equal(GameStatus.PLAYER2_WINS, uttt.Status);
        }

        [Fact]
        public void UpdateGameStatus_SetsStatusToDraw_WhenGlobalBoardIsFull()
        {
            for (int i = 0; i < 9; i++)
            {
                uttt.Board.Board[i] = MarkType.DRAW;
            }

            uttt.UpdateGameStatus();

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
            Assert.Equal(move.Mark, uttt.Board.LocalBoards[move.LbIndex].Board[move.MarkIndex]);
        }

        [Fact]
        public void MakeMove_UpdatesStatus_WhenGameFinishes()
        {
            for (int i = 1; i < 3; i++)
            {
                uttt.Board.Board[i] = MarkType.PLAYER1;
                uttt.Board.LocalBoards[move.LbIndex].Board[i] = MarkType.PLAYER1;
            }

            uttt.MakeMove(move);

            Assert.Equal(GameStatus.PLAYER1_WINS, uttt.Status);
        }

        [Fact]
        public void MakeMove_UpdatesFocus()
        {
            move.MarkIndex = 3;
            var nextLb = uttt.Board.LocalBoards[move.MarkIndex];

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
                uttt.Board.Board[i] = MarkType.PLAYER1;
                uttt.Board.LocalBoards[move.LbIndex].Board[i] = MarkType.PLAYER1;
            }

            uttt.MakeMove(move);

            Assert.Equal(move.Mark, uttt.CurrentPlayer);
        }
    }
}