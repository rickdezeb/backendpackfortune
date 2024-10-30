using Packfortune.Logic;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System;


namespace Packfortune.Tests
{
    [TestClass]
    public class AddCoinsTest
    {
        [TestMethod]
        public async Task AddNegativeCoinsAsync()
        {
            IUserCoins userCoins = new AddUserCoinsTest();
            UserCoinService coinService = new UserCoinService(userCoins);

            var user = new User { Coins = -10 };

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => coinService.ProcessUserCoinDataAsync(user));
            Assert.AreEqual("Coins cannot be negative.", exception.Message);
        }
    }
}