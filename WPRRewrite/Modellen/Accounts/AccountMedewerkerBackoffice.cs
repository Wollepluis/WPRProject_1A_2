using Microsoft.AspNetCore.Identity;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerBackoffice(IPasswordHasher<Account> passwordHasher) : AccountMedewerker(passwordHasher)
{
    public int AccountMedewerkerBackOfficeId { get; set; }
    public int Account { get; set; }
    
    public override PasswordVerificationResult WachtwoordVerifieren(string password)
    {
        return passwordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}