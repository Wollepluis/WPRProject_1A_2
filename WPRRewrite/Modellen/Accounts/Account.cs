using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class Account : IAccount
{
    protected readonly IPasswordHasher<Account> PasswordHasher;
    protected Account(IPasswordHasher<Account> passwordHasher)
    {
        PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public Account()
    {

    }

    public int AccountId { get; set; }
    public string Email { get; set; }
    public string Wachtwoord { get; set; }

    public abstract void UpdateAccount(IAccount account);
}