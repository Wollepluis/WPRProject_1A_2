using WPRRewrite.Dtos;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerFrontoffice : AccountMedewerker
{
    public AccountMedewerkerFrontoffice() { }
    public AccountMedewerkerFrontoffice(string email, string wachtwoord) 
        : base(email, wachtwoord) { }
    
    public override void UpdateAccount(AccountDto nieuweGegevens)
    {
        Email = nieuweGegevens.Email;
        Wachtwoord = nieuweGegevens.Wachtwoord;
    }
}