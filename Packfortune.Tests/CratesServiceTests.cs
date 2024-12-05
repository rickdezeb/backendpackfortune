using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Packfortune.Logic;
using Packfortune.Logic.Exceptions;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Packfortune.Logic.Tests
{
    [TestClass()]
    public class CratesServiceTests
    {
        private CratesService _crateService;
        private Mock<ICratesRepository> _mockCrateRepository;
        private Mock<IHostEnvironment> _mockEnvironment;

        [TestInitialize]
        public void Setup()
        {
            _mockCrateRepository = new Mock<ICratesRepository>();
            _mockEnvironment = new Mock<IHostEnvironment>();
            _mockEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _crateService = new CratesService(_mockCrateRepository.Object, _mockEnvironment.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidIdException))]
        public async Task UpdateCrate_InvalidId_ShouldThrowInvalidIdException()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.jpg");

            await _crateService.UpdateCrate(0, "Test Crate", 100, mockFile.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NegativePriceException))]
        public async Task UpdateCrate_NegativePrice_ShouldThrowNegativePriceException()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.jpg");

            await _crateService.UpdateCrate(1, "Test Crate", -100, mockFile.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NoNameException))]
        public async Task UpdateCrate_EmptyName_ShouldThrowNoNameException()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.jpg");

            await _crateService.UpdateCrate(1, "", 100, mockFile.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidImageExtensionException))]
        public async Task UpdateCrate_InvalidImageExtension_ShouldThrowInvalidImageExtensionException()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.txt");

            await _crateService.UpdateCrate(1, "Test Crate", 100, mockFile.Object);
        }
    }
}