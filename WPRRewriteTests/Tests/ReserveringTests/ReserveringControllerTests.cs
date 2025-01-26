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
                .UseSqlServer(@"Server=LaptopMorris\SQLEXPRESS;Database=CarandallTest;Trusted_Connection=True;TrustServerCertificate=True")
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
        public async Task GetAll_ReserveringenAanwezig_RetourneertOk()
        {
            // Arrange
            _mockContext.Reserveringen.Add(new Reservering
            {
                AccountId = 1,
                VoertuigId = 3,
                Begindatum = DateTime.Now,
                Einddatum = DateTime.Now.AddDays(5),
                IsGoedgekeurd = true,
                TotaalPrijs = 200,
                IsBetaald = true,
                Comment = "N",
                Herinnering = false
            });
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetAlleReserveringen();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAccountReserveringen_GeenReserveringenVoorAccount_RetourneertNotFound()
        {
            // Act
            var result = await _controller.GetAlleReserveringenPerAccount(15);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>(), 
                "Expected NotFoundObjectResult, but got a different response.");

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null, "Expected result to not be null.");
            Assert.That(notFoundResult!.Value, Is.EqualTo("Geen reserveringen gevonden voor accountId: 15"),
                "Expected a specific error message.");
        }
    }
}
