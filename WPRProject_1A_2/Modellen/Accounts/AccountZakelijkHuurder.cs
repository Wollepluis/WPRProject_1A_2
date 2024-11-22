using WPRProject_1A_2.Modellen.Abonnement;
using WPRProject_1A_2.Modellen.Voertuigmodellen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class AccountZakelijkHuurder : AccountZakelijk
{
    public AccountZakelijkHuurder(string email, string wachtwoord/*, Bedrijf bedrijf*/) : base(email, wachtwoord/*, bedrijf*/)
    {
        
    }

    public void HuurVoertuig(Voertuig voertuig)
    {
        
    }
}