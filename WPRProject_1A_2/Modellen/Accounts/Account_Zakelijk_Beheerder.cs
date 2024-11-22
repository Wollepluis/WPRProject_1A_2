using WPRProject_1A_2.Modellen.Voertuigmodellen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class Account_Zakelijk_Beheerder : Account_Zakelijk
{
    public Account_Zakelijk_Beheerder(string email, string wachtwoord/*, Bedrijf bedrijf*/) : base(email, wachtwoord/*,
        bedrijf*/)
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

    public void WijzigAbonnement(/*Abonnement abonnement*/)
    {
        
    }

    public void VoegZakelijkeHuurderToe(Account_Zakelijk_Huurder account)
    {
        
    }

    public void VerwijderZakelijkeHuurder(Account_Zakelijk_Huurder account)
    {
        
    }

    public void OverzichtHuurautos(/*List<Auto> autos*/)
    {
        
    }
}