using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Dtos;

public class ReserveringDto
{
    public int ReserveringId { get; set; }
    public DateTime Begindatum { get; set; }
    public DateTime Einddatum { get; set; }
    /*public string AardVanReis { get; set; }
    public string VersteBestemming { get; set; }
    public int VerwachteHoeveelheidkm { get; set; }
    public int Rijbewijsnummer { get; set; }*/
    public int TotaalPrijs { get; set; }
    public bool IsBetaald { get; set; }
    public bool IsGoedgekeurd { get; set; }
    public string Comment { get; set; }
    public int VoertuigId { get; set; }
    public int AccountId { get; set; }

    public ReserveringDto()
    {
        
    }

    public ReserveringDto(DateTime begindatum, DateTime einddatum, int totaalPrijs, int voertuigId, int accountId, string comment)
    {
        Begindatum = begindatum;
        Einddatum = einddatum;
        TotaalPrijs = totaalPrijs;
        VoertuigId = voertuigId;
        AccountId = accountId;
        IsGoedgekeurd = false;
        IsBetaald = false;
        Comment = comment;
    }
    
}