using Moq;
using Packfortune.Logic;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Packfortune.Tests
{
    [TestClass]
    public class AddCoinsTest
    {
        [TestMethod]
        public async Task AddNegativeCoinsAsync()
        {
            var mockUserCoins = new Mock<IUserCoins>();

            var coinService = new UserCoinService(mockUserCoins.Object);

            var user = new User { Coins = -10 };

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => coinService.ProcessUserCoinDataAsync(user));
            Assert.AreEqual("Coins cannot be negative.", exception.Message);
        }

        [TestMethod]
        public async Task AddPositiveCoinsAsync()
        {
            var mockUserCoins = new Mock<IUserCoins>();

            mockUserCoins.Setup(repo => repo.AddUserCoinDataAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var coinService = new UserCoinService(mockUserCoins.Object);

            var user = new User { Coins = 10 };

            await coinService.ProcessUserCoinDataAsync(user);
            Assert.AreEqual(10, user.Coins);

            mockUserCoins.Verify(repo => repo.AddUserCoinDataAsync(It.IsAny<User>()), Times.Once);
        }
    }
}
