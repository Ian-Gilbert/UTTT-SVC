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

            if (lb.Focus && lb.Board[move.SquareIndex] == 0)
            {
                return true;
            }

            return false;
        }

        public bool CheckPlayerMove(MoveObject move)
        {
            int xCount = 0;
            int oCount = 0;

            foreach (var lb in LocalBoards)
            {
                xCount += lb.CountPlayer(PlayerShape.X);
                oCount += lb.CountPlayer(PlayerShape.O);
            }

            if (move.Player == PlayerShape.X && xCount == oCount)
            {
                return true;
            }
            if (move.Player == PlayerShape.O && xCount == oCount + 1)
            {
                return true;
            }

            return false;
        }

        public void MakeMove(MoveObject move)
        {
            LocalBoard lb = LocalBoards[move.LbIndex];

            lb.MarkBoard(move);

            if (lb.HasTicTacToe(move.Player))
            {
                lb.Playable = false;
                MarkBoard(new MoveObject { Player = move.Player, SquareIndex = move.LbIndex });
            }
            else if (lb.IsFull())
            {
                lb.Playable = false;
                MarkBoard(new MoveObject { Player = PlayerShape.DRAW, SquareIndex = move.LbIndex });
            }
        }

        public void UpdateFocus(MoveObject move)
        {
            LocalBoard nextLb = LocalBoards[move.SquareIndex];

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