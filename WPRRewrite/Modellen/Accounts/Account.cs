using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class Account(IPasswordHasher<Account> passwordHasher) : IAccount
{
    public int AccountId { get; set; }
    public string Email { get; set; }
    public string Wachtwoord { get; set; }
    public string TypeAccount { get; set; }

    public abstract void UpdateAccount(IAccount account);

    public abstract PasswordVerificationResult WachtwoordVerifieren(string password);

}