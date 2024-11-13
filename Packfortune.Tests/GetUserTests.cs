using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using Packfortune.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packfortune.Logic.Exceptions;

namespace Packfortune.Tests
{
    [TestClass]
    public class GetUserTests
    {
        [TestMethod]
        public async Task EmptySteamId()
        {
            IUserCoins userCoins = new AddUserCoinsTest();
            UserCoinService coinService = new UserCoinService(userCoins);
            string steamId = "";

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => coinService.GetUserInfo(steamId));
            Assert.AreEqual("Steam ID cannot be null or empty. (Parameter 'steamId')", exception.Message);
        }

        [TestMethod]
        public async Task UserNotFound()
        {
            IUserCoins userCoins = new AddUserCoinsTest();
            UserCoinService coinService = new UserCoinService(userCoins);
            string steamId = "342545df";

            var exception = await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => coinService.GetUserInfo(steamId));
            Assert.AreEqual($"User with Steam ID '{steamId}' was not found.", exception.Message);
        }

        [TestMethod]
        public async Task UserFound()
        {
            IUserCoins userCoins = new AddUserCoinsTest();
            UserCoinService coinService = new UserCoinService(userCoins);
            string steamId = "existingUserId";

            var user = await coinService.GetUserInfo(steamId);

            Assert.IsNotNull(user);
            Assert.AreEqual("existingUserId", user.SteamId);
            Assert.AreEqual(100, user.Coins);
        }

    }
}
