using WPRProject_1A_2.Modellen.Voertuigmodellen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class Account_Zakelijk_Huurder : Account_Zakelijk
{
    public Account_Zakelijk_Huurder(string email, string wachtwoord/*, Bedrijf bedrijf*/) : base(email, wachtwoord/*, bedrijf*/)
    {
        
    }

    public void HuurVoertuig(Voertuig voertuig)
    {
        
    }
}