using WPRRewrite.Dtos;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkHuurder : AccountZakelijk
{
    public AccountZakelijkHuurder() { }
    public AccountZakelijkHuurder(string email, string wachtwoord, int bedrijfId) 
        : base (email,wachtwoord, bedrijfId) { }
    
    public override void UpdateAccount(AccountDto nieuweGegevens)
    {
        Email = nieuweGegevens.Email;
        Wachtwoord = nieuweGegevens.Wachtwoord;
    }
}