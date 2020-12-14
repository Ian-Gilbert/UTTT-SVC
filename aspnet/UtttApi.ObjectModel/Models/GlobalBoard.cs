using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.ObjectModel.Models
{
    public class GlobalBoard : ATicTacToeBoard
    {
        private LocalBoard[] _localboards = new LocalBoard[9];
        public LocalBoard[] LocalBoards { get => _localboards; set => _localboards = value; }

        public bool IsValidMove(int player, int lb_index, int move)
        {
            LocalBoard lb = LocalBoards[lb_index];

            if (lb.Focus && lb.Board[move] == 0)
            {
                return true;
            }

            return false;
        }

        public void MakeMove(int player, int lb_index, int move)
        {
            LocalBoard lb = LocalBoards[lb_index];

            lb.MarkBoard(player, move);

            if (lb.HasTicTacToe(player))
            {
                lb.Playable = false;
                MarkBoard(player, lb_index);
            }
            else if (lb.IsFull())
            {
                lb.Playable = false;
                MarkBoard(-1, lb_index);
            }
        }

        public void UpdateFocus(int move)
        {
            LocalBoard nextLb = LocalBoards[move];

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