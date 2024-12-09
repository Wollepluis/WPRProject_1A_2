using Microsoft.AspNetCore.Identity;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerFrontoffice : AccountMedewerker
{
    public AccountMedewerkerFrontoffice(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        
    }

    public AccountMedewerkerFrontoffice()
    {
    }

    public override PasswordVerificationResult WachtwoordVerify(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Wachtwoord mag niet null of leeg zijn", nameof(password));
        }
        
        return PasswordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}