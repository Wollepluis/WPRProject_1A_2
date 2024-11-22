namespace WPRProject_1A_2.Modellen.Accounts;

public class Account_Zakelijk : Account
{
    /*public Bedrijf bedrijf { get; set; }*/
    
    public Account_Zakelijk(string email, string wachtwoord/*, Bedrijf bedrijf*/) : base(email, wachtwoord)
    {
        /*Bedrijf = bedrijf;*/
    }
    
    public void VraagHuurAan()
    {
        
    }
}