using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewriteTests.Tests.SecurityTests
{
    [TestFixture]
    public class InputValidationTests
    {
        private HttpClient _client;
        private string _baseUrl = "https://localhost:5001/api/";
        private IPasswordHasher<Account> _mockPasswordHasher;
        private CarAndAllContext _mockContext;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _mockPasswordHasher = new PasswordHasher<Account>();

            var options = new DbContextOptionsBuilder<CarAndAllContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            _mockContext = new CarAndAllContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _mockContext.Dispose();
        }

        [Test]
        public async Task TestSQLInjection_ZouMoetenFalen()
        {
            // Arrange
            var maliciousEmail = "' OR '1'='1";
            var loginData = new LoginDto(maliciousEmail, "wachtwoord");

            // Act
            var response = await _client.PostAsJsonAsync(_baseUrl + "Backoffice/Login", loginData);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }

        [Test]
        public void TestWachtwoordHash()
        {
            // Arrange
            var password = "TestPassword123";
            var account = new AccountParticulier("test@test.com", password, "Test User", 123456789, 1, _mockPasswordHasher, _mockContext);

            // Act & Assert
            Assert.That(account.Wachtwoord, Is.Not.EqualTo(password), "Het wachtwoord moet gehasht zijn.");
        }

        [TestCase("test@test.com", "kort")] // Te kort wachtwoord
        [TestCase("nietvalide", "password123")] // Ongeldig email
        public void TestInputValidatie_MetOngeldigeData_GooitException(string email, string password)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new AccountParticulier(email, password, "Test", 123456789, 1, _mockPasswordHasher, _mockContext));
            
            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public async Task TestXSS_ZouMoetenFalen()
        {
            // Arrange
            var xssPayload = "<script>alert('xss')</script>";
            var accountData = new ParticulierDto(xssPayload + "@test.nl", "wachtwoord123", xssPayload, 123456789, "1234AB", 12);

            // Act
            var response = await _client.PostAsJsonAsync(_baseUrl + "Particulier/MaakAccount", accountData);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }
}
