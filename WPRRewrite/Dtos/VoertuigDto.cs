using WPRRewrite.Enums;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Dtos;

public class VoertuigDto
{
    public string Kenteken { get; set; }
    public string Merk { get; set; }
    public string Model { get; set; }
    public string Kleur { get; set; }
    public int Aanschafjaar { get; set; }
    public int Prijs { get; set; }
    public string VoertuigStatus { get; set; }
    public string VoertuigType { get; set; }
    public BrandstofType BrandstofType { get; set; }
}