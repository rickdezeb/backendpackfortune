using Packfortune.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Logic.Interfaces
{
    public interface ICratesRepository
    {
        Task AddCrateAsync(Crate crateData);
        Task<List<Crate>> GetAllCratesAsync();
        Task UpdateCrate(Crate data);
        Task RemoveCrateAsync(int id);
        Task BuyCrateAsync(string steamId, int crateId);
        Task<Crate> GetCrateByIdAsync(int crateId);
        Task AddOwnerCrateAsync(OwnerCrate ownerCrate);

    }
}
