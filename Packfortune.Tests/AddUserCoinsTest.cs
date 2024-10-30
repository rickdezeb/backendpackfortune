using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;

namespace Packfortune.Tests
{
    public class AddUserCoinsTest : IUserCoins
    {
        public Task AddUserCoinDataAsync(User userCoinData)
        {
            return Task.CompletedTask;
        }
        public Task<User> GetUserBySteamIdAsync(string steamId)
        {
            return (Task<User>)Task.CompletedTask;
        }
    }
}