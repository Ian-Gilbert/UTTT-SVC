using System.Net;
using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Exceptions;
using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.ObjectModel.Models
{
    /// <summary>
    /// Class to manage the overall game
    /// </summary>
    public class UtttObject : AEntity
    {
        public GameStatus Status { get; set; }
        public MarkType CurrentPlayer { get; set; }
        public GlobalBoard GlobalBoard { get; set; }

        // public Player Player1 { get; set; }
        // public Player Player2 { get; set; }

        public UtttObject()
        {
            GlobalBoard = new GlobalBoard();
            CurrentPlayer = MarkType.PLAYER1;
        }

        /// <summary>
        /// Make a move and mark boards/update focus and playablity accordingly
        /// </summary>
        /// <param name="move"></param>
        public void MakeMove(Move move)
        {
            string message;
            if (!IsValidMove(move, out message))
            {
                throw new HttpResponseException(
                    HttpStatusCode.BadRequest,
                    message
                );
            }
            GlobalBoard.MakeMove(move);
            UpdateGameStatus(move.Mark);
            GlobalBoard.UpdateFocus(move.MarkIndex, Status);
            if (Status == GameStatus.IN_PROGRESS)
            {
                SwitchCurrentPlayer();
            }
        }

        /// <summary>
        /// Check if a move is valid. If not, return false with an appropriate error message
        /// </summary>
        /// <param name="move"></param>
        /// <param name="message"></param>
        /// <returns></returns>//
        public bool IsValidMove(Move move, out string message)
        {
            // game is not in progress
            if (Status != GameStatus.IN_PROGRESS)
            {
                message = $"The game is finished @ id = {Id}";
                return false;
            }

            // not player's turn
            if (move.Mark != CurrentPlayer)
            {
                message = $"It is not player {move.Mark.ToString("d")}'s turn @ id = {Id}";
                return false;
            }

            // move has already been made or lb is not in focus
            if (!GlobalBoard.IsValidMove(move))
            {
                message = $"The move ({move.LbIndex}, {move.MarkIndex}) is not valid for player {move.Mark.ToString("d")} @ id = {Id}";
                return false;
            }

            message = "valid move";
            return true;
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
        public void UpdateGameStatus(MarkType player)
        {
            if (GlobalBoard.HasTicTacToe(player))
            {
                if (player == MarkType.PLAYER1)
                {
                    Status = GameStatus.PLAYER1_WINS;
                }
                else if (player == MarkType.PLAYER2)
                {
                    Status = GameStatus.PLAYER2_WINS;
                }
            }
            else if (GlobalBoard.IsFull())
            {
                Status = GameStatus.DRAW;
            }
        }
    }
}