namespace WPRProject_1A_2.Modellen.Abonnement;

public class Bedrijf
{
    public int Id { get; set; }
    public required string Bederijfsnaam { get; set; }
    public required string Domeinnaam { get; set; }
    public required Adres Adres { get; set; }
    public required int KVKNummer { get; set; }
    public Abonnement? Abonnement { get; set; }
    /*public required List<Account> Accounts { get; set; }

    public Bedrijf(int id, string bederijfsnaam, string domeinnaam, Adres adres, int kVKNummer, Abonnement abonnement, List<Account> accounts)
    {
        Id = id;
        Bederijfsnaam = bederijfsnaam;
        Domeinnaam = domeinnaam;
        Adres = adres;
        KVKNummer = kVKNummer;
        Abonnement = abonnement;
        Accounts = accounts;
    }
    */
    
}