using System.ComponentModel.DataAnnotations;
using WPRProject_1A_2.Modellen.Abonnementen;
using WPRProject_1A_2.Modellen.Accounts;

namespace WPRProject_1A_2.Modellen.Abonnementen;

public class Bedrijf
{
    [Key]
    public int Id { get; set; }
    public required string Bedrijfsnaam { get; set; }
    public required string Domeinnaam { get; set; }
    public required Adres Adres { get; set; }
    public required int KvkNummer { get; set; }
    public Abonnement? Abonnement { get; set; }
    public required List<Account> Accounts { get; set; }

    public Bedrijf(string bedrijfsnaam, string domeinnaam, Adres adres, int kVkNummer)
    {
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
        Adres = adres;
        KvkNummer = kVkNummer;
        Accounts = new List<Account>();
    }
}