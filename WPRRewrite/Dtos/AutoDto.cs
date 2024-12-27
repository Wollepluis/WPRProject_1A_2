

namespace WPRRewrite.Dtos;

public class AutoDto : VoertuigDto
{
    public AutoDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
    }

    public AutoDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string autoVoertuigStatus, string brandstofType)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = autoVoertuigStatus;
        BrandstofType = brandstofType;
    }
}