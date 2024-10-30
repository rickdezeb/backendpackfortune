using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packfortune.Logic.Models;
using Packfortune.Logic.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Packfortune.Logic
{
    public class UserCoinService
    {
        private readonly IUserCoins _userCoinRepository;

        public UserCoinService(IUserCoins userCoinRepository)
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
            return await _userCoinRepository.GetUserBySteamIdAsync(steamId);
        }
    }
}
