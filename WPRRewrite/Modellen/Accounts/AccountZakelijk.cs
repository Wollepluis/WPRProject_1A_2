using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountZakelijk(IPasswordHasher<Account> passwordHasher) : Account(passwordHasher), IAccountZakelijk
{
    public int BedrijfId { get; set; }

    public override void UpdateAccount(IAccount updatedAccount)
    {
        Email = updatedAccount.Email;
        Wachtwoord = updatedAccount.Wachtwoord;
    }
}