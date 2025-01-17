using WPRRewrite.Enums;
using WPRRewrite.Modellen;

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
    
    public VoertuigStatusEnum VoertuigStatus { get; set; }
    public VoertuigTypeEnum VoertuigType { get; set; }
    
    public void UpdateVoertuig(IVoertuig updatedVoertuig);
}