namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerFrontoffice : AccountMedewerker
{
    public int AccountMedewerkerFrontofficeId { get; set; }
    public int Account { get; set; }
    
    public void UpdateAccountMedewerkerFrontoffice(AccountMedewerkerFrontoffice updatedAccountMedewerkerFrontoffice)
    {
        Email = updatedAccountMedewerkerFrontoffice.Email;
        Wachtwoord = updatedAccountMedewerkerFrontoffice.Wachtwoord;
    }
}