using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Modellen.Accounts;

public abstract class Account : IAccount
{
    protected readonly IPasswordHasher<Account> PasswordHasher;
    protected readonly CarAndAllContext Context;
    protected Account(IPasswordHasher<Account> passwordHasher, CarAndAllContext context)
    {
        PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Account()
    {

    }

    public int AccountId { get; set; }
    public string Email { get; set; }
    public string Wachtwoord { get; set; }

    public abstract void UpdateAccount(IAccount account);

}