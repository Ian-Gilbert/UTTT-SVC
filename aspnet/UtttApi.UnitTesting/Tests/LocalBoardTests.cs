using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class LocalBoardTests
    {
        private readonly LocalBoard lb;

        public LocalBoardTests()
        {
            lb = new LocalBoard();
        }

        [Fact]
        public void FocusAndPlayable_InitializeAsTrue()
        {
            Assert.True(lb.Focus);
            Assert.True(lb.Playable);
        }
    }
}