using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.ObjectModel.Models
{
    public class GameObject : AEntity
    {
        private GlobalBoard _board = new GlobalBoard();
        public GlobalBoard Board { get => _board; set => _board = value; }
        // public Player XPlayer { get; set; }
        // public Player OPlayer { get; set; }
        public GameStatus Status { get; set; }

        public void MakeMove(MoveObject move)
        {
            Board.MakeMove(move);
            Board.UpdateFocus(move);
        }

        public bool IsValidMove(MoveObject move)
        {
            return Board.IsValidMove(move);
        }

        public bool CheckPlayerMove(MoveObject move)
        {
            return Board.CheckPlayerMove(move);
        }

        public void UpdateGameStatus()
        {
            if (Board.HasTicTacToe(PlayerShape.X))
            {
                Status = GameStatus.X_WINS;
            }
            else if (Board.HasTicTacToe(PlayerShape.O))
            {
                Status = GameStatus.O_WINS;
            }
            else if (Board.IsFull())
            {
                Status = GameStatus.DRAW;
            }
        }
    }
}