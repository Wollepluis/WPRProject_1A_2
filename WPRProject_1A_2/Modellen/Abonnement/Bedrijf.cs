namespace WPRProject_1A_2.Modellen.Abonnement;

public class Bedrijf
{
    public int Id { get; set; }
    public required string Bedrijfsnaam { get; set; }
    public required string Domeinnaam { get; set; }
    public required Adres Adres { get; set; }
    public required int KvkNummer { get; set; }
    public Abonnement? Abonnement { get; set; }
}