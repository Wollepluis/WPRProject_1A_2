/*// Tests/ReserveringTests/ReserveringIntegratieTests.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WPRRewrite.Controllers;
using WPRRewrite.DTOs;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Tests.Tests.ReserveringTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<Context> _mockContext;
        private AccountController _controller;
        private Mock<DbSet<Account>> _mockDbSet;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<Context>(new DbContextOptions<Context>());
            _mockDbSet = new Mock<DbSet<Account>>();
            _mockContext.Setup(c => c.Accounts).Returns(_mockDbSet.Object);
            _controller = new AccountController(_mockContext.Object);
        }

        [Test]
        public async Task GetAll_GeenAccounts_RetourneertNotFound()
        {
            // Arrange
            var accounts = new List<Account>().AsQueryable();
            _mockDbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(accounts.Provider);
            _mockDbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(accounts.Expression);
            _mockDbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(accounts.ElementType);
            _mockDbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());
            _mockDbSet.Setup(m => m.Any()).Returns(true);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task Login_CorrectWachtwoord_RetourneertOk()
        {
            // Arrange
            var testAccount = new AccountParticulier("test@test.com", "hashedPassword123", "Test User", 123456789, 1);
            _mockDbSet.Setup(d => d.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Account, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(testAccount);

            var loginDto = new LoginDto("test@test.com", "correctPassword");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }
    }
}

// SecurityTests.cs
[TestFixture]
public class SecurityTests
{
    [Test]
    public void TestWachtwoordHash()
    {
        // Arrange
        var password = "TestPassword123";
        var account = new AccountParticulier("test@test.com", password, "Test User", 123456789, 1);

        // Act & Assert
        Assert.That(account.Wachtwoord, Is.Not.EqualTo(password));
    }

    [TestCase("test@test.com", "kort")] // Te kort wachtwoord
    [TestCase("nietvalide", "password123")] // Ongeldig email
    public void TestInputValidatie(string email, string password)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new AccountParticulier(email, password, "Test", 123456789, 1));
        Assert.That(exception, Is.Not.Null);
    }
}

// IntegratieTests.cs
[TestFixture]
public class IntegratieTests
{
    private WebApplicationFactory<Program> _factory;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task AccountRegistratie_CompleteWorkflow_Succesvol()
    {
        // Arrange
        var client = _factory.CreateClient();
        var accountDto = new AccountDto(
            "Particulier",
            "test@test.com",
            "Password123!",
            "Test User",
            123456789,
            0,
            1
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/Accounts/Registreer", accountDto);

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/json"));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}*/