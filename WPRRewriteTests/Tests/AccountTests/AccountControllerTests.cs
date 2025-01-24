// Tests/AccountTests/AccountControllerTests.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Controllers;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewriteTests.Tests.AccountTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Context _mockContext;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockContext = new Context(options);
            _controller = new AccountController(_mockContext);
        }

        [Test]
        public async Task GetAll_GeenAccounts_RetourneertNotFound()
        {
            // Arrange: Geen accounts toevoegen aan de in-memory database

            // Act
            var result = await _controller.GetAccounts(null, null);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetAll_AccountsAanwezig_RetourneertOk()
        {
            // Arrange
            _mockContext.Accounts.Add(new AccountParticulier("test@test.com", "test123", "Test Naam", 1234567890, 1));
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetAccounts(null, null);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        
        [TearDown]
        public void TearDown()
        {
            _mockContext.Dispose();
        }
    }
}









