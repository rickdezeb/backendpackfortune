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

        public async Task <List<Crate>> GetAllCratesAsync()
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
    }
}
