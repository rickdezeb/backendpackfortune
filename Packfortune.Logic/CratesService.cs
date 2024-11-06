using Packfortune.Logic.Exceptions;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Logic
{
    public class CratesService
    {
        private readonly ICrates _crateRepository;

        public CratesService(ICrates crateRepository)
        {
            _crateRepository = crateRepository;
        }

        public async Task AddCrate(Crate data)
        {
            if (data.Price <= 0)
            {
                throw new NegativePriceException("The price is too low.");
            }
            if (string.IsNullOrEmpty(data.Name))
            {
                throw new NoNameException("You can't leave the name empty.");
            }

            await _crateRepository.AddCrateAsync(data);
        }
    }
}
