namespace WPRRewrite.Modellen.Accounts;

public class AccountMedewerkerBackoffice : AccountMedewerker
{
    public int AccountMedewerkerBackOfficeId { get; set; }
    public int Account { get; set; }
    
    public void UpdateAccountMedewerkerBackoffice(AccountMedewerkerBackoffice updatedAccountMedewerkerBackoffice)
    {
        Email = updatedAccountMedewerkerBackoffice.Email;
        Wachtwoord = updatedAccountMedewerkerBackoffice.Wachtwoord;
    }
}