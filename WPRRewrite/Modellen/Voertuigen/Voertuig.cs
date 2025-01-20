using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Dtos;
using WPRRewrite.Enums;
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
    public VoertuigStatusEnum VoertuigStatus { get; set; }
    public VoertuigTypeEnum VoertuigType { get; set; }
    
    public int? SchadeclaimId { get; set; }
    [ForeignKey(nameof(SchadeclaimId))] public Schadeclaim Schadeclaim { get; set; }
    
    protected Voertuig() { }
    protected Voertuig(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs,
        VoertuigStatusEnum voertuigStatus, VoertuigTypeEnum voertuigType)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigStatus;
        VoertuigType = voertuigType;
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

    public void UpdateVoertuigStatus(VoertuigStatusEnum status)
    {
        VoertuigStatus = status;
    }

    public static Voertuig MaakVoertuig(VoertuigDto gegevens)
    {
        return gegevens.VoertuigType switch
        {
            VoertuigTypeEnum.Auto => new Auto(
                gegevens.Kenteken, 
                gegevens.Merk, 
                gegevens.Model,
                gegevens.Kleur, 
                gegevens.Aanschafjaar,
                gegevens.Prijs,
                VoertuigStatusEnum.Beschikbaar,
                gegevens.VoertuigType
            ),
            VoertuigTypeEnum.Camper => new Camper(
                gegevens.Kenteken, 
                gegevens.Merk, 
                gegevens.Model,
                gegevens.Kleur, 
                gegevens.Aanschafjaar,
                gegevens.Prijs,
                VoertuigStatusEnum.Beschikbaar,
                gegevens.VoertuigType
            ),
            VoertuigTypeEnum.Caravan => new Caravan(
                gegevens.Kenteken, 
                gegevens.Merk, 
                gegevens.Model,
                gegevens.Kleur, 
                gegevens.Aanschafjaar,
                gegevens.Prijs,
                VoertuigStatusEnum.Beschikbaar,
                gegevens.VoertuigType
            ),
            _ => throw new ArgumentException($"Onbekend account type: {gegevens.VoertuigType}")
        };
    }
}