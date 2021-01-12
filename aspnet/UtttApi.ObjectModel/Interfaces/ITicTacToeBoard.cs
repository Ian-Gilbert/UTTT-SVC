using System.Collections.Generic;
using UtttApi.ObjectModel.Models;

namespace UtttApi.ObjectModel.Interfaces
{
    public interface ITicTacToeBoard
    {
        MarkType[] Board { get; set; }

        void MarkBoard(MoveObject move);

        bool HasTicTacToe(MarkType player);

        bool IsFull();
    }
}