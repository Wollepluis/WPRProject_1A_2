using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WPRRewrite;
using WPRRewrite.Controllers;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewriteTests.Tests.VoertuigTests
{
    [TestFixture]
    public class VoertuigControllerTests
    {
        private Mock<CarAndAllContext> _mockContext;
        private VoertuigController _controller;
        private Mock<DbSet<Voertuig>> _mockVoertuigSet;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<CarAndAllContext>();
            _mockVoertuigSet = new Mock<DbSet<Voertuig>>();
            _mockContext.Setup(c => c.Voertuigen).Returns(_mockVoertuigSet.Object);
            _controller = new VoertuigController(_mockContext.Object);
        }

        [Test]
        public async Task GetAll_GeenVoertuigen_RetourneertLegeLijst()
        {
            // Arrange
            _mockVoertuigSet.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Voertuig>());

            // Act
            var resultaat = await _controller.GetAlleVoertuigen();

            // Assert
            Assert.That(resultaat.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = resultaat.Result as OkObjectResult;
            Assert.That(((List<Voertuig>)okResult.Value).Count, Is.EqualTo(0));
        }
    }
}