using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountMedewerker : Account, IAccountMedewerker
{
    protected AccountMedewerker(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        
    }

    protected AccountMedewerker()
    {
    }

    public override void UpdateAccount(IAccount updatedAccount)
    {
        if (updatedAccount == null) throw new ArgumentNullException(nameof(updatedAccount));
        
        Email = updatedAccount.Email;
        Wachtwoord = updatedAccount.Wachtwoord;
    }
}