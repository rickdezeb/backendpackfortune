using Microsoft.EntityFrameworkCore;
using Packfortune.data;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
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
    }
}
