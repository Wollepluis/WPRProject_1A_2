using WPRProject_1A_2.Modellen.Abonnementen;
using WPRProject_1A_2.Modellen.Accounts;

namespace WPRProject_1A_2.Modellen.Abonnementen;

public class Bedrijf
{
    public int Id { get; set; }
    public required string Bedrijfsnaam { get; set; }
    public required string Domeinnaam { get; set; }
    public required Adres Adres { get; set; }
    public required int KvkNummer { get; set; }
    public Abonnement? Abonnement { get; set; }
    public required List<Account> Accounts { get; set; }

    public Bedrijf(int id, string bedrijfsnaam, string domeinnaam, Adres adres, int kVkNummer, Abonnement abonnement, List<Account> accounts)
    {
        Id = id;
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
        Adres = adres;
        KvkNummer = kVkNummer;
        Abonnement = abonnement;
        Accounts = accounts;
    }
}