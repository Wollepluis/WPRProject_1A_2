namespace WPRRewrite.Dtos;

public class CamperDto : VoertuigDto
{
    public CamperDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
    }

    public CamperDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string camperVoertuigStatus)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = camperVoertuigStatus;
    }
    
}