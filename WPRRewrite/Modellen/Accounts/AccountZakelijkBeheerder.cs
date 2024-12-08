using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkBeheerder : AccountZakelijk
{
    public AccountZakelijkBeheerder(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        
    }
    
    public int AccountZakelijkBeheerderId { get; set; }
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