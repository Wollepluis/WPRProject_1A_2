using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountMedewerker(IPasswordHasher<Account> passwordHasher) : Account(passwordHasher), IAccountMedewerker
{
    public override void UpdateAccount(IAccount updatedAccount)
    {
        Email = updatedAccount.Email;
        Wachtwoord = updatedAccount.Wachtwoord;
    }
}