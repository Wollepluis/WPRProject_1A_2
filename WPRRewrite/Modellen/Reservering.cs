using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Modellen;

public class Reservering
{

    public int ReserveringId { get; set; }
    public DateTime Begindatum { get; set; }
    public DateTime Einddatum { get; set; }
    /*public string AardVanReis { get; set; }
    public string VersteBestemming { get; set; }
    public int VerwachteHoeveelheidkm { get; set; }
    public int Rijbewijsnummer { get; set; }*/
    public double TotaalPrijs { get; set; }
    public bool IsBetaald { get; set; }
    public bool IsGoedgekeurd { get; set; }
    public string Comment { get; set; }
    public int VoertuigId { get; set; }
    [ForeignKey("VoertuigId")]
    public Voertuig Voertuig { get; set; }
    public int AccountId { get; set; }
    [ForeignKey(nameof(AccountId))] public Account Account { get; set; }

    //public bool Herinnering { get; set; }
    

    public Reservering()
    {
    }

    public Reservering(DateTime begindatum, DateTime einddatum, double totaalPrijs, int voertuigId, int accountId)
    {
        Begindatum = begindatum;
        Einddatum = einddatum;
        TotaalPrijs = totaalPrijs;
        VoertuigId = voertuigId;
        AccountId = accountId;
        IsBetaald = false;
        IsGoedgekeurd = false;
        //Herinnering = false;
        Comment = "";
        
    }

    /*public void UpdateHerinnering()
    {
        Herinnering = true;
    }*/
}