using WPRRewrite.Dtos;

namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerBackoffice: AccountMedewerker
{
    public AccountMedewerkerBackoffice() { }
    public AccountMedewerkerBackoffice(string email, string wachtwoord) 
        : base(email, wachtwoord) { }
    
    public override void UpdateAccount(AccountDto nieuweGegevens)
    {
        Email = nieuweGegevens.Email;
        Wachtwoord = nieuweGegevens.Wachtwoord;
    }
}