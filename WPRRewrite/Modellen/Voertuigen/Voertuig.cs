using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public abstract class Voertuig : IVoertuig
{
    public int VoertuigId { get; set; }
    [MaxLength(255)] public string Kenteken { get; set; }
    [MaxLength(255)] public string Merk { get; set; }
    [MaxLength(255)] public string Model { get; set; }
    [MaxLength(255)] public string Kleur { get; set; }
    public int Aanschafjaar { get; set; }
    public int Prijs { get; set; }
    public string VoertuigStatus { get; set; }
    public string VoertuigType { get; set; }
    public string BrandstofType { get; set; }
    public string AantalZitplaatsen { get; set; }
    
    public int? SchadeclaimId { get; set; }
    [ForeignKey(nameof(SchadeclaimId))] public Schadeclaim Schadeclaim { get; set; }
    
    protected Voertuig() { }
    protected Voertuig(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs,
        string voertuigStatus, string voertuigType, string brandstofType, string aantalZitplaatsen)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigStatus;
        VoertuigType = voertuigType;
        BrandstofType = brandstofType;
        AantalZitplaatsen = aantalZitplaatsen;
    }
    
    public void UpdateVoertuig(VoertuigDto updatedVoertuig)
    {
        Kenteken = updatedVoertuig.Kenteken;
        Merk = updatedVoertuig.Merk;
        Model = updatedVoertuig.Model;
        Kleur = updatedVoertuig.Kleur;
        Aanschafjaar = updatedVoertuig.Aanschafjaar;
        Prijs = updatedVoertuig.Prijs;
    }

    public void UpdateVoertuigStatus(string status)
    {
        VoertuigStatus = status;
    }

    public static Voertuig MaakVoertuig(VoertuigDto gegevens)
    {
        return gegevens.VoertuigType switch
        {
            "Auto" => new Auto(
                gegevens.Kenteken, 
                gegevens.Merk, 
                gegevens.Model,
                gegevens.Kleur, 
                gegevens.Aanschafjaar,
                gegevens.Prijs,
                "Beschikbaar",
                gegevens.VoertuigType,
                gegevens.BrandstofType,
                gegevens.AantalZitplaatsen
            ),
            "Camper" => new Camper(
                gegevens.Kenteken, 
                gegevens.Merk, 
                gegevens.Model,
                gegevens.Kleur, 
                gegevens.Aanschafjaar,
                gegevens.Prijs,
                "Beschikbaar",
                gegevens.VoertuigType,
                gegevens.BrandstofType,
                gegevens.AantalZitplaatsen
            ),
            "Caravan" => new Caravan(
                gegevens.Kenteken, 
                gegevens.Merk, 
                gegevens.Model,
                gegevens.Kleur, 
                gegevens.Aanschafjaar,
                gegevens.Prijs,
                "Beschikbaar",
                gegevens.VoertuigType,
                gegevens.BrandstofType,
                gegevens.AantalZitplaatsen
            ),
            _ => throw new ArgumentException($"Onbekend account type: {gegevens.VoertuigType}")
        };
    }
}