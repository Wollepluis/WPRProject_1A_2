namespace WPRRewrite.Modellen.Abonnementen;

public class PayAsYouGo : Abonnement
{
    public PayAsYouGo(int maxMedewerkers, int maxVoertuigen) 
    {
        MaxMedewerkers = maxMedewerkers;
        MaxVoertuigen = maxVoertuigen;
    }
}