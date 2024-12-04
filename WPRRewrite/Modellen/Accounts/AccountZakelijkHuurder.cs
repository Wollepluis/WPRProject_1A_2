namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkHuurder : AccountZakelijk
{
    public int AccountZakelijkHuurderId { get; set; }
    public int Account { get; set; }
    
    public void UpdateAccountZakelijkHuurder(AccountZakelijkHuurder updatedAccountZakelijkHuurder)
    {
        Email = updatedAccountZakelijkHuurder.Email;
        Wachtwoord = updatedAccountZakelijkHuurder.Wachtwoord;
    }
}