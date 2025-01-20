using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountZakelijk : Account, IAccountZakelijk
{
    
    protected AccountZakelijk(IPasswordHasher<Account> passwordHasher, CarAndAllContext context)
        : base(passwordHasher, context)
    {
        
    }

    public AccountZakelijk()
    {
        
    }
    public int BedrijfId { get; set; }
    [ForeignKey(nameof(BedrijfId))]
    public Bedrijf Bedrijf { get; set; }

    public override void UpdateAccount(IAccount updatedAccount)
    {
        if (updatedAccount == null) throw new ArgumentNullException(nameof(updatedAccount));
        
        Email = updatedAccount.Email;
        Wachtwoord = updatedAccount.Wachtwoord;
    }
}