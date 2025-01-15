using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Modellen;

public class Bedrijf
{
    public int BedrijfId { get; set; }
    public int KvkNummer { get; set; }
    public string Bedrijfsnaam { get; set; }
    public int BedrijfAdres { get; set; }
    [ForeignKey("BedrijfAdres")]
    public Adres Adres { get; set; }
    public string Domeinnaam { get; set; }
    public int AbonnementId { get; set; }
    [ForeignKey("AbonnementId")]
    public Abonnement Abonnement { get; set; }

    public int ToekomstigAbonnementId {get; set;}
    [ForeignKey("ToekomstigAbonnementId")]
    public Abonnement ToekomstigAbonnement { get; set; }

    public List<AccountZakelijk> BevoegdeMedewerkers { get; set; }
    
    public void UpdateBedrijf(Bedrijf updatedBedrijf)
    {
        Bedrijfsnaam = updatedBedrijf.Bedrijfsnaam;
        Domeinnaam = updatedBedrijf.Domeinnaam;
    }

    public Bedrijf(int kvkNummer, string bedrijfsnaam, int bedrijfAdres, int abonnementId, string domeinnaam)
    {
        KvkNummer = kvkNummer;
        Bedrijfsnaam = bedrijfsnaam;
        BedrijfAdres = bedrijfAdres;
        AbonnementId = abonnementId;
        Domeinnaam = domeinnaam;
        BevoegdeMedewerkers= new List<AccountZakelijk>();
        
    }

    public Bedrijf(string bedrijfsnaam, string domeinnaam)
    {
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
        BevoegdeMedewerkers= new List<AccountZakelijk>();
    }

    public void VoegMedewerkerToe(AccountZakelijk account)
    {
        BevoegdeMedewerkers.Add(account);
    }
}