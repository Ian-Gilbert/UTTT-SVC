using System.Collections.Generic;

namespace UTTT.Service.ObjectModel.Interfaces
{
    public interface ITicTacToeBoard
    {
        public int[,] Board { get; set; }

        public bool HasTicTacToe(int player);

        public bool IsFull();
    }
}