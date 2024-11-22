using WPRProject_1A_2.Modellen.Abonnementen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class AccountZakelijk : Account
{
    public Bedrijf Bedrijf { get; set; }
    
    public AccountZakelijk(string email, string wachtwoord, Bedrijf bedrijf) : base(email, wachtwoord)
    {
        Bedrijf = bedrijf;
    }
    
    public void VraagHuurAan()
    {
        
    }
}