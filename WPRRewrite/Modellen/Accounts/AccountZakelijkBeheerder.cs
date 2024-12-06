using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;
using Microsoft.AspNetCore.Identity;

public class AccountZakelijkBeheerder(IPasswordHasher<Account> passwordHasher) : AccountZakelijk(passwordHasher)
{
    public int AccountZakelijkBeheerderId { get; set; }
    public int Account { get; set; }
    
    public override PasswordVerificationResult WachtwoordVerifieren(string password)
    {
        return passwordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}