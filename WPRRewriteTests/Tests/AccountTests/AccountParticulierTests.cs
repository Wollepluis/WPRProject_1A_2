using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite;

namespace WPRRewriteTests.Tests.AccountTests
{
    [TestFixture]
    public class AccountParticulierTests
    {
        private IPasswordHasher<Account> _mockPasswordHasher;
        private CarAndAllContext _mockContext;

        [SetUp]
        public void Setup()
        {
            _mockPasswordHasher = new PasswordHasher<Account>();

            var options = new DbContextOptionsBuilder<CarAndAllContext>()
                .UseSqlServer(@"Server=LaptopMorris\SQLEXPRESS;Database=CarandallTest;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;
            _mockContext = new CarAndAllContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Dispose();
        }

        [Test]
        public void UpdateAccount_WijzigtGegevensCorrect()
        {
            // Arrange
            var account = new AccountParticulier("test@test.nl", "wachtwoord123", "Test Gebruiker", 1, 123456789, _mockPasswordHasher, _mockContext);
    
            var nieuwAccount = new AccountParticulier("nieuw@test.nl", "nieuwWachtwoord", "Nieuwe Naam", 12, 987654321, _mockPasswordHasher, _mockContext);

            // Act
            account.UpdateAccount(nieuwAccount);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Email, Is.EqualTo("nieuw@test.nl"));
                Assert.That(account.Naam, Is.EqualTo("Nieuwe Naam"));
                Assert.That(account.Telefoonnummer, Is.EqualTo(987654321));
            });
        }

        [Test]
        public void NieuwAccount_Aanmaken_MetGeldigeData()
        {
            // Arrange & Act
            var account = new AccountParticulier("valid@test.com", "SterkWachtwoord123", "Geldige Gebruiker", 123456789, 1, _mockPasswordHasher, _mockContext);

            // Assert
            Assert.That(account.Email, Is.EqualTo("valid@test.com"));
            Assert.That(account.Naam, Is.EqualTo("Geldige Gebruiker"));
            Assert.That(account.Wachtwoord, Is.Not.Empty);
        }

        [Test]
        public void NieuwAccount_Aanmaken_MetOngeldigeData_GooitException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => 
                new AccountParticulier("", "", "Naam", 123, 1, _mockPasswordHasher, _mockContext), 
                "Email en wachtwoord mogen niet leeg zijn.");
        }
    }
}
