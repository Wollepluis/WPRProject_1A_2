
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
    public string BrandstofType { get; set; }

    public VoertuigDto()
    {
        
    }
    public VoertuigDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigStatus, string voertuigType, string brandstofType)
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
    }
}