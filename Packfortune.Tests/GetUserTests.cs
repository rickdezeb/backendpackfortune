using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using Packfortune.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packfortune.Logic.Exceptions;
using Moq;

namespace Packfortune.Tests
{
    [TestClass]
    public class GetUserTests
    {
        [TestMethod]
        public async Task EmptySteamId()
        {
            var mockUserCoins = new Mock<IUserCoinsRepository>();
            var coinService = new UserCoinService(mockUserCoins.Object);
            string steamId = "";

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => coinService.GetUserInfo(steamId));
            Assert.AreEqual("Steam ID cannot be null or empty. (Parameter 'steamId')", exception.Message);
        }

        [TestMethod]
        public async Task UserNotFound()
        {
            var mockUserCoins = new Mock<IUserCoinsRepository>();

            mockUserCoins.Setup(repo => repo.GetUserBySteamIdAsync("342545df")).ReturnsAsync((User)null);

            var coinService = new UserCoinService(mockUserCoins.Object);
            string steamId = "342545df";

            var exception = await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => coinService.GetUserInfo(steamId));
            Assert.AreEqual($"User with Steam ID '{steamId}' was not found.", exception.Message);
        }

        [TestMethod]
        public async Task UserFound()
        {
            var mockUserCoins = new Mock<IUserCoinsRepository>();

            mockUserCoins.Setup(repo => repo.GetUserBySteamIdAsync("existingUserId")).ReturnsAsync(new User { SteamId = "existingUserId", Coins = 100 });

            var coinService = new UserCoinService(mockUserCoins.Object);
            string steamId = "existingUserId";

            var user = await coinService.GetUserInfo(steamId);

            Assert.IsNotNull(user);
            Assert.AreEqual("existingUserId", user.SteamId);
            Assert.AreEqual(100, user.Coins);
        }
    }
}
