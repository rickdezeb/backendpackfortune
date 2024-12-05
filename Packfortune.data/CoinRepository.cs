using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;


namespace Packfortune.data
{
    public class CoinRepository : IUserCoinsRepository
    {
        private readonly ApplicationDBContext _context;

        public CoinRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddUserCoinDataAsync(User userCoinData)
        {
            var existingUser = await _context.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == userCoinData.SteamId);

            if (existingUser != null)
            {
                existingUser.Coins += userCoinData.Coins;
                _context.UserCoinData.Update(existingUser);
            }
            else
            {
                _context.UserCoinData.Add(userCoinData);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserBySteamIdAsync(string steamId)
        {
            var user = await _context.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == steamId);
            return user;
        }

        public async Task UpdateUserCoinsAsync(string steamId, int coins)
        {
            var user = await _context.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == steamId);
            int newCoins = user.Coins + coins;
            user.Coins = newCoins;
            _context.UserCoinData.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
