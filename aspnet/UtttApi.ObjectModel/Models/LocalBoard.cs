using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.ObjectModel.Models
{
    public class LocalBoard : ATicTacToeBoard
    {
        // next move must be played on a board with focus == true
        public bool Focus { get; set; }

        // a board becomes unplayable if the state is determined (win/lose/full)
        public bool Playable { get; set; }

        public LocalBoard() : base()
        {
            Focus = true;
            Playable = true;
        }

        public int CountPlayer(MarkType player)
        {
            int count = 0;

            foreach (var square in Board)
            {
                if (square == player)
                {
                    count++;
                }
            }

            return count;
        }
    }
}