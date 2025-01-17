using Microsoft.AspNetCore.Identity;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkBeheerder : AccountZakelijk
{
    public AccountZakelijkBeheerder() { }
    public AccountZakelijkBeheerder(string email, string wachtwoord, int bedrijfId)
        : base (email,wachtwoord, bedrijfId) { }
    
    public override void UpdateAccount(AccountDto nieuweGegevens)
    {
        Email = nieuweGegevens.Email;
        Wachtwoord = nieuweGegevens.Wachtwoord;
        BedrijfId = nieuweGegevens.Nummer;
    }
}