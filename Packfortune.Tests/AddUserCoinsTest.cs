using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System.Threading.Tasks;

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
            if (steamId == "existingUserId")
            {
                return Task.FromResult(new User { SteamId = "existingUserId", Coins = 100 });
            }

            return Task.FromResult<User>(null);
        }
    }
}
