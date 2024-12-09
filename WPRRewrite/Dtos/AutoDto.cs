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

    public AutoDto(string caravanKenteken, string caravanMerk, string caravanModel, string caravanKleur, int caravanAanschafjaar, int caravanPrijs, string caravanVoertuigStatus)
    {
        Kenteken = caravanKenteken;
        Merk = caravanMerk;
        Model = caravanModel;
        Kleur = caravanKleur;
        Aanschafjaar = caravanAanschafjaar;
        Prijs = caravanPrijs;
        VoertuigStatus = caravanVoertuigStatus;
    }
}