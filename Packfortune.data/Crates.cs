﻿using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.data
{
    public class Crates : ICrates
    {
        private readonly ApplicationDBContext _context;
        public Crates(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task AddCrateAsync(Crate crateData)
        {
            _context.CrateData.Add(crateData);
            await _context.SaveChangesAsync();
        }
    }
}
