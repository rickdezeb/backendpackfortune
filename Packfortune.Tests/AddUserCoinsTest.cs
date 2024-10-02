using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;

namespace Packfortune.Tests
{
    public class AddUserCoinsTest : IAddUserCoins
    {
        public Task AddUserCoinDataAsync(User userCoinData)
        {
            return Task.CompletedTask;
        }
    }
}