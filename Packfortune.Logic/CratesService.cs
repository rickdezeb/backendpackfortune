﻿using Microsoft.AspNetCore.Http;
using Packfortune.Logic.Exceptions;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Packfortune.Logic
{
    public class CratesService
    {
        private readonly ICrates _crateRepository;
        private readonly IHostEnvironment _environment;

        public CratesService(ICrates crateRepository, IHostEnvironment environment)
        {
            _crateRepository = crateRepository;
            _environment = environment;
        }

        public async Task AddCrate(string name, int price, IFormFile Picture)
        {
            string imagePath = await SavePicture(Picture);

            Crate data = new Crate();
            data.Name = name;
            data.Price = price;
            data.ImagePath = imagePath;

            if (data.Price <= 0)
            {
                throw new NegativePriceException("The price is too low.");
            }
            if (string.IsNullOrEmpty(data.Name))
            {
                throw new NoNameException("The name field is required.");
            }
            if (string.IsNullOrEmpty(data.ImagePath))
            {
                throw new Exception("You need to upload an image!");
            }

            await _crateRepository.AddCrateAsync(data);
        }

        private async Task<string> SavePicture(IFormFile Picture)
        {
            string filePath;
            filePath = Path.Combine(_environment.ContentRootPath, "CratesImages");
            filePath = Path.Combine(filePath, Picture.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Picture.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
