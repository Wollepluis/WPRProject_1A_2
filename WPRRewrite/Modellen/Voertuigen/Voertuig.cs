using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public abstract class Voertuig : IVoertuig
{
    public int VoertuigId { get; set; }
    public string Kenteken { get; set; }
    public string Merk { get; set; }
    public string Model { get; set; }
    public string Kleur { get; set; }
    public int Aanschafjaar { get; set; }
    public int Prijs { get; set; }
    public string VoertuigStatus { get; set; }
    public string VoertuigType { get; set; }
    public string BrandstofType{ get; set; }
    
    public int? SchadeclaimId { get; set; }
    
    

    public List<Reservering> Reserveringen { get; set; }

    public List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
    public void UpdateVoertuig(IVoertuig updatedVoertuig)
    {
        Kenteken = updatedVoertuig.Kenteken;
        Merk = updatedVoertuig.Merk;
        Model = updatedVoertuig.Model;
        Kleur = updatedVoertuig.Kleur;
        Aanschafjaar = updatedVoertuig.Aanschafjaar;
        Prijs = updatedVoertuig.Prijs;
    }

    public void updateVoertuigStatus(string status)
    {
        VoertuigStatus = status;
    }
}