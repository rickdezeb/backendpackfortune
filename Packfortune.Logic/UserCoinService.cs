using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packfortune.Logic.Models;
using Packfortune.Logic.Interfaces;

namespace Packfortune.Logic
{
    public class UserCoinService
    {
        private readonly IAddUserCoins _userCoinRepository;

        public UserCoinService(IAddUserCoins userCoinRepository)
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
    }
}
