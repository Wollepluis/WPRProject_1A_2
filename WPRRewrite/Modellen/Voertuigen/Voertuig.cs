using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [ForeignKey(nameof(Schadeclaim))] public Schadeclaim Schadeclaim { get; set; }
    
    public void UpdateVoertuig(IVoertuig updatedVoertuig)
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
}