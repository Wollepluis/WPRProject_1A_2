// Tests/SecurityTests/InputValidationTests.cs
using System.Net.Http.Json;
using WPRRewrite.Dtos;
using WPRRewrite.Enums;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewriteTests.Tests.SecurityTests
{
    [TestFixture]
    public class InputValidationTests
    {
        private HttpClient _client;
        private string _baseUrl = "https://localhost:5001/api/";

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
        }

        [Test]
        public async Task TestSQLInjection_ZouMoetenFalen()
        {
            // Arrange
            var maliciousEmail = "' OR '1'='1";
            var loginData = new LoginDto(maliciousEmail, "wachtwoord");

            // Act
            var response = await _client.PostAsJsonAsync(_baseUrl + "Accounts/Login", loginData);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
        
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

        [Test]
        public async Task TestXSS_ZouMoetenFalen()
        {
            // Arrange
            var xssPayload = "<script>alert('xss')</script>";
            var accountData = new AccountDto(
                AccountTypeEnum.Particulier,
                xssPayload + "@test.nl",
                "wachtwoord123",
                xssPayload,
                0612345678,
                1
            );

            // Act
            var response = await _client.PostAsJsonAsync(_baseUrl + "Accounts/Registreer", accountData);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
        
        [TearDown]
        public void TearDown()
        {
            _client.Dispose(); 
        }
    }
}