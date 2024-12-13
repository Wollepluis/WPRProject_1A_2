namespace WPRRewrite.Modellen.Abonnementen;

public class UpFront : Abonnement
{
    public UpFront(int maxMedewerkers, int maxVoertuigen) 
    {
        MaxMedewerkers = maxMedewerkers;
        MaxVoertuigen = maxVoertuigen;
    }
}