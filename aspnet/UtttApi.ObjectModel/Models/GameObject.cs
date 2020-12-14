using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UtttApi.ObjectModel.Models
{
    public class GameObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; }
        public GlobalBoard Board { get; }
        // public Player XPlayer { get; set; }
        // public Player OPlayer { get; set; }
        public GameStatus Status { get; set; }

        public void MakeMove(int player, int lb_index, int move)
        {
            Board.MakeMove(player, lb_index, move);
            Board.UpdateFocus(move);
        }

        public bool IsValidMove(int player, int lb_index, int move)
        {
            return Board.IsValidMove(player, lb_index, move);
        }

        public void UpdateGameStatus()
        {
            if (Board.HasTicTacToe(1))
            {
                Status = GameStatus.X_WINS;
            }
            else if (Board.HasTicTacToe(2))
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