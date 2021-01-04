using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.ObjectModel.Models
{
    /// <summary>
    /// Class to control to overall game
    /// </summary>
    public class GameObject : AEntity
    {
        private GlobalBoard _board = new GlobalBoard();
        public GlobalBoard Board { get => _board; set => _board = value; }
        // public Player XPlayer { get; set; }
        // public Player OPlayer { get; set; }
        public GameStatus Status { get; set; }
        public MarkShape CurrentPlayer { get; set; }

        public GameObject()
        {
            CurrentPlayer = MarkShape.X;
        }

        /// <summary>
        /// Make a move and mark boards/update focus and playablity accordingly
        /// </summary>
        /// <param name="move"></param>
        public void MakeMove(MoveObject move)
        {
            Board.MakeMove(move);
            Board.UpdateFocus(move);
        }

        /// <summary>
        /// Check if a move has been played already and lb is in focus
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool IsValidMove(MoveObject move) => Board.IsValidMove(move);

        /// <summary>
        /// Check to make sure it is the player's turn
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public void UpdateCurrentPlayer()
        {
            if (CurrentPlayer == MarkShape.X)
            {
                CurrentPlayer = MarkShape.O;
            }
            else
            {
                CurrentPlayer = MarkShape.X;
            }
        }

        /// <summary>
        /// Check if the game has ended, and update status accordingly
        /// </summary>
        public void UpdateGameStatus()
        {
            if (Board.HasTicTacToe(MarkShape.X))
            {
                Status = GameStatus.X_WINS;
            }
            else if (Board.HasTicTacToe(MarkShape.O))
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