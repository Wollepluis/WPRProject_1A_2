namespace WPRRewrite.Modellen.Accounts;

public abstract class Account
{
    public int AccountId { get; set; }
    public string Email { get; set; }
    public string Wachtwoord { get; set; }
    public string TypeAccount { get; set; }
}