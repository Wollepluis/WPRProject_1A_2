using Microsoft.AspNetCore.Identity;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkHuurder : AccountZakelijk
{
    private readonly IPasswordHasher<Account> _passwordHasher;
    public AccountZakelijkHuurder(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }
    
    public int AccountZakelijkHuurderId { get; set; }
    public int Account { get; set; }

    public override PasswordVerificationResult WachtwoordVerify(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Wachtwoord mag niet null of leeg zijn", nameof(password));
        }

        return _passwordHasher.VerifyHashedPassword(this, Wachtwoord, nameof(password));
    }
}