// Tests/ReserveringTests/ReserveringControllerTests.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Controllers;
using WPRRewrite.Modellen;

namespace WPRRewrite.Tests.Tests.ReserveringTests
{
    [TestFixture]
    public class ReserveringControllerTests
    {
        private Context _mockContext;
        private ReserveringController _controller;

        [SetUp]
        public void Setup()
        {
            // Gebruik een unieke naam voor de in-memory database
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockContext = new Context(options);
            _controller = new ReserveringController(_mockContext);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Dispose();
        }

        [Test]
        public async Task GetAll_GeenReserveringen_RetourneertNotFound()
        {
            // Act
            var result = await _controller.GetReserveringen(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetAll_ReserveringenAanwezig_RetourneertOk()
        {
            // Arrange: Voeg een reservering toe aan de in-memory database
            var testDate = new DateTime(2025, 1, 1); // Gebruik een vaste datum
            _mockContext.Reserveringen.Add(new Reservering
            {
                AccountId = 1,
                VoertuigId = 1,
                Begindatum = new DateOnly(testDate.Year, testDate.Month, testDate.Day),
                Einddatum = new DateOnly(testDate.Year, testDate.Month, testDate.Day).AddDays(1),
                IsGoedgekeurd = true,
                TotaalPrijs = 100.0,
                IsBetaald = false
            });
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetReserveringen(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAccountReserveringen_GeenReserveringenVoorAccount_RetourneertNotFound()
        {
            // Act
            var result = await _controller.GetReserveringen(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetAccountReserveringen_ReserveringenVoorAccount_RetourneertOk()
        {
            // Arrange: Voeg reservering toe aan het account
            var testDate = new DateTime(2025, 1, 1);
            _mockContext.Reserveringen.Add(new Reservering
            {
                AccountId = 1,
                VoertuigId = 1,
                Begindatum = new DateOnly(testDate.Year, testDate.Month, testDate.Day),
                Einddatum = new DateOnly(testDate.Year, testDate.Month, testDate.Day).AddDays(1),
                IsGoedgekeurd = true,
                TotaalPrijs = 100.0,
                IsBetaald = false
            });
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetReserveringen(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
    }
}