using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Models;

namespace UtttApi.ObjectModel.Abstracts
{
    public abstract class ATicTacToeBoard
    {
        public MarkType[] Board { get; set; }

        protected ATicTacToeBoard()
        {
            Board = new MarkType[9];
            for (int i = 0; i < 9; i++)
            {
                Board[i] = MarkType.EMPTY;
            }
        }

        public bool HasTicTacToe(MarkType player)
        {
            // Check for horizontal and vertical tic tac toe
            for (int i = 0; i < 3; i++)
            {
                // Horizontal Tic Tac Toe
                if (Board[i * 3] == player && Board[i * 3] == Board[i * 3 + 1] && Board[i * 3] == Board[i * 3 + 2])
                {
                    return true;
                }

                // Vertical Tic Tac Toe
                if (Board[i] == player && Board[i] == Board[i + 3] && Board[i] == Board[i + 6])
                {
                    return true;
                }
            }

            // Check for negative diagonal tic tac toe
            if (Board[0] == player && Board[0] == Board[4] && Board[0] == Board[8])
            {
                return true;
            }

            // Check for positive diagonal tic tac toe
            if (Board[2] == player && Board[2] == Board[4] && Board[2] == Board[6])
            {
                return true;
            }

            // If there is no tic tac toe
            return false;
        }

        public bool IsFull()
        {
            for (int i = 0; i < 9; i++)
            {
                if (Board[i] == MarkType.EMPTY)
                {
                    return false;
                }
            }

            return true;
        }

        public void MarkBoard(Move move)
        {
            Board[move.MarkIndex] = move.Mark;
        }
    }
}