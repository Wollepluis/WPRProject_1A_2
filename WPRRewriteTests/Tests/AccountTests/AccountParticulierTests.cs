// Tests/AccountTests/AccountParticulierTests.cs
using NUnit.Framework;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewriteTests.Tests.AccountTests
{
    [TestFixture]
    public class AccountParticulierTests
    {
        [Test]
        public void UpdateAccount_WijzigtGegevensCorrect()
        {
            // Arrange
            var account = new AccountParticulier("test@test.nl", "wachtwoord123", "Test Gebruiker", 0612345678, 1);
            var nieuweGegevens = new AccountDto(
                "Particulier",
                "nieuw@test.nl",
                "nieuwWachtwoord",
                "Nieuwe Naam",
                0687654321,
                0,
                1
            );

            // Act
            account.UpdateAccount(nieuweGegevens);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account.Email, Is.EqualTo("nieuw@test.nl"));
                Assert.That(account.Naam, Is.EqualTo("Nieuwe Naam"));
                Assert.That(account.Telefoonnummer, Is.EqualTo(0687654321));
            });
        }
    }
}