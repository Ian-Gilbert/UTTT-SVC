using System.Net;
using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Exceptions;

namespace UtttApi.ObjectModel.Models
{
    /// <summary>
    /// Class to manage the overall game
    /// </summary>
    public class UtttObject : IEntity
    {
        public string Id { get; set; }
        public GameStatus Status { get; set; }
        public MarkType CurrentPlayer { get; set; }

        private GlobalBoard _board = new GlobalBoard();
        public GlobalBoard Board { get => _board; set => _board = value; }
        // public Player Player1 { get; set; }
        // public Player Player2 { get; set; }

        public UtttObject()
        {
            CurrentPlayer = MarkType.PLAYER1;
        }

        /// <summary>
        /// Make a move and mark boards/update focus and playablity accordingly
        /// </summary>
        /// <param name="move"></param>
        public void MakeMove(Move move)
        {
            CheckValidMove(move); // throw exception if not valid move
            Board.MakeMove(move);
            UpdateGameStatus();
            Board.UpdateFocus(move, Status);
            if (Status == GameStatus.IN_PROGRESS)
            {
                SwitchCurrentPlayer();
            }
        }

        /// <summary>
        /// Check if a move is valid, if not throw HttpResponseException with 400 Bad Request status
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public void CheckValidMove(Move move)
        {
            // game is not in progress
            if (Status != GameStatus.IN_PROGRESS)
            {
                throw new HttpResponseException(
                    HttpStatusCode.BadRequest,
                    $"The game is finished @ id = {Id}"
                );
            }

            // not player's turn
            if (move.Mark != CurrentPlayer)
            {
                throw new HttpResponseException(
                    HttpStatusCode.BadRequest,
                    $"It is not player {move.Mark.ToString("d")}'s turn @ id = {Id}"
                );
            }

            // move has already been made or lb is not in focus
            if (!Board.IsValidMove(move))
            {
                throw new HttpResponseException(
                    HttpStatusCode.BadRequest,
                    $"The move ({move.LbIndex}, {move.MarkIndex}) is not valid for player {move.Mark.ToString("d")} @ id = {Id}"
                );
            }
        }

        /// <summary>
        /// Switch the current player after a turn
        /// </summary>
        public void SwitchCurrentPlayer()
        {
            if (CurrentPlayer == MarkType.PLAYER1)
            {
                CurrentPlayer = MarkType.PLAYER2;
            }
            else
            {
                CurrentPlayer = MarkType.PLAYER1;
            }
        }

        /// <summary>
        /// Check if the game has ended, and update status accordingly
        /// </summary>
        public void UpdateGameStatus()
        {
            if (Board.HasTicTacToe(MarkType.PLAYER1))
            {
                Status = GameStatus.PLAYER1_WINS;
            }
            else if (Board.HasTicTacToe(MarkType.PLAYER2))
            {
                Status = GameStatus.PLAYER2_WINS;
            }
            else if (Board.IsFull())
            {
                Status = GameStatus.DRAW;
            }
        }
    }
}