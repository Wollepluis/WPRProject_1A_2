using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkBeheerder : AccountZakelijk
{
    public AccountZakelijkBeheerder(string email, string wachtwoord, int bedrijfsId ,IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        BedrijfsId = bedrijfsId;
    }

    public AccountZakelijkBeheerder()
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