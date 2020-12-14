using System.Collections.Generic;

namespace UtttApi.ObjectModel.Interfaces
{
    public interface ITicTacToeBoard
    {
        int[] Board { get; set; }

        void MarkBoard(int player, int move);

        bool HasTicTacToe(int player);

        bool IsFull();
    }
}