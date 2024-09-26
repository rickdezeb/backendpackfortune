using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;


namespace Packfortune.data
{
    public class AddUserCoins : IAddUserCoins
    {
        private readonly ApplicationDBContext _context;

        public AddUserCoins(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddUserCoinDataAsync(User userCoinData)
        {
            _context.UserCoinData.Add(userCoinData);
            await _context.SaveChangesAsync();
        }
    }
}
