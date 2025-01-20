// Tests/AccountTests/AccountParticulierTests.cs
using WPRRewrite.Dtos;
using WPRRewrite.Enums;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Tests.Tests.AccountTests
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
                AccountTypeEnum.Particulier,
                "nieuw@test.nl",
                "nieuwWachtwoord",
                "Nieuwe Naam",
                0687654321,
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