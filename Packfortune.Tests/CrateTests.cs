using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Packfortune.Logic.Exceptions;
using Packfortune.Logic.Interfaces;
using Packfortune.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Packfortune.Logic.Models;

namespace Packfortune.Tests
{
    [TestClass]
    public class CrateTests
    {
        private Mock<ICrates> _mockCrateRepository;
        private Mock<IHostEnvironment> _mockEnvironment;
        private CratesService _crateService;

        [TestInitialize]
        public void Setup()
        {
            string testDirectory = Path.Combine(Directory.GetCurrentDirectory(), "CratesImages");
            if (!Directory.Exists(testDirectory))
            {
                Directory.CreateDirectory(testDirectory);
            }

            _mockCrateRepository = new Mock<ICrates>();
            _mockEnvironment = new Mock<IHostEnvironment>();
            _mockEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _crateService = new CratesService(_mockCrateRepository.Object, _mockEnvironment.Object);
        }

        [TestMethod]
        public async Task AddCrate_WithNegativePrice_ThrowsNegativePriceException()
        {
            var mockFile = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Test File Content");
            writer.Flush();
            stream.Position = 0;

            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            await Assert.ThrowsExceptionAsync<NegativePriceException>(() => _crateService.AddCrate("Valid Name", -10, mockFile.Object));
        }

        [TestMethod]
        public async Task AddCrate_WithEmptyName_ThrowsNoNameException()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            await Assert.ThrowsExceptionAsync<NoNameException>(() => _crateService.AddCrate("", 100, mockFile.Object));
        }

        [TestMethod]
        public async Task AddCrate_WithValidData_CallsAddCrateAsync()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

            await _crateService.AddCrate("Valid Name", 100, mockFile.Object);

            _mockCrateRepository.Verify(repo => repo.AddCrateAsync(It.IsAny<Crate>()), Times.Once);
        }
    }
}
