using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Modellen;

public sealed class Bedrijf
{
    public int BedrijfId { get; set; }
    public int KvkNummer { get; set; }
    [MaxLength(255)] public string Bedrijfsnaam { get; set; }
    [MaxLength(255)] public string Domeinnaam { get; set; }
    
    public int AdresId { get; set; }
    [ForeignKey(nameof(AdresId))] public Adres Adres { get; set; }
    
    public int AbonnementId { get; set; }
    [ForeignKey(nameof(AbonnementId))] public Abonnement Abonnement { get; set; }

    public int? ToekomstigAbonnementId {get; set;}
    [ForeignKey(nameof(ToekomstigAbonnementId))] public Abonnement ToekomstigAbonnement { get; set; }

    public ICollection<AccountZakelijk> BevoegdeMedewerkers { get; set; } = new List<AccountZakelijk>();
    
    public Bedrijf() { }
    public Bedrijf(string bedrijfsnaam, string domeinnaam)
    {
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
    }
    public Bedrijf(int kvkNummer, string bedrijfsnaam, string domeinnaam, int adresId, int abonnementId)
    {
        KvkNummer = kvkNummer;
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
        AdresId = adresId;
        AbonnementId = abonnementId;
        BevoegdeMedewerkers = new List<AccountZakelijk>();
    }
    
    public void UpdateBedrijf(Bedrijf updatedBedrijf)
    {
        Bedrijfsnaam = updatedBedrijf.Bedrijfsnaam;
        Domeinnaam = updatedBedrijf.Domeinnaam;
    }
}