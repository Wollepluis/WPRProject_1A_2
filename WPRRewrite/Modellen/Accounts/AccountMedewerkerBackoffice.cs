using Microsoft.AspNetCore.Identity;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerBackoffice : AccountMedewerker
{
    public AccountMedewerkerBackoffice(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher) 
    {
        
    }

    private AccountMedewerkerBackoffice() 
    {
        
    }
    
    public int AccountMedewerkerBackOfficeId { get; set; }
    public int Account { get; set; }

    public override PasswordVerificationResult WachtwoordVerify(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Wachtwoord mag niet null of leeg zijn", nameof(password));
        }
        
        return PasswordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}