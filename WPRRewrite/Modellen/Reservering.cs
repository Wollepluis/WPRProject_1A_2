﻿using System.ComponentModel.DataAnnotations.Schema;
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
    public int TotaalPrijs { get; set; }
    public bool IsBetaald { get; set; }
    public bool IsGoedgekeurd { get; set; }
    public List<string> comments { get; set; }
    public int VoertuigId { get; set; }
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }
    public string Voertuigstatus { get; set; }

    public Reservering()
    {
        comments = new List<string>();
    }

    public Reservering(DateTime begindatum, DateTime einddatum, int totaalPrijs, int voertuigId, int accountId)
    {
        Begindatum = begindatum;
        Einddatum = einddatum;
        TotaalPrijs = totaalPrijs;
        VoertuigId = voertuigId;
        AccountId = accountId;
        IsGoedgekeurd = false;
        IsBetaald = false;
        comments = new List<string>();
        Voertuigstatus = "Gereserveerd";
    }
}