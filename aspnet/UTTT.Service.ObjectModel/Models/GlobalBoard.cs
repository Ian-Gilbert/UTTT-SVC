using UTTT.Service.ObjectModel.Abstracts;

namespace UTTT.Service.ObjectModel.Models
{
    public class GlobalBoard : ATicTacToeBoard
    {
        private LocalBoard[,] _localboards = new LocalBoard[3, 3];
        public LocalBoard[,] LocalBoards { get => _localboards; set => _localboards = value; }

        public void MarkGlobalBoard(int player, int index)
        {
            int row = index / 3;
            int col = index % 3;

            Board[row, col] = player;
        }

        public void UpdateFocus(int move_row, int move_col)
        {
            LocalBoard nextLb = LocalBoards[move_row, move_col];

            // if nextLb is playable, set focus to true and all others to false
            if (nextLb.Playable)
            {
                foreach (LocalBoard lb in LocalBoards)
                {
                    lb.Focus = false;
                }

                nextLb.Focus = true;
            }

            // if nextLb is not playable, all playble boards are in focus and all unplayable boards are not
            else
            {
                foreach (LocalBoard lb in LocalBoards)
                {
                    lb.Focus = lb.Playable;
                }
            }
        }
    }
}