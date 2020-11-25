using UTTT.Service.ObjectModel.Interfaces;

namespace UTTT.Service.ObjectModel.Abstracts
{
    public abstract class ATicTacToeBoard : ITicTacToeBoard
    {
        private int[,] _board = new int[3, 3];
        public int[,] Board { get => _board; set => _board = value; }

        public bool HasTicTacToe(int player)
        {
            // Check for horizontal and vertical tic tac toe
            for (int i = 0; i < 3; i++)
            {
                // Horizontal Tic Tac Toe
                if (Board[i, 0] == player && Board[i, 0] == Board[i, 1] && Board[i, 0] == Board[i, 2])
                {
                    return true;
                }

                // Vertical Tic Tac Toe
                if (Board[0, i] == player && Board[0, i] == Board[1, i] && Board[0, i] == Board[2, i])
                {
                    return true;
                }
            }

            // Check for negative diagonal tic tac toe
            if (Board[0, 0] == player && Board[0, 0] == Board[1, 1] && Board[0, 0] == Board[2, 2])
            {
                return true;
            }

            // Check for positive diagonal tic tac toe
            if (Board[2, 0] == player && Board[2, 0] == Board[1, 1] && Board[2, 0] == Board[0, 2])
            {
                return true;
            }

            // If there is no tic tac toe
            return false;
        }

        public bool IsFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}