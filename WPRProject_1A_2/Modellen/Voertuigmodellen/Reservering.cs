using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRProject_1A_2.Modellen.Accounts;
using WPRProject_1A_2.Modellen.Enums;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Reservering
{
    [Key]
    public int ReserveringId { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime Begindatum { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime Einddatum { get; set; }
    public string AardVanReis { get; set; }
    public string VersteBestemming { get; set; }
    public int VerwachteHoeveelheidKm { get; set; }
    
    public List<Voertuig> BesteldeVoertuigen { get; set; }
    
    public Account Account { get; set; }
    public int RijbewijsDocumentnummer { get; set; }
    
    [DataType(DataType.Currency)]
    public Double Totaalprijs { get; set; }
    public Huuraanvraag Huuraanvraag { get; set; }
    
    public bool IsBetaald { get; set; }

    private Reservering() {}

    public Reservering(List<Voertuig> besteldeVoertuigen, DateTime begindatum, DateTime einddatum, string aardVanReis, string versteBestemming, int verwachteHoeveelheidKm, Account account, int rijbewijsDocumentnummer, double totaalprijs, bool isBetaald)
    {
        BesteldeVoertuigen = besteldeVoertuigen;
        Begindatum = begindatum;
        Einddatum = einddatum;
        AardVanReis = aardVanReis;
        VersteBestemming = versteBestemming;
        VerwachteHoeveelheidKm = verwachteHoeveelheidKm;

        Account = account;
        RijbewijsDocumentnummer = rijbewijsDocumentnummer;
        
        Totaalprijs = totaalprijs;
        Huuraanvraag = Huuraanvraag.InBehandeling;
        
        IsBetaald = isBetaald;
    }

    public int BerekenPrijs()
    {
        int prijs = 0;
        return prijs;
    }

    public void MaakFactuurAan()
    {
        
    }
}