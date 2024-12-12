using Microsoft.EntityFrameworkCore;
using Packfortune.data;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Packfortune.Data
{
    public class CratesRepository : ICratesRepository
    {
        private readonly ApplicationDBContext _context;

        public CratesRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddCrateAsync(Crate crateData)
        {
            _context.CrateData.Add(crateData);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Crate>> GetAllCratesAsync()
        {
            var crates = await _context.CrateData.ToListAsync();
            return crates;
        }

        public async Task UpdateCrate(Crate data)
        {
            _context.CrateData.Update(data);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCrateAsync(int id)
        {
            var crate = await _context.CrateData.FirstOrDefaultAsync(c => c.Id == id);
            _context.CrateData.Remove(crate);
            await _context.SaveChangesAsync();
        }

        public async Task<Crate> GetCrateByIdAsync(int crateId)
        {
            return await _context.CrateData.FirstOrDefaultAsync(c => c.Id == crateId);
        }
        public async Task AddOwnerCrateAsync(OwnerCrate ownerCrate)
        {
            _context.OwnerCrates.Add(ownerCrate);
            await _context.SaveChangesAsync();
        }

        public async Task BuyCrateAsync(string steamId, int crateId)
        {
            var user = await _context.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == steamId);
            var crate = await _context.CrateData.FirstOrDefaultAsync(c => c.Id == crateId);

            if (user == null || crate == null)
            {
                throw new ArgumentException("User or Crate not found.");
            }

            user.Coins -= crate.Price;
            _context.UserCoinData.Update(user);

            var ownerCrate = new OwnerCrate
            {
                SteamId = steamId,
                CrateId = crateId
            };

            _context.OwnerCrates.Add(ownerCrate);

            await _context.SaveChangesAsync();
        }
    }
}