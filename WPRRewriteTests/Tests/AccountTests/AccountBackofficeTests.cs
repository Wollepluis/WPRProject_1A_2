using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Controllers;

namespace WPRRewriteTests.Tests.AccountTests
{
    [TestFixture]
    public class AccountBackofficeTests
    {
        private CarAndAllContext _mockContext;
        private AccountMedewerkerBackofficeController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarAndAllContext>()
                .UseSqlServer(@"Server=LaptopMorris\SQLEXPRESS;Database=CarandallTest;Trusted_Connection=True;TrustServerCertificate=True") 
                .Options;

            _mockContext = new CarAndAllContext(options);
            _mockContext.Database.OpenConnection();
            _mockContext.Database.EnsureCreated();

            _controller = new AccountMedewerkerBackofficeController(_mockContext);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Database.CloseConnection();
            _mockContext.Dispose();
        }
        
        [Test]
        public async Task GetAll_AccountsAanwezig_RetourneertOk()
        {
            // Arrange
            _mockContext.Accounts.Add(new AccountMedewerkerBackoffice 
            { 
                Email = "test@test.com", 
                Wachtwoord = "test123" 
            });
            await _mockContext.SaveChangesAsync();
            _mockContext.ChangeTracker.Clear();  // Ensures fresh query

            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            Assert.That(result, Is.Not.Null, "Expected result to not be null.");
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>(), "Expected OkObjectResult, but got something else.");
    
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.Value, Is.Not.Null, "Expected non-null value in OkObjectResult.");
        }
    }
}