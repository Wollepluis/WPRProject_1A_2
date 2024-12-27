

namespace WPRRewrite.Dtos;

public class CaravanDto : VoertuigDto
{
    public CaravanDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
    }

    public CaravanDto(string caravanKenteken, string caravanMerk, string caravanModel, string caravanKleur, int caravanAanschafjaar, int caravanPrijs, string caravanVoertuigStatus, string brandstofType)
    {
        Kenteken = caravanKenteken;
        Merk = caravanMerk;
        Model = caravanModel;
        Kleur = caravanKleur;
        Aanschafjaar = caravanAanschafjaar;
        Prijs = caravanPrijs;
        VoertuigStatus = caravanVoertuigStatus;
        BrandstofType = brandstofType;
    }
}