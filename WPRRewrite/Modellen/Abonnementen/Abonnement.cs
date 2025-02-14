﻿namespace WPRRewrite.Modellen.Abonnementen;

public class Abonnement
{
    public int AbonnementId { get; set; }
    public int MaxVoertuigen { get; set; }
    public int MaxMedewerkers { get; set; }
    public string AbonnementType { get; set; }
    public DateTime? Begindatum { get; set; }

    public Abonnement()
    {
        
    }
    public Abonnement(int maxVoertuigen, int maxMedewerkers, string abonnementType)
    {
        MaxVoertuigen = maxVoertuigen;
        MaxMedewerkers = maxMedewerkers;
        AbonnementType = abonnementType;
    }
}