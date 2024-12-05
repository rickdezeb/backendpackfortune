using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packfortune.Logic.Models;
using Packfortune.Logic.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Packfortune.Logic.Exceptions;

namespace Packfortune.Logic
{
    public class UserCoinService
    {
        private readonly IUserCoinsRepository _userCoinRepository;

        public UserCoinService(IUserCoinsRepository userCoinRepository)
        {
            _userCoinRepository = userCoinRepository;
        }

        public async Task ProcessUserCoinDataAsync(User data)
        {
            if (data.Coins < 0)
            {
                throw new ArgumentException("Coins cannot be negative.");
            }

            await _userCoinRepository.AddUserCoinDataAsync(data);
        }

        public async Task<User> GetUserInfo(string steamId)
        {
            if (string.IsNullOrWhiteSpace(steamId))
            {
                throw new ArgumentException("Steam ID cannot be null or empty.", nameof(steamId));
            }

            var user = await _userCoinRepository.GetUserBySteamIdAsync(steamId);

            if (user == null)
            {
                throw new UserNotFoundException($"User with Steam ID '{steamId}' was not found.");
            }

            return user;
        }

        public async Task UpdateUserCoinsAsync(string steamId, int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException("Coins cannot be negative.");
            }

            await _userCoinRepository.UpdateUserCoinsAsync(steamId, coins);
        }

    }
}
