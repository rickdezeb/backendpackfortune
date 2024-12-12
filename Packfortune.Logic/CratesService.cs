using Microsoft.AspNetCore.Http;
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
        private readonly ICratesRepository _crateRepository;
        private readonly IUserCoinsRepository _coinsRepository;
        private readonly IHostEnvironment _environment;

        public CratesService(ICratesRepository crateRepository, IUserCoinsRepository coinsRepository, IHostEnvironment environment)
        {
            _crateRepository = crateRepository;
            _coinsRepository = coinsRepository;
            _environment = environment;
        }

        public async Task AddCrate(string name, int price, IFormFile picture)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(picture.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidImageExtensionException("The file must be a PNG or JPG image.");
            }

            string imagePath = await SavePicture(picture);

            Crate data = new Crate
            {
                Name = name,
                Price = price,
                ImagePath = imagePath
            };

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

        public async Task<string> SavePicture(IFormFile picture)
        {
            string directoryPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "CratesImages");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = Path.GetFileName(picture.FileName);
            string filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }

            return Path.Combine("CratesImages", fileName);
        }

        public async Task<List<Crate>> GetAllCrates()
        {
            return await _crateRepository.GetAllCratesAsync();
        }


        public async Task UpdateCrate(int id, string name, int price, IFormFile picture)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(picture.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidImageExtensionException("The file must be a PNG or JPG image.");
            }

            string imagePath = await SavePicture(picture);

            if (id <= 0)
            {
                throw new InvalidIdException("The ID is invalid.");
            }

            if (price <= 0)
            {
                throw new NegativePriceException("The price is too low.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new NoNameException("The name field is required.");
            }

            if (string.IsNullOrEmpty(imagePath))
            {
                throw new Exception("You need to upload an image!");
            }

            var data = new Crate
            {
                Id = id,
                Name = name,
                Price = price,
                ImagePath = imagePath

            };

            await _crateRepository.UpdateCrate(data);
        }
        public async Task RemoveCrate(int id)
        {
            if (id <= 0)
            {
                throw new InvalidIdException("The ID is invalid.");
            }

            await _crateRepository.RemoveCrateAsync(id);
        }

        public async Task BuyCrate(string steamId, int crateId)
        {
            if (crateId <= 0)
            {
                throw new InvalidIdException("The ID is invalid.");
            }

            var user = await _coinsRepository.GetUserBySteamIdAsync(steamId);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var crate = await _crateRepository.GetCrateByIdAsync(crateId);
            if (crate == null)
            {
                throw new ArgumentException("Crate not found.");
            }

            if (user.Coins < crate.Price)
            {
                throw new ArgumentException("You don't have enough coins to buy this crate.");
            }

            user.Coins -= crate.Price;

            await _coinsRepository.UpdateUserAsync(user);

            var ownerCrate = new OwnerCrate
            {
                SteamId = steamId,
                CrateId = crateId
            };

            await _crateRepository.AddOwnerCrateAsync(ownerCrate);
        }
    }
}
