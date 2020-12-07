using System.Collections.Generic;

namespace UTTT.Service.ObjectModel.Interfaces
{
    public interface ITicTacToeBoard
    {
        public int[] Board { get; set; }

        public void MarkBoard(int player, int move);

        public bool HasTicTacToe(int player);

        public bool IsFull();
    }
}