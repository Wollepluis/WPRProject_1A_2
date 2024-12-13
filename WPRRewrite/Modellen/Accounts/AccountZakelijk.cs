using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountZakelijk : Account, IAccountZakelijk
{
    public int BedrijfId { get; set; }
    protected AccountZakelijk(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        
    }

    public AccountZakelijk()
    {
        
    }
    
    public Bedrijf Bedrijf { get; set; }
    public List<Reservering> Reserveringen { get; set; }

    public override void UpdateAccount(IAccount updatedAccount)
    {
        if (updatedAccount == null) throw new ArgumentNullException(nameof(updatedAccount));
        
        Email = updatedAccount.Email;
        Wachtwoord = updatedAccount.Wachtwoord;
    }

    public void AddReservering(Reservering reservering)
    {
        if (reservering == null) throw new ArgumentNullException(nameof(reservering));
        
        Reserveringen.Add(reservering);
    }
}