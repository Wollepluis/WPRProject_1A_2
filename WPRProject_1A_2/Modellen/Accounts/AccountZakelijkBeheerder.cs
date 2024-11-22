using WPRProject_1A_2.Modellen.Abonnementen;
using WPRProject_1A_2.Modellen.Voertuigmodellen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class AccountZakelijkBeheerder : AccountZakelijk
{
    public AccountZakelijkBeheerder(string email, string wachtwoord, Bedrijf bedrijf) : base(email, wachtwoord,
        bedrijf)
    {
        
    }

    public void GetStatusVoertuig(Voertuig voertuig)
    {
        
    }

    public void GetHuurverzoek()
    {
        
    }

    public void GetHuurabonnement()
    {
        
    }

    public void WijzigAbonnement(Abonnement abonnement)
    {
        
    }

    public void VoegZakelijkeHuurderToe(AccountZakelijkHuurder account)
    {
        
    }

    public void VerwijderZakelijkeHuurder(AccountZakelijkHuurder account)
    {
        
    }
}