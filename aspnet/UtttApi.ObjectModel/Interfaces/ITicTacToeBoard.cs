using System.Collections.Generic;
using UtttApi.ObjectModel.Models;

namespace UtttApi.ObjectModel.Interfaces
{
    public interface ITicTacToeBoard
    {
        MarkShape[] Board { get; set; }

        void MarkBoard(MoveObject move);

        bool HasTicTacToe(MarkShape player);

        bool IsFull();
    }
}