using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkHuurder(IPasswordHasher<Account> passwordHasher) : AccountZakelijk(passwordHasher)
{
    public int AccountZakelijkHuurderId { get; set; }
    public int Account { get; set; }

    public override PasswordVerificationResult WachtwoordVerifieren(string password)
    {
        return passwordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}