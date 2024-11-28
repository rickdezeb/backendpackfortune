using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using Packfortune.Logic.Exceptions;
using Packfortune.Logic;
using System;
using Packfortune.data;


namespace Packfortune.Tests
{

    [TestClass]
    public class GetUserIntegrationTests
    {
        private DbContextOptions<ApplicationDBContext> _dbContextOptions;
        private ApplicationDBContext _dbContext;
        private IUserCoinsRepository _userCoinsRepository;
        private UserCoinService _coinService;

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase("TestDatabase")
                .Options;

            _dbContext = new ApplicationDBContext(_dbContextOptions);

            _dbContext.UserCoinData.Add(new User { SteamId = "testUser", Username = "tester", Coins = 50 });
            _dbContext.SaveChanges();

            _userCoinsRepository = new CoinRepository(_dbContext);
            _coinService = new UserCoinService(_userCoinsRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetUser_UserFound()
        {
            string steamId = "testUser";

            var user = await _coinService.GetUserInfo(steamId);

            Assert.IsNotNull(user);
            Assert.AreEqual("testUser", user.SteamId);
            Assert.AreEqual("tester", user.Username);
            Assert.AreEqual(50, user.Coins);
        }

        [TestMethod]
        public async Task GetUser_Throws_UserNotFoundException_WhenSteamIDIsNotFound()
        {
            string steamId = "nonExistentUser";

            var exception = await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _coinService.GetUserInfo(steamId));
            Assert.AreEqual($"User with Steam ID '{steamId}' was not found.", exception.Message);
        }

        [TestMethod]
        public async Task GetUser_Throws_ArgumentException_WhenSteamIDIsLeftEmpty()
        {
            string steamId = "";

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _coinService.GetUserInfo(steamId));
            Assert.AreEqual("Steam ID cannot be null or empty. (Parameter 'steamId')", exception.Message);
        }
        [TestMethod]
        public async Task AddUser_AddsNewUser()
        {
            var newUser = new User { SteamId = "newUser", Username = "New Tester", Coins = 100 };

            await _userCoinsRepository.AddUserCoinDataAsync(newUser);

            var userInDb = await _dbContext.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == "newUser");
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("New Tester", userInDb.Username);
            Assert.AreEqual(100, userInDb.Coins);
        }

        [TestMethod]
        public async Task AddUser_UpdatesCoins_WhenUserReceivesCoins()
        {
            var existingUser = new User { SteamId = "existingUser", Username = "Existing Tester", Coins = 50 };
            _dbContext.UserCoinData.Add(existingUser);
            _dbContext.SaveChanges();

            var updateUser = new User { SteamId = "existingUser", Coins = 30 };

            await _userCoinsRepository.AddUserCoinDataAsync(updateUser);

            var userInDb = await _dbContext.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == "existingUser");
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("Existing Tester", userInDb.Username);
            Assert.AreEqual(80, userInDb.Coins);
        }

        [TestMethod]
        public async Task AddUser_DoesNotChangeUsername_WhenUserWithSameSteamIDIsUpdatedInDatabase()
        {
            var existingUser = new User { SteamId = "user123", Username = "OriginalName", Coins = 50 };
            _dbContext.UserCoinData.Add(existingUser);
            _dbContext.SaveChanges();

            var updateUser = new User { SteamId = "user123", Username = "UpdatedName", Coins = 20 };

            await _userCoinsRepository.AddUserCoinDataAsync(updateUser);

            var userInDb = await _dbContext.UserCoinData.FirstOrDefaultAsync(u => u.SteamId == "user123");
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("OriginalName", userInDb.Username);
            Assert.AreEqual(70, userInDb.Coins);
        }
    }
}
