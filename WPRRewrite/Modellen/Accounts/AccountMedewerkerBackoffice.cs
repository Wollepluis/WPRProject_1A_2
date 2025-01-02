using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerBackoffice : AccountMedewerker
{
    public AccountMedewerkerBackoffice(string email, string wachtwoord, IPasswordHasher<Account> passwordHasher, CarAndAllContext context)
        : base(passwordHasher, context)
    {
        Email = email;
        Wachtwoord = wachtwoord;
    }

    public AccountMedewerkerBackoffice() { }

    
}