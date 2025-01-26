using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite;
using WPRRewrite.Controllers;
using WPRRewrite.Modellen;

namespace WPRRewriteTests.Tests.ReserveringTests
{
    [TestFixture]
    public class ReserveringControllerTests
    {
        private CarAndAllContext _mockContext;
        private ReserveringController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarAndAllContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _mockContext = new CarAndAllContext(options);
            _controller = new ReserveringController(_mockContext);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Dispose();
        }

        [Test]
        public async Task GetAll_GeenReserveringen_RetourneertLegeLijst()
        {
            // Act
            var result = await _controller.GetAlleReserveringen();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(((List<Reservering>)okResult.Value).Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAll_ReserveringenAanwezig_RetourneertOk()
        {
            // Arrange
            _mockContext.Reserveringen.Add(new Reservering
            {
                AccountId = 1,
                VoertuigId = 1,
                Begindatum = DateTime.Now,
                Einddatum = DateTime.Now.AddDays(5),
                IsGoedgekeurd = true,
                TotaalPrijs = 200.0,
                IsBetaald = true
            });
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetAlleReserveringen();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAccountReserveringen_GeenReserveringenVoorAccount_RetourneertLegeLijst()
        {
            // Act
            var result = await _controller.GetAlleReserveringenPerAccount(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(((List<Reservering>)okResult.Value).Count, Is.EqualTo(0));
        }
    }
}
