using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkHuurder : AccountZakelijk
{
    public AccountZakelijkHuurder(string email, string wachtwoord, int bedrijfId, IPasswordHasher<Account> passwordHasher, CarAndAllContext context)
        : base(passwordHasher, context)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        BedrijfId = bedrijfId;
    }

    public AccountZakelijkHuurder()
    {
    }

   
}