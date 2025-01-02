using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerFrontoffice : AccountMedewerker
{
    public AccountMedewerkerFrontoffice(string email, string wachtwoord, IPasswordHasher<Account> passwordHasher, CarAndAllContext context)
        : base(passwordHasher, context)
    {
        Email = email;
        Wachtwoord = wachtwoord;
    }

    public AccountMedewerkerFrontoffice()
    {
    }
}