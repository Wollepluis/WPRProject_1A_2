namespace WPRRewrite.Modellen.Abonnementen;

public abstract class Abonnement
{
    public int AbonnementId { get; set; }
    public int MaxVoertuigen { get; set; }
    public int MaxMedewerkers { get; set; }
    public string TypeAbonnement { get; set; }
}