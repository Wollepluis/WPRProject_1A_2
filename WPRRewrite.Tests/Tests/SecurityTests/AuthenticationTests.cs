// Tests/SecurityTests/AuthenticationTests.cs
using System.Net.Http.Json;
using WPRRewrite.Dtos;

namespace WPRRewrite.Tests.Tests.SecurityTests
{
    [TestFixture]
    public class AuthenticationTests
    {
        private HttpClient _client;
        private string _baseUrl = "https://localhost:5001/api/";

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
        }

        [Test]
        public async Task Login_MetInvalidCredentials_RetourneertUnauthorized()
        {
            // Arrange
            var loginData = new LoginDto("nietbestaand@test.nl", "foutWachtwoord");

            // Act
            var response = await _client.PostAsJsonAsync(_baseUrl + "Accounts/Login", loginData);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
        }
        
        [TearDown]
        public void TearDown()
        {
            _client.Dispose(); 
        }
    }
}