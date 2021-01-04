using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.ObjectModel.Models
{
    public class GlobalBoard : ATicTacToeBoard
    {
        private LocalBoard[] _localBoards = new LocalBoard[9];
        public LocalBoard[] LocalBoards { get => _localBoards; set => _localBoards = value; }

        public GlobalBoard() : base()
        {
            for (int i = 0; i < 9; i++)
            {
                _localBoards[i] = new LocalBoard();
            }
        }

        public bool IsValidMove(MoveObject move)
        {
            LocalBoard lb = LocalBoards[move.LbIndex];

            if (lb.Focus && lb.Board[move.MarkIndex] == MarkShape.EMPTY)
            {
                return true;
            }

            return false;
        }

        public void MakeMove(MoveObject move)
        {
            LocalBoard lb = LocalBoards[move.LbIndex];

            lb.MarkBoard(move);

            if (lb.HasTicTacToe(move.Mark))
            {
                lb.Playable = false;
                MarkBoard(new MoveObject { Mark = move.Mark, MarkIndex = move.LbIndex });
            }
            else if (lb.IsFull())
            {
                lb.Playable = false;
                MarkBoard(new MoveObject { Mark = MarkShape.DRAW, MarkIndex = move.LbIndex });
            }
        }

        public void UpdateFocus(MoveObject move)
        {
            LocalBoard nextLb = LocalBoards[move.MarkIndex];

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