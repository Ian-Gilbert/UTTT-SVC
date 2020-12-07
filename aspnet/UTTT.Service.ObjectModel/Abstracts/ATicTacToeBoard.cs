using UTTT.Service.ObjectModel.Interfaces;

namespace UTTT.Service.ObjectModel.Abstracts
{
    public abstract class ATicTacToeBoard : ITicTacToeBoard
    {
        private int[] _board = new int[9];
        public int[] Board { get => _board; set => _board = value; }

        public bool HasTicTacToe(int player)
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
                if (Board[i] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void MarkBoard(int player, int move)
        {
            Board[move] = player;
        }
    }
}