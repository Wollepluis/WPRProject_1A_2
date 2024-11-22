namespace WPRProject_1A_2.Modellen.Abonnement;

public abstract class Abonnement
{
    public int Id { get; set; }
    public int Maxvoertuigen  { get; set; }
    public int MaxMedewerkers { get; set; }

    public Abonnement(int id, int maxvoertuigen, int maxMedewerkers)
    {
        Id = id;
        Maxvoertuigen = maxvoertuigen;
        MaxMedewerkers = maxMedewerkers;
    }
}