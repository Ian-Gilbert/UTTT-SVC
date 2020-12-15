using System.Collections.Generic;
using UtttApi.ObjectModel.Models;

namespace UtttApi.ObjectModel.Interfaces
{
    public interface ITicTacToeBoard
    {
        PlayerShape[] Board { get; set; }

        void MarkBoard(MoveObject move);

        bool HasTicTacToe(PlayerShape player);

        bool IsFull();
    }
}