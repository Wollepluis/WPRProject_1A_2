using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountZakelijk : Account, IAccountZakelijk
{
    protected AccountZakelijk(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        
    }
    
    public int BedrijfId { get; set; }

    public override void UpdateAccount(IAccount updatedAccount)
    {
        if (updatedAccount == null) throw new ArgumentNullException(nameof(updatedAccount));
        
        Email = updatedAccount.Email;
        Wachtwoord = updatedAccount.Wachtwoord;
    }
}