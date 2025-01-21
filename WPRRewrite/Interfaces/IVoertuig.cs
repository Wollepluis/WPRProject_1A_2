using WPRRewrite.Dtos;

namespace WPRRewrite.Interfaces;

public interface IVoertuig
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
    
    public string BrandstofType { get; set; }
    public string AantalZitplaatsen { get; set; }
    
    public void UpdateVoertuig(VoertuigDto updatedVoertuig);
}