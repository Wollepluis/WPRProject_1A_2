namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkBeheerder : AccountZakelijk
{
    public int AccountZakelijkBeheerderId { get; set; }
    public int Account { get; set; }

    public void UpdateAccountZakelijkBeheerder(AccountZakelijkBeheerder updatedAccountZakelijkBeheerder)
    {
        Email = updatedAccountZakelijkBeheerder.Email;
        Wachtwoord = updatedAccountZakelijkBeheerder.Wachtwoord;
    }
}