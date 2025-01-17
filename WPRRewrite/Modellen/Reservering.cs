using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Modellen;

public class Reservering
{
    public int ReserveringId { get; set; }
    public DateOnly Begindatum { get; set; }
    public DateOnly Einddatum { get; set; }
    
    public bool IsGoedgekeurd { get; set; }
    public double TotaalPrijs { get; set; }
    public bool IsBetaald { get; set; }
   
    [MaxLength(255)] public string? Comment { get; set; }
    public bool Herinnering { get; set; }
    
    public int VoertuigId { get; set; }
    [ForeignKey(nameof(VoertuigId))] public Voertuig Voertuig { get; set; }
    
    public int AccountId { get; set; }
    [ForeignKey(nameof(AccountId))] public Account Account { get; set; }

    public Reservering() { }
    public Reservering(DateOnly begindatum, DateOnly einddatum, double totaalPrijs, int voertuigId, int accountId)
    {
        Begindatum = begindatum;
        Einddatum = einddatum;
        TotaalPrijs = totaalPrijs;
        VoertuigId = voertuigId;
        AccountId = accountId;
        IsBetaald = false;
        IsGoedgekeurd = false;
        Herinnering = false;
        Comment = "";
        
    }

    public void UpdateHerinnering()
    {
        Herinnering = true;
    }
}