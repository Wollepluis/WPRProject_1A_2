// Tests/VoertuigTests/VoertuigControllerTests.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WPRRewrite.Controllers;
using WPRRewrite.Modellen.Kar;

namespace WPRRewrite.Tests.Tests.VoertuigTests
{
    [TestFixture]
    public class VoertuigControllerTests
    {
        private Mock<Context> _mockContext;
        private VoertuigController _controller;
        private Mock<DbSet<Voertuig>> _mockVoertuigSet;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<Context>();
            _mockVoertuigSet = new Mock<DbSet<Voertuig>>();
            _mockContext.Setup(c => c.Voertuigen).Returns(_mockVoertuigSet.Object);
            _controller = new VoertuigController(_mockContext.Object);
        }

        [Test]
        public async Task GetAll_GeenVoertuigen_RetourneertNotFound()
        {
            // Arrange
            _mockVoertuigSet.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Voertuig>());

            // Act
            var resultaat = await _controller.GetAll(null, null);

            // Assert
            Assert.That(resultaat.Result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}