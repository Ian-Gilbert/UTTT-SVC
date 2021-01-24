using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class GlobalBoardTests
    {
        public GlobalBoard gb { get; set; }

        public GlobalBoardTests()
        {
            gb = new GlobalBoard();
        }

        [Fact]
        public void GlobalBoard_ShouldCreateLocalBoards()
        {
            Assert.IsType<LocalBoard[]>(gb.LocalBoards);
            Assert.Equal(9, gb.LocalBoards.Length);
        }

        [Fact]
        public void IsValidMove_ReturnsFalse_WhenLbIsNotInFocus()
        {

        }
    }
}